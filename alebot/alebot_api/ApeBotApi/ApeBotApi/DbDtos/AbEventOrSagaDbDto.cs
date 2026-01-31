using AleBotApi.DbDtos.Enums;
using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbEventsOrSagas")]
    public class AbEventOrSagaDbDto : EntityDbDtoBase
    {
        public DateTime? CompletedDate { get; set; }
        public string? Body { get; set; }
        public AbEventOrSagaType Type { get; set; }
    }
}
