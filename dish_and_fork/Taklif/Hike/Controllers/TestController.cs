using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Hike.Clients;
using Hike.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Runtime.Intrinsics.Arm;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;

namespace Hike.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;
        private readonly IDostavistaClient _dostavistaClient;
        private readonly IYooKassaClient _yooKassa;
        private readonly IBaseUriRepository _baseUri;

        public TestController(ILogger<TestController> logger, IDostavistaClient dostavistaClient,
            IYooKassaClient yooKassa,
           IBaseUriRepository baseUri
            )
        {
            _logger = logger;
            _dostavistaClient = dostavistaClient;
            _yooKassa = yooKassa;
            _baseUri = baseUri;
        }

        [HttpPost("calculate-yookassa-pyament-self-delivered/{id}")]
        public async Task<string> CalculateYooKassaPaymentSelfDelivered([FromRoute] string id)
        {
            var data = await _yooKassa.CratePayment(Guid.NewGuid().ToString(), 0m, 22.2m,
           new Uri(_baseUri.Get(), $"api/v1/pay-any-way/default/{Guid.NewGuid()}").AbsoluteUri,
              Guid.NewGuid().ToString(), id);
            return data.redirect;
        }

        [HttpPost("calculate-yookassa-pyament/{id}")]
        public async Task<string> CalculateYooKassaPayment([FromRoute] string id)
        {
            var data = await _yooKassa.CratePayment(Guid.NewGuid().ToString(), 11.1m, 22.2m,
           new Uri(_baseUri.Get(), $"api/v1/pay-any-way/default/{Guid.NewGuid()}").AbsoluteUri,
              Guid.NewGuid().ToString(), id);
            return data.redirect;
        }


        [HttpPost("calculate-dostavista-order")]
        public async Task<DostavistaCalculateOrderResponse> CalculateDoctavistaOrder(DostavistaCalculateOrderRequest request)
        {
            return await _dostavistaClient.CalculateOrder(request);
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public void Create(WeatherForecast model)
        {
            throw new ApplicationException("Ошибка валидации")
            {
                Data = { ["id"] = 3 }
            };
        }

        [HttpPut]
        public void Update()
        {
            throw new Exception("Ошибка");
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> GetTest([FromQuery] TestGetModel model)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
                .ToArray();
        }

        [HttpGet("[action]")]
        public IActionResult Get403Error()
        {
            return Forbid();
        }

        public class TestGetModel
        {
            public int Id { get; set; }
            public List<string> Names { get; set; }
            public TestRng TestRng { get; set; }
            public List<TestRng> TestList { get; set; }
        }

        public class TestRng
        {
            public int From { get; set; }
            public int To { get; set; }
            public List<int> Names { get; set; }
        }
    }
}
