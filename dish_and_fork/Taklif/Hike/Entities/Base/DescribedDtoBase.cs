namespace Hike.Entities.Base
{
    public abstract class DescribedDtoBase : TitledDtoBase
    {
        [StringLength(1000)]
        public string? Description { get; set; }
    }
}
