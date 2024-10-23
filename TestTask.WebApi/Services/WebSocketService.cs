using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using TestTask.WebApi.Interfaces;

public class WebSocketService : IWebSocketService
{
    private readonly ConcurrentDictionary<WebSocket, Task> _clients = new();

    public async Task AcceptWebSocketAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                _clients.TryAdd(webSocket, Echo(webSocket));
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public async Task NotifyClients(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tasks = new List<Task>();

        foreach (var client in _clients.Keys)
        {
            tasks.Add(client.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None));
        }

        await Task.WhenAll(tasks);
    }

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
    }
}
