namespace Hike.Entities
{
    public class MerchandiseImageDto : DbDtoBase
    {
        public MerchandiseDto Merchandise { get; set; }
        public Guid MerchandiseId { get; set; }
        public FileDto File { get; set; }
        public Guid FileId { get; set; }
        /// <summary>
        /// Порядок в котором отображается эта картинка
        /// </summary>
        public int Order { get; set; }
    }
}
