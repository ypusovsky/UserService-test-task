namespace TestTask.WebApi.Interfaces
{
    public interface IWebSocketService
    {
        Task AcceptWebSocketAsync(HttpContext context);
        Task NotifyClients(string message);
    }
}
