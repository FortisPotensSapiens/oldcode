namespace Hike.Models.Base
{
    public class PageResultModel<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}