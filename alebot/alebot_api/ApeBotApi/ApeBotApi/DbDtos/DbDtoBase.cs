using System.ComponentModel.DataAnnotations;

namespace ApeBotApi.DbDtos
{
    public abstract class DbDtoBase
    {

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [ConcurrencyCheck]
        [MaxLength(100)]
        public string ConcurrencyToken { get; set; } = string.Empty;
    }
}
