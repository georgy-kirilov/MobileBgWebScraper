namespace MobileBgWebScraper.App
{
    using static AdvertisementPropertyParsers;

    using AngleSharp.Dom;
    using MobileBgWebScraper.Services;

    public class AdvertisementParser
    {
        public static AdvertisementInputModel Parse(string advertisementUrl, IDocument document)
        {
            var inputModel = new AdvertisementInputModel();

            ParseRemoteId(advertisementUrl, inputModel);

            ParseTitle(document, inputModel);
            ParseBrandAndModelName(document, inputModel);

            ParseTechnicalCharacteristics(document, inputModel);
            ParsePrice(document, inputModel);

            ParseViews(document, inputModel);
            ParseImageUrls(document, inputModel);

            ParseDescription(document, inputModel);
            ParseRegionAndTownName(document, inputModel);

            return inputModel;
        }
    }
}
