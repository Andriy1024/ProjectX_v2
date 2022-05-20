using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectX.Tasks.Infrastructure.Handlers.Tasks;
using ProjectX.Tasks.Persistence.Context;
using System.Reflection;

namespace ProjectX.Tasks.API;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services) 
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<TasksDbContext>(o => o.UseInMemoryDatabase(databaseName: "ProjectX.Tasks"));
        services.AddMediatR(Assembly.GetAssembly(typeof(TasksQueryHandler))!);
    }

    public static void Configure(WebApplication app) 
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}