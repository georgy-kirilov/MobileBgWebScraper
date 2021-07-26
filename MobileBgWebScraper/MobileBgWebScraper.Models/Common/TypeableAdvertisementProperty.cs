namespace MobileBgWebScraper.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public class TypeableAdvertisementProperty : OneToManyAdvertisementsProperty
    {
        [Required]
        public string Type { get; set; }
    }
}
