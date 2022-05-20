using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Persistence.Implementations;

namespace ProjectX.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbServices<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        where T : DbContext
    {
        return services.AddDbContext<T>(optionsAction)
                       .AddScoped<IUnitOfWork, UnitOfWork<T>>()
                       .AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    //public static IServiceCollection AddTransactinBehaviour(this IServiceCollection services)
    //     => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
}