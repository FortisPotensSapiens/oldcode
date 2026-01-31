namespace Daf.MessagingModule.Domain
{
    public record SendPhoneVerificationSmsMessage(string Phone) : SmsMessage;
}
