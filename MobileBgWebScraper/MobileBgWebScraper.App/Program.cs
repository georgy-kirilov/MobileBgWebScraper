namespace MobileBgWebScraper.App
{
    using MobileBgWebScraper.Data;
    using MobileBgWebScraper.Models;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AngleSharp;

    using static AdvertisementPropertyParsers;

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

            var propertiesParsingTable = new Dictionary<string, Action<string, AdvertisementInputModel>>()
            {
                { "дата на производство", ParseManufacturingDate },
                { "тип двигател", ParseEngineType },
                { "мощност", ParseHorsePowers },
                { "скоростна кутия", ParseTransmissionType },
                { "категория", ParseBodyStyle },
                { "пробег", ParseKilometrage },
                { "цвят", ParseColorName },
                { "евростандарт", ParseEuroStandard },
            };

            for (int page = 1; page <= 10; page++)
            {
                var document = await context.OpenAsync($"{address}{page}");
                var urls = document.QuerySelectorAll(query).Select(a => a.GetAttribute("href").Trim());

                foreach (string url in urls)
                {
                    var input = new AdvertisementInputModel();
                    var advertisementDocument = await context.OpenAsync($"https:{url}");

                    input.Title = advertisementDocument.QuerySelector("h1").TextContent;

                    string href = advertisementDocument.QuerySelector("a.fastLinks").GetAttribute("href").Trim();
                    var hrefArgs = href.Split("?")[1].Split("&");

                    input.BrandName = hrefArgs[1].Split("=")[1];
                    input.ModelName = hrefArgs[2].Split("=")[1];

                    var carProperties = advertisementDocument.QuerySelectorAll("ul.dilarData > li");

                    for (int i = 0; i < carProperties.Length; i += 2)
                    {
                        string currentPropertyName = carProperties[i].TextContent.ToLower();
                        string currentPropertyValue = carProperties[i + 1].TextContent;

                        propertiesParsingTable[currentPropertyName].Invoke(currentPropertyValue, input);
                    }

                    try
                    {
                        string priceAsString = advertisementDocument
                                                .QuerySelector("#details_price")
                                                .TextContent
                                                .Replace("лв.", string.Empty)
                                                .Replace(" ", string.Empty);

                        input.Price = decimal.Parse(priceAsString);
                    }
                    catch (Exception)
                    {
                        input.Price = null;
                    }

                    input.Views = int.Parse(advertisementDocument.QuerySelector("span.advact").TextContent);

                    var imagesUrls = advertisementDocument
                                                .QuerySelectorAll("div#pictures_moving > a")
                                                .Select(a => a.GetAttribute("data-link"));

                    foreach (string imageUrl in imagesUrls)
                    {
                        input.ImageUrls.Add(imageUrl);
                    }

                    input.Description = advertisementDocument
                                                        .QuerySelectorAll("form[name='search'] > table")[2]
                                                        .QuerySelector("tbody > tr > td")
                                                        .TextContent;

                    var addressBlocks = advertisementDocument.QuerySelectorAll("div.adress");
                    int addressBlockIndex = addressBlocks.Length > 1 ? 1 : 0;
                    string fullAddress = addressBlocks[addressBlockIndex].TextContent.Trim();

                    var fullAddressArgs = fullAddress.Split(", ");

                    input.RegionName = fullAddressArgs[0];
                    input.TownName = fullAddressArgs[1];
                }
            }
        }
    }
}
