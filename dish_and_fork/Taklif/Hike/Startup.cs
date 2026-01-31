using System.Data;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CorePush.Google;
using CorePush.Interfaces;
using Daf.FilesModule.SecondaryAdaptersInterfaces;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.BackgroundWorks;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Hubs;
using Hike.Models;
using Hike.Modules.AdminSettings;
using Hike.Options;
using Hike.Repositories;
using Hike.Services;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using TwilioClient = Hike.Clients.TwilioClient;

namespace Hike
{

    //TODO: ��������� ����� ����� �������
    public class Startup
    {
        private readonly IPasswordHasher<HikeUser> _passwordHasher = new PasswordHasher<HikeUser>();

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigureNamedOptions<JwtBearerOptions>, JwtBearerOptionsConfig>();
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsConfig>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            var amso = new AuthMessageSenderOptions();
            Configuration.Bind(amso);
            services.AddDbContext<HikeDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<HikeUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.NameIdentifier;
                    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HikeDbContext>();
            var isc = services.AddIdentityServer()
                .AddJwtBearerClientAuthentication()
                  .AddApiAuthorization<HikeUser, HikeDbContext>()
                  .AddProfileService<ProfileService>();
            services.AddAuthentication()
                .AddIdentityServerJwt()
                .AddGoogle(x =>
                {
                    x.ClientId = amso.GoogleSsoId;
                    x.ClientSecret = amso.GoogleSsoSecretCode;
                    x.AccessDeniedPath = "/Identity/Account/AccessDeniedToExternalLogin";
                })
                //.AddFacebook(x =>
                //{
                //    x.AppId = amso.FacebookAppId;
                //    x.AppSecret = amso.FacebookAppSecret;
                //    x.AccessDeniedPath = "/Identity/Account/AccessDeniedToExternalLogin";
                //})
                .AddVkontakte(x =>
                {
                    x.ClientId = amso.VkClientId;
                    x.ClientSecret = amso.VkClientSecret;
                    x.AccessDeniedPath = "/Identity/Account/AccessDeniedToExternalLogin";
                    x.Scope.Add("email");
                })
                .AddApple(x =>
            {
                x.ClientId = Configuration["AppleClientId"];
                x.KeyId = Configuration["AppleKeyId"];
                x.TeamId = Configuration["AppleTeamId"];
                x.UsePrivateKey((keyId) =>
                    Environment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8")
                    );
            });
            var info = Environment.ContentRootFileProvider.GetFileInfo($"AuthKey_{Configuration["AppleKeyId"]}.p8");
            services.AddControllersWithViews()
                .AddControllersAsServices();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "clientapp/build";
            });
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IEmailClient, EmailSender>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hike", Version = typeof(Program).Assembly?.GetName()?.Version?.ToString(), Description = "Dish&Fork api. Health check available on path /health-check. SignalR hub on path /real-time-hub" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSignalRSwaggerGen();
            });
            services.AddSession();
            services.AddMemoryCache();
            services.AddTransient<IReturnUrlRepository, ReturnUrlRepository>();
            services.AddTransient<IRedirectService, RedirectService>();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });
            services.AddMvc(x =>
            {
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorModel), 500));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(AppErrorModel), 488));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(AppProblemDetails), 400));
                x.Filters.Add<CustomExceptionFilterAttribute>();
            })
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });
            //isc.AddDeveloperSigningCredential();
            ////if (!Environment.IsDevelopment())
            ////{
            ////    var c = GetKey();
            ////    isc.AddSigningCredential(c);
            ////}
            ////else
            ////{
            ////    isc.AddDeveloperSigningCredential(true);
            ////}
            //var cert = LoadFromFile();
            //isc.AddSigningCredential(cert);
            isc.AddDeveloperSigningCredential();
            var descriptors = services.Where(x => x.ServiceType == typeof(IConfigureOptions<ApiAuthorizationOptions>))
                .ToList();
            foreach (var serviceDescriptor in descriptors)
            {
                services.Remove(serviceDescriptor);
            }
            services.AddSingleton<IConfigureOptions<ApiAuthorizationOptions>, HikeAuthOption>();
            services.AddAuthorization();
            var version = typeof(Startup).Assembly.GetName().Version.ToString();
            services.AddHealthChecks()
                .AddCheck($"API State (version : {version})", x => HealthCheckResult.Healthy())
                .AddDbContextCheck<HikeDbContext>("DB State");
            services.AddTransient<ICancellationTokensRepository, CancellationTokensRepository>();
            services.AddTransient<LoggingHandler>();
            services.AddCors(x =>
                x.AddPolicy("default", y => y.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            services.AddHttpClient<IDostavistaClient, DostavistaClient>(x =>
           {
               x.BaseAddress = new Uri(Configuration["Dostavista:Url"], UriKind.Absolute);
               x.DefaultRequestHeaders.Add("X-DV-Auth-Token", Configuration["Dostavista:Token"]);
           }).AddHttpMessageHandler<LoggingHandler>();
            services.AddSingleton<ITwilioClient>(x =>
            {
                return new TwilioClient(Configuration["Twilio:VerifiServiceSid"], x.GetRequiredService<IDistributedCache>());
            });
            services.AddSignalR();
            services.AddTransient<IWebSocketsClient, WebSocketsClient>();
            services.AddHttpClient<IFcmSender, FcmSender>();
            var fcmSettings = new FcmSettings();
            Configuration.GetSection("FcmSettings").Bind(fcmSettings);
            services.AddSingleton(fcmSettings);
            services.AddTransient<IPushNotificationsClient, PushNotificationsClient>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.TryAddEnumerable(
                ServiceDescriptor
                .Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                );
            services.AddTransient<IAdminSettingsRepository, AdminSettingsRepository>();
            services.AddTransient<IFileSystemClient, FileSystemClient>();
            services.AddTransient<IBaseUriRepository, BaseUriRepository>();
            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                options.LoginPath = "/Identity/Account/Register";

            });
            services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = "/Identity/Account/Register";
            });
            services.AddHttpClient<IYooKassaClient, YooKassaClient>();
            services.AddHostedService<CheckingPaymentStatusBackgroundWork>();
            services.AddHostedService<CheckingDeliveryStatusBackgroundWork>();
            //  CodePackage.YooKassa.YooKassaManagerWrap.RegisterYooKassa(services);
            //   services.AddScoped<Yandex.Checkout.V3.AsyncClient>(x =>
            //   {
            //       var client = new Yandex.Checkout.V3.Client(
            //shopId: Configuration["YooKassa:MyShop"],
            //secretKey: Configuration["YooKassa:MySecretKey"]);
            //       return client.MakeAsync();
            //   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HikeDbContext db, IOptions<AuthMessageSenderOptions> options)
        {
            if (!env.IsDevelopment())
                app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next(context);
            });

            Console.WriteLine($"Start with Options {JsonSerializer.Serialize(options.Value)}");
            Twilio.TwilioClient.Init(Configuration["Twilio:AccountSid"], Configuration["Twilio:AuthToken"]);
            db.Database.Migrate();
            Seed(db);
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hike v1"));
            if (!env.IsDevelopment())
                app.UseHsts();
            app.UseSession();
            app.UseHttpsRedirection();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles(new StaticFileOptions()
                {
                    OnPrepareResponse = ctx =>
                    {
                        var headers = ctx.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(0)
                        };

                    }
                });
            }
            app.UseRouting();
            app.UseCors(b =>
            {
                b = b.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
                if (Configuration["IsTesting"]?.ToLower() != "true")
                    b = b.SetIsOriginAllowed(x => x.ToLower().Contains("dishnfork"));
                else
                    b = b.SetIsOriginAllowed(x => true);
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseMiddleware<LogUserNameMiddleware>();
            app.UseRequestResponseLogging();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health-check", new HealthCheckOptions()
                {
                    AllowCachingResponses = false,
                    ResponseWriter = WriteResponse
                });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<RealtimeHub>("/real-time-hub");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "clientapp";
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                {
                    OnPrepareResponse = ctx =>
                    {
                        var headers = ctx.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(0)
                        };
                    },
                };
                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
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

        /// <summary>
        /// Seed db
        /// </summary>
        /// <param name="db"></param>
        private void Seed(HikeDbContext db)
        {
            if (!db.Roles.Any())
            {
                db.Roles.AddRange(
                      new IdentityRole("admin")
                      {
                          ConcurrencyStamp = Guid.NewGuid().ToString(),
                          NormalizedName = "ADMIN"
                      });
                db.Roles.AddRange(
                    new IdentityRole("seller")
                    {
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedName = "SELLER"
                    });
                db.SaveChanges();
            }
            var ar = db.Roles.First(x => x.NormalizedName == "ADMIN");
            foreach (var email in new[] { "andrei.makarov.uvarov@DishnFork.onmicrosoft.com",
            "pavel.budiyakov@DishnFork.onmicrosoft.com",
            "sergei.efimov@DishnFork.onmicrosoft.com",
            "vladislav.kokarev@DishnFork.onmicrosoft.com",
            "MuhammadZumunov@DishnFork.onmicrosoft.com"})
                if (!db.Users.Any(r => r.NormalizedEmail == email.ToUpper()))
                {
                    var u = new HikeUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        PhoneNumber = "1234567891",
                        UserName = email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = email.ToUpper(),
                        NormalizedUserName = email.ToUpper(),
                        Email = email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    u.PasswordHash = _passwordHasher.HashPassword(u, "Pass@word1");
                    db.Users.Add(u);
                    db.SaveChanges();
                    db.UserRoles.Add(new IdentityUserRole<string> { RoleId = ar.Id, UserId = u.Id });
                    db.SaveChanges();
                }
            if (!db.Categories.Any())
            {
                var ct = new[] {
                    (CategoryType.Additionally, "Без лактозы"),
                    (CategoryType.Additionally, "Веган"),
                    (CategoryType.Additionally, "Халяль"),
                    (CategoryType.Additionally, "На завтрак"),
                    (CategoryType.Kind, "Выпечка"),
                    (CategoryType.Kind, "Мороженное"),
                    (CategoryType.Kind, "Торт"),
                    (CategoryType.Composition, "Орехи"),
                    (CategoryType.Composition, "Шоколад"),
                    (CategoryType.Composition, "Ягоды"),
                }.Select(s => new CategoryDto
                {
                    Id = Guid.NewGuid(),
                    Title = s.Item2,
                    NormalizedTitle = s.Item2.GetNormalizedName(),
                    Type = s.Item1
                });

                db.Categories.AddRange(ct);
                db.SaveChanges();
            }
            var data = new[]
           {
"Агар-агар                                                 ",
"Джем                                                ",
"Чернослив                                                ",
"Фисташки                                                 ",
"Кедровые орехи                                                 ",
"Изюм                                                ",
"Кешью",
"Миндаль",
"Фундук",
"Грецкий орех",
"Семена тыквы",
"Кокосовая стружка",
"Курага",
"Ароматизаторы, усилители вкуса                            ",
"Арахис",
"Белковые продукты                                         ",
"Витамины, премиксы                                        ",
"Волокна пищевые                                           ",
"Гели кондитерские                                         ",
"Глазирующие агенты (глянцеватели, воска)                  ",
"Глазури                                                   ",
"Декор                                                     ",
"Дрожжи                                                    ",
"Желатины                                                  ",
"Загустители                                               ",
"Закваски                                                  ",
"Замороженные изделия                                      ",
"Зерновые посыпки                                          ",
"Зернопродукты (мука, отруби, крупы, ПЭК)                  ",
"Какао, какао-продукты и заменители                        ",
"Камеди                                                    ",
"Каррагинаны                                               ",
"Кислоты, регуляторы кислотности                           ",
"Кондитерские жиры и масла                                 ",
"Кондитерские массы                                        ",
"Кондитерские смеси и основы                               ",
"Консерванты                                               ",
"Красители и красящие продукты                             ",
"Крахмалы и крахмало-паточные продукты                     ",
"Крема и сливки                                            ",
"Маргарины                                                 ",
"Молоко и молочные ингредиенты                             ",
"Начинки гастрономические                                  ",
"Начинки и наполнители                                     ",
"Начинки карамельные                                       ",
"Начинки кремовые                                          ",
"Начинки молочные                                          ",
"Начинки ореховые                                          ",
"Начинки фруктовые                                         ",
"Начинки шоколадные                                        ",
"Орехи и пасты                                             ",
"Пектины                                                   ",
"Помады и мастики                                          ",
"Разрыхлители                                              ",
"Сахар, сахарозаменители и подсластители                   ",
"Сиропы                                                    ",
"Смазки для форм и оборудования                            ",
"Солод и солодовые продукты                                ",
"Специи, травы, овощи                                      ",
"Стабилизаторы и стабилизационные системы                  ",
"Улучшители                                                ",
"Ферменты                                                  ",
"Фосфаты пищевые                                           ",
"Фрукты, ягоды                                             ",
"Функциональные ингредиенты для здорового питания          ",
"Хлебные смеси и концентраты                               ",
"Шоколад                                                   ",
"Эмульгаторы                                               ",
"Яичные продукты и их заменители                           ",
            }.Select(x => x.Trim())
           .Distinct()
           .Select(x => new CategoryDto
           {
               Id = Guid.NewGuid(),
               Title = x,
               NormalizedTitle = x.GetNormalizedName(),
               Type = CategoryType.Composition,
           })
           .ToList();
            var cts = db.Categories.Select(x => x.NormalizedTitle).ToList();
            foreach (var item in cts)
            {
                data.RemoveAll(x => x.NormalizedTitle == item);
            }
            if (data.Count > 0)
            {
                db.Categories.AddRange(data);
                db.SaveChanges();
            }
            if (Configuration["IsTesting"]?.ToLower() != "true")
                return;
            if (!db.Users.Any(x => x.NormalizedEmail == "ADMIN@MAIL.RU"))
            {
                using (var tran = db.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    db.Users.AddRange(CreateUsers());
                    db.SaveChanges();
                    var admin = db.Users.First(x => x.NormalizedEmail == "ADMIN@MAIL.RU");
                    var seller = db.Users.First(x => x.NormalizedEmail == "SELLER@MAIL.RU");
                    var user = db.Users.First(x => x.NormalizedEmail == "USER@MAIL.RU");
                    var adminRole = db.Roles.First(x => x.Name == "admin");
                    var sellerRole = db.Roles.First(x => x.Name == "seller");
                    db.UserRoles.Add(
                        new IdentityUserRole<string>() { UserId = admin.Id, RoleId = adminRole.Id }
                    );
                    db.UserRoles.Add(
                        new IdentityUserRole<string>() { UserId = seller.Id, RoleId = sellerRole.Id }
                    );
                    db.SaveChanges();
                    var id = Guid.NewGuid();
                    var partner = new PartnerDto()
                    {
                        Id = id,
                        Title = "Пекарня Seller",
                        NormalizedTitle = "Пекарня Seller".GetNormalizedName(),
                        ContactEmail = seller.Email,
                        CreatedById = seller.Id,
                        State = PartnerState.Confirmed,
                        Type = PartnerType.SelfEmployed,
                        Inn = "123456789012",
                        Employes = { new PartnerUser() { UserId = seller.Id, PartnerId = id } },
                        Address = new AddressDto { Street = "2-я Машиностроения", House = "9", ApartmentNumber = "9" },
                        ContactPhone = "+79104271878",
                        ExternalId = "223743"
                    };
                    db.Partners.Add(partner);
                    var shop = new ShopDto
                    {
                        Id = id,
                        PartnerId = partner.Id,
                    };
                    db.Shops.Add(shop);
                    db.SaveChanges();
                    tran.Commit();
                }
            }

            if (!db.Merchandises.Any())
            {
                var partner = db.Partners.First();
                db.Merchandises.Add(new MerchandiseDto
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Price = new MoneyDto { Value = 1, CurrencyType = CurrencyType.Rub },
                    ServingSize = 2,
                    AvailableQuantity = 1000,
                    ServingGrossWeightInKilograms = 0.1,
                    Title = "Печенье",
                    NormalizedTitle = "ПЕЧЕНЬЕ",
                    Description = "Вкусное шоколадное печенье",
                    State = MerchandisesState.Published,
                    ShopId = partner.Id,
                    UnitTypeType = MerchandiseUnitType.Kilograms,
                });
                db.SaveChanges();
            }

            if (!db.Ratings.Any())
            {
                var partner = db.Shops.Include(x => x.Partner).First();
                var merch = db.Merchandises.First();
                partner.Partner.Rating = 3;
                merch.Rating = 3;
                for (int i = 5; i < 25; i++)
                {
                    var user = new HikeUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        PhoneNumber = $"1234567890{i}",
                        UserName = $"User{i}",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        NormalizedEmail = $"USER{i}@MAIL.RU",
                        NormalizedUserName = $"USER{i}",
                        Email = $"user{i}@mail.ru",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    user.PasswordHash = _passwordHasher.HashPassword(user, $"Pass@word{i}");
                    db.Users.Add(user);
                    var pr = new RatingDto
                    {
                        ShopId = partner.Id,
                        EvaluatorId = user.Id,
                        RatingType = RatingType.Partner,
                        Rating = Random.Shared.Next(1, 5),
                        Comment = $"Комментарий про продавца {i}",
                    };
                    db.Ratings.Add(pr);
                    var mr = new RatingDto
                    {
                        MerchandiseId = merch.Id,
                        EvaluatorId = user.Id,
                        RatingType = RatingType.Merch,
                        Rating = Random.Shared.Next(1, 5),
                        Comment = $"Комментарий про товар {i}",
                    };
                    db.Ratings.Add(mr);
                }
                db.SaveChanges();
            }
        }

        private HikeUser[] CreateUsers()
        {
            var user = new HikeUser()
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567890",
                UserName = "User",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "USER@MAIL.RU",
                NormalizedUserName = "USER",
                Email = "user@mail.ru",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var admin = new HikeUser()
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567891",
                UserName = "Admin",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "ADMIN@MAIL.RU",
                NormalizedUserName = "ADMIN",
                Email = "admin@mail.ru",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var seller = new HikeUser()
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567891",
                UserName = "Seller",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "SELLER@MAIL.RU",
                NormalizedUserName = "SELLER",
                Email = "seller@mail.ru",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            admin.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");
            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word2");
            seller.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word3");
            return new[]
            {
               user,
               admin,
               seller
            };
        }

        private X509Certificate2 LoadFromFile()
        {
            using (var file = File.OpenRead("hiketest.pfx"))
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return new X509Certificate2(ms.ToArray());
            }
        }
    }
}
