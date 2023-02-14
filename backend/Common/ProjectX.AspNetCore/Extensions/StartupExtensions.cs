using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.AspNetCore.Http;
using ProjectX.Core;
using Serilog;

namespace ProjectX.AspNetCore.Extensions;

public static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureAspNetCore(this WebApplicationBuilder app)
    {
        app
            .Services
            .AddControllers(o =>
            {
                o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
            .ConfigureApiBehaviorOptions(o => o.InvalidModelStateResponseFactory = c =>
            {
                var errors = string.Join(' ', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage));

                return new BadRequestObjectResult(new ResultOf<Unit>(ApplicationError.InvalidData(message: errors)));
            })
            .Services
            .AddProjecXCors();

        return app;
    }

    public static IServiceCollection AddProjecXCors(this IServiceCollection services)
        => services.AddCors(options =>
        {
            options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

    public static WebApplication UseProjectXCors(this WebApplication app)
    {
        app.UseCors("Open");

        return app;
    }

    public static WebApplication UseErrorHandler(this WebApplication app) 
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        return app;
    }

    public static WebApplication UseProjectXLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(o =>
        {
            // o.EnrichDiagnosticContext = TODO: enrich with IContext
        });

        return app;
    }
}
