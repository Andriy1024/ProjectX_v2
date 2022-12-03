using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectX.Core.StartupTasks;
using ProjectX.Identity.API.Database.Models;

namespace ProjectX.Identity.API.Database;

public sealed class DbStartupTask : IStartupTask
{
    private readonly IdentityDatabase _dbContext;
    private readonly UserManager<AccountEntity> _userManager;


    public DbStartupTask(IdentityDatabase dbContext, UserManager<AccountEntity> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync();

        if (!await _dbContext.Users.AnyAsync( x=> x.Email == "root@root.com"))
        {
            var newUser = new AccountEntity()
            {
                Email = "root@root.com",
                UserName = "root@rooe.com",
                EmailConfirmed = false,
                FirstName = "root",
                LastName = "root"
            };

            var isCreated = await _userManager.CreateAsync(newUser, "projectX#8");
            
            if (isCreated.Succeeded == false)
            {
                var error = string.Join(", ", isCreated.Errors.Select(e => e.Code));

                throw new Exception(error); 
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}