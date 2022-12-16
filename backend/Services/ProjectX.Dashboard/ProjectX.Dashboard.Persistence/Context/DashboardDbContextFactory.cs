using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Dashboard.Persistence.Context;

public class DashboardDbContextFactory : IDesignTimeDbContextFactory<DashboardDbContext>
{
    public DashboardDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DashboardDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectX.Tasks;Username=postgres;Password=root");

        return new DashboardDbContext(optionsBuilder.Options);
    }
}