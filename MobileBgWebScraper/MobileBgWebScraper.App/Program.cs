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

            var propertiesParsingTable = new Dictionary<string, Action<string, Advertisement>>()
            {
                { "дата на производство", ParseManufacturingDate },
                { "тип двигател", (input, advert) => advert.Engine = input?.Trim() },
                { "мощност", ParseHorsePowers },
                { "скоростна кутия", (input, advert) => advert.Transmission = input?.Trim() },
                { "категория", (input, advert) => advert.BodyStyle = input?.Trim() },
                { "пробег", ParseKilometrage },
                { "цвят", (input, advert) => advert.Color = input?.Trim() },
                { "евростандарт", (input, advert) => advert.EuroStandard = input?.Trim() },
            };

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

                    var carProperties = advertisementDocument.QuerySelectorAll("ul.dilarData > li");

                    for (int i = 0; i < carProperties.Length; i += 2)
                    {
                        string currentPropertyName = carProperties[i].TextContent.ToLower();
                        string currentPropertyValue = carProperties[i + 1].TextContent;

                        propertiesParsingTable[currentPropertyName].Invoke(currentPropertyValue, advertisement);
                    }

                    try
                    {
                        string priceAsString = advertisementDocument
                                                .QuerySelector("#details_price")
                                                .TextContent
                                                .Replace("лв.", string.Empty)
                                                .Replace(" ", string.Empty);

                        advertisement.Price = decimal.Parse(priceAsString);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    advertisement.Views = int.Parse(advertisementDocument.QuerySelector("span.advact").TextContent);

                    var imagesUrls = advertisementDocument
                                                .QuerySelectorAll("div#pictures_moving > a")
                                                .Select(a => a.GetAttribute("data-link"));

                    foreach (string imageUrl in imagesUrls)
                    {
                        advertisement.ImagesUrls.Add(imageUrl);
                    }

                    advertisements.Add(advertisement);
                }
            }

            Console.WriteLine(advertisements.Count);
        }

        public static void ParseManufacturingDate(string input, Advertisement advertisement)
        {
            if (input == null)
            {
                return;
            }

            string rawDateInput = input.Replace("г.", string.Empty);
            string[] rawDateInputArgs = rawDateInput.Split(" ");

            int month = DateTime.ParseExact(rawDateInputArgs[0], "MMMM", new CultureInfo("bg-BG")).Month;
            int year = int.Parse(rawDateInputArgs[1]);

            advertisement.ManufacturingDate = new DateTime(year, month, 1);
        }

        public static void ParseHorsePowers(string input, Advertisement advertisement)
        {
            if (input == null)
            {
                return;
            }

            int horsePowers = int.Parse(input
                                        .Replace(" ", string.Empty)
                                        .ToLower()
                                        .Replace("к.с.", string.Empty));

            advertisement.HorsePowers = horsePowers;
        }

        public static void ParseKilometrage(string input, Advertisement advertisement)
        {
            if (input == null)
            {
                return;
            }

            int kilometrage = int.Parse(input.Replace(" ", string.Empty).ToLower().Replace("км", string.Empty));
            advertisement.Kilometrage = kilometrage;
        }
    }
}
