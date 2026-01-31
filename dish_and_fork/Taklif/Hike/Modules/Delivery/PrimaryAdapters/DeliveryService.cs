using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Daf.DeliveryModule.Domain;
using Daf.DeliveryModule.SecondaryAdaptersInterfaces;
using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.DeliveryModule.PrimaryAdapters
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDostavistaClient _dostavista;
        private readonly IDostavistaSettingsRepository _settings;

        public DeliveryService(IDostavistaClient dostavista, IDostavistaSettingsRepository settings)
        {
            _dostavista = dostavista;
            _settings = settings;
        }

        public async Task<(Money price, DateTime startTime, DateTime finishTime)> CalculateDeliveryPrice(
            NotDefaultDateTime? deliveryDate,
            Kilograms weight,
            StreetName sellerStreet,
            HouseNumber sellerhouseNumber,
            StreetName buyerStreet,
        HouseNumber buyerHouseNumber
            )
        {
            var intervals = await _dostavista.GetDeliveryIntervals(deliveryDate?.Value);
            var interval = intervals.GetEarliest();

            var request = new DostavistaCalculateOrderRequest
            {
                Matter = "Сладости",
                Type = DostavistaOrderType.same_day,
                TotalWeightKg = (uint)Math.Ceiling(weight),
                Points = new()
                {
                    new DostavistaPoint
                    {
                        Address = $"Москва, {sellerStreet.Value}, {sellerhouseNumber.Value}",
                        ContactPerson = new DostavistaContact{ Name = "Петя", Phone = "+79857428504"}
                    },
                    new DostavistaPoint
                    {
                        Address = $"Москва, {buyerStreet.Value}, {buyerHouseNumber.Value}",
                        ContactPerson = new DostavistaContact{ Name = "Вася", Phone = "+79857428505"},
                        RequiredStartDatetime = interval.RequiredStartDatetime,
                        RequiredFinishDatetime = interval.RequiredFinishDatetime
                    }
                }
            };
            var response = await _dostavista.CalculateOrder(request);
            if (!response.IsSuccessful)
                throw new ApplicationException("Ошибка при попытке вычислить стоимость заказа!")
                {
                    Data = { ["args"] = new
                    {
                        response
                    }}
                };
            var point = response.Order.points.OrderByDescending(x => x.required_start_datetime).ThenByDescending(x => x.required_finish_datetime).First();
            return (new Rub(decimal.Parse(response.Order.payment_amount)), point.required_start_datetime ?? default, point.required_finish_datetime ?? default);
        }

        public async Task<DeliveryInfo> Delivery(Address from, Address to, NotDefaultDateTime? deliveryDate, double weight, IEnumerable<(string wareCode, string description, int itemsCount, string itemPaymentAmount)> items, string clientOrderId)
        {
            var packages = items.Select(x => new DostavistaPakage
            {
                WareCode = x.wareCode,
                Description = x.description,
                ItemsCount = x.itemsCount.ToString(),
                ItemPaymentAmount = x.itemPaymentAmount
            }).ToList();

            var intervals = await _dostavista.GetDeliveryIntervals(deliveryDate?.Value);
            var interval = intervals.GetEarliest();

            var request = new DostavistaCalculateOrderRequest
            {
                Matter = "Сладости",
                Type = DostavistaOrderType.same_day,
                TotalWeightKg = (uint)Math.Ceiling(weight),
                Points = new()
                {
                    new DostavistaPoint
                    {
                        Address = $"Москва, {from.Street}, {from.House}",
                        ContactPerson = new DostavistaContact{ Name = "Петя", Phone = "+79857428504"}
                    },
                    new DostavistaPoint
                    {
                        Address = $"Москва, {to.Street}, {to.House}",
                        ContactPerson = new DostavistaContact{ Name = "Вася", Phone = "+79857428505"},
                        RequiredStartDatetime = interval.RequiredStartDatetime,
                        RequiredFinishDatetime = interval.RequiredFinishDatetime
                    }
                }
            };
            var response = await _dostavista.CreateOrder(request);
            if (!response.IsSuccessful)
                throw new ApplicationException("Ошибка при попытке запустить доставку заказа!")
                {
                    Data = { ["args"] = new
                    {
                        response
                    }}
                };
            var order = response.Order;
            var res = (
               new Rub(decimal.Parse(order.payment_amount)),
                order.points[0].tracking_url?.ToString(),
                order.points[0].required_start_datetime ?? default,
                order.points[0].required_finish_datetime ?? default,
                order.points[1].tracking_url?.ToString(),
                order.points[1].required_start_datetime ?? default,
                order.points[1].required_finish_datetime ?? default
                );
            return new DeliveryInfo(
                res.Item1,
                new PeriodEntity(res.Item3, res.Item4),
                res.Item2,
                new PeriodEntity(res.Item6, res.Item7),
                res.Item5
                );
        }

        public async Task<List<DeliveryInterval>> GetDeliveryIntervals(NotDefaultDateTime? date = null)
        {
            var response = await _dostavista.GetDeliveryIntervals(date?.Value);
            return response.DeliveryIntervals;
        }

        public bool IsOrderChangedEventSignatureValid(string signature, string txt)
        {
            using var sha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.GetCallBackToken()));
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(txt));
            return Encoding.UTF8.GetBytes(signature).SequenceEqual(hash);
        }
    }
}
