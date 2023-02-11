using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Identity.Persistence;

public class IdentityDatabaseFactory : IDesignTimeDbContextFactory<IdentityDatabase>
{
    public IdentityDatabase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDatabase>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectX.Identity;Username=postgres;Password=root");

        return new IdentityDatabase(optionsBuilder.Options);
    }
}