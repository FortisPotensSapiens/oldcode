using System.Threading.Tasks;
using Hike.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Hike.Hubs
{
    /// <summary>
    /// Хаб для получение уведомлений с сервера
    /// </summary>
    [SignalRHub]
    [Authorize]
    public class RealtimeHub : Hub<IWebSocketsClient>
    {
        /// <summary>
        /// Проверка
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SomeMethod(int value)
        {

        }
    }
}
