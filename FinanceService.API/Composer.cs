using FinanceService.Application.Interfaces;
using FinanceService.Application.Queries.GetCurrencies;
using FinanceService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Application.Interfaces;
using Shared.Infrastructure.Database;
using Shared.Infrastructure.Options;
using Shared.Infrastructure.Repositories;
using FinanceService.API.Middleware;

namespace FinanceService.API;

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

        services.AddScoped<IFinanceDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddExceptionHandler<ExceptionHandler>();

        services.AddJwtAuth(configuration);
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGetCurrenciesHandler, GetCurrenciesHandler>();
        services.AddScoped<ITokenBlacklistRepository, TokenBlacklistRepository>();

        return services;
    }

    private static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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
}
