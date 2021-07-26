namespace MobileBgWebScraper.Models.Common
{
    using System.Collections.Generic;

    public abstract class OneToManyAdvertisementsProperty : BaseModel<int>
    {
        public OneToManyAdvertisementsProperty()
        {
            Advertisements = new HashSet<Advertisement>();
        }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
