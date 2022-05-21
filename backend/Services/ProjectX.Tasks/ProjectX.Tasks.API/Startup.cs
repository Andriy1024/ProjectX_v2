using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectX.Persistence.Extensions;
using ProjectX.Tasks.Infrastructure.Handlers.Tasks;
using ProjectX.Tasks.Persistence.Context;
using System.Reflection;
using ProjectX.Tasks.Application.Mapper;

namespace ProjectX.Tasks.API;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services) 
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbServices<TasksDbContext>(o => o.UseInMemoryDatabase(databaseName: "ProjectX.Tasks"));
        services.AddMediatR(Assembly.GetAssembly(typeof(TasksHandlers))!);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(TaskProfile))!);
    }

    public static void Configure(WebApplication app) 
    {
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