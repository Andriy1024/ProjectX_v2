using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Persistence.Abstractions;
using ProjectX.Persistence.Implementations;
using ProjectX.Persistence.Transaction;

namespace ProjectX.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbServices<T>(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
        where T : DbContext
    {
        return services.AddDbContext<T>(optionsAction)
                       .AddScoped<IUnitOfWork, UnitOfWork<T>>()
                       .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                       .AddScoped<IDbConnectionStringAccessor, DbConnectionStringAccessor>();
    }

    public static IServiceCollection AddTransactinBehaviour(this IServiceCollection services)
         => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
}