using Application.Common.Interfaces;
using MediatR;
using Survey.Shared.Models;

namespace Survey.Application.Worker;

public class SurveyConfirmationCommand : IRequest<GenericResponse<SurveyConfirmationResponse>>
{
    public SurveyModel Model { get; set; }
}

public class SurveyConfirmationCommandHandler : IRequestHandler<SurveyConfirmationCommand, GenericResponse<SurveyConfirmationResponse>>
{
    private readonly IApplicationDbContext _context;

    public SurveyConfirmationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<SurveyConfirmationResponse>> Handle(SurveyConfirmationCommand request, CancellationToken cancellationToken)
    {
        var question = request.Model.SurveyQuestionRequest;

        var surveyItem = _context.UserSurveys.FirstOrDefault(x => x.UserId == request.Model.UserId && x.InstanceId == request.Model.InstanceId);
        if (surveyItem == null)
            return GenericResponse<SurveyConfirmationResponse>.NotFoundException("Anket işlem yapan kullanıcıya ait değil.", 404);

        var answer = _context.UserSurveyAnswers.FirstOrDefault(x=> x.UserSurveyId == request.Model.InstanceId && x.SurveyQuestionId == question.SurveyQuestionId);
        if (answer != null)
            return GenericResponse<SurveyConfirmationResponse>.NotFoundException("Anket maddesi daha önce onaylanmış",404);

        var response = _context.UserSurveyAnswers.Add(new UserSurveyAnswer
        {
            UserSurveyAnswerId = Guid.NewGuid().ToString(),
            UserSurveyId = question.InstanceId,
            SurveyQuestionId = question.SurveyQuestionId,
            Answer = question.Answer
        }).Entity;

        _context.SaveChanges();

        return GenericResponse<SurveyConfirmationResponse>.Success(200);
    }
}
