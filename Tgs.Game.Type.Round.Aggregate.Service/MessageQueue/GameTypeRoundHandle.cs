using MediatR;
using Tgs.Game.Round.Aggregate.Service;
using Tgs.Game.Type.Round.Aggregate.Core;
using Tgs.MessageQueue;

namespace Tgs.Game.Type.Round.Aggregate.Service.MessageQueue;

public class GameTypeRoundHandle : IMessageHandler
{
    private readonly ILogger<GameTypeRoundHandle> _logger;
    private readonly IMediator _mediator;

    public GameTypeRoundHandle(IMediator mediator, ILogger<GameTypeRoundHandle> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async ValueTask HandleAsync(ReadOnlyMemory<byte> data,
        CancellationToken cancellationToken = new())
    {
        var request = GameTypeRoundRequest.Parser.ParseFrom(data.Span);

        _logger.LogInformation("Process generate GameTypeRound {from} ~ {to}", request.From.ToDateTime(),
            request.To.ToDateTime());

        await _mediator.Send(new CreateGameRoundAggregateRequest(request.From.ToDateTime(), request.To.ToDateTime()),
            cancellationToken);
    }
}