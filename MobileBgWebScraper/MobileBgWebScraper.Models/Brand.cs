namespace MobileBgWebScraper.Models
{
    using MobileBgWebScraper.Models.Common;

    using System.Collections.Generic;

    public class Brand : NameableAdvertisementProperty
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public virtual ICollection<Model> Models { get; set; }
    }
}
