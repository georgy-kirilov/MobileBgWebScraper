namespace MobileBgWebScraper.App
{
    using System;
    using System.Globalization;

    using MobileBgWebScraper.Services;

    public static class TechnicalCharacteristicsParsers
    {
        public static void ParseManufacturingDate(string input, AdvertisementInputModel advertisement)
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

        public static void ParseKilometrage(string input, AdvertisementInputModel advertisement)
        {
            if (input == null)
            {
                return;
            }

            int kilometrage = int.Parse(input
                                         .Replace(" ", string.Empty)
                                         .ToLower()
                                         .Replace("км", string.Empty));

            advertisement.Kilometrage = kilometrage;
        }

        public static void ParseHorsePowers(string input, AdvertisementInputModel advertisement)
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

        public static void ParseEngineType(string input, AdvertisementInputModel advertisement)
        {
            advertisement.EngineType = input?.Trim();
        }

        public static void ParseTransmissionType(string input, AdvertisementInputModel advertisement)
        {
            advertisement.TransmissionType = input?.Trim();
        }

        public static void ParseBodyStyle(string input, AdvertisementInputModel advertisement)
        {
            advertisement.BodyStyle = input?.Trim();
        }

        public static void ParseColorName(string input, AdvertisementInputModel advertisement)
        {
            advertisement.ColorName = input?.Trim();
        }

        public static void ParseEuroStandard(string input, AdvertisementInputModel advertisement)
        {
            advertisement.EuroStandard = input?.Trim();
        }
    }
}
