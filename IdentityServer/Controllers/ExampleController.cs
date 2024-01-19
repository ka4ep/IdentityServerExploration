using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    [HttpPost("Get")]
    public async Task<IActionResult> Get(string data)
    {
        await Response.WriteAsJsonAsync(data);
        return new EmptyResult();
    }


    [Authorize]
    [HttpGet(nameof(Test))]
    public async Task<IActionResult> Test()
    {
        await Response.WriteAsJsonAsync($"{{ \"Test\": \"Successful\" }}");
        return new EmptyResult();
    }

    [Authorize(Roles = "admin")]
    [HttpGet(nameof(AdminOnly))]
    public async Task<IActionResult> AdminOnly()
    {
        await Response.WriteAsJsonAsync($"{{ \"Administrator\": \"Allowed\" }}");
        return new EmptyResult();
    }

    
    [Authorize(Roles = "viewer")]
    [HttpGet(nameof(AdminAndInvoiceOnly))]
    public async Task<IActionResult> AdminAndInvoiceOnly()
    {
        await Response.WriteAsJsonAsync($"{{ \"Administrator\": \"Allowed\", \"InvoiceOperator\": \"Allowed\" }}");
        return new EmptyResult();
    }
}
