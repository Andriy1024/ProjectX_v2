using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectX.Persistence.Extensions;
using ProjectX.Tasks.Infrastructure.Handlers.Tasks;
using ProjectX.Tasks.Persistence.Context;
using System.Reflection;
using ProjectX.Tasks.Application.Mapper;
using ProjectX.AspNetCore.Http;
using ProjectX.Authentication;

namespace ProjectX.Tasks.API;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder app) 
    {
        var services = app.Services;
        var configuration = app.Configuration;

        services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddProjectXAuthentication(configuration);

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

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseProjectXAuthentication();

        app.UseAuthorization();

        app.MapControllers();
    }
}