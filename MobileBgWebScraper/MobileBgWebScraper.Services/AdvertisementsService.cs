namespace MobileBgWebScraper.Services
{
    using System;
    using System.Threading.Tasks;

    using MobileBgWebScraper.Data;
    using MobileBgWebScraper.Models;

    public class AdvertisementsService : IAdvertisementsService
    {
        private readonly MobileBgDbContext dbContext;
        private readonly ModelFactory modelFactory;

        public AdvertisementsService(MobileBgDbContext dbContext, ModelFactory modelFactory)
        {
            this.dbContext = dbContext;
            this.modelFactory = modelFactory;
        }

        public async Task AddAdvertisementAsync(AdvertisementInputModel inputModel)
        {
            if (inputModel == null)
            {
                throw new ArgumentNullException(nameof(inputModel));
            }

            Brand brand = modelFactory.CreateBrand(inputModel);
            Model model = modelFactory.CreateModel(inputModel, brand);
            Color color = modelFactory.CreateColor(inputModel);
            Engine engine = modelFactory.CreateEngine(inputModel);
            Transmission transmission = modelFactory.CreateTransmission(inputModel);
            BodyStyle bodyStyle = modelFactory.CreateBodyStyle(inputModel);
            Region region = modelFactory.CreateRegion(inputModel);
            Town town = modelFactory.CreateTown(inputModel, region);
            EuroStandard euroStandard = modelFactory.CreateEuroStandard(inputModel);

            var advertisement = new Advertisement
            {
                Brand = brand,
                Model = model,
                Color = color,
                Engine = engine,
                Transmission = transmission,
                BodyStyle = bodyStyle,
                Region = region,
                Town = town,
                EuroStandard = euroStandard,
                Views = inputModel.Views,
                Kilometrage = inputModel.Kilometrage,
                HorsePowers = inputModel.HorsePowers,
                Title = inputModel.Title,
                Description = inputModel.Description,
                Price = inputModel.Price,
                ManufacturingDate = inputModel.ManufacturingDate,
            };

            modelFactory.AddImagesToAdvertisement(inputModel, advertisement);

            await dbContext.Advertisements.AddAsync(advertisement);
            await dbContext.SaveChangesAsync();
        }
    }
}
