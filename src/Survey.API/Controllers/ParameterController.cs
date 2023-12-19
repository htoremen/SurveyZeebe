using Microsoft.AspNetCore.Mvc;
using Survey.Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParameterController : ApiControllerBase
    {
        [HttpGet(Name = "GetParameter")]
        public async Task<GenericResponse<List<GetParameter>>> GetParameter(string name)
        {
            var response = await Mediator.Send(new GetParameterQuery { Name = name });
            return response;
        }
    }
}