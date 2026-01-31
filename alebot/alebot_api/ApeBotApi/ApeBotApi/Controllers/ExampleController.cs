using System;
using AleBotApi.Clients;
using AleBotApi.Controllers.AdminPanel;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.Migrations;
using AleBotApi.Models;
using ApeBotApi.DbDtos;
using ApeBotApi.Extensions;
using ApeBotApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApeBotApi.Controllers
{
    /// <summary>
    /// Тестовый контроллер
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExampleController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ExampleController> _logger;
        private readonly AbDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IAxlCrmClient _client;
        private readonly IAmoCrmClient _amo;

        public ExampleController(
            ILogger<ExampleController> logger,
            AbDbContext db,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            IAxlCrmClient client,
            IAmoCrmClient amo
            )
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
            _environment = environment;
            _client = client;
            _amo = amo;
        }

#if DEBUG
        [HttpGet("all-users")]
        public IEnumerable<AbUserDbDto> GetAllUsers()
        {
            return _db.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .Include(x => x.Claims)
                .Include(x => x.Tokens)
                .Include(x => x.Logins)
                .Include(x => x.UserServers)
                .Include(x => x.UserLicense)
                .ToList();
        }

        [HttpPost("lead")]
        public async Task<string> CreateLead(LeadCreateBinding binding)
        {
            return await _client.CreateLead(binding);
        }

        [HttpGet("lead/{id}")]
        public async Task<LeadReadBinding> GetLeadById(string id)
        {
            return await _client.GetLeadById(id);
        }

        [HttpGet("lead")]
        public async Task<LeadReadBinding[]> GetLeadByEmail(string email)
        {
            return await _client.GetLeadByEmail(email);
        }

        [HttpPut("lead")]
        public async Task UpdateLead(LeadUpdateBinding binding)
        {
            await _client.UpdateLead(binding);
        }

        [HttpDelete("lead")]
        public async Task DeleteLeadById(string id)
        {
            await _client.DeleteLeadById(id);
        }

        [HttpPost("amo/contact")]
        public async Task<AmoCreatedContactList> AmoCreateContact([FromQuery] string name, [FromQuery] string email, [FromQuery] string phone, [FromQuery] string comment)
        {
            return await _amo.CreateContact(name, phone, email, comment);
        }

        [HttpPost("amo/lead")]
        public async Task<AmoCreatedLeadResponseDto> AmoCreateLead([FromQuery] string name, [FromQuery] int id)
        {
            return await _amo.CreateLead(id, name);
        }
#endif
        [Authorize(Roles = "admin")]
        [HttpPut("fill-db-with-data")]
        public async Task<IActionResult> Fill()
        {
#if !DEBUG
            await _db.Database.MigrateAsync();
            using var tran = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
#endif

            var currency = await _db.Currencies.FirstAsync(x => x.Code == "USDT");
            var paymentNetwork = await _db.PaymentNetworks.FirstAsync(x => x.Name == "TRC20");
            //var users = await InitUsers();

            //await InitAccounts(currency, users);

            //var merchs = await InitMerch(currency, paymentNetwork);
            //var licenses = await InitLicenses();
            //var servers = await InitServers();
            //var courses = await InitCourses();
            var serverExtentions = await InitServerExtentions();
            var serverOnlyMerch = await InitServerOnlyMerch(currency, paymentNetwork);
            var extentionOnlyMerch = await InitExtentionOnlyMerch(currency, paymentNetwork);
            await AdMerchServers(serverOnlyMerch);
            await AddMerchExtentions(serverExtentions, extentionOnlyMerch);
            //1-иконка.png

            //bool itemAdded = false;
            //foreach (var item in licenses)
            //    if (!await _db.Merches.AnyAsync(x => x.Name == item.Name))
            //    {
            //        _db.Licenses.Add(item);
            //        itemAdded = true;
            //    }
            //if (itemAdded)
            //    await _db.SaveChangesAsync();
            //await InitLessons(courses);

            //await InitMerchItems(merchs, licenses, servers);

            //await InitUserCourses(users, courses);

            //foreach (var u in users)
            //{
            //    if (u.Email == "callidussapiens@gmail.com")
            //    {
            //        var license = licenses.First(l => l.Name == "Invest");
            //        _db.UserLicenses.Add(new AbUserLicenseDbDto
            //        {
            //            LicenseId = license.Id,
            //            UserId = u.Id,
            //            ActivationKey = "40",
            //            TradingAccount = "1235709551"
            //        });
            //        var server = servers.First(x => x.Name == "Invest 3 месяца");
            //        _db.UserServers.Add(new AbUserServerDbDto
            //        {
            //            ServerId = server.Id,
            //            UserId = u.Id,
            //            Login = "Trader",
            //            Password = "htpjv9qe",
            //            Address = "136.243.60.122:41010"
            //        });
            //        await _db.SaveChangesAsync();
            //    }
            //}
#if !DEBUG

            await tran.CommitAsync();
#endif
            return Ok();
        }

        private async Task AddMerchExtentions(List<AbServerExtentionDto> serverExtentions, AbMerchDbDto[] extentionOnlyMerch)
        {
            var eom = extentionOnlyMerch.Zip(serverExtentions, (a, b) => new AbMerchServerExtentionsDbDto
            {
                MerchId = a.Id,
                ServerExtentionId = b.Id,
                Qty = 1
            }).ToList();
            bool itemAdded = false;
            foreach (var item in eom)
                if (!await _db.MerchServerExtentions.AnyAsync(x => x.MerchId == item.MerchId && x.ServerExtentionId == item.ServerExtentionId))
                {
                    _db.MerchServerExtentions.Add(item);
                    itemAdded = true;
                }
            if (itemAdded)
                await _db.SaveChangesAsync();
        }

        private async Task AdMerchServers(AbMerchDbDto[] serverOnlyMerch)
        {
            var servers = await _db.Servers.AsNoTracking().ToListAsync();
            var som = serverOnlyMerch.Zip(servers, (a, b) => new AbMerchServerDbDto
            {
                MerchId = a.Id,
                ServerId = b.Id,
                Qty = 1
            }).ToList();
            bool itemAdded = false;
            foreach (var item in som)
                if (!await _db.MerchServers.AnyAsync(x => x.MerchId == item.MerchId && x.ServerId == item.ServerId))
                {
                    _db.MerchServers.Add(item);
                    itemAdded = true;
                }
            if (itemAdded)
                await _db.SaveChangesAsync();
        }

        private async Task InitUserCourses(List<AbUserDbDto> users, List<AbCourseDbDto> courses)
        {
            foreach (var u in users)
                foreach (var c in courses)
                {
                    if (!await _db.UserCourses.AnyAsync(x => x.UserId == u.Id && x.CourseId == c.Id))
                    {
                        _db.UserCourses.Add(new AbUserCourseDbDto
                        {
                            UserId = u.Id,
                            CourseId = c.Id
                        });
                        await _db.SaveChangesAsync();
                    }
                }
        }

        private async Task InitMerchItems(AbMerchDbDto[] merchs, List<AbLicenseDbDto> licenses, List<AbServerDbDto> servers)
        {
            bool merchAdded = false;
            var i = merchs.Zip(licenses, servers);
            foreach (var item in i)
            {
                if (!await _db.MerchLicenses.AnyAsync(x => x.LicenseId == item.Second.Id && x.MerchId == item.First.Id))
                {
                    _db.MerchLicenses.Add(new AbMerchLicenseDbDto
                    {
                        LicenseId = item.Second.Id,
                        MerchId = item.First.Id
                    });
                    merchAdded = true;
                }

                if (!await _db.MerchServers.AnyAsync(x => x.ServerId == item.Third.Id && x.MerchId == item.First.Id))
                {
                    _db.MerchServers.Add(new AbMerchServerDbDto
                    {
                        ServerId = item.Third.Id,
                        MerchId = item.First.Id
                    });
                    merchAdded = true;
                }
            }
            if (merchAdded)
                await _db.SaveChangesAsync();
        }

        private async Task InitLessons(List<AbCourseDbDto> courses)
        {
            var lessons = new[] { new AbLessonDbDto
                      {
                          Id = Guid.NewGuid(),
                          Name = "Подключиться к VDS серверу и настроить торговый терминал на использование вашего счета",
                          CourseId = courses[1].Id,
                          Number = 4,
                          Body = @" 
Запускаем приложение “Подключение к удаленному рабочему столу” на Windows  
![](https://lh7-us.googleusercontent.com/HeslYifqYfphxqTS8Sph2_cs1qiBn4zNVc7Qa6gdrOcgKhAFYQrkIHbCa3_tfzCoiStZVYrvVIDUUgTA2Fe0oSMQfwjFNpAPyrQ5mRom6tyGebWn5MzHLXipS78Yn53HIApz8BSn5uzxdszqE2f49-g)  
В поле компьютер вводим адрес сервера из раздела “Сервер VDS” и когда спросят имя пользователя и пароль то вводим их оттуда же.  
![](https://lh7-us.googleusercontent.com/x32HLZmys4N2WV6reeQbChrHrO-Z8Oh7xV5L_ObGXjrEuCGb6QNmG4N2UxnPClHo5aN14JhAjQ77iSIlB9D1bZkv2mJTUydGvgThXWXZov6IZTWNBjtjQoea8Ce_OlpRC7eMQ8-201IFg9c-HWFLLYc)  
Если ваш счет еще не подключен то кликаем на счета правой кнопкой мыши и выбираем “Подключиться к торговому счету”.  
![](https://lh7-us.googleusercontent.com/Q2nIytqSKX5J0rGGlHSm8d7hQoXK6TO-Ctu_BhLfBTSFAL4nchHDtklZRXRLxQbE25NLFxJWjnHxGF8sbxUu7EubK1jwxDRbiBiX_4MQiRW24Z3CE98raW5FvYAnaSALr69q9EJsQXQPtoDrEdWhbSo)  
Данные для подключения вам должны были прийти на почту при создании счета в Forex4You  
![](https://lh7-us.googleusercontent.com/qA29v0kt0lNR9TZpPCwJuUG3XB2RMievDDvq-yY8OrGG1lvw45nO2Ty0ZstmMjU4-iFrZLt_dDiVM8mahD8lOsTbCjM8dpJC8xoG6IX7TuSqoRZFqGAI6Ppg6M1T7xKBbzahc0YJqHgI80AdHYy6cGI)  
Логин вводим из Login на почте. Север из Server Name а Пароль из Password  
![](https://lh7-us.googleusercontent.com/eomIVk2ZYwG1j3uq4qevgGqjdSYkGUpg875bEb3GhwH1AVEwztYBuadXzOimllGqZpPCRoylsksYgE3LZyQ7lvG0NOfnnAtwzzameXOwCrFRU4p5w03yM4ajfucW-x-2gE3qsGsWjvv5yOWr-dGuwc0)  
Далее двойным кликом на Ale Bot в списке советники запускаем его на нашем трафике. Он покажет окно с настройками где надо нажать ОК.  
![](https://lh7-us.googleusercontent.com/-e3WnCcCr2BCc4U0XoM4r48CknVfdadIvoHZ5wvZY1-TRdaHqUREz6bwub8zWk3pB8T9S1UU_frwk2MkeantxO2yTbUS4T2NLMZo1wqt0gLxooZSZKYHyw-MpnfF915Vy12OIZdLdnwHWmQnr12fcUE)  
"
                      },
            new AbLessonDbDto
                      {
                          Id = Guid.NewGuid(),
                          Name = "Зарегестрировать брокерский счет",
                          CourseId = courses[1].Id,
                          Number = 1,
                          Body = @"
Перейдите по ссылке [https://account6.forex4you.ru.com/ru/user-registration/?affid=ntf9wtk](https://account6.forex4you.ru.com/ru/user-registration/?affid=ntf9wtk)  
И заполните форму вашими данными. Поле реферальный код обязательно должно быть заполнено значением [ntf9wtk](https://account6.forex4you.ru.com/ru/user-registration/?affid=ntf9wtk)  
![](https://lh7-us.googleusercontent.com/l1rUxFt_ZX-5YWNFDNkELvH5xgeGKMz2SYtVitwMPOiUp9he0EUxc9Zafl2iNq9Gkk3ARTwr2jpGrFeUviOeAtoyXXikCkl313Umff384YNfoneD3dhF3bGm7DoQkXG9vzDfi2XxLiu5ykeGYGmpdXU) 
Введи код который вам пришлют в SMS  
![](https://lh7-us.googleusercontent.com/iS_8Wc5u9CABcEggAOWf5AznNTJ_61NK-3TWNJ28X-H9Col0-pn-j6lH1vkSarNERCrOnK1C2drz-TGmTNZiw6D_bHlebDqzkeVT37xtLw8B5kKbWOG0wVyPc9J-CdDmUDPskkxLfHk7Eisq8yxFyUA)  
На почту вам придет письмо где будет номер торгового счета и он будет называться Login  
![](https://lh7-us.googleusercontent.com/qA29v0kt0lNR9TZpPCwJuUG3XB2RMievDDvq-yY8OrGG1lvw45nO2Ty0ZstmMjU4-iFrZLt_dDiVM8mahD8lOsTbCjM8dpJC8xoG6IX7TuSqoRZFqGAI6Ppg6M1T7xKBbzahc0YJqHgI80AdHYy6cGI)  
Или у себя на сайте в разделе Dashboard  
![](https://lh7-us.googleusercontent.com/2gwlxAPKAavl4NPJO3_j6HSoHtrfvScG_XUbC7VZhORS5wiY3Zq1Lt-aW2f1JjTABuSTHSmA9JBlDydY3j_ZVILYoT70cwHgBL7PK0DDa7x1f7VtzPKZ3jXAZpzbZLutXbXxOm0lSrY9qBuQY70V3Dg)  

"
                      },
              new AbLessonDbDto
                      {
                          Id = Guid.NewGuid(),
                          Name = "Обмен обычных денег на крипту",
                          CourseId = courses[0].Id,
                          Number = 1,
                          Body = @"

Переходим по ссылке [https://www.bybit.com/en/register](https://www.bybit.com/en/register) и заполняем форму регистрации  
![](https://lh7-us.googleusercontent.com/fxE3M27tunx6nZrRQVoRUxkyMOWJQbhfNIVU7AjEpcSpGZWe-qdX3Ra3FKVhc4gdWMITMPAbnHHGypr1tDsjpxbnTLuG4jdk6TqP-g49i9BcFrVA_dQzHhxBYQ9639PGBfHX28EHgXxbtZTWrMO2J8w)  
На почту или номер телефона пришлют код  
![](https://lh7-us.googleusercontent.com/--LlsJLxnVjRrbMwJq7puWz4C8Vd30QChTs3IX8QCm0ZCCNrpjMAN-wer0AYRhKdf_lcfEPuakp6urosX5sqJ9Im5td8X2nsMsM9Hozs3xjKKK8sexKtnd4xruoln6zYDvcWp_Ckczf5vdVoSlAIUwk)  
Который нужно ввести форму для вводу кода.  
![](https://lh7-us.googleusercontent.com/LPBCHF1JN9w3OvxZQrmqfidTGA43AgX_jo4I6oSZct1UDf20SvtAobp8B2otb3uerYqzWg9WmJepoYyriqF0dUWSyP2_wGk4bpo-vJlg70xODxDLnZB1YxYXcDexsC-LI0XeFzJRyOmWfEoTab6Z8qk)  
Нажмите кнопку Buy  
![](https://lh7-us.googleusercontent.com/UYuZJccOI99XKPKZAKzkTtkBHQrvkMLlCKMkZsrSw3E52nFlYGB5GAmeJQoy8Df2xEbvrlwAeyzmxiegat6s6Hyh_s6FuLm0y9RZys2I5wQlq46z4zYTzA7FwHhWc9jYU6_8Id3KaOrMB2ePxKO_cWI)  
Выбираете что хотите потратить рубли и получить крипту  
![](https://lh7-us.googleusercontent.com/eUb-otcJ5E81_8o8CUPLYrU3aHWEFW97ZKswNvbeH9dqo_55Y8McRAeYN4CP4DByPAkxmcoeleG7rBj9S6VCZJ3pa3Hknf_ydTmDOWVpEIbXvKvGaDZc--GGkXw96NGPMZ6PkJP2ZqZ5PbVC16i5NY8)  
Если вы еще не верифицировали свой аккаунт то вас попросят заполнить ваш профиль  
![](https://lh7-us.googleusercontent.com/w8xFM7G5EKyWnda7mfJ7fMMS1uxoARw5V1P8KDq2MrcmyDQoKcJp4BoEZSwenFNQk8_gtqlPEg8Z49fwQ8DWEwHPUZOOWFQDjgE_KudibQcd7QihX5G1Z-WGt_6m8yaArIdHL9kjRxVYV9mkb1ZACCo)  
И надо выбрать для начала Lv.1  
![](https://lh7-us.googleusercontent.com/Em1UBRhAtQq77ewHEGvQ6WmfIhv8__u0RDYKjwa72-C4nWoaOKEfBQXuaB07lwESiiqbMmXa76fkcBvJv1LloyhsTI7qtocSy4tF7QfPi4miBNHii49mtsz_d4YFebbDGVfQ9zTSodDhDl8Jndmcdkc)  
Далее выбираете как верифицировать свой аккаунт. Обычно через фото паспорта.  
![](https://lh7-us.googleusercontent.com/8vaQwZiqoq6V9oQU0_leN4LSOTJGl-4FUJ30VzmZUgF2cv1m9mViY0hHZYp-goUQwDtenVveLtOG0r1I-b5olDq-_VY6wZ-vaauJw5nu4PqlOGEZ2suOFjJwzyyIxwUs0YU6Kff6sVvN-Ji_FJRLKOA)  
![](https://lh7-us.googleusercontent.com/MRlRRR3soaNBY1NFriJnt454llrf2ELKNyfzuccwcVdBiJIaO9TbHrh7mvOygTscpeFj_gF2qAQ5cX5ctlVA8oueBd3QRgyJmv_xuuJlqn5Lg9JwKZDsd66tVL8JcIvXDcMxAij5yGuvaacxmAJ04iA)   
Далее фото своего паспорта прикрепляете и ждете ответа от сервера.  
![](https://lh7-us.googleusercontent.com/G2prCyiO4VQQsGaKNfMEiuE7h58lHcUG9yer1Po65xJXU_BtRs8RKS0Y7p2YOYZXhDb0HVN0NMLTKQha7GwZ4F3SMjwv6LKHypf7hzZj2ZOlAfFmDbIduo8yNIuKi7PcFZg8gt_6Cu3196RSNLkXm1I)  
Так же то же самое можно через P2P торговлю где вы покупаете крипту у человека напрямую.  
![](https://lh7-us.googleusercontent.com/f76vVbaU38YIP5kIMYCY_SXIXjWInc81ztEnpqZR2XhmeQLiSFOaG8BJyS30JhIoN0FjexUIDUWDAw4NWIRjLjfNaUtmAB5bgtfnOFNvCk0HrMkltLFLQUF5W3pPyR3X8XQ9FYyuz8A_GoD1o7p0Wys)  
"
                      },  new AbLessonDbDto
                      {
                          Id = Guid.NewGuid(),
                          Name = "Верифицировать брокерский счет.",
                          CourseId = courses[1].Id,
                          Number = 2,
                          Body = @"
Нажимаем на наше имя или на предложение верифицироваться и попадаем на страницу настройки профиля где переходим на вкладку Verification  
![](https://lh7-us.googleusercontent.com/dIsOc19hsa7agq3_QYa2HriPdAHOiD9F2lr8L4jffNa3t_n61DbXdbCZNvfX5-pWFopVNaE1zp4PcGtqH2VpbOI5SCYIh9spc2HAMVuoLuxtLx8zB23N8biOaS9dtezSk99Tf0-HnGGgQHBppYfVOvw)  
Заполняем форму своими данными и жмем кнопку UPLOAD DOCUMENTS  
![](https://lh7-us.googleusercontent.com/lxz2kSaEuGymaDm-T22QKM_NeGU5QKRuFaAfVkJGuhTXkgPoouMKCnK9okid3-_uAjDa09WPL2ptmwwsB4FZnBJFsnC1ai5idfqxPXg0IDPE_6_BIQ9TNs62b6vPZbBOxq_Bl1Y-bTB6Sqf53P1_acs)  
Далее первым загружаем фото своего паспорта а вторым фото штампа о регистрации и жмем кнопку внизу. Так же свой адрес проживания можно подтвердить сделав выписку об остатке в Сбербанке или Тинькофф.  
![](https://lh7-us.googleusercontent.com/gOI-IwqHqbQG8kiwHUHs6EK8wuMqQr_Yi6l_ZT0kr9VWa5xhzKRYUVXTdzGQLezW-oaeJLsPAJYrlT9ElLVnXep6n5bIZdnpu51PypGbipaYq0gEv59Ulpp5LBKjyzuHO4RFMRtFkUxvvcWOeyr9afk)  
После этого ожидаем пока на почту придет письмо с информацией о том что ваш аккаунт подтвержден или причиной отказа.  
![](https://lh7-us.googleusercontent.com/kI5cCO7BjpwVrX2taUCXGs3JukE9P5sEGLbLHKohW_NeXaNpgZPAr_TtQvmXXYMgX_EHrcWp6Tmc24FJEFTtjcxrof_t0HyYkihDUpClEIDPCk4q17r5IHQO2AhOzlgDbLIP2tVi0fDWEJUUO6n6BaA)   
"
                      },  new AbLessonDbDto
                      {
                          Id = Guid.NewGuid(),
                          Name = "Пополнить брокерский счет",
                          CourseId = courses[1].Id,
                          Number = 3,
                          Body = @"
 Нажмите кнопку ПОПОЛНИТЬ или просто DEPOSIT если у вас сайт на английском и выберите способ оплаты.  
![](https://lh7-us.googleusercontent.com/3EAX1LHM5VaaQkZB6JwHL81EZ5KE25vLMvo1iYttYZyikGpydLCTsyNFPqnWDPOFq_yrmzrZrudlzGVS8CLwkYVUByanxrp68zk5GrQnxcbgpvhtfWsflMwISHwo4sYxDA6ILKwQAy84GB95MHCl1KY)  
Выбираете способ пополнения и следуете инструкциям от сайта. Укажите сумму не менее минимальной в 100 долларов и выберите свой торговый счет из списка.  
![](https://lh7-us.googleusercontent.com/Ytc9Ja0b88na3_jvQ2Hjf_3zhbIrSecoAQ4W3RPt4TkstVw5IjOfMgSm2G8Od_3E7UXDIHQiTkUHWUJFD8okNod0TVoyPLCHxeKArI0uC_q49aRSScQiy2lgu22IS0HSU4XvEW3C7G2HAJ14idfzLUI)  
Для оплаты криптой выбираете Cryptocurrencies. Ставите галочку что платежные реквизиты действительно ваши.  
![](https://lh7-us.googleusercontent.com/5MTy2jf7j5MtrPp2w7bE6JezYZm51DkrYP0t9qrU-fLlH2nBifjFWn3cm-D4oYdNoh84VgViLspXJQDg0trO8YhGNhtLrPTPiej8_TLUyi7rNyw8m8QXUGcr7YFIPBP0LITI4NRRvjSo26y8Ooy3Mok)  
Жмем пополнить и проверяем что все верно.  
![](https://lh7-us.googleusercontent.com/UPbb6MNSAFqQFW4WJnQ4BeHPb9VaybuR16qPqJ_r3ft88DpmhP24gG3cHtRT_Q3yQ6hMXeQRadxelrQ136qxrqacPCymbjhBqSpcnv77_AlXC363Qsesz_so3_S6j_LOEv5_h_Oiu-ENcncavUfiK-A)  
Если все верно то жмем подтвердить и продолжить. Учтите что Комиссия 0 это комиссия Forex4you. У вашего крипто кошелька с которого вы отправляете крипту может быть своя собственная комиссия.  
![](https://lh7-us.googleusercontent.com/u3khkoCjq2hilt67f6hSv4KBKUZzYVd3rfGMUIsyWiu44UFc15ddH2CECW7_AY__vfVXKqWCmcDoKeX5yPMUMYeV_6cOFxgLQdBw0zw0Dp_EmpBsvSev8mz6pOTOp95DDcFm9H8JjR3i9c_JtudNbwA)  
![](https://lh7-us.googleusercontent.com/ifjfk-wYyBfBm8JZQCy8wowNWPTLco-e-qCrf1k5OeXQ8WfjlMu3njKgEaWkJ4AfNT_RWaLVAvQK5DUgwMFK4RnZhoRwgjcG0QpFHnz6SpS6IxfpcKF2aKp3ZbA2ljaFj4Y2cjd9JmALdhAGo4nFn5w)  
![](https://lh7-us.googleusercontent.com/rd9deZGmstCiCVOBckIy7StF4GwqEHcScPq_4s3gKOFjv6SPjoFPdlNWMcth3jx64OzdrbyKB1buo_00SWTdH3fMjwd51u0lPM52sBvePJc9BqijE9079OdYfnwZEwlA38_VQ2TB1IlqkZyuMjOpanE)  
![](https://lh7-us.googleusercontent.com/lBH4PQmgsQwxxB50nqK2X5TdONJvUwjP5yh4iM1BbsBuQLBgo5ny-tIylmD0xoXryXdWG0d2x9_SNMvnsAVDe_q7aal2g253qlKMxJipTZ3H7NKiP9Og7tGi3FZTuQiLlDqb0bJX8BhkOdctSlZlQIU)  
![](https://lh7-us.googleusercontent.com/PLHyOK-_MFR4UPi7DQTy3bv7gGcjxnxWOYuh5u8sccpiivJT8cT_x8DzzQ59nEGg5qT_k3fBPco07RfQhFSoBZ78aLjs7SAUxBflpKxkckkSlHTtz7u1kh9AT9YdaYZfXJCnOMB4IUStutW-dhiVZXs)  
Заходим на биржу Bybit в ваш аккаунт. Переходим в раздел Assets. Если предложат перейти на универсальный торговый аккаунт то со всем соглашаемся. Он более удобный.  
![](https://lh7-us.googleusercontent.com/NR67yRJqDT64apBiujZKhEJTS3sSSE3Gzr3IyzAtnIUCRQfOqcsqFhULFq84qIgmZaV697iuSv6FNog2eefIjrh30MNwKVt-G_AU5gIhSEfBW4-UGeMIWCPEre38JlVuEO7n6ugxes2ToWxzTyJrQ5I)  
![](https://lh7-us.googleusercontent.com/K3xQgaw6dhVMPPP69xKZsIOlkyd42iMSfxKipt6csJdaU4hb2ujBvIapYGiZSp426XG4aBcULjx5gJBqywyvCksYWvQ6DQjkPmHS1CZDBdF_CYPogGVgZzuxFEm_ly3qcAj1Pxhl5FowQlOwLkHbANU)  
Везде выбираете первый вариант  
![](https://lh7-us.googleusercontent.com/gIfQe-F8sHPYisrwTYq8Lb7XqL9ItxJ5_OlZAqDUl6V04RVTD0i-qWtfovQr7_qfw4HBtCP4z6nzDzJ-dLa7FFMyJNE1cM-5VfA5QPPdXlKN88dMm_HdMK-g51b_WtW7uG975oUQUs8Mm6KHI2PwUb4)  
![](https://lh7-us.googleusercontent.com/Ce8c8UeucVyQ8pGcOIMn5LF2nMjI4ji3xbicEsh4mrgYOj_tQBOyl91cy2tOj0CkFW19Z-PcixjH4dKn9wcjgP_llubMTIzYsTt6cq7_mY11rdX5wO8Q9eT-FaIdoXl4K6HFYuLOcoVOpnSIvpaPhvg)  
Переводим с торгового счета на счет для финансирования сумму которую нужно оплатить потому что выводить можно со счета Финансирования.  
![](https://lh7-us.googleusercontent.com/--8M7tLu6fIIbmDtHV469kKB3H7nXwWyStF24X8xWV-hu5GRKcT4mAWymsXNsgatrVyXTTp6RhXrOXRrS9dOccPmbRP4kgBTzrZPLgG0ZiuJQ4JjDA7kyw8HDzBsCRuZvyI9cUDlaa7wu7ME_RJRGW4)  
Переводим ту сумму что собирались перевести на Forex4You  
![](https://lh7-us.googleusercontent.com/BmsvwjdVnQFljdcwwoQJZXzXVCM2HHZX2FgzhIhJEyBrbmfnJZF326gN02o3Wf0FAVi7wPvdtTLnCM1wYTKnYzCCsFDXVyeDtVATQVgFEiP0qnaLpEwzpvM0iNXQseeysPp_un-Rc6NpwEQ1-uO9lpg)  
Переходим на вкладку финансирование и жмем кнопку вывести для той валюты в которой вы пополняете Forex4You.  
![](https://lh7-us.googleusercontent.com/oDA5g_DHaVhFBT3-za_xtftM48KAPhsKwfQGbkGiOUD4SlWHt1gaLDBdbLNFAWWvC7FeRZ3eIFkryZMJ2Kbfz-sRqpMrqazK_R66eE2929c7diDYWxtYm0Wy3V702GMJz1HRiALyQ3xHMgkLonm42ME)  
Выбираете Вывод в блокчейне и вставляете адрес кошелька Forex4you и выбираете тип сети что был там (ERC20).   
![](https://lh7-us.googleusercontent.com/Yc22689wf71XKJtU8PT3B0NHo3dA9yo7BY-Q2Xao1YU7hB-UJKHYnCuYakjA5TtUH8vcP-GMAwJ5Xacqden2TtwbIzNCpFzvhOJQc-tw0Q0nQ3ejVUrCHdWvjnjYODSoCUPRx8Eh2sgn3vTEnbLYtUQ)  
Копируем адрес кошелька вот отсюда и тип сети тут же указан по названием валюты. Тут это ERC20. После того как перевели нажмите кнопку Я перевел внизу.  
![](https://lh7-us.googleusercontent.com/PLHyOK-_MFR4UPi7DQTy3bv7gGcjxnxWOYuh5u8sccpiivJT8cT_x8DzzQ59nEGg5qT_k3fBPco07RfQhFSoBZ78aLjs7SAUxBflpKxkckkSlHTtz7u1kh9AT9YdaYZfXJCnOMB4IUStutW-dhiVZXs)  
"
                      }};
            bool lessonAdded = false;
            foreach (var lesson in lessons)
                if (!await _db.Lessons.AnyAsync(x => x.CourseId == lesson.Id))
                {
                    _db.Lessons.Add(lesson);
                    lessonAdded = true;
                }
            if (lessonAdded)
                await _db.SaveChangesAsync();
        }

        private async Task<List<AbCourseDbDto>> InitCourses()
        {
            var courses = new[] {
                 new AbCourseDbDto
             {
                 Id = Guid.NewGuid(),
                 Name = "Обмен обычных денег на крипту",
                 Description = $"В этом курсе вы узнаете как обменять рубли на крипту",
                 Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", $"Rectangle 202.png")),
                 Free = true
             } ,
                new AbCourseDbDto
             {
                 Id = Guid.NewGuid(),
                 Name = "Создание и использование торгового счета",
                 Description = @"В этом курсе вы узнаете как  
1)Зарегестрировать брокерский счет.   
2) Верифицировать брокерский счет.   
3) Пополнить брокерский счет.
4) Подключиться к VDS серверу и настроить торговый терминал на использование вашего счета",
                 Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", $"Rectangle 201.png")),
                 Free = true
             }}
            .ToList();
            bool itemAdded = false;
            foreach (var item in courses)
                if (!await _db.Courses.AnyAsync(x => x.Name == item.Name))
                {
                    _db.Courses.Add(item);
                    itemAdded = true;
                }
            if (itemAdded)
                await _db.SaveChangesAsync();
            return courses;
        }

        private async Task<List<AbServerExtentionDto>> InitServerExtentions()
        {
            var serverExtention = new[] { 1, 3, 6, 12 }
              .Select(x => new AbServerExtentionDto { Id = Guid.NewGuid(), ServerDurationInMonth = (uint)x })
              .ToList();
            bool itemAdded = false;
            foreach (var item in serverExtention)
                if (!await _db.ServerExtentions.AnyAsync(x => x.ServerDurationInMonth == item.ServerDurationInMonth))
                {
                    _db.ServerExtentions.Add(item);
                    itemAdded = true;
                }
            if (itemAdded)
                await _db.SaveChangesAsync();
            return serverExtention;
        }

        private async Task<List<AbServerDbDto>> InitServers()
        {
            var servers = new[] { ("Trial 1 месяц", 1), ("Invest 3 месяца", 3), ("Worker 6 месяцев", 6), ("VIP 12 месяцев", 12) }
                .Select(x => new AbServerDbDto
                {
                    Id = Guid.NewGuid(),
                    Name = x.Item1,
                    ServerDurationInMonth = (uint)x.Item2
                })
                .ToList();
            bool itemAdded = false;
            foreach (var item in servers)
                if (!await _db.Servers.AnyAsync(x => x.Name == item.Name))
                {
                    _db.Servers.Add(item);
                    itemAdded = true;
                }
            if (itemAdded)
                await _db.SaveChangesAsync();
            return servers;
        }

        private async Task<List<AbLicenseDbDto>> InitLicenses()
        {
            var licenses = new[] { "Trial", "Invest", "Worker", "VIP" }
                .Select(x => new AbLicenseDbDto
                {
                    Id = Guid.NewGuid(),
                    Name = x
                })
                .ToList();
            bool itemAdded = false;
            foreach (var item in licenses)
                if (!await _db.Merches.AnyAsync(x => x.Name == item.Name))
                {
                    _db.Licenses.Add(item);
                    itemAdded = true;
                }
            if (itemAdded)
                await _db.SaveChangesAsync();
            return licenses;
        }

        private async Task<AbMerchDbDto[]> InitExtentionOnlyMerch(AbCurrencyDbDto currency, AbPaymentNetworkDbDto paymentNetwork)
        {
            var merchs = new[] {
                new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Продление сервера на 1 месяц",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "1-иконка.png")),
                     ShortDescription =  $"Продление сервера на 1 месяц",
                     FullDescription =  $"Продление сервера на 1 месяц",
                    Price = 40M
                },
                  new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Продление сервера на 3 месяца",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "2-иконка.png")),
                     ShortDescription = $"Продление сервера на 3 месяца",
                     FullDescription = $"Продление сервера на 3 месяца",
                    Price = 400M
                },

  new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Продление сервера на 6 месяцев",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "3-иконка.png")),
                     ShortDescription = $"Продление сервера на 6 месяцев",
                     FullDescription = $"Продление сервера на 6 месяцев",
                    Price = 700M
                },  new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Продление сервера на 12 месяцев",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "4-иконка.png")),
                     ShortDescription = $"Продление сервера на 12 месяцев",
                     FullDescription = $"Продление сервера на 12 месяцев",
                    Price = 1000M
                },            };

            bool merhcAdded = false;
            foreach (var item in merchs)
                if (!await _db.Merches.AnyAsync(x => x.Name == item.Name))
                {
                    _db.Merches.Add(item);
                    merhcAdded = true;
                }
            if (merhcAdded)
                await _db.SaveChangesAsync();
            return merchs;
        }

        private async Task<AbMerchDbDto[]> InitServerOnlyMerch(AbCurrencyDbDto currency, AbPaymentNetworkDbDto paymentNetwork)
        {
            var merchs = new[] {
                new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Сервер на 1 месяц",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "1-иконка.png")),
                     ShortDescription =  $"Сервер на 1 месяц",
                     FullDescription =  $"Сервер на 1 месяц",
                    Price = 40M
                },
                  new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Сервер на 3 месяца",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "2-иконка.png")),
                     ShortDescription = $"Сервер на 3 месяца",
                     FullDescription = $"Сервер на 3 месяца",
                    Price = 400M
                },

  new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Сервер на 6 месяцев",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "3-иконка.png")),
                     ShortDescription = $"Сервер на 6 месяцев",
                     FullDescription = $"Сервер на 6 месяцев",
                    Price = 700M
                },  new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Сервер на 12 месяцев",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "4-иконка.png")),
                     ShortDescription = $"Сервер на 12 месяцев",
                     FullDescription = $"Сервер на 12 месяцев",
                    Price = 1000M
                },            };

            bool merhcAdded = false;
            foreach (var item in merchs)
                if (!await _db.Merches.AnyAsync(x => x.Name == item.Name))
                {
                    _db.Merches.Add(item);
                    merhcAdded = true;
                }
            if (merhcAdded)
                await _db.SaveChangesAsync();
            return merchs;
        }

        private async Task<AbMerchDbDto[]> InitMerch(AbCurrencyDbDto currency, AbPaymentNetworkDbDto paymentNetwork)
        {
            var merchs = new[] {
                new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Ale Bot: Trial",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 201.png")),
                     ShortDescription = @"
Демо-версия    
Доходность: До 10%    
Минимальный депозит от 200$    
Валютные пары EUR/USD    
Использует базовые алгоритмы для торговле на валютном рынке   
1 месяц лицензии    
Доступ в чат комьюнити   
Партнёрская программа: 10%   
1 месяца VDS сервера бесплатно",
FullDescription = @"
Демо-версия    
Доходность: 10%    
Минимальный депозит от 200$    
Валютные пары EUR/USD    
Использует базовые алгоритмы для торговле на валютном рынке   
1 месяц лицензии    
Доступ в чат комьюнити   
Партнёрская программа: 10%   
1 месяца VDS сервера бесплатно",
                    Price = 49M
                },
                new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Ale Bot: Invest",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 202.png")),
                     ShortDescription = @"
Минимальный риск   
Доходность: 3-10% в месяц   
Минимальный депозит от 200$   
Рекомендуемый объём депозита: 70%   
Свыше 10 валютных пар  
Истользует множество последовательно задействованых алгоритмов  
Анализирует рынок в моменте и закрывает сделки согласно своим расчетам, не используя заранее выставленные t/p   
Бессрочная лицензий  
Бессрочный доступ в чат комьюнити   
Партнёрская программа: 25% и 10%  
3 месяца VDS сервера бесплатно",
                     FullDescription = @"
Минимальный риск   
Доходность: 3-10% в месяц   
Минимальный депозит от 200$   
Рекомендуемый объём депозита: 70%   
Свыше 10 валютных пар  
Истользует множество последовательно задействованых алгоритмов  
Анализирует рынок в моменте и закрывает сделки согласно своим расчетам, не используя заранее выставленные t/p   
Бессрочная лицензий  
Бессрочный доступ в чат комьюнити   
Партнёрская программа: 25% и 10%  
3 месяца VDS сервера бесплатно",
                    Price = 490M
                },
                new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"Ale Bot: Worker",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 203.png")),
                     ShortDescription = @"
Высокий риск  
Доходность: 7-30% в месяц   
Минимальный депозит от 200$   
Рекомендуемый объём депозита: 30%  
Валютные пары EUR/USD  
Использует базовые алгоритмы для торговле на валютном рынке   
Бессрочная лицензий   
Бессрочный доступ в чат комьюнити  
Партнёрская программа: 25% и 10%  
6 месяцев VDS сервера бесплатно",
                     FullDescription = @"
Высокий риск  
Доходность: 7-30% в месяц   
Минимальный депозит от 200$   
Рекомендуемый объём депозита: 30%  
Валютные пары EUR/USD  
Использует базовые алгоритмы для торговле на валютном рынке   
Бессрочная лицензий   
Бессрочный доступ в чат комьюнити  
Партнёрская программа: 25% и 10%  
6 месяцев VDS сервера бесплатно",
                    Price = 790M
                },
                new AbMerchDbDto
                {
                     Id = Guid.NewGuid(),
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Name = $"VIP : Ale Bot Invest + Ale Bot Worker",
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 204.png")),
                     ShortDescription = @"
12 месяцев VDS сервера бесплатно   
Расширенная партнерская программа 35% и 15%",
                     FullDescription = @"
12 месяцев VDS сервера бесплатно   
Расширенная партнерская программа 35% и 15%",
                     Price = 1090M
                }
            };

            bool merhcAdded = false;
            foreach (var item in merchs)
                if (!await _db.Merches.AnyAsync(x => x.Name == item.Name))
                {
                    _db.Merches.Add(item);
                    merhcAdded = true;
                }
            if (merhcAdded)
                await _db.SaveChangesAsync();
            return merchs;
        }

        private async Task InitAccounts(AbCurrencyDbDto currency, List<AbUserDbDto> users)
        {
            var accounts = users.Select(x => new AbAccountDbDto
            {
                UserId = x.Id,
                CurrencyId = currency.Id,
                Amount = 0,
            }).ToList();

            var accountAdded = false;
            foreach (var account in accounts)
            {
                if (!await _db.Accounts.AnyAsync(x => x.UserId == account.UserId && x.CurrencyId == account.CurrencyId) && await _db.Users.AnyAsync(u => u.Id == account.UserId))
                {
                    _db.Accounts.Add(account);
                    accountAdded = true;
                }
            }

            if (accountAdded)
                await _db.SaveChangesAsync();
        }

        private async Task<List<AbUserDbDto>> InitUsers()
        {
            var adminEmails = new[] {"alebot.info@gmail.com", "callidussapiens@gmail.com",
            "fortis.potens.sapiens@gmail.com", "Alebot174@gmail.com"};
            var passwordHasher = new PasswordHasher<AbUserDbDto>();
            var users = adminEmails
                .Select(x => new AbUserDbDto()
                {
                    Id = Guid.NewGuid(),
                    UserName = x,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = x.GetNormalizedName(),
                    NormalizedUserName = x.GetNormalizedName(),
                    Email = x,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                })
                .ToList();
            var role = await _db.Roles.FirstAsync(x => x.NormalizedName == "ADMIN");
            foreach (var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, Guid.NewGuid().ToString());
                if (!await _db.Users.AnyAsync(x => x.UserName == user.UserName))
                {
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                    _db.UserRoles.Add(new UserRoleDbDto { RoleId = role.Id, UserId = user.Id });
                    await _db.SaveChangesAsync();
                }
            }
            return users;
        }

        /// <summary>
        /// Тестовый метод с авторизацией
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Тестовый метод
        /// </summary>
        /// <returns></returns>
        [HttpGet("new")]
        public IEnumerable<WeatherForecast> GetNew()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Тестовый метод для админа
        /// </summary>
        /// <returns></returns>
        [HttpGet("new-admin")]
        [Authorize(Roles = "admin")]
        public IEnumerable<WeatherForecast> GetNewAdmin()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
