using MediatR;

namespace Tgs.Game.Type.Round.Aggregate.Core;

public record CreateGameRoundAggregateRequest(DateTime From, DateTime To) : IRequest;