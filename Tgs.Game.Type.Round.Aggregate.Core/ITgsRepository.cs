namespace Tgs.Game.Type.Round.Aggregate.Core;

public interface ITgsRepository
{
    Task GenerateGameTypeRoundsAggregateAsync(DateTime from, DateTime to,
        CancellationToken cancellationToken = default);

    Task<bool> GameTypeRoundsExistsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

    Task<int> CreateExecutionTimeAsync(AggregateType type, DateTime from, DateTime to,
        CancellationToken cancellationToken = default);

    Task<bool> ExecutionTimeIsExistsAsync(AggregateType type, DateTime from, DateTime to,
        CancellationToken cancellationToken = default);
}