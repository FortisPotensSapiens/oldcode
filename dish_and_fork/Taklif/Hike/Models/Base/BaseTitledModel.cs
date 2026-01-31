namespace Hike.Models.Base
{
    public abstract class BaseTitledModel
    {
        protected BaseTitledModel()
        {

        }
        protected BaseTitledModel(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Название
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Title { get; set; }
    }
}
