namespace ApeBotApi.Models
{
    /// <summary>
    /// Внутрення ошибка сервера
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Сообщения 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Идентификатор по которому можно в логах остследить что произошло на сервере
        /// </summary>
        public string TraceId { get; set; }
        /// <summary>
        /// Время сервера по которому можно отстелидить в логах что происходило на сервере
        /// </summary>
        public DateTime ServerTime { get; set; }
    }
}
