using AleBotApi.Clients;
using ApeBotApi.DbDtos;
using ApeBotApi.Extensions;

namespace AleBotApi.Services
{
    public interface ICrmService
    {
        Task<string?> Create(AbUserDbDto user);
        Task Update(AbUserDbDto user);
    }

    public class CrmService : ICrmService
    {
        private readonly IAxlCrmClient _axl;
        private readonly ILogger<CrmService> _logger;
        private readonly IAmoCrmClient _amo;

        public CrmService(IAxlCrmClient axl, ILogger<CrmService> logger, IAmoCrmClient amo)
        {
            _axl = axl;
            _logger = logger;
            _amo = amo;
        }

        public async Task<string?> Create(AbUserDbDto user)
        {
            var r = await _amo.CreateContact(user?.FullName ?? "Не указано", user?.PhoneNumber ?? "Не указано", user?.Email ?? "Не указано", user?.RegistrationQueryParams ?? "Не указано");
            var id = r?._embedded?.contacts?.FirstOrDefault()?.id;
            if (!id.HasValue)
                new { user }.ThrowApplicationException("Не удалось создать контакт с Амо для пользователя!");
            await _amo.CreateLead(id.Value, (user?.FullName ?? user?.Email) ?? "Не указано");
            return id.Value.ToString();
            //var binding = new LeadCreateBinding
            //{
            //    email = user?.Email,
            //    firstName = user.FullName,
            //    phone = user?.PhoneNumber,
            //    comment = user?.RegistrationQueryParams
            //};
            //var id = await _axl.CreateLead(binding);
            //user.ExternalId = id;
            //if (string.IsNullOrWhiteSpace(user?.RegistrationQueryParams))
            //    return id;
            //try
            //{

            //    var utms = user.RegistrationQueryParams.Split("; ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            //        .Select(x => x.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            //        .Where(x => x.Length > 1)
            //        .DistinctBy(x => x[0])
            //        .ToDictionary(x => x[0], x => x[1]);
            //    await _axl.SetLeadCustomAttributes(id,
            //        utms.GetValueOrDefault("utm_source"),
            //        utms.GetValueOrDefault("utm_medium"),
            //        utms.GetValueOrDefault("utm_campaign"),
            //        utms.GetValueOrDefault("utm_term"),
            //        utms.GetValueOrDefault("utm_content"),
            //        binding
            //        );
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Ошбика при отправки доп атрибутов в AXL");
            //}
            //return id;
        }

        public async Task Update(AbUserDbDto user)
        {
            //var update = new LeadUpdateBinding
            //{
            //    id = user?.ExternalId,
            //    email = user?.Email,
            //    firstName = user?.FullName,
            //    phone = user?.PhoneNumber,
            //};
            //await _axl.UpdateLead(update);
        }
    }
}
