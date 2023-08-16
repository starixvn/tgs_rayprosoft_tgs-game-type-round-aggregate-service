using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tgs.Game.Round.Aggregate.Repositories.Entities;
using Tgs.Game.Type.Round.Aggregate.Core;

namespace Tgs.Game.Type.Round.Aggregate.Repositories.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbContextConfigure)
        => services
            .AddDbContext<tgsContext>(dbContextConfigure)
            .AddTransient<ITgsRepository, TgsRepository>();
}