using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Globalization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text;
using ApeBotApi.Models;
using ApeBotApi.DbDtos;
using ApeBotApi.Attributes;
using AleBotApi.Clients;
using Microsoft.AspNetCore.Identity.UI.Services;
using AleBotApi.DelegatingHandlers;
using AleBotApi.Filters;
using AleBotApi.Middlewares;
using AleBotApi.DbContexts;
using AleBotApi.Services;
using AleBotApi.DbDtos;
using AleBotApi.Models;
using AleBotApi.BackgroundWorks;
using ApeBotApi.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Diagnostics;

namespace ApeBotApi
{
    public class Program
    {
        //static List<(Telegram.Bot.Types.ChatId, DateTime)> Recivers = new List<(Telegram.Bot.Types.ChatId, DateTime)>();
        //static Task MessageSending;
        //static ITelegramBotClient bot = new TelegramBotClient("7042344311:AAE8L0hzt1U8EuI3JYbvaWyb4UsYJf9nLo8");
        //public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        //{
        //    // Некоторые действия
        //    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        //    if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        //    {
        //        var message = update.Message;
        //        if (message.Text.ToLower() == "/start")
        //        {
        //            Recivers.Add((message.Chat.Id, DateTime.UtcNow));
        //            return;
        //        }
        //    }
        //}

        //public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        //{
        //    // Некоторые действия
        //    Log.Error(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        //}

        public static async Task Main(string[] args)
        {
            //MessageSending = Task.Run(async () =>
            //{
            //    try
            //    {
            //        while (true)
            //        {
            //            var now = DateTime.UtcNow;
            //            var sendTo = Recivers.Where(x => (now - x.Item2).TotalMinutes > 30).ToList();
            //            Recivers.RemoveAll(x => (now - x.Item2).TotalMinutes > 30);
            //            foreach (var item in sendTo)
            //            {
            //                await bot.SendTextMessageAsync(item.Item1, "Давайте знакомиться по ближе! Моя группа в Телеграм: https://t.me/Ale_bot1  Моя страница в Инстаграм: https://www.instagram.com/own.174/ Мой ютуб канал: https://www.youtube.com/watch?v=L55GUqVEfkA ");
            //            }
            //            await Task.Delay(10000);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        //Ignore
            //        Log.Error(e, "Error on sending message to telegram");
            //    }
            //});
            //var cts = new CancellationTokenSource();
            //var cancellationToken = cts.Token;
            //var receiverOptions = new ReceiverOptions
            //{
            //    AllowedUpdates = { }, // receive all update types
            //};
            //await bot.DeleteWebhookAsync();
            //bot.StartReceiving(
            //    HandleUpdateAsync,
            //    HandleErrorAsync,
            //    receiverOptions,
            //    cancellationToken
            //);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddControllersAsServices();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AleBot API",
                    Description = "An ASP.NET Core Web API for managing Ale Bot Site",
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            var cfg = builder.Configuration;
            builder.Services.AddDbContext<AbDbContext>(
#if DEBUG
    options => options.UseInMemoryDatabase("AppDb")
#else
 options => options.UseNpgsql(cfg.GetConnectionString("DefaultConnection"))
#endif
    );
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<AbUserDbDto>(x =>
            {
                x.SignIn.RequireConfirmedAccount = true;
                x.Password.RequiredLength = 4;
                x.Password.RequireUppercase = false;
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AbRoleDbDto>()
                .AddEntityFrameworkStores<AbDbContext>();
            builder.Services.AddTransient<IEmailClient, EmailClient>();
            builder.Host.UseSerilog();
            var services = builder.Services;
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });
            services.AddMvc(x =>
            {
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), 588));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(AppErrorModel), 488));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(AppProblemDetails), 400));
                x.Filters.Add<CustomExceptionFilterAttribute>();
            })
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            var version = typeof(Program).Assembly.GetName().Version.ToString();
            services.AddHealthChecks()
                .AddCheck($"API State (version : {version})", x => HealthCheckResult.Healthy())
                .AddDbContextCheck<AbDbContext>("DB State");
            services.AddTransient<LoggingHandler>();
            services.AddTransient<IEmailSender, AbEmailService>();
            services.AddTransient<IEmailSender<AbUserDbDto>, AbEmailService>();
            services.AddTransient<IEmailService, AbEmailService>();
            services.AddTransient<ICryptoCloudService, CryptoCloudService>();
            services.AddHostedService<CryptoCloudWalletPaymentStatusCheck>();
            services.AddCors(x =>
               x.AddPolicy("default", y => y.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
               );
            services.AddTransient<IAccountsService, AccountsService>();
            services.AddHttpClient<IAxlCrmClient, AxlCrmClient>((IServiceProvider sp, HttpClient hc) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                hc.BaseAddress = new Uri(config["AXL:Host"], UriKind.Absolute);
                hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config["AXL:ApiKey"]);
            }).AddHttpMessageHandler<LoggingHandler>();
            services.AddTransient<ICrmService, CrmService>();
            services.AddHttpClient<IDarkbandRuClient, DarkbandRuClient>((IServiceProvider sp, HttpClient hc) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                hc.BaseAddress = new Uri(config["Darkband:Host"], UriKind.Absolute);
            }).AddHttpMessageHandler<LoggingHandler>();
            services.AddHttpClient<IAmoCrmClient, AmoCrmClient>((IServiceProvider sp, HttpClient hc) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                hc.BaseAddress = new Uri(config["Amo:Host"], UriKind.Absolute);
                hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config["Amo:ApiKey"]);
            }).AddHttpMessageHandler<LoggingHandler>();
            services.AddHostedService<EventsAndSagasHandling>();









            var app = builder.Build();
            app.UseCors(b =>
            {
                b = b.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()
                  .SetIsOriginAllowed(x => true);
            });
            app.MapHealthChecks("/healthz", new HealthCheckOptions()
            {
                AllowCachingResponses = false,
                ResponseWriter = WriteResponse
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseMiddleware<LogUserNameMiddleware>();
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
#if DEBUG
            app.Use(async (context, next) =>
            {
                var originalBody = context.Response.Body;
                var newBody = new MemoryStream();
                context.Response.Body = newBody;
                await LogRequest(context);
                await next(context);
                await LogResponse(context, newBody, originalBody);
            });
#endif
            app.UseStaticFiles();
            app.MapIdentityApi<AbUserDbDto>();
            app.UseHsts();
            app.UseHttpsRedirection();

            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next(context);
            });
            app.UseAuthorization();

            app.MapControllers();

            SeedDb(app);

            var logDir = Path.Combine(Environment.CurrentDirectory, "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            var logPath = Path.Combine(logDir, "log_file.txt");
            if (!System.IO.File.Exists(logPath))
                System.IO.File.Create(logPath).Dispose();
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
#else
                .MinimumLevel.Error()
#endif
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{SourceContext} {Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    logPath,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10L * 1024 * 1024,
                    flushToDiskInterval: TimeSpan.FromSeconds(2))
                .CreateLogger();
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
                Log.Information("Starting up");
                app.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task LogResponse(HttpContext context, Stream newBody, Stream originalBody)
        {
            var requestStr = new
            {
                Headers = context.Response.Headers.Select(x => new { x.Key, Value = x.Value.Select(x => x).ToArray() }).ToDictionary(x => x.Key, x => x.Value),
                context.Response.StatusCode
            }.ToJson();
            newBody.Seek(0, SeekOrigin.Begin);
            var bodyStr = await new StreamReader(newBody).ReadToEndAsync();
            bodyStr = Regex.Replace(bodyStr, "\"photo\":\"[^\"]*\"", "");
            bodyStr = Regex.Replace(bodyStr, "\"coursePhoto\":\"[^\"]*\"", "");
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
            Console.WriteLine($"RESPONSE: {(requestStr?.Length > 1000 ? requestStr.Substring(0, 1000) : requestStr)} with body: {(bodyStr?.Length > 1000 ? bodyStr.Substring(0, 1000) : bodyStr)}");
        }

        private static async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestStr = new
            {
                Path = context.Request.GetDisplayUrl(),
                Query = context.Request.Query.Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value),
                Headers = context.Request.Headers.Select(x => new { x.Key, Value = x.Value.Select(x => x).ToArray() }).Where(x => x.Key.ToLower() != "cookie").ToDictionary(x => x.Key, x => x.Value),
                Cookies = context.Request.Cookies.Select(x => new { x.Key, x.Value }).ToDictionary(x => x.Key, x => x.Value)
            }.ToJson();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            var bodyStr = await new StreamReader(context.Request.Body).ReadToEndAsync();
            bodyStr = Regex.Replace(bodyStr, "\"photo\":\"[^\"]*\"", "");
            bodyStr = Regex.Replace(bodyStr, "\"coursePhoto\":\"[^\"]*\"", "");
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            Console.WriteLine($"REQUEST: {(requestStr?.Length > 1000 ? requestStr.Substring(0, 1000) : requestStr)} with body: {(bodyStr?.Length > 1000 ? bodyStr.Substring(0, 1000) : bodyStr)}");
        }

        private static void SeedDb(WebApplication app)
        {
            using (var s = app.Services.CreateScope())
            {
                var db = s.ServiceProvider.GetRequiredService<AbDbContext>();
                var environment = s.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
#if !DEBUG
                db.Database.Migrate();
#endif
                var roleId = Guid.NewGuid();
                if (!db.Roles.Any())
                {
                    db.Roles.AddRange(
                          new AbRoleDbDto
                          {
                              Id = roleId,
                              ConcurrencyStamp = Guid.NewGuid().ToString(),
                              NormalizedName = "ADMIN",
                              Name = "admin",
                          });
                    db.SaveChanges();
#if DEBUG
                    var passwordHasher = new PasswordHasher<AbUserDbDto>();
                    var email = "admin@mail.ru";
                    var admin = new AbUserDbDto()
                    {
                        Id = Consts.AdminUserId,
                        PhoneNumber = "1234567891",
                        UserName = email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = email.ToUpper(),
                        NormalizedUserName = email.ToUpper(),
                        Email = email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        RefererId = Consts.TestUserId
                    };
                    admin.PasswordHash = passwordHasher.HashPassword(admin, "1234Qwert!");
                    db.Users.Add(admin);
                    db.SaveChanges();
                    db.UserRoles.Add(new UserRoleDbDto { RoleId = roleId, UserId = Consts.AdminUserId });
                    db.SaveChanges();

                    email = "user@mail.ru";
                    var user = new AbUserDbDto()
                    {
                        Id = Consts.TestUserId,
                        PhoneNumber = "1234567892",
                        UserName = email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = email.ToUpper(),
                        NormalizedUserName = email.ToUpper(),
                        Email = email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    user.PasswordHash = passwordHasher.HashPassword(user, "1234Qwert!");
                    db.Users.Add(user);
                    db.SaveChanges();
#endif
                }
                var currency = new AbCurrencyDbDto
                {
                    Id = Guid.NewGuid(),
                    Code = "USDT",
                    Name = "USDT"
                };
                if (!db.Currencies.Any())
                {
                    db.Currencies.Add(currency);
                    db.SaveChanges();
#if DEBUG
                    db.Accounts.Add(new AbAccountDbDto
                    {
                        UserId = Consts.AdminUserId,
                        CurrencyId = currency.Id,
                        Amount = 0,
                    });
                    var acc = new AbAccountDbDto
                    {
                        Id = Guid.NewGuid(),
                        UserId = Consts.TestUserId,
                        CurrencyId = currency.Id,
                        Amount = 0,
                    };
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    var ti = new[] { 1, 2, 3, 4 }
                    .Select(x => new AbAccountTransactionDbDto
                    {
                        Id = Guid.NewGuid(),
                        AccountId = acc.Id,
                        OperationType = AleBotApi.DbDtos.Enums.AccountTransactionOperationType.Accrual,
                        Reason = AleBotApi.DbDtos.Enums.AccountTransactionReason.AccrualByCryptoCloud,
                        Amount = 1,
                        State = AleBotApi.DbDtos.Enums.AccountTransactionState.Completed,
                        CompletedOn = DateTime.UtcNow,
                        OperationDescription = "Пополнение",
                        DebitFee = 0.001M,
                    }).ToList();
                    var to = new[] { 1, 2, 3, 4 }
               .Select(x => new AbAccountTransactionDbDto
               {
                   Id = Guid.NewGuid(),
                   AccountId = acc.Id,
                   OperationType = AleBotApi.DbDtos.Enums.AccountTransactionOperationType.Debiting,
                   Reason = AleBotApi.DbDtos.Enums.AccountTransactionReason.DebitingToCryptoCloud,
                   Amount = 1,
                   State = AleBotApi.DbDtos.Enums.AccountTransactionState.Completed,
                   CompletedOn = DateTime.UtcNow,
                   OperationDescription = "Вывод",
                   DebitFee = 0.001M,
                   DebitCurrencyId = currency.Id
               }).ToList();
                    db.AccountTransactions.AddRange(ti);
                    db.AccountTransactions.AddRange(to);
                    db.SaveChanges();
#endif
                }
                var paymentSystme = new AbPaymentSystemDbDto
                {
                    Id = Guid.NewGuid(),
                    Name = "CryptoCloud"
                };
                if (!db.PaymentSystems.Any())
                {
                    db.PaymentSystems.Add(paymentSystme);
                    db.SaveChanges();
                }
                var paymentNetwork = new AbPaymentNetworkDbDto
                {
                    Id = Guid.NewGuid(),
                    Name = "TRC20"
                };
                if (!db.PaymentNetworks.Any())
                {
                    db.PaymentNetworks.Add(paymentNetwork);
                    db.SaveChanges();
                }

#if DEBUG
                var longText = @"Минимальный риск

Выбор доходности: 3-15% в месяц   
Минимальный депозит от 200$   
Рекомендуемый объём депозита:70%   
Свыше 10 валютных пар  
Истользует множество последовательно задействованых алгоритмов   
Анализирует рынок в моменте и закрывает сделки согласно своим расчетам, не используя заранее выставленные t/p   
Бессрочная лицензий   
Бессрочный доступ в чат комьюнити    
Партнёрская программа: 25% и 10%    
3 месяца VDS сервера бесплатно  
";
                var couses = new[] { 1, 2, 3, 4 }
               .Select(x => new AbCourseDbDto
               {
                   Id = Guid.NewGuid(),
                   Name = $"Курс {x}",
                   Description = x == 3 ? "Описание курса 3" : $"Описание курса {x} {longText}",
                   Photo = System.IO.File.ReadAllBytes(Path.Combine(environment.WebRootPath, "files", $"Rectangle 20{x}.png")),
                   Free = x == 1
               }).ToList();
                var uc = couses
                    .Select(x => new AbUserCourseDbDto
                    {
                        Id = Guid.NewGuid(),
                        UserId = Consts.TestUserId,
                        CourseId = x.Id,
                    })
                    .ToList();
                if (!db.Courses.Any())
                {
                    db.Courses.AddRange(couses);
                    db.UserCourses.AddRange(uc);
                    db.SaveChanges();
                }


                if (!db.Lessons.Any())
                {
                    foreach (var c in couses)
                    {
                        var lessons = new[] { 1, 2, 3 }
                      .Select(x => new AbLessonDbDto
                      {
                          Id = Guid.NewGuid(),
                          Name = $"Урок {x} {c.Id}",
                          CourseId = c.Id,
                          Number = (uint)x,
                          Body = x == 3 ? "https://docs.google.com/document/d/1DuDlMzUaBJxKEs_j8apo90WnV5XNG38LSS4pGk9-Eew/edit?usp=sharing" : x == 1 ? @"
                            Мое видео!
<center>
<iframe width=""560"" height=""315"" src=""https://www.youtube.com/embed/L55GUqVEfkA?si=4C51yIeOQV1SAet3"" title=""YouTube video player"" frameborder=""0"" allow=""accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"" allowfullscreen></iframe>
<center>
" : $"Тело **урока** {x} {c.Id} {longText}"
                      });
                        db.Lessons.AddRange(lessons);
                        db.SaveChanges();
                    }
                }

                var liceses = new[] { "Invest", "Invest", "Worker", "VIP" }
                 .Select(x => new AbLicenseDbDto
                 {
                     Id = Guid.NewGuid(),
                     Name = x,
                 })
                 .ToList();
                var ul = liceses
                .Select(x => new AbUserLicenseDbDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Consts.TestUserId,
                    LicenseId = x.Id,
                    ActivationKey = "61",
                    TradingAccount = x.Name == "Лицензия 1" ? null : x.Name,
                });
                if (!db.Licenses.Any())
                {

                    db.Licenses.AddRange(liceses);
                    db.UserLicenses.AddRange(ul);
                    db.SaveChanges();
                }

                var servers = new[] { 1, 3, 6, 12 }
             .Select(x => new AbServerDbDto
             {
                 Id = Guid.NewGuid(),
                 Name = $"Сервер {x}",
                 ServerDurationInMonth = (uint)x
             }).ToList();
                var us = servers
                        .Select(x => new AbUserServerDbDto
                        {
                            Id = Guid.NewGuid(),
                            UserId = Consts.TestUserId,
                            ServerId = x.Id,
                            Login = x.Id.ToString(),
                            Password = x.Id.ToString(),
                            Address = x.Id.ToString(),
                            ExpirationDate = DateTime.UtcNow,
                            Created = DateTime.UtcNow,
                            ExternalId = "1"
                        });
                if (!db.Servers.Any())
                {

                    db.Servers.AddRange(servers);
                    db.UserServers.AddRange(us);
                    db.SaveChanges();
                }

                var items = new[] { 1, 2, 3, 4 }
                 .Select(x => new AbMerchDbDto
                 {
                     Id = Guid.NewGuid(),
                     Name = $"Продукт {x}",
                     CurrencyId = currency.Id,
                     PaymentNetworkId = paymentNetwork.Id,
                     Photo = System.IO.File.ReadAllBytes(Path.Combine(environment.WebRootPath, "files", $"Rectangle 20{x}.png")),
                     ShortDescription = $"Краткое **описание** {x} {longText} {(x == 3 ? longText : "")}",
                     FullDescription = $"Полное **описание** {x} {longText} {(x == 3 ? longText : "")}",
                     Price = x == 1 ? 0m : 1.11M
                 }).ToList();
                var merchCouse = new[] { 1, 2, 3, 4 }
                    .Select(x => new AbMerchCourseDbDto
                    {
                        Id = Guid.NewGuid(),
                        MerchId = items[x - 1].Id,
                        CourseId = couses[x - 1].Id,
                    }).ToList();
                var merchServer = new[] { 1, 2, 3, 4 }
            .Select(x => new AbMerchServerDbDto
            {
                Id = Guid.NewGuid(),
                MerchId = items[x - 1].Id,
                ServerId = servers[x - 1].Id,
            }).ToList();
                var merchLicense = new[] { 1, 2, 3, 4 }
           .Select(x => new AbMerchLicenseDbDto
           {
               Id = Guid.NewGuid(),
               MerchId = items[x - 1].Id,
               LicenseId = liceses[x - 1].Id,
           }).ToList();
                if (!db.Merches.Any())
                {
                    db.Merches.AddRange(items);
                    db.MerchCourses.AddRange(merchCouse);
                    db.MerchLicenses.AddRange(merchLicense);
                    db.MerchServers.AddRange(merchServer);
                    db.SaveChanges();
                }

                var serverMerch = new[] { (Guid.NewGuid(), 1), (Guid.NewGuid(), 2), (Guid.NewGuid(), 3), (Guid.NewGuid(), 4), }
                .Select(x => new AbMerchDbDto
                {
                    Id = x.Item1,
                    Name = $"Продукт c продлением сервера {x.Item1}",
                    CurrencyId = currency.Id,
                    PaymentNetworkId = paymentNetwork.Id,
                    Photo = System.IO.File.ReadAllBytes(Path.Combine(environment.WebRootPath, "files", $"Rectangle 20{x.Item2}.png")),
                    ShortDescription = $"Краткое **описание** {x} {longText}",
                    FullDescription = $"Полное **описание** {x} {longText}",
                    Price = 1.11M
                })
                .ToList();
                var serverExtention = new[] { 1, 3, 6, 12 }
                .Select(x => new AbServerExtentionDto { Id = Guid.NewGuid(), ServerDurationInMonth = (uint)x })
                .ToList();
                var mse = serverMerch.Zip(serverExtention, (a, b) => new AbMerchServerExtentionsDbDto
                {
                    Id = Guid.NewGuid(),
                    MerchId = a.Id,
                    ServerExtentionId = b.Id,
                    Qty = 1
                }).ToList();

                db.Merches.AddRange(serverMerch);
                db.ServerExtentions.AddRange(serverExtention);
                db.MerchServerExtentions.AddRange(mse);
                db.SaveChanges();

                var serverOnlyMerch = new[] { (Guid.NewGuid(), 1), (Guid.NewGuid(), 2), (Guid.NewGuid(), 3), (Guid.NewGuid(), 4), }
                .Select(x => new AbMerchDbDto
                {
                    Id = x.Item1,
                    Name = $"Продукт с покупкой сервера {x}",
                    CurrencyId = currency.Id,
                    PaymentNetworkId = paymentNetwork.Id,
                    Photo = System.IO.File.ReadAllBytes(Path.Combine(environment.WebRootPath, "files", $"Rectangle 20{x.Item2}.png")),
                    ShortDescription = $"Краткое **описание** {x} {longText} {(x.Item2 == 3 ? longText : "")}",
                    FullDescription = $"Полное **описание** {x} {longText} {(x.Item2 == 3 ? longText : "")}",
                    Price = 1.11M
                })
                .ToList();
                //var serverOnly = new[] { 1, 3, 6, 12 }
                //.Select(x => new AbServerDbDto { Id = Guid.NewGuid(), ServerDurationInMonth = (uint)x, Name = $"Только сервер на {x}" })
                //.ToList();
                var mso = serverOnlyMerch.Zip(servers, (a, b) => new AbMerchServerDbDto
                {
                    Id = Guid.NewGuid(),
                    MerchId = a.Id,
                    ServerId = b.Id,
                    Qty = 1
                }).ToList();

                db.Merches.AddRange(serverOnlyMerch);
                db.MerchServers.AddRange(mso);
                db.SaveChanges();
                var merch = db.Merches.Skip(1).FirstOrDefault();
                Console.WriteLine("MERCH ID");
                Console.WriteLine(merch.Id);
                Console.WriteLine(Consts.TestUserId);
#endif
            }
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", result.Status.ToString());
                    writer.WriteStartObject("results");
                    foreach (var entry in result.Entries)
                    {
                        writer.WriteStartObject(entry.Key);
                        writer.WriteString("status", entry.Value.Status.ToString());
                        writer.WriteString("description", entry.Value.Description);
                        writer.WriteStartObject("data");
                        foreach (var item in entry.Value.Data)
                        {
                            writer.WritePropertyName(item.Key);
                            JsonSerializer.Serialize(
                                writer, item.Value, item.Value?.GetType() ??
                                                    typeof(object));
                        }
                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return context.Response.WriteAsync(json);
            }
        }
    }
}
