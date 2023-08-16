namespace Tgs.Game.Type.Round.Aggregate.Client;

public interface IGameTypeRoundClient
{
    Task GenerateGameTypeRoundsAsync(DateTime from, DateTime to);
}