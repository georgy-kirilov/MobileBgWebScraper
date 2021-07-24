namespace MobileBgWebScraper.Models
{
    using System;
    using System.Collections.Generic;

    public class Advertisement
    {
        public Advertisement()
        {
            ImagesUrls = new HashSet<string>();
        }

        public string Title { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public string Engine { get; set; }

        public int HorsePowers { get; set; }

        public string Transmission { get; set; }

        public string EuroStandard { get; set; }

        public int Kilometrage { get; set; }

        public string Color { get; set; }

        public string BodyStyle { get; set; }

        public int Views { get; set; }

        public string Description { get; set; }

        public ICollection<string> ImagesUrls { get; set; }
    }
}
