using Man.Dapr.Sidekick;
using Microsoft.AspNetCore.Mvc;

namespace MovieBackend.Infra.Status;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class StatusController : ControllerBase
{
    [HttpGet]
    public ActionResult GetStatus([FromServices] IDaprSidecarHost daprSidecarHost) => Ok(new
    {
        Process = daprSidecarHost.GetProcessInfo(),
        Options = daprSidecarHost.GetProcessOptions()
    });
}