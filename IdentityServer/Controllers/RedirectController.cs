using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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


    [Authorize]
    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        var rts = this.HttpContext.RequestServices.GetService<IRefreshTokenService>();


        await Response.WriteAsJsonAsync($"{{ \"Test\": \"Successful\" }}");
        return new EmptyResult();
    }
}
