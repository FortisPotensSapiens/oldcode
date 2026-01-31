using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record OrderPaidMessage(BigCount OrderNumber, IEnumerable<UserId> UserIds, EmailVo StoreEmail, IEnumerable<(ShortText MerchTitle, Count MerchCount)> Items) : NotificatonsMessage { }
}
