using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Survey.Application.Surveys;
using Survey.Shared.Models;

namespace Survey.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SurveyController : ApiControllerBase
{
    [HttpPost]
    [Route("survey-assignment")]
    public async Task<string> SurveyAssignment(ProcessSurveyRequest request)
    {
        var instanceId = Guid.NewGuid().ToString();
        await Mediator.Send(new ProcessSurveyCommand
        {
            ProcessSurveyRequest = request,
            UserId = UserService.UserId,
            InstanceId = instanceId,
        });

        return instanceId;
    }

    [HttpPost]
    [Route("vote-the-survey")]
    public async Task VoteTheSurvey(SurveyQuestionRequest request)
    {
        await Mediator.Send(new VoteTheSurveyCommand
        {
            Model = request,
            UserId = UserService.UserId,
        });
    }
}
