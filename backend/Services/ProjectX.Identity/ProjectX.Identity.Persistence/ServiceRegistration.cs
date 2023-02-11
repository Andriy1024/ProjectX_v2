using Microsoft.Extensions.DependencyInjection;
using ProjectX.Core.StartupTasks;
using ProjectX.Persistence.Abstractions;
using ProjectX.Persistence.Extensions;

namespace ProjectX.Identity.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) 
    {
        return services
            .AddDbServices<IdentityDatabase>((p, o) =>
            {
                o.UseNpgsql(p.GetRequiredService<IDbConnectionStringAccessor>().GetConnectionString(),
                    x => x.MigrationsHistoryTable("MigrationsHistory", IdentityDatabase.SchemaName));
            })
            .AddIdentity<AccountEntity, RoleEntity>(o => { o.User.RequireUniqueEmail = true; })
            .AddRoles<RoleEntity>()
            .AddEntityFrameworkStores<IdentityDatabase>()
            .AddUserManager<UserManager<AccountEntity>>()
            .Services
            .AddScoped<IStartupTask, DbStartupTask>();
    }
}