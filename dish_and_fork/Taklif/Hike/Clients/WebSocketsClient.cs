using System.Threading.Tasks;
using CorePush.Google;
using CorePush.Interfaces;
using Hike.Entities;
using Hike.Hubs;
using Hike.Models;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Hike.Clients
{
    /// <summary>
    /// Хаб для получение уведомлений на клиенте
    /// </summary>
    [SignalRHub]
    public interface IWebSocketsClientEvents
    {
        /// <summary>
        /// Информация о том что статус заказа сменился
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task OrderStatusChanged(OrderState state);
        /// <summary>
        /// Информация о том что появился новый комментарий
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task OfferCommentAdded(OfferCommentReadModel model);
    }

    public interface IWebSocketsClient
    {

        /// <summary>
        /// Информация о том что статус заказа сменился
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task OrderStatusChanged(string userId, OrderState state);
        /// <summary>
        /// Информация о том что появился новый комментарий
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task OfferCommentAdded(string userId, OfferCommentReadModel model);
        /// <summary>
        /// Информация о том что статус заказа сменился
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task OrderStatusChanged(OrderState state);
        /// <summary>
        /// Информация о том что появился новый комментарий
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task OfferCommentAdded(OfferCommentReadModel model);
    }

    public class WebSocketsClient : IWebSocketsClient
    {
        private readonly IHubContext<RealtimeHub> _hubcontext;
        private readonly ICancellationTokensRepository _cancellationTokens;

        public WebSocketsClient(
            IHubContext<RealtimeHub> hubcontext,
            ICancellationTokensRepository cancellationTokens
            )
        {
            _hubcontext = hubcontext;
            _cancellationTokens = cancellationTokens;
        }

        public Task OfferCommentAdded(OfferCommentReadModel model)
        {
            return _hubcontext.Clients.All.SendAsync(nameof(OfferCommentAdded), model, _cancellationTokens.GetDefault());
        }

        public Task OfferCommentAdded(string userId, OfferCommentReadModel model)
        {
            return _hubcontext.Clients.User(userId).SendAsync(nameof(OfferCommentAdded), model, _cancellationTokens.GetDefault());
        }

        public Task OrderStatusChanged(OrderState state)
        {
            return _hubcontext.Clients.All.SendAsync(nameof(OrderStatusChanged), state, _cancellationTokens.GetDefault());
        }

        public Task OrderStatusChanged(string userId, OrderState state)
        {
            return _hubcontext.Clients.User(userId).SendAsync(nameof(OrderStatusChanged), state, _cancellationTokens.GetDefault());
        }
    }
}
