using Tgs.Game.Type.Round.Aggregate.Client;

namespace Tgs.Game.Type.Round.Aggregate.IntegrationTests;

public class GameTypeRoundsTest
{
    [Test]
    public async Task Generate_Game_Type_Rounds()
    {
        await using var web = new TestApplication();
        var httpClient = web.CreateClient();

        var sut = new GameTypeRoundClient(httpClient);

        await sut.GenerateGameTypeRoundsAsync(DateTime.Parse("2023-08-01 00:15:00Z"),
            DateTime.Parse("2023-08-01 00:30:00Z"));
    }
}