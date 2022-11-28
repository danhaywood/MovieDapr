using System.Diagnostics;
using Man.Dapr.Sidekick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Models;
using MovieData;

namespace MovieBackend.Controllers
{
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
}