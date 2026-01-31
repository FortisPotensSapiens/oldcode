using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record OrderDelivered(BigCount OrderNumber, IEnumerable<UserId> UserIds) : NotificatonsMessage { }
}
