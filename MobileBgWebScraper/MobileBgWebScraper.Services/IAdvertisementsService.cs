namespace MobileBgWebScraper.Services
{
    using System.Threading.Tasks;

    public interface IAdvertisementsService
    {
        Task AddAdvertisementAsync(AdvertisementInputModel inputModel);
    }
}
