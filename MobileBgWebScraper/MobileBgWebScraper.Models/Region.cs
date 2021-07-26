namespace MobileBgWebScraper.Models
{
    using MobileBgWebScraper.Models.Common;

    using System.Collections.Generic;

    public class Region : NameableAdvertisementProperty
    {
        public Region()
        {
            Towns = new HashSet<Town>();
        }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
