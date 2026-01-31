namespace Hike.Entities.Base
{
    public abstract class DbDtoBase
    {

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [ConcurrencyCheck]
        [MaxLength(100)]
        public string ConcurrencyToken { get; set; }
        [MaxLength(100)]
        public string CreatedById { get; set; }
        [MaxLength(100)]
        public string UpdatedById { get; set; }
    }
}
