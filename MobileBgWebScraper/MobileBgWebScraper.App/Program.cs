namespace MobileBgWebScraper.App
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using AngleSharp;
    using MobileBgWebScraper.Models;

    public class Program
    {
        public static async Task Main()
        {
            var advertisements = new List<Advertisement>();

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var address = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=kvo3k4&f1=";
            var query = "a.mmm";

            for (int page = 1; page <= 150; page++)
            {
                var document = await context.OpenAsync($"{address}{page}");
                var urls = document.QuerySelectorAll(query).Select(a => a.GetAttribute("href").Trim());

                foreach (var url in urls)
                {
                    var advertisement = new Advertisement();
                    var advertisementDocument = await context.OpenAsync($"https:{url}");

                    advertisement.Title = advertisementDocument.QuerySelector("h1").TextContent;

                    var href = advertisementDocument.QuerySelector("a.fastLinks").GetAttribute("href").Trim();
                    var hrefArgs = href.Split("?")[1].Split("&");
                    advertisement.Brand = hrefArgs[1].Split("=")[1];
                    advertisement.Model = hrefArgs[2].Split("=")[1];

                    var carProperties = advertisementDocument
                        .QuerySelectorAll("ul.dilarData > li")
                        .Select(l => l.TextContent)
                        .ToArray();

                    string rawDateInput = carProperties[1].Replace("г.", string.Empty);
                    string[] rawDateInputArgs = rawDateInput.Split(" ");
                    
                    int month = DateTime.ParseExact(rawDateInputArgs[0], "MMMM", new CultureInfo("bg-BG")).Month;
                    int year = int.Parse(rawDateInputArgs[1]);
                    advertisement.ManufacturingDate = new DateTime(year, month, 1);

                    advertisement.Engine = carProperties[3];
                    advertisement.HorsePowers = int.Parse(carProperties[5].Replace("к.с.", string.Empty).Trim());
                    advertisement.EuroStandard = carProperties[7];
                    advertisement.Transmission = carProperties[9];
                    advertisement.BodyStyle = carProperties[11];
                    advertisement.Kilometrage = int.Parse(carProperties[13].Replace("км", string.Empty).Replace(" ", string.Empty));
                    advertisement.Color = carProperties[15];

                    advertisement.Price = decimal.Parse(
                        advertisementDocument.QuerySelector("#details_price")
                                             .TextContent
                                             .Replace("лв.", string.Empty)
                                             .Replace(" ", string.Empty));

                    advertisement.Views = int.Parse(advertisementDocument.QuerySelector("span.advact").TextContent);
                }
            }
        }
    }
}
