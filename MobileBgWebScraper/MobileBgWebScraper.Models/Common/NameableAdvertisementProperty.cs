namespace MobileBgWebScraper.Models.Common
{
    using System.ComponentModel.DataAnnotations;

    public abstract class NameableAdvertisementProperty : OneToManyAdvertisementsProperty
    {
        [Required]
        public string Name { get; set; }
    }
}
