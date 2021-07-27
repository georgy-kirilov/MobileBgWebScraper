namespace MobileBgWebScraper.Services
{
    using System;
    using System.Linq;
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

            var advertisement = dbContext.Advertisements.FirstOrDefault(a => a.RemoteId == inputModel.RemoteId) 
                ?? new Advertisement { RemoteId = inputModel.RemoteId };

            Brand brand = modelFactory.CreateBrand(inputModel);
            Model model = modelFactory.CreateModel(inputModel, brand);
            Color color = modelFactory.CreateColor(inputModel);
            Engine engine = modelFactory.CreateEngine(inputModel);
            Transmission transmission = modelFactory.CreateTransmission(inputModel);
            BodyStyle bodyStyle = modelFactory.CreateBodyStyle(inputModel);
            Region region = modelFactory.CreateRegion(inputModel);
            Town town = modelFactory.CreateTown(inputModel, region);
            EuroStandard euroStandard = modelFactory.CreateEuroStandard(inputModel);
            
            advertisement.Brand = brand;
            advertisement.Model = model;
            advertisement.Color = color;
            advertisement.Engine = engine;
            advertisement.Transmission = transmission;
            advertisement.BodyStyle = bodyStyle;
            advertisement.Region = region;
            advertisement.Town = town;
            advertisement.EuroStandard = euroStandard;
            advertisement.Views = inputModel.Views;
            advertisement.Kilometrage = inputModel.Kilometrage;
            advertisement.HorsePowers = inputModel.HorsePowers;
            advertisement.Title = inputModel.Title;
            advertisement.Description = inputModel.Description;
            advertisement.Price = inputModel.Price;
            advertisement.ManufacturingDate = inputModel.ManufacturingDate;

            modelFactory.AddImagesToAdvertisement(inputModel, advertisement);

            await dbContext.Advertisements.AddAsync(advertisement);
            await dbContext.SaveChangesAsync();
        }
    }
}
