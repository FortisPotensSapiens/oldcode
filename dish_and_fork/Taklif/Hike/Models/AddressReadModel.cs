using Hike.Entities;

namespace Hike.Models
{
    public class AddressReadModel
    {

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// Домофон (код домофона)
        /// </summary>
        public string Intercom { get; set; }
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
        public string ApartmentNumber { get; set; }
        /// <summary>
        /// Подьезд
        /// </summary>
        public string Entrance { get; set; }
        public string Country { get; set; }
        /// <summary>
        /// Для России это область за исключением Москвы и Питера которые сами являются регионами
        /// Для США это Штат.
        /// Для Китая надо разбиратся что там у них является еденицей административной в которую входя несколько городов.
        /// </summary>
        public string Region { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public AddressDto ToAddress() => new AddressDto()
        {
            ZipCode = ZipCode,
            ApartmentNumber = ApartmentNumber,
            Entrance = Entrance,
            Intercom = Intercom,
            Street = Street,
            House = House,
            Country = Country,
            Region = Region,
            City = City,
            Longitude = Longitude,
            Latitude = Latitude
        };

        public static AddressReadModel From(AddressDto dto)
        {
            if (dto == null)
                return null;
            return new AddressReadModel()
            {
                ZipCode = dto.ZipCode,
                Intercom = dto.Intercom,
                Street = dto.Street,
                House = dto.House,
                ApartmentNumber = dto.ApartmentNumber,
                Entrance = dto.Entrance,
                Country = dto.Country,
                Region = dto.Region,
                City = dto.City,
                Longitude = dto.Longitude ?? 0,
                Latitude = dto.Latitude ?? 0
            };
        }
    }
}
