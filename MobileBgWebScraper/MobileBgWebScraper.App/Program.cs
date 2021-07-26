namespace MobileBgWebScraper.App
{
    using MobileBgWebScraper.Data;
    using MobileBgWebScraper.Models;

    using System;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AngleSharp;

    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Startup.ConfigureDatabase();

            var dbContext = new MobileBgDbContext();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();


            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var address = "https://www.mobile.bg/pcgi/mobile.cgi?act=3&slink=kvo3k4&f1=";
            var query = "a.mmm";

            for (int page = 2; page <= 2; page++)
            {
                var document = await context.OpenAsync($"{address}{page}");
                var urls = document.QuerySelectorAll(query).Select(a => a.GetAttribute("href").Trim());

                foreach (string url in urls)
                {
                    var advertisementDocument = await context.OpenAsync($"https:{url}");
                    var inputModel = AdvertisementParser.Parse(advertisementDocument);

                    Brand brand = CreateBrand(dbContext, inputModel);
                    Model model = CreateModel(dbContext, inputModel, brand);
                    Color color = CreateColor(dbContext, inputModel);
                    Engine engine = CreateEngine(dbContext, inputModel);
                    Transmission transmission = CreateTransmission(dbContext, inputModel);
                    BodyStyle bodyStyle = CreateBodyStyle(dbContext, inputModel);
                    Region region = CreateRegion(dbContext, inputModel);
                    Town town = CreateTown(dbContext, inputModel, region);
                    EuroStandard euroStandard = CreateEuroStandard(dbContext, inputModel);

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

                    AddImagesToAdvertisement(inputModel, advertisement);

                    await dbContext.Advertisements.AddAsync(advertisement);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public static Brand CreateBrand(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            return dbContext.Brands.FirstOrDefault(b => b.Name == inputModel.BrandName)
                ?? new Brand { Name = inputModel.BrandName };
        }

        public static Model CreateModel(MobileBgDbContext dbContext, AdvertisementInputModel inputModel, Brand brand)
        {
            return brand.Models.FirstOrDefault(m => m.Name == inputModel.ModelName)
                ?? new Model { Name = inputModel.ModelName, Brand = brand };
        }

        public static Color CreateColor(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            return dbContext.Colors.FirstOrDefault(c => c.Name == inputModel.ColorName)
                ?? new Color { Name = inputModel.ColorName };
        }

        public static Engine CreateEngine(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            return dbContext.Engines.FirstOrDefault(e => e.Type == inputModel.EngineType)
                ?? new Engine { Type = inputModel.EngineType };
        }

        public static Transmission CreateTransmission(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            return dbContext.Transmissions.FirstOrDefault(t => t.Type == inputModel.TransmissionType)
                ?? new Transmission { Type = inputModel.TransmissionType };
        }

        public static BodyStyle CreateBodyStyle(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            return dbContext.BodyStyles.FirstOrDefault(bs => bs.Name == inputModel.BodyStyle)
                ?? new BodyStyle { Name = inputModel.BodyStyle };
        }

        public static Region CreateRegion(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            return dbContext.Regions.FirstOrDefault(r => r.Name == inputModel.RegionName)
                ?? new Region { Name = inputModel.RegionName };
        }

        public static Town CreateTown(MobileBgDbContext dbContext, AdvertisementInputModel inputModel, Region region)
        {
            return dbContext.Towns.FirstOrDefault(t => t.Name == inputModel.TownName)
                ?? new Town { Name = inputModel.TownName, Region = region };
        }

        public static EuroStandard CreateEuroStandard(MobileBgDbContext dbContext, AdvertisementInputModel inputModel)
        {
            if (inputModel.EuroStandard == null)
            {
                return null;
            }

            return dbContext.EuroStandards.FirstOrDefault(es => es.Type == inputModel.EuroStandard)
                ?? new EuroStandard { Type = inputModel.EuroStandard };
        }

        public static void AddImagesToAdvertisement(AdvertisementInputModel inputModel, Advertisement advertisement)
        {
            foreach (string imageUrl in inputModel.ImageUrls)
            {
                advertisement.Images.Add(new Image { Url = imageUrl });
            }
        }
    }
}
