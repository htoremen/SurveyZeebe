using Core.Infrastructure;
using Service.Services;
using Survey.Shared.Common.Interfaces;

namespace Survey.Worker.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, WebApplicationBuilder builder, AppSettings appSettings)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<HttpContextAccessor>();
        services.AddHttpClient();
        services.AddScoped<HttpClient>();
        services.AddSingleton<IConfiguration>(builder.Configuration);
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }

    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHealthChecks();
        return services;
    }
}