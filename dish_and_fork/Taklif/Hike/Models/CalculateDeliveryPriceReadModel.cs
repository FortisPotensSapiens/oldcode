public class CalculateDeliveryPriceReadModel
{
    /// <summary>
    /// Цена доставки
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Дата и время начала интервала когда курьер сможет доставить заказ
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// Дата и время конца интервала когда курьер сможет доставить заказ
    /// </summary>
    public DateTime FinishTime { get; set; }
}
//required_start_datetime timestamp / null
//Ожидаемое время прибытия курьера на адрес (от).

//Значение по умолчанию: null.

//Для заказов типа same_day
//Для заказов типа same_day должно быть задано ровно 2 точки. Причем, на первой точке required_start_datetime не должен быть задан, а на второй точке required_start_datetime должен соответствовать диапозону, который можно получить из метода получения списка интервалов

//required_finish_datetime timestamp / null
//Ожидаемое время прибытия курьера на адрес (до).

//Значение по умолчанию: null.

//Для заказов типа same_day
//Для заказов типа same_day должно быть задано ровно 2 точки. Причем, на первой точке required_finish_datetime не должен быть задан, а на второй точке required_finish_datetime должен соответствовать диапозону, который можно получить из метода получения списка интервалов
