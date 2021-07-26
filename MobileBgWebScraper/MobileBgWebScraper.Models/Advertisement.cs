namespace MobileBgWebScraper.Models
{
    using MobileBgWebScraper.Models.Common;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Advertisement : BaseModel<Guid>
    {
        public Advertisement()
        {
            Images = new HashSet<Image>();
        }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public int HorsePowers { get; set; }

        public int Kilometrage { get; set; }

        public int Views { get; set; }

        public int BodyStyleId { get; set; }

        public BodyStyle BodyStyle { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        public int ColorId { get; set; }

        public Color Color { get; set; }

        public int EngineId { get; set; }

        public Engine Engine { get; set; }

        public int? EuroStandardId { get; set; }

        public EuroStandard EuroStandard { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public int TransmissionId { get; set; }

        public Transmission Transmission { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
