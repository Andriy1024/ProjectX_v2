namespace ProjectX.Core.StartupTasks;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}