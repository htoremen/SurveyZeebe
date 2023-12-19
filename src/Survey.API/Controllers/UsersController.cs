using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.Application;
using Survey.Shared.Models;

namespace Survey.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<GenericResponse<LoginResponse>> Login(LoginRequest model)
    {
        var command = Mapper.Map<LoginCommand>(model);

        var response = await Mediator.Send(command);
        return response;
    }

}
