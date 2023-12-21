using Application;
using Azure.Core;
using Core.Infrastructure;
using MediatR;
using Survey.Run.App.Models;
using Survey.Run.App.Services;
using Survey.Shared.Models;

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

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var settings = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(options => builder.Configuration.GetSection(nameof(AppSettings)).Bind(options));

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(settings);

StaticValues.SurveyAPI = settings.Service.SurveyAPI;

builder.Services.AddScoped<IAppRunService, AppRunService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var _service = serviceProvider.GetRequiredService<IAppRunService>();
    if (_service != null)
    {
        var userList = new List<UserModel>();
        var users = _service.GetUsers();
        foreach (var user in users)
        {
            var userModel = _service.Login(user.Username);
            userList.Add(userModel);
        }

        var surveyItems = _service.GetSurveyItems();
        foreach (var user in userList)
        {
            foreach (var surveyItem in surveyItems)
            {
                var instanceId = _service.SurveyAssignment(new ProcessSurveyRequest { SurveyItemId = surveyItem.SurveyItemId }, user.Token);
            }
        }

        foreach (var user in userList)
        {
            var userSurveys = _service.GetUserSurveys(user.UserId);
            foreach (var userSurvey in userSurveys)
            {
                var questions = _service.GetSurveyQuestions(userSurvey.SurveyItemId);
                foreach (var question in questions)
                {
                    _service.VoteTheSurvey(new SurveyQuestionRequest { InstanceId = userSurvey.InstanceId, UserSurveyId = userSurvey.UserSurveyId, SurveyQuestionId = question.SurveyQuestionId, Answer = "A" }, user.Token);
                }
            }
        }
    }
}