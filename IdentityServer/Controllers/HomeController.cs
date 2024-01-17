using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[AllowAnonymous]
[Route("/")]
public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("FetchLogs")]    
    public IActionResult FetchLogs()
    {
        return PartialView("LogItemsList", Program.logSink.Lines);
    }
}
