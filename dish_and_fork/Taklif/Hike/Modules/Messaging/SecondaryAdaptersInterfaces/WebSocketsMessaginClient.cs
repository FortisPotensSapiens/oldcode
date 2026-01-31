using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.MessagingModule.SecondaryAdaptersInterfaces;
using Daf.SharedModule.Domain;
using Hike.Clients;
using Hike.Entities;
using Hike.Models;

namespace Infrastructure.SecondaryAdapters.Clients
{
    public class WebSocketsMessaginClient : IWebSocketsMessaginClient
    {
        private readonly IWebSocketsClient _webSockets;

        public WebSocketsMessaginClient(IWebSocketsClient webSockets)
        {
            _webSockets = webSockets;
        }

        public Task SendWebSocketMessage(UserId id, WebSocketMessage message)
        {
            return message switch
            {
                OfferCommentAddedWebSocketMessage oca => OfferCommentAddedWebSocketMessage(id, oca),
                OrderDeliveredWebSocketMessage od => OrderDeliveredWebSocketMessage(id, od),
                OrderPaidWebSocketMessage op => OrderPaidWebSocketMessage(id, op),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task OrderPaidWebSocketMessage(UserId id, OrderPaidWebSocketMessage message)
        {
            await _webSockets.OrderStatusChanged(id, OrderState.Paid);
        }

        public async Task OrderDeliveredWebSocketMessage(UserId id, OrderDeliveredWebSocketMessage message)
        {
            await _webSockets.OrderStatusChanged(id, OrderState.Delivered);
        }

        public async Task OfferCommentAddedWebSocketMessage(UserId id, OfferCommentAddedWebSocketMessage message)
        {
            await _webSockets.OfferCommentAdded(id, new OfferCommentReadModel(message, id));
        }
    }
}
