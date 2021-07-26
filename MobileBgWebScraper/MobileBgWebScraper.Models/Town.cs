namespace MobileBgWebScraper.Models
{
    using MobileBgWebScraper.Models.Common;

    public class Town : NameableAdvertisementProperty
    {
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}
