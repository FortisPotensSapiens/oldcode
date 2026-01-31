using System.ComponentModel.DataAnnotations;

namespace ApeBotApi.DbDtos
{
    public abstract class EntityDbDtoBase : DbDtoBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
