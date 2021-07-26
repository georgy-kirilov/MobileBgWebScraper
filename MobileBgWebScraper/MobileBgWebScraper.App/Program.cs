namespace MobileBgWebScraper.App
{
    using MobileBgWebScraper.Data;
    using MobileBgWebScraper.Models;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AngleSharp;

    public class Program
    {
        public static async Task Main()
        {
            Startup.ConfigureDatabase();

            var dbContext = new MobileBgDbContext();
            await dbContext.Database.EnsureCreatedAsync();

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var address = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=kvo3k4&f1=";
            var query = "a.mmm";

            
            for (int page = 1; page <= 10; page++)
            {
                var document = await context.OpenAsync($"{address}{page}");
                var urls = document.QuerySelectorAll(query).Select(a => a.GetAttribute("href").Trim());

                foreach (string url in urls)
                {
                    var advertisementDocument = await context.OpenAsync($"https:{url}");
                    var inputModel = AdvertisementParser.Parse(advertisementDocument);
                }
            }
        }       
    }
}
