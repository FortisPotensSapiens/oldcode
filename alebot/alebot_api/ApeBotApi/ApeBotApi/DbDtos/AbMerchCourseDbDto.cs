using AleBotApi.Migrations;
using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbMerchCourses")]
    [Index(nameof(MerchId), nameof(CourseId), IsUnique = true)]
    public class AbMerchCourseDbDto : EntityDbDtoBase
    {
        public AbMerchDbDto Merch { get; set; }
        public Guid MerchId { get; set; }
        public AbCourseDbDto Course { get; set; }
        public Guid CourseId { get; set; }
    }
}
