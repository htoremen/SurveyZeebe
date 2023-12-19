using MediatR;
using Survey.Application.Common.Interfaces;
using Survey.Shared.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Survey.Application.Surveys;

public class ProcessSurveyCommand : IRequest
{
    public string InstanceId { get; set; }
    public string UserId { get; set; }
    public ProcessSurveyRequest ProcessSurveyRequest { get; set; }
}
public class ProcessSurveyCommandHandler : IRequestHandler<ProcessSurveyCommand>
{
    private readonly IZeebeService _zeebe;

    public ProcessSurveyCommandHandler(IZeebeService zeebe)
    {
        _zeebe = zeebe;
    }

    public async Task<Unit> Handle(ProcessSurveyCommand request, CancellationToken cancellationToken)
    {
        var instanceId = request.InstanceId;
        var expireInMinutes = "PT10M";
        var retryFrequence = "PT10M";
        var maxRetryCount = 3;

        var model = new SurveyModel
        {
            InstanceId = instanceId,
            ProcessSurveyRequest = request.ProcessSurveyRequest,
            UserId = request.UserId,
            ExpireInMinutes = expireInMinutes,
            RetryFrequence = retryFrequence,
            MaxRetryCount = maxRetryCount,
        };

        string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
        var response = await _zeebe.SendMessage(instanceId.ToString(), "Message_Survey_Start", payload);


        return Unit.Value;
    }
}
