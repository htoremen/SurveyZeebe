using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Survey.Shared.Models;

namespace Survey.Application.Worker;

public class SurveyAssignmentCommand  :IRequest<GenericResponse<SurveyAssignmentResponse>>
{
    public SurveyModel Model { get; set; }
    public long ProcessInstanceKey { get; set; }
}

public class SurveyAssignmentCommandHandler : IRequestHandler<SurveyAssignmentCommand, GenericResponse<SurveyAssignmentResponse>>
{
    private readonly IApplicationDbContext _context;

    public SurveyAssignmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<SurveyAssignmentResponse>> Handle(SurveyAssignmentCommand request, CancellationToken cancellationToken)
    {
        var check = _context.UserSurveys.FirstOrDefault(x => x.UserId == request.Model.UserId && x.SurveyItemId == request.Model.ProcessSurveyRequest.SurveyItemId);
        if(check != null)
            return GenericResponse<SurveyAssignmentResponse>.Success("Anket daha önce atanmıştır", 200);

        _context.UserSurveys.Add(new UserSurvey
        {
            UserSurveyId = request.Model.InstanceId, // Guid.NewGuid().ToString(),
            SurveyItemId = request.Model.ProcessSurveyRequest.SurveyItemId,
            UserId = request.Model.UserId,
            Status = SurveyStatus.Appointed,
            Created = DateTime.Now,
            IsActived = 1,
            InstanceId = request.Model.InstanceId,  
            ProcessInstanceKey = request.ProcessInstanceKey
        });

        _context.SaveChanges();
        return GenericResponse<SurveyAssignmentResponse>.Success("", 200);
    }
}
