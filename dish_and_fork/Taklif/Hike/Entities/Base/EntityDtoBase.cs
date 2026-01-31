namespace Hike.Entities.Base
{
    public abstract class EntityDtoBase : DbDtoBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
