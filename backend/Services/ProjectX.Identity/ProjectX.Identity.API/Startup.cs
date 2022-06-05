using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.Authentication.Configuration;
using System.Text;

namespace ProjectX.Identity.API;

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

        services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)));

        services.AddAuthentication(o => 
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt => 
        {
            var secret = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuer = false,   //TODO later true
                ValidateAudience = false, //TODO later true
                RequireExpirationTime = false, //TODO later true
                ValidateLifetime = true
            };
        });

        services.AddDbContext<ProjectXIdentityDbContext>(o =>
        {
            o.UseInMemoryDatabase(databaseName: "ProjectX.Identity");
        });

        services.AddIdentity<UserEntity, RoleEntity>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<RoleEntity>()
        .AddEntityFrameworkStores<ProjectXIdentityDbContext>();
        //.AddUserManager<UserManager>()

        services.AddTransient<JwtService>();
    }

    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
    }
}
