using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Application.Commands.AddFavorite;
using UserService.Application.Commands.RegisterUser;
using UserService.Application.Commands.RemoveFavorite;
using UserService.Application.Interfaces;
using UserService.Application.Queries.LoginUser;
using Shared.Infrastructure.Database;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;
using Shared.Infrastructure.Options;

namespace UserService.API;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DbConnectionSettings>()
            .Bind(configuration.GetRequiredSection(nameof(DbConnectionSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<AppDbContext>(options =>
        {
            var dbSettings = configuration
                .GetRequiredSection(nameof(DbConnectionSettings))
                .Get<DbConnectionSettings>()!;
            options.UseNpgsql(dbSettings.ConnectionString);
        });

        services.AddScoped<IUserDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddJwtAuth(configuration);
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<AddFavoriteHandler>();
        services.AddScoped<RemoveFavoriteHandler>();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetRequiredSection(nameof(JwtOptions)));

        var jwtOptions = configuration
            .GetRequiredSection(nameof(JwtOptions))
            .Get<JwtOptions>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Convert.FromBase64String(jwtOptions.Secret)),
                };
            });

        services.AddAuthorization();

        return services;
    }
}
