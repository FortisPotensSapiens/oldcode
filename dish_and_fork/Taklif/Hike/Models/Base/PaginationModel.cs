namespace Hike.Models.Base
{
    public class PaginationModel
    {
        private const uint MAX_ITEMS_PER_PAGE = 1000;

        /// <summary>
        /// Номер страницы (начиная с 1)
        /// </summary>
        [Range(1, uint.MaxValue)]
        public uint PageNumber { get; set; } = 1;

        /// <summary>
        /// Размер страницы (максимум 1000)
        /// </summary>
        [Range(1, MAX_ITEMS_PER_PAGE)]
        public uint PageSize { get; set; } = MAX_ITEMS_PER_PAGE;

        public uint Skip() => (PageNumber - 1) * PageSize;
    }
}
