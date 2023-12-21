using Application.Common.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Survey.Application;
using Survey.Run.App.Models;
using Survey.Shared.Models;

namespace Survey.Run.App.Services;

public interface IAppRunService
{
    List<User> GetUsers();
    List<SurveyItem> GetSurveyItems();
    List<SurveyQuestion> GetSurveyQuestions(string surveyItemId);
    List<UserSurvey> GetUserSurveys(string userId);

    UserModel Login(string userName);
    string SurveyAssignment(ProcessSurveyRequest request, string token);
    string VoteTheSurvey(SurveyQuestionRequest surveyQuestionRequest, string token);
}
public class AppRunService : IAppRunService
{
    private readonly IApplicationDbContext _context;
    private readonly RestClient _restClient;
    public AppRunService(IApplicationDbContext context)
    {
        _context = context;
        _restClient = new RestClient(new RestClientOptions(StaticValues.SurveyAPI) { MaxTimeout = -1 });
    }

    public List<SurveyItem> GetSurveyItems()
    {
        return _context.SurveyItems.ToList();
    }

    public List<SurveyQuestion> GetSurveyQuestions(string surveyItemId)
    {
        return _context.SurveyQuestion.Where(x => x.SurveyItemId == surveyItemId).ToList();
    }

    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public List<UserSurvey> GetUserSurveys(string userId)
    {
        return _context.UserSurveys.Where(x => x.UserId == userId).ToList();
    }

    public UserModel Login(string userName)
    {
        var request = new RestRequest("/api/Users/login", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var login = new LoginRequest
        {
            Username = userName,
            Password = "123"
        };

        var json = JsonConvert.SerializeObject(login);
        request.AddJsonBody(json);
        var response = _restClient.Execute(request);

        var r = JsonConvert.DeserializeObject<GenericResponse<LoginResponse>>(response.Content);
        return new UserModel
        {
            UserId = r.Data.UserId,
            Token = r.Data.JwtToken
        };
    }

    public string SurveyAssignment(ProcessSurveyRequest model, string token)
    {
        var request = new RestRequest("/api/Survey/survey-assignment", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + token);

        var json = JsonConvert.SerializeObject(model);
        request.AddJsonBody(json);
        var response = _restClient.Execute(request);

        var r = JsonConvert.DeserializeObject<string>(response.Content);
        return r.ToString();
    }

    public string VoteTheSurvey(SurveyQuestionRequest model, string token)
    {
        var request = new RestRequest("/api/Survey/vote-the-survey", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + token);

        var json = JsonConvert.SerializeObject(model);
        request.AddJsonBody(json);
        var response = _restClient.Execute(request);

        return "";
    }
}