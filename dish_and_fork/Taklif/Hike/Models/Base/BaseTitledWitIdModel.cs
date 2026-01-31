namespace Hike.Models.Base
{
    public class BaseTitledWitIdModel : BaseTitledModel
    {
        [Obsolete]
        public BaseTitledWitIdModel() : base(default)
        {

        }

        public BaseTitledWitIdModel(Guid routeId, DateTime created, string title) : base(title)
        {
            RouteId = routeId;
            Created = created;
        }

        public Guid RouteId { get; set; }
        public DateTime Created { get; set; }
    }
}
