using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Survey.Shared.Models;

namespace Survey.Application.Worker;

public class TimeOutCommand : IRequest
{
    public SurveyModel Model { get; set; }
}
public class TimeOutCommandHandler : IRequestHandler<TimeOutCommand>
{
    private readonly IApplicationDbContext _context;

    public TimeOutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(TimeOutCommand request, CancellationToken cancellationToken)
    {
        var userSurvey = _context.UserSurveys.FirstOrDefault(x => x.InstanceId == request.Model.InstanceId);
        if (userSurvey != null)
        {
            userSurvey.Status = SurveyStatus.TimeOut;
            _context.UserSurveys.Update(userSurvey);
            _context.SaveChanges();
        }

        return Unit.Value;
    }
}
