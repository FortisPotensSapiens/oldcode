using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.SecondaryAdaptersInterfaces
{
    public interface IWebSocketsMessaginClient
    {
        Task SendWebSocketMessage(UserId id, WebSocketMessage message);
    }
}
