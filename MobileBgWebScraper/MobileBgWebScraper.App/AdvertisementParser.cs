namespace MobileBgWebScraper.App
{
    using AngleSharp.Dom;

    using static AdvertisementPropertyParsers;

    public class AdvertisementParser
    {
        public static AdvertisementInputModel Parse(IDocument document)
        {
            var inputModel = new AdvertisementInputModel();

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
