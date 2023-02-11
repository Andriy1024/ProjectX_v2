using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Core.StartupTasks;

namespace ProjectX.AspNetCore.StartupTasks;

public static class WebHostExtensions
{
    public static async Task RunWithTasksAsync(this WebApplication webHost)
    {
        await using (var scope = webHost.Services.CreateAsyncScope())
        {
            var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();

            foreach (var startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync();
            }
        }

        await webHost.RunAsync();
    }
}