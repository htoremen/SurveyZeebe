using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Survey.Shared.Models;

namespace Survey.Application.Worker;

public class SurveyCompletedCommand : IRequest<GenericResponse<SurveyCompletedResponse>>
{
    public SurveyModel Model { get; set; }
}

public class SurveyCompletedCommandhandler : IRequestHandler<SurveyCompletedCommand, GenericResponse<SurveyCompletedResponse>>
{
    private readonly IApplicationDbContext _context;

    public SurveyCompletedCommandhandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<SurveyCompletedResponse>> Handle(SurveyCompletedCommand request, CancellationToken cancellationToken)
    {
        var userSurvey = _context.UserSurveys.FirstOrDefault(x => x.InstanceId == request.Model.InstanceId);
        if (userSurvey != null)
        {
            userSurvey.Status = SurveyStatus.Completed;
            _context.UserSurveys.Update(userSurvey);
            _context.SaveChanges();
        }
        return GenericResponse<SurveyCompletedResponse>.Success(200);
    }
}
