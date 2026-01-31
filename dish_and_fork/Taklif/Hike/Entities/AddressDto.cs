

namespace Hike.Entities
{
    public class AddressDto
    {
        public string? ZipCode { get; set; } = "777888";
        public string Country { get; set; } = "Россия";
        public string Region { get; set; } = "Москва";
        public string City { get; set; } = "Москва";
        public string Street { get; set; }
        /// <summary>
        /// Номер дома
        /// </summary>
        public string House { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Intercom { get; set; }
        /// <summary>
        /// ПОдьезд
        /// </summary>
        public string? Entrance { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        /// <summary>
        /// Этаж
        /// </summary>
        public string? FloorNumber { get; set; }
    }
}
