using Microsoft.AspNetCore.Mvc;
using TestTask.WebApi.Interfaces;

[ApiController]
public class WebSocketController(IWebSocketService webSocketService) : ControllerBase
{
    [Route("/ws")]
    public async Task Get()
    {
        await webSocketService.AcceptWebSocketAsync(HttpContext);
    }
}