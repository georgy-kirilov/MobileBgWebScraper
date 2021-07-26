namespace MobileBgWebScraper.Data
{
    public static class DatabaseConfig
    {
        public static bool IsDatabaseLocal = false;

        public const string DatabaseName = "MobileBg";

        public static string ConnectionString
        {
            get
            {
                if (IsDatabaseLocal)
                {
                    return string.Format(LocalConnectionStringFormat, DatabaseName);
                }
                else
                {
                    return string.Format(RemoteConnectionStringFormat, DatabaseName);
                }
            }
        }

        private const string LocalConnectionStringFormat = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;";

        private const string RemoteConnectionStringFormat = "Server=.\\SQLEXPRESS;Database={0};Integrated Security=True;";
    }
}
