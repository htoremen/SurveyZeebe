using Application.Common.Interfaces;
using MediatR;
using Survey.Application.Common.Interfaces;
using Survey.Shared.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Survey.Application.Surveys;

public class VoteTheSurveyCommand : IRequest
{
    public SurveyQuestionRequest Model { get; set; }
    public string UserId { get; set; }
}

public class VoteTheSurveyCommandhandler : IRequestHandler<VoteTheSurveyCommand>
{
    private readonly IZeebeService _zeebe;

    public VoteTheSurveyCommandhandler(IZeebeService zeebe)
    {
        _zeebe = zeebe;
    }

    public async Task<Unit> Handle(VoteTheSurveyCommand request, CancellationToken cancellationToken)
    {
        var model = new SurveyModel
        {
            SurveyQuestionRequest = request.Model,
            InstanceId = request.Model.InstanceId,
            UserId = request.UserId,
        };

        string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
        var response = await _zeebe.SendMessage(request.Model.InstanceId.ToString(), "Message_VoteTheSurvey", payload);

        return Unit.Value;
    }

}
