using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Survey.Shared.Models;

namespace Application;

public class CreateParameterCommand : IRequest<GenericResponse<bool>>
{
    public string ParameterTypeId {  get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

}

public class CreateParameterCommandHandler : IRequestHandler<CreateParameterCommand, GenericResponse<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateParameterCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GenericResponse<bool>> Handle(CreateParameterCommand request, CancellationToken cancellationToken)
    {
        var check = _context.Parameters.FirstOrDefaultAsync(x=> x.Name == request.Name.Trim(), cancellationToken).Result;
        if (check != null)
            return GenericResponse<bool>.Success(request.Name + " parametresi sistede kayıtlı", 201);

        _context.Parameters.Add(new Parameter
        {
            ParameterId = Guid.NewGuid().ToString(),
            ParameterTypeId = request.ParameterTypeId,
            Name = request.Name,
            IsActive = request.IsActive
        });
        await _context.SaveChangesAsync(cancellationToken);
        return GenericResponse<bool>.Success(true, 200);
    }
}