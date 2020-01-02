using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using UserDataManager;

namespace VirtoServer
{
    public class Program
    {
        public static DatabaseManager database;
        public static void Main(string[] args)
        {
            try
            {
                database = DatabaseManager.Instance;
            }
            catch (Exception e)
            {
                database = new DatabaseManager();
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
