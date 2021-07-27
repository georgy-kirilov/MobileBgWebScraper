namespace MobileBgWebScraper.Services
{
    using System;
    using System.Collections.Generic;

    public class AdvertisementInputModel
    {
        public AdvertisementInputModel()
        {
            ImageUrls = new HashSet<string>();
        }

        public string RemoteId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string RegionName { get; set; }

        public string TownName { get; set; }

        public decimal? Price { get; set; }

        public int HorsePowers { get; set; }

        public int Kilometrage { get; set; }

        public int Views { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public string EngineType { get; set; }

        public string TransmissionType { get; set; }

        public string BodyStyle { get; set; }

        public string ColorName { get; set; }

        public string EuroStandard { get; set; }

        public ICollection<string> ImageUrls { get; set; }
    }
}
