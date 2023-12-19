using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Survey.Shared.Models;

namespace Survey.Application.Worker;

public class CheckSurveyCommand : IRequest<GenericResponse<CheckSurveyResponse>>
{
    public SurveyModel Model { get; set; }
}

public class CheckSurveyCommandHandler : IRequestHandler<CheckSurveyCommand, GenericResponse<CheckSurveyResponse>>
{
    private readonly IApplicationDbContext _context;

    public CheckSurveyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<CheckSurveyResponse>> Handle(CheckSurveyCommand request, CancellationToken cancellationToken)
    {
        var isSurveyCompleted = false;
        var surveyQuestion = _context.SurveyQuestion.Count(x=> x.SurveyItemId == request.Model.InstanceId);
        if (surveyQuestion > 0)
        {
            var userSurveyAnswers = _context.UserSurveyAnswers.Count(x=> x.UserSurvey.InstanceId == request.Model.InstanceId);
            if(surveyQuestion == userSurveyAnswers)
            {
                isSurveyCompleted = true;
            }
        }

        return GenericResponse<CheckSurveyResponse>.Success(new CheckSurveyResponse { IsSurveyCompleted = isSurveyCompleted},200);
    }
}
