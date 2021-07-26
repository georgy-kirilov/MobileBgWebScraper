namespace MobileBgWebScraper.Models
{
    using MobileBgWebScraper.Models.Common;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class Image : BaseModel<int>
    {
        [Required]
        public string Url { get; set; }

        public Guid AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }
    }
}
