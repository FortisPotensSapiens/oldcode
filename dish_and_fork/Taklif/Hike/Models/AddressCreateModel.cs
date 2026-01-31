using Hike.Entities;

namespace Hike.Models
{
    public class AddressCreateModel
    {
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string? ZipCode { get; set; }
        /// <summary>
        /// Домофон (код домофона)
        /// </summary>
        public string? Intercom { get; set; }
        /// <summary>
        /// Название улицы
        /// </summary>
        [Required]
        public string Street { get; set; }
        /// <summary>
        /// Номер здания 
        /// </summary>
        [Required]
        public string House { get; set; }
        /// <summary>
        /// Номер квартиры или офиса
        /// </summary>
        public string? ApartmentNumber { get; set; }
        /// <summary>
        /// Подьезд
        /// </summary>
        public string? Entrance { get; set; }
        /// <summary>
        /// Долгота 
        /// </summary>
        public double? Longitude { get; set; }
        /// <summary>
        /// Широта
        /// </summary>
        public double? Latitude { get; set; }

        public AddressDto ToAddress() => new AddressDto()
        {
            ZipCode = ZipCode,
            ApartmentNumber = ApartmentNumber,
            Entrance = Entrance,
            Intercom = Intercom,
            Street = Street,
            House = House,
            Longitude = Longitude ?? 0,
            Latitude = Latitude ?? 0
        };

        public void Map(AddressDto dto)
        {
            dto.ZipCode = ZipCode;
            dto.ApartmentNumber = ApartmentNumber;
            dto.Entrance = Entrance;
            dto.Intercom = Intercom;
            dto.Street = Street;
            dto.House = House;
            dto.Longitude = Longitude ?? 0;
            dto.Latitude = Latitude ?? 0;
        }
    }
}
