using ProjectX.AspNetCore.StartupTasks;
using ProjectX.Dashboard.API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.Configure(app);

try
{
    Log.Information("Starting web host");

    await app.RunWithTasksAsync();
    
    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Program terminated unexpectedly!");
    
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }