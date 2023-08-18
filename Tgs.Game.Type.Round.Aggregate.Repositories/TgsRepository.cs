using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tgs.Game.Round.Aggregate.Repositories.Entities;
using Tgs.Game.Type.Round.Aggregate.Core;

namespace Tgs.Game.Type.Round.Aggregate.Repositories;

public class TgsRepository : ITgsRepository
{
    private readonly tgsContext _context;

    public TgsRepository(tgsContext context)
    {
        _context = context;
    }

    public Task GenerateGameTypeRoundsAggregateAsync(DateTime from, DateTime to,
        CancellationToken cancellationToken = default)
    {
        const string spName = @"
WITH rounds AS(
		SELECT
		r.id,
		r.bet_amount,
		r.valid_bet_amount,
		r.turnover,
		r.win_loss,
		r.game_id,
		r.player_code,
		r.brand_id,
		r.effective_betting_percentage AS round_effective_betting_percentage
		FROM bet.rounds r
		WHERE r.closed_on >= @from and r.closed_on < @to and r.reopened_on is null
		)
		,rows AS (select
		p.brand_id,
		p.agent_id,
		g.game_provider_id,
        g.id game_id,
		p.player_code,
        v.level vip_level,
		g.type game_type,
		sum(r.bet_amount) bet_amount,
		sum(r.valid_bet_amount) valid_bet_amount,
		sum(r.turnover) original_turnover,
		sum(CASE 
		WHEN r.round_effective_betting_percentage is not null THEN r.turnover * r.round_effective_betting_percentage / 100
		WHEN bg.effective_betting_percentage is not null THEN r.turnover * bg.effective_betting_percentage / 100 
		ELSE r.turnover END) turnover,
		sum(r.win_loss) win_loss,
        count(1) round_count
		from rounds r
		join dbo.games g on r.game_id = g.id
		join dbo.players p on r.player_code = p.player_code
        join dbo.vip_levels v on v.id = p.vip_id
        left join dbo.brand_games bg on bg.brand_id = r.brand_id and bg.game_id = r.game_id
		group by p.brand_id, g.game_provider_id, g.id, p.player_code, p.agent_id, v.id,g.type)

        INSERT INTO report.game_type_round_aggregates (brand_id,agent_id,player_code,game_provider_id,game_id,game_type,bet_amount,valid_bet_amount,original_turnover,turnover,win_loss,created_on,""from"",""to"",vip_level,round_count)
		SELECT brand_id,agent_id,player_code,game_provider_id,game_id,game_type,bet_amount,valid_bet_amount,original_turnover,turnover,win_loss,@created_on,@from,@to,vip_level,round_count
        FROM rows";

        var conn = _context.Database.GetDbConnection();
        var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

        return conn.ExecuteAsync(spName,
            new Dictionary<string, object>
            {
                { "from", from },
                { "to", to },
                { "created_on", DateTime.UtcNow }
            }, transaction);
    }

    public Task<bool> GameTypeRoundsExistsAsync(DateTime from, DateTime to,
        CancellationToken cancellationToken = default)
        => _context.GameTypeRoundAggregates.AnyAsync(x =>
            x.From == from && x.To == to, cancellationToken);

    public async Task<int> CreateExecutionTimeAsync(AggregateType type, DateTime from, DateTime to,
        CancellationToken cancellationToken = default)
    {
        _ = await _context.AggregateExecutionTimes.AddAsync(new()
        {
            AggregateType = (short)type,
            From = from,
            To = to
        }, cancellationToken);

        return await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExecutionTimeIsExistsAsync(AggregateType type, DateTime from, DateTime to,
        CancellationToken cancellationToken = default)
        => _context.AggregateExecutionTimes.AnyAsync(
            x => x.AggregateType == (short)type && x.From == from && x.To == to,
            cancellationToken);
}