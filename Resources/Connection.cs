namespace ejemplov1.Resources
{
    public class Connection
    {
        public static string GetConnection()
        {
            var constructor = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            return constructor.GetSection("ConnectionStrings:Connection").Value;
        }
    }
}
