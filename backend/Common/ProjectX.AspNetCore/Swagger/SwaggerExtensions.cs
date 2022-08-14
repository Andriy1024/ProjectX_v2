using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ProjectX.AspNetCore.Swagger;

public static class SwaggerExtensions
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    public static WebApplicationBuilder AddProjectXSwagger(this WebApplicationBuilder app)
    {
        var serviceName = $"{app.Environment.ApplicationName}.{app.Environment.EnvironmentName}";

        app.Services.AddEndpointsApiExplorer();

        app.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = serviceName, Version = "v1" });
            c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer".ToLowerInvariant(), //JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                Name = "Authorization",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.OperationFilter<AuthResponsesOperationFilter>();
        });

        return app;
    }

    public static void UseProjectXSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}