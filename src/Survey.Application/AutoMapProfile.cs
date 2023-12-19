using AutoMapper;
using Survey.Application;

namespace Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}