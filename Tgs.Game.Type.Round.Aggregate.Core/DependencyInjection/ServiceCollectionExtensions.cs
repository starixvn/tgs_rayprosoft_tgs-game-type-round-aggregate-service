using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Tgs.Game.Type.Round.Aggregate.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        //var opts = ConnectionFactory.GetDefaultOptions();

        //options?.Invoke(opts);

        services
            //.AddSingleton<IConnection>(_ => new ConnectionFactory().CreateConnection(opts))
            .AddMediatR(r => r.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}