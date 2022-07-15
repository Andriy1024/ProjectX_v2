using ProjectX.Tasks.API;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder);

var app = builder.Build();

Startup.Configure(app);

try
{
    await app.RunAsync();
}
catch (Exception e)
{
    app.Logger.LogError(e, e.Message);
}