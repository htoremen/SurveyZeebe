using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Survey.Shared.Models;

namespace Application;

public class GetParameterQuery: IRequest<GenericResponse<List<GetParameter>>>
{
    public string Name { get; set; }
}

public class GetParameterQueryHandler : IRequestHandler<GetParameterQuery, GenericResponse<List<GetParameter>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetParameterQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<GetParameter>>> Handle(GetParameterQuery request, CancellationToken cancellationToken)
    {
        var response = _context.Parameters.Where(x=> x.IsActive).ToList();
        var parameters = _mapper.Map<List<GetParameter>>(response);
        return GenericResponse<List<GetParameter>>.Success(parameters, 200);
    }
}
