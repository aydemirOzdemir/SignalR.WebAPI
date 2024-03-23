using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.WebAPI.Hubs;

namespace SignalR.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<MyHub> hubContext;

    public NotificationController(IHubContext<MyHub> hubContext)
    {
        this.hubContext = hubContext;
    }
    [HttpGet("{teamCount}")]
    public async Task<IActionResult> Index(int teamCount)
    {
        await hubContext.Clients.All.SendAsync("Notify",$"Arkadaşlar takım {teamCount} kişi olacaktır.");
        return Ok();
    }
}
