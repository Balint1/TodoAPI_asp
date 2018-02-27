using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IntegrationTests
{
    public abstract class TestBase
    {
        protected readonly DateTime UtcNow;
     
        protected readonly HttpClient RestClient;

        protected TestBase()
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(AppContext.BaseDirectory)
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //var connectionStringsAppSettings = new ConnectionStringsAppSettings();
            //configuration.GetSection("ConnectionStrings").Bind(connectionStringsAppSettings);

            ////You can now access your appsettings with connectionStringsAppSettings.MYKEY

            //UtcNow = DateTime.UtcNow;
            
            //WebHostBuilder webHostBuilder = new WebHostBuilder();
            //webHostBuilder.ConfigureServices(s => s.AddSingleton<IStartupConfigurationService, TestStartupConfigurationService>());
            //webHostBuilder.UseStartup<Startup>();
            //TestServer testServer = new TestServer(webHostBuilder);
            //RestClient = testServer.CreateClient();
        }
    }
}
