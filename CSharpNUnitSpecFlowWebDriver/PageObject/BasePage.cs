using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using NLog.Web;
using OpenQA.Selenium;
using Cqrs.Hosts;
using NLog;
using TechTalk.SpecFlow;
using NUnit.Framework;
using BoDi;

namespace CSharpNUnitSpecFlowWebDriver.PageObject
{
    [Binding, Parallelizable]
    public class BasePage
    {
        public IWebDriver driver { get; set; }

        public static Logger logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
        

        //public static Logger logger = LogManager.GetCurrentClassLogger();

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void TestLog()
        {
            try
            {
                logger.Debug("init main");
                CreateHostBuilder().Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        
        public IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<StartUp>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                })
                .UseNLog();  // NLog: Setup NLog for Dependency injection
    }

}