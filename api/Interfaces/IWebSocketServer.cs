using Fleck;

namespace api.Interfaces
{
    public interface IWebSocketServer : IDisposable
    {
        void Start(Action<IWebSocketConnection> configure);
        void Broadcast(string roomName, string message);
    }
}