using Application;
using Core.Infrastructure;
using Survey.Application.Common.Interfaces;
using Survey.Infrastructure.Services;
using Survey.Worker.Service;
using System;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
if (environment.EnvironmentName == "Development")
{
    builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
}
else
{
    builder.Configuration
            .AddJsonFile($"appsettings.json", false, true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddUserSecrets<Program>()
            .Build();
}

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);


builder.Services.AddApplication();
builder.Services.AddInfrastructure(appSettings);
builder.Services.AddWebUIServices(builder, appSettings);
builder.Services.AddHealthChecksServices(appSettings);


builder.Services.AddHostedService<ZeebeWorkService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var settings = builder.Configuration.Get<AppSettings>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var zeebeService = serviceProvider.GetRequiredService<IZeebeService>();
    await zeebeService.Deploy(settings.Zeebe.ModelFilename);

    while (true)
    {
        using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
        {
            var _service = serviceProvider.GetRequiredService<ISurveyWorkerService>();
            if (_service != null)
                _service.StartWorkers();

            // blocks main thread, so that worker can run
            signal.WaitOne();
        }
    }
}