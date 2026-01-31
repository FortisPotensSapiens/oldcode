using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.Domain
{
    public record OrderPaidEmailMessage(BigCount OrderNumber, EmailVo StoreEmail, IEnumerable<(ShortText MerchTitle, Count MerchCount)> Items) : EmailMessage;
}
