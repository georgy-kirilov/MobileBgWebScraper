namespace MobileBgWebScraper.Models
{
    using MobileBgWebScraper.Models.Common;

    public class Model : NameableAdvertisementProperty
    {
        public int BrandId { get; set; }

        public Brand Brand { get; set; }
    }
}