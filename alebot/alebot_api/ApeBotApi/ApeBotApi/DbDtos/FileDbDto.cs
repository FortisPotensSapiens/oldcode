using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AleBotApi.DbDtos
{
    [Index(nameof(Hash), IsUnique = true)]
    public class FileDbDto : EntityDbDtoBase
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public byte[] Hash { get; set; }
        public long Size { get; set; }
        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }
        [Required]
        [MaxLength(100)]
        public string Extention { get; set; }
    }
}
