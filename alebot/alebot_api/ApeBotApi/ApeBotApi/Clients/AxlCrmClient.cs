using System.Net.Http.Headers;
using ApeBotApi.Extensions;
using Microsoft.OpenApi.Writers;

namespace AleBotApi.Clients
{
    public interface IAmoCrmClient
    {
        Task<AmoCreatedContactList> CreateContact(string name, string phone, string email, string comment);
        Task<AmoCreatedLeadResponseDto> CreateLead(int contactId, string name);
    }

    public class AmoCrmClient : IAmoCrmClient
    {
        private readonly HttpClient _client;
        public AmoCrmClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<AmoCreatedContactList> CreateContact(string name, string phone, string email, string comment)
        {
            return await _client.PostAsync<AmoCreatedContactList, AmoCreateContactDto[]>(new Uri("/api/v4/contacts", UriKind.Relative), new[] { new AmoCreateContactDto { Name = name,
                custom_fields_values = new[]{
                    new { field_id = 664413, values = new[] { new { value = phone } }},
                 new { field_id = 664415, values = new[] { new { value = email } }},
                 new { field_id = 820255, values = new[] { new { value = comment } }},
                }
            }
            });
        }

        public async Task<AmoCreatedLeadResponseDto> CreateLead(int contactId, string name)
        {
            return await _client.PostAsync<AmoCreatedLeadResponseDto, AmoCreateLeadDto[]>(new Uri("/api/v4/leads", UriKind.Relative), new[] { new AmoCreateLeadDto
            {
                Name = name,
                _embedded = new { contacts = new[] { new { id = contactId } } }
            } });
        }

    }

    public class AmoCreatedLeadResponseDto
    {
        public dynamic _links { get; set; }
    }

    public class AmoCreateLeadDto
    {
        public string Name { get; set; }
        public dynamic _embedded { get; set; }
    }

    public class AmoCreateContactDto
    {
        public string Name { get; set; }
        public dynamic[] custom_fields_values { get; set; }
    }
    public class AmoCreatedContact
    {
        public int id { get; set; }
    }

    public class AmoCreatedContactList
    {
        public AmoEmbedded _embedded { get; set; }
    }

    public class AmoEmbedded
    {
        public List<AmoCreatedContact> contacts { get; set; }
    }

    /// <summary>
    /// Интегрвация с https://admin.accelonline.io/swagger
    /// </summary>
    public interface IAxlCrmClient
    {
        Task<string?> CreateLead(LeadCreateBinding binding);
        Task<LeadReadBinding?> GetLeadById(string id);
        Task<LeadReadBinding[]> GetLeadByEmail(string email);
        Task UpdateLead(LeadUpdateBinding binding);
        Task DeleteLeadById(string id);
        Task SetLeadCustomAttributes(
            string leadId,
            string lastUrtSource,
            string lastUtmMedium,
            string lastUtmCampaign,
            string lastUtmTerm,
            string lastUtmContent,
            LeadCreateBinding cleateData
            );
    }

    /// <summary>
    /// Интегрвация с https://admin.accelonline.io/swagger
    /// </summary>
    public class AxlCrmClient : IAxlCrmClient
    {
        private readonly HttpClient _client;
        private const string _fields = "{leadAttributeValues{valueString},id,email,firstName,middleName,lastName,phone,birthday,gender,needChangePassword,registrationDate,lastActivityDate,isEmailConfirmed,isSchoolEmailConfirmed,paymentsAmount, enterUTMSource,enterUTMMedium,enterUTMCampaign,enterUTMTerm,enterUTMContent,enterUrlParametersJson}";
        public AxlCrmClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string?> CreateLead(LeadCreateBinding binding)
        {
            var response = await _client.PostAsync<AxlResponse<string>, LeadCreateBinding>(new Uri("/api/v1/crm/lead", UriKind.Relative), binding);
            if (response?.success != true)
                new { response, binding }.ThrowApplicationException("Ошибка при запросе к AXL на создание лида");
            return response?.body;
        }

        public async Task<LeadReadBinding?> GetLeadById(string id)
        {
            var response = await _client.GetJsonAsync<AxlResponse<LeadReadBinding>>(new Uri($"/api/v1/crm/lead/{id}?fields={_fields}", UriKind.Relative));
            if (response?.success != true)
                new { response, id }.ThrowApplicationException("Ошибка при запросе к AXL на получение лида по ID лида");
            return response?.body;
        }

        public async Task<LeadReadBinding[]> GetLeadByEmail(string email)
        {
            var response = await _client.GetJsonAsync<AxlResponse<LeadReadListBindgin>>(new Uri($"/api/v1/crm/lead?email={email}&fields={_fields}", UriKind.Relative));
            if (response?.success != true)
                new { response, email }.ThrowApplicationException("Ошибка при запросе к AXL на получение лида по email   лида");
            return response?.body?.items?.ToArray() ?? new LeadReadBinding[0];
        }

        public async Task UpdateLead(LeadUpdateBinding binding)
        {
            var response = await _client.PutAsync<AxlResponse<string>, LeadUpdateBinding>(new Uri("/api/v1/crm/lead", UriKind.Relative), binding);
            if (response?.success != true)
                new { response, binding }.ThrowApplicationException("Ошибка при запросе к AXL на создание лида");
        }

        public async Task DeleteLeadById(string id)
        {
            var response = await _client.DeleteAsync<AxlResponse<string>>(new Uri($"/api/v1/crm/lead/{id}", UriKind.Relative));
            if (response?.success != true)
                new { response, id }.ThrowApplicationException("Ошибка при запросе к AXL на удаление лида");
        }

        public async Task SetLeadCustomAttributes(
            string leadId,
            string lastUrtSource,
            string lastUtmMedium,
            string lastUtmCampaign,
            string lastUtmTerm,
            string lastUtmContent,
                LeadCreateBinding data
            )
        {
            var uri = "/api/v1/crm/lead/profile";
            var binding = new AxlSetLeadCustomAttributes
            {
                id = leadId,
                attributes = new List<AxlAttributeUpdate>
                {
                    new AxlAttributeUpdate
                    {
                        id = "FZxx4Y5G7U6W-igBrM26GA",
                        value = lastUrtSource,
                    },
                      new AxlAttributeUpdate
                    {
                        id = "7SjbmIOTY0S9PAFvMuHRqg",
                        value = lastUtmMedium,
                    },
                        new AxlAttributeUpdate
                    {
                        id = "ce7k3P6sGEmpRzuNC7pBzQ",
                        value = lastUtmCampaign,
                    },
                          new AxlAttributeUpdate
                    {
                        id = "T6nhflTBdk6p0r1bYPjSrw",
                        value = lastUtmTerm,
                    },
                            new AxlAttributeUpdate
                    {
                        id = "a-xc8qRWKkCKWMOcv-BzUw",
                        value = lastUtmContent,
                    }
                },
                emails = new[] { new { isPrimary = true, value = data.email } },
                firstName = data.firstName,
                phones = new[] { new { isPrimary = true, value = data.phone } },
                comment = data.comment,
            };
            var response = await _client.PutAsync<AxlResponse<string>, AxlSetLeadCustomAttributes>(new Uri(uri, UriKind.Relative), binding);
            if (response?.success != true)
                new { response, binding }.ThrowApplicationException("Ошибка при запросе к AXL на удаление лида");
        }
    }

    public class AxlSetLeadCustomAttributes
    {
        public string id { get; set; }
        public List<AxlAttributeUpdate> attributes { get; set; } = new();
        public string comment { get; set; }
        public string firstName { get; set; }
        public dynamic phones { get; set; }
        public dynamic emails { get; set; }

    }

    public class AxlAttributeUpdate
    {
        public string id { get; set; }
        public string value { get; set; }
    }

    public class LeadCreateBinding
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string phone { get; set; }
        public string comment { get; set; }
    }

    public class LeadUpdateBinding
    {
        public string id { get; set; }
        public string? email { get; set; }
        public string? firstName { get; set; }
        public string? phone { get; set; }
        public string? comment { get; set; }
        public string? exitUTMSource { get; set; }
        public string? exitUTMMedium { get; set; }
        public string? exitUTMCampaign { get; set; }
        public string? exitUTMTerm { get; set; }
        public string? exitUTMContent { get; set; }
        public string? enterUTMSource { get; set; }
        public string? enterUTMMedium { get; set; }
        public string? enterUTMCampaign { get; set; }
        public string? enterUTMTerm { get; set; }
        public string? enterUTMContent { get; set; }
    }

    public sealed class AxlResponse<T>
    {
        public bool success { get; set; }
        public List<string> errors { get; set; } = new List<string>();
        public T body { get; set; }
        public bool resetToken { get; set; }
    }

    public sealed class LeadReadListBindgin
    {
        public List<LeadReadBinding> items { get; set; } = new();
    }
    public sealed class LeadReadBinding
    {
        public string id { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public bool needChangePassword { get; set; }
        public string registrationDate { get; set; }
        public string lastActivityDate { get; set; }
        public bool isEmailConfirmed { get; set; }
        public bool isSchoolEmailConfirmed { get; set; }
        public decimal paymentsAmount { get; set; }
        public string enterUTMSource { get; set; }
        public string enterUTMMedium { get; set; }
        public string enterUTMCampaign { get; set; }
        public string enterUTMTerm { get; set; }
        public string enterUTMContent { get; set; }
        public string enterUrlParametersJson { get; set; }
    }

    //{"success":true,"errors":[],"body":"5FIhsOJzCUOCid5FczQzQA","resetToken":false}
    //{"success":true,"errors":[],"body":{"items":[{"leadAttributeValues":[],"id":"5FIhsOJzCUOCid5FczQzQA","email":"string@mail.ru","firstName":"string","middleName":null,"lastName":null,"phone":"+79104271877","birthday":null,"gender":null,"needChangePassword":false,"registrationDate":null,"lastActivityDate":null,"isEmailConfirmed":false,"isSchoolEmailConfirmed":false,"paymentsAmount":0.0,"enterUTMSource":null,"enterUTMMedium":null,"enterUTMCampaign":null,"enterUTMTerm":null,"enterUTMContent":null,"enterUrlParametersJson":null}],"filter":{"id":null,"excludeId":null,"excludeIds":null,"email":"string@mail.ru","primaryEmail":null,"phone":null,"primaryPhone":null,"emails":null,"primaryEmails":null,"phones":null,"primaryPhones":null,"emailOrPhone":null,"hasPrimaryEmailOrPhone":null,"firstName":null,"lastName":null,"concurrencyStamp":null,"resetPasswordToken":null,"emailConfirmationToken":null,"adminId":null,"chatUserId":null,"similarChatUserId":null,"moderateForumThreadId":null,"broadcastListId":null,"responsibleAdminId":null,"partnerId":null,"excludePartnerId":null,"emailBroadcastUnsubscribed":null,"hasLastActivity":null,"hasPhoneE164":null,"hasCountry":null,"hasCity":null,"hasTimeZone":null,"hasPurchase":null,"isTemporary":null,"hasEmail":null,"hasChatUserId":null,"isRegistered":null,"emailConfirmed":null,"schoolEmailConfirmed":null,"emailOrSchoolEmailConfirmed":null,"emailSuppressed":null,"merging":null,"hasPartnerId":null,"partnerExpired":null,"banned":null,"hasAdminId":null,"hasEnterUtm":null,"hasExitUtm":null,"hasUtmTag":null,"schools":null,"partnerIds":null,"referralIds":null,"createdDateFrom":null,"createdDateTo":null,"registrationDateFrom":null,"registrationDateTo":null,"lastActivityDateFrom":null,"lastActivityDateTo":null,"bonusBalanceMoreOrEqualThan":null,"extendedFilter":null,"extendedFilterId":null,"itemsTotal":0,"take":25,"skip":0,"search":null,"sortName":"CreatedDate","sortType":"Desc","useSort":true,"useBaseFilter":true,"useItemsTotal":false,"softDeleted":false,"ids":null}},"resetToken":false}
    //{"success":true,"errors":[],"body":{"leadAttributeValues":[],"id":"5FIhsOJzCUOCid5FczQzQA","email":"string@mail.ru","firstName":"string","middleName":null,"lastName":null,"phone":"+79104271877","birthday":null,"gender":null,"needChangePassword":false,"registrationDate":null,"lastActivityDate":null,"isEmailConfirmed":false,"isSchoolEmailConfirmed":false,"paymentsAmount":0.0,"enterUTMSource":null,"enterUTMMedium":null,"enterUTMCampaign":null,"enterUTMTerm":null,"enterUTMContent":null,"enterUrlParametersJson":null},"resetToken":false}
    // On put {"success":true,"errors":[],"body":null,"resetToken":false}
    // On delete {"success":true,"errors":[],"body":null,"resetToken":false}

}
