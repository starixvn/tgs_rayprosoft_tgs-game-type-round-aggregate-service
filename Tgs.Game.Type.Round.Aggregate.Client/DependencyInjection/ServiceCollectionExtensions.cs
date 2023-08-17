using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Tgs.Game.Type.Round.Aggregate.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameTypeRoundAggregateClient(
		this IServiceCollection services,
		Action<GameTypeRoundAggregateOptions, IServiceProvider> configure)
		=> services
			.AddOptions<GameTypeRoundAggregateOptions>()
			.Configure(configure)
			.Services
			.AddHttpClient<IGameTypeRoundClient, GameTypeRoundClient>((sp, c) =>
			{
				var options = sp.GetRequiredService<IOptions<GameTypeRoundAggregateOptions>>().Value;

				c.BaseAddress = options.Url;
			}).Services;
}