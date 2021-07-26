namespace MobileBgWebScraper.App
{
    using MobileBgWebScraper.Data;

    public class Startup
    {
        public static void ConfigureDatabase()
        {
            DatabaseConfig.IsDatabaseLocal = true;
        }
    }
}
