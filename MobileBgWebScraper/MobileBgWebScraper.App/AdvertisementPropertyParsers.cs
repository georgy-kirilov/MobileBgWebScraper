namespace MobileBgWebScraper.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;

    using static TechnicalCharacteristicsParsers;

    using MobileBgWebScraper.Services;

    public class AdvertisementPropertyParsers
    {
        public delegate void TechnicalCharacteristicsParser(string input, AdvertisementInputModel advertisement);

        public static readonly Dictionary<string, TechnicalCharacteristicsParser> CharacteristicsParsingTable = new()
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

        public static void ParseBrandAndModelName(IDocument document, AdvertisementInputModel advertisement)
        {
            var args = document
                        .QuerySelector("a.fastLinks")
                        .GetAttribute("href")
                        .Trim()
                        .Split("?")[1]
                        .Split("&");

            advertisement.BrandName = args[1].Split("=")[1];
            advertisement.ModelName = args[2].Split("=")[1];
        }

        public static void ParsePrice(IDocument document, AdvertisementInputModel advertisement)
        {
            try
            {
                string input = document
                                .QuerySelector("#details_price")?
                                .TextContent
                                .Replace("лв.", string.Empty)
                                .Replace(" ", string.Empty);

                advertisement.Price = decimal.Parse(input);
            }
            catch (Exception)
            {
                advertisement.Price = null;
            }
        }

        public static void ParseDescription(IDocument document, AdvertisementInputModel advertisement)
        {
            string description = document
                                  .QuerySelectorAll("form[name='search'] > table")[2]
                                  .QuerySelector("tbody > tr > td")?
                                  .TextContent;

            advertisement.Description = description;
        }

        public static void ParseRegionAndTownName(IDocument document, AdvertisementInputModel advertisement)
        {
            var addressBlocks = document.QuerySelectorAll("div.adress");
            int addressBlockIndex = addressBlocks.Length > 1 ? 1 : 0;
            string fullAddress = addressBlocks[addressBlockIndex].TextContent.Trim();

            var fullAddressArgs = fullAddress.Split(", ");
            advertisement.RegionName = fullAddressArgs[0];
            advertisement.TownName = fullAddressArgs[1];
        }

        public static void ParseTitle(IDocument document, AdvertisementInputModel advertisement)
        {
            advertisement.Title = document.QuerySelector("h1")?.TextContent;
        }

        public static void ParseViews(IDocument document, AdvertisementInputModel advertisement)
        {
            advertisement.Views = int.Parse(document.QuerySelector("span.advact")?.TextContent);
        }

        public static void ParseImageUrls(IDocument document, AdvertisementInputModel advertisement)
        {
            var imagesUrls = document
                              .QuerySelectorAll("div#pictures_moving > a")
                              .Select(a => a.GetAttribute("data-link"));

            foreach (string imageUrl in imagesUrls)
            {
                advertisement.ImageUrls.Add(imageUrl);
            }
        }

        public static void ParseTechnicalCharacteristics(IDocument document, AdvertisementInputModel advertisement)
        {
            var carProperties = document.QuerySelectorAll("ul.dilarData > li");

            for (int i = 0; i < carProperties.Length; i += 2)
            {
                string currentPropertyName = carProperties[i].TextContent.ToLower();
                string currentPropertyValue = carProperties[i + 1].TextContent;

                CharacteristicsParsingTable[currentPropertyName].Invoke(currentPropertyValue, advertisement);
            }
        }

        public static void ParseRemoteId(string advertisementUrl, AdvertisementInputModel advertisement)
        {
            string remoteId = advertisementUrl.Split("?")[1].Split("&")[1].Split("=")[1];
            advertisement.RemoteId = remoteId;
        }
    }
}
