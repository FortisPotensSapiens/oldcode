namespace Daf.SharedModule.Domain
{

    public record Address
    {
        public Address(
            StreetName street,
            HouseNumber house,
            ApartmentNumber apartmentNumber,
            IntercomCode intercom,
            EntranceNumber entrance,
            FloorNumber floorNumber,
            ZipCode? ZipCode,
            Longitude? Longitude,
            Latitude? Latitude
            )
        {
            Street = street;
            House = house;
            ApartmentNumber = apartmentNumber;
            Intercom = intercom;
            Entrance = entrance;
            FloorNumber = floorNumber;
            this.ZipCode = ZipCode;
            this.Longitude = Longitude;
            this.Latitude = Latitude;
        }

        protected Address(Address other)
        {
            Street = other.Street;
            House = other.House;
            ApartmentNumber = other.ApartmentNumber;
            Intercom = other.Intercom;
            Entrance = other.Entrance;
            FloorNumber = other.FloorNumber;
        }

        /// <summary>
        /// Название улиц
        /// </summary>
        public StreetName Street { get; init; }
        /// <summary>
        /// Номер дома
        /// </summary>
        public HouseNumber House { get; init; }
        /// <summary>
        /// Номер квартиры
        /// </summary>
        public ApartmentNumber ApartmentNumber { get; init; }
        /// <summary>
        /// Код домофона
        /// </summary>
        public IntercomCode Intercom { get; init; }
        /// <summary>
        /// Пoдьезд
        /// </summary>
        public EntranceNumber Entrance { get; init; }
        /// <summary>
        /// Этаж
        /// </summary>
        public FloorNumber FloorNumber { get; init; }
        public ZipCode? ZipCode { get; init; }
        public Longitude? Longitude { get; init; }
        public Latitude? Latitude { get; init; }
    }

}

