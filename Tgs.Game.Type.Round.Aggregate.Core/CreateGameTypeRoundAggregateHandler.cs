using MediatR;

namespace Tgs.Game.Type.Round.Aggregate.Core;

public class
    CreateGameTypeRoundAggregateHandler : IRequestHandler<CreateGameRoundAggregateRequest>
{
    private readonly ITgsRepository _tgsRepository;

    public CreateGameTypeRoundAggregateHandler(
        ITgsRepository tgsRepository)
    {
        _tgsRepository = tgsRepository;
    }

    public async Task Handle(CreateGameRoundAggregateRequest request,
        CancellationToken cancellationToken)
    {
        if (await _tgsRepository.ExecutionTimeIsExistsAsync(AggregateType.GameTypeRound, request.From, request.To,
                cancellationToken))
        {
            return;
        }

        if (!await _tgsRepository.GameTypeRoundsExistsAsync(request.From, request.To, cancellationToken))
        {
            await _tgsRepository.GenerateGameTypeRoundsAggregateAsync(request.From, request.To, cancellationToken);
        }

        _ = await _tgsRepository.CreateExecutionTimeAsync(AggregateType.GameTypeRound, request.From,
            request.To, cancellationToken);
    }
}