using System.IO;
using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Clients;
using Hike.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hike.Controllers
{
    [Route("api/v1/messaging")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IEmailClient _emailSender;
        private readonly ITwilioClient _twilio;

        public MessagingController(
            IEmailClient emailSender,
            ITwilioClient twilio
            )
        {
            _emailSender = emailSender;
            _twilio = twilio;
        }

        [HttpPost("send-feedback")]
        public async Task SendFeedback([FromForm] SendFeedbackModel model)
        {
            await _emailSender.SendEmailAsync(
             "info@dishfork.ru",
                "Форма обратной связи",
                $"<div>{model.Name}<br/> {model.Email} </br> {model.Message} ",
                model.Files.Select(x =>
                {
                    var ms = new MemoryStream();
                    x.CopyTo(ms);
                    return (x.Name, x.ContentType, ms.ToArray());
                }).ToList()
            );
        }

        [HttpPost("send-from-lending")]
        public async Task SendCookMessage(LendingMessageModel model)
        {
            await _emailSender.SendEmailAsync(
               "info@dishfork.ru",
                model.Type == LendingMessageType.Client ? "Заявка от клиента" : "Завявка от повара",
                $"<div>{model.Name}<br/> {model.Email} </br> {model.Message} "
            );
        }

        [Authorize]
        [HttpPost("send-verification-code")]
        [ProducesResponseType(200, Type = typeof(VerivicationStatus))]
        public async Task<VerivicationStatus> SendVerificationSms(SendVerificationSmsModel model)
        {
            return await _twilio.SendVerificationToken(model.Phone);
        }

        [Authorize]
        [HttpPost("check-verification-code")]
        [ProducesResponseType(200, Type = typeof(VerivicationStatus))]
        public async Task<bool> CheckVerificationCode(CheckVerificationCodeModel model)
        {
            var status = await _twilio.CheckVerificationCode(model.Phone, model.Code);
            return status == VerivicationStatus.Approved;
        }

        public class SendVerificationSmsModel
        {
            [Required]
            [PhoneNumberValidation]
            public string Phone { get; set; }
        }

        public class CheckVerificationCodeModel : SendVerificationSmsModel
        {
            [Required]
            public string Code { get; set; }
        }

        public class SendFeedbackModel
        {
            public string Name { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; }
            public string Message { get; set; }
            public List<IFormFile> Files { get; set; } = new();
        }

        public class LendingMessageModel
        {
            public LendingMessageType Type { get; set; }
            public string Name { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; }
            public string Message { get; set; }
            public List<IFormFile> Files { get; set; }
        }

        public enum LendingMessageType
        {
            Client = 10,
            Cook
        }
    }
}
