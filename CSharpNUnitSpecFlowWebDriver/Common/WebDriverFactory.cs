using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace CSharpNUnitSpecFlowWebDriver.Utilities
{
    [Binding, Parallelizable]
    class WebDriverFactory
    {
        public RemoteWebDriver Create(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return GetChromeDriver();
                case BrowserType.Firefox:
                    return GetFirefoxDriver();
                case BrowserType.Edge:
                    return GetEdgeDriver();
                default:
                    throw new ArgumentOutOfRangeException("No such browser exists.");
            }
        }


        private RemoteWebDriver GetChromeDriver()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("no-sandbox");
            return new ChromeDriver(path, option, TimeSpan.FromSeconds(130));
            //return new ChromeDriver(path);
        }


        private RemoteWebDriver GetFirefoxDriver()
        {
            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";
            return new FirefoxDriver(service);
        }

        public RemoteWebDriver GetEdgeDriver()
        {
            EdgeOptions options = new EdgeOptions();
            EdgeDriverService service = EdgeDriverService.CreateDefaultService(@"C:\\CSharpNUnitSpecFlowWebDriver\\CSharpNUnitSpecFlowWebDriver\\", @"msedgedriver.exe");
            service.UseVerboseLogging = true;
            service.UseSpecCompliantProtocol = true;
            service.Host = "::1";
            return new EdgeDriver(service, options);

        }

    }
 }

