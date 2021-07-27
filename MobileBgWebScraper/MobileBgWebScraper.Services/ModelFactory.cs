namespace MobileBgWebScraper.Services
{
    using System.Linq;

    using MobileBgWebScraper.Data;
    using MobileBgWebScraper.Models;

    public class ModelFactory
    {
        private readonly MobileBgDbContext dbContext;

        public ModelFactory(MobileBgDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Brand CreateBrand(AdvertisementInputModel inputModel)
        {
            return dbContext.Brands.FirstOrDefault(b => b.Name == inputModel.BrandName)
                ?? new Brand { Name = inputModel.BrandName };
        }

        public Model CreateModel(AdvertisementInputModel inputModel, Brand brand)
        {
            return brand.Models.FirstOrDefault(m => m.Name == inputModel.ModelName)
                ?? new Model { Name = inputModel.ModelName, Brand = brand };
        }

        public Color CreateColor(AdvertisementInputModel inputModel)
        {
            if (inputModel.ColorName == null)
            {
                return null;
            }

            return dbContext.Colors.FirstOrDefault(c => c.Name == inputModel.ColorName)
                ?? new Color { Name = inputModel.ColorName };
        }

        public Engine CreateEngine(AdvertisementInputModel inputModel)
        {
            return dbContext.Engines.FirstOrDefault(e => e.Type == inputModel.EngineType)
                ?? new Engine { Type = inputModel.EngineType };
        }

        public Transmission CreateTransmission(AdvertisementInputModel inputModel)
        {
            return dbContext.Transmissions.FirstOrDefault(t => t.Type == inputModel.TransmissionType)
                ?? new Transmission { Type = inputModel.TransmissionType };
        }

        public BodyStyle CreateBodyStyle(AdvertisementInputModel inputModel)
        {
            return dbContext.BodyStyles.FirstOrDefault(bs => bs.Name == inputModel.BodyStyle)
                ?? new BodyStyle { Name = inputModel.BodyStyle };
        }

        public Region CreateRegion(AdvertisementInputModel inputModel)
        {
            return dbContext.Regions.FirstOrDefault(r => r.Name == inputModel.RegionName)
                ?? new Region { Name = inputModel.RegionName };
        }

        public Town CreateTown(AdvertisementInputModel inputModel, Region region)
        {
            return dbContext.Towns.FirstOrDefault(t => t.Name == inputModel.TownName)
                ?? new Town { Name = inputModel.TownName, Region = region };
        }

        public EuroStandard CreateEuroStandard(AdvertisementInputModel inputModel)
        {
            if (inputModel.EuroStandard == null)
            {
                return null;
            }

            return dbContext.EuroStandards.FirstOrDefault(es => es.Type == inputModel.EuroStandard)
                ?? new EuroStandard { Type = inputModel.EuroStandard };
        }

        public void AddImagesToAdvertisement(AdvertisementInputModel inputModel, Advertisement advertisement)
        {
            foreach (string imageUrl in inputModel.ImageUrls)
            {
                advertisement.Images.Add(new Image { Url = imageUrl });
            }
        }
    }
}
