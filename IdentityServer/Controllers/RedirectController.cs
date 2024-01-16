using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedirectController : ControllerBase
{
    [HttpPost("Get")]
    public IResult Get(string data)
    {
        return Results.Ok();
    }

    [AllowAnonymous]
    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        await Response.WriteAsJsonAsync($"{{ \"Test\": \"Successful\" }}");
        return new EmptyResult();
    }
}
