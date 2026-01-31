namespace Hike.Entities.Base
{
    public abstract class TitledDtoBase : EntityDtoBase
    {

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string NormalizedTitle { get; set; }
    }
}
