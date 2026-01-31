namespace Hike.Models.Base
{
    public abstract class BaseDescribedModel : BaseTitledModel
    {
        protected BaseDescribedModel()
        {

        }

        protected BaseDescribedModel(string title, string description) : base(title)
        {
            Description = description;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}
