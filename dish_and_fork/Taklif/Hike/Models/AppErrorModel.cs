using System.Collections;

namespace Hike.Models
{
    /// <summary>
    /// Данные нарушили внутренние условия сервера
    /// </summary>
    /// <inheritdoc cref="ErrorModel"/>
    public class AppErrorModel : ErrorModel
    {
        /// <summary>
        /// Полнительные данные о ошибке
        /// Наример {id:3, name:'user'}
        /// </summary>
        public IDictionary Data { get; set; } = new Dictionary<string, string>();
    }
}
