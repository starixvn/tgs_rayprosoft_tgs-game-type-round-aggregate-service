namespace Tgs.Game.Type.Round.Aggregate.Client;

public class GameTypeRoundClient : IGameTypeRoundClient
{
    private readonly HttpClient _httpClient;

    public GameTypeRoundClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }


    public async Task GenerateGameTypeRoundsAsync(DateTime from, DateTime to)
    {
        var response = await _httpClient.PostAsync(
            $"api/gameTypeRound/{from:u}/{to:u}",
            null).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();
    }
}