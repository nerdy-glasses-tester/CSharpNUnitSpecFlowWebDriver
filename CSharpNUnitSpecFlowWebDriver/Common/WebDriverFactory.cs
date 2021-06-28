using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace CSharpNUnitSpecFlowWebDriver.Utilities
{
    [Binding, Parallelizable]
    class WebDriverFactory
    {
        public IWebDriver Create(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return GetChromeDriver();
                case BrowserType.Firefox:
                    return GetFirefoxDriver();
                default:
                    throw new ArgumentOutOfRangeException("No such browser exists.");
            }
        }


        private IWebDriver GetChromeDriver()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("no-sandbox");
            return new ChromeDriver(path, option, TimeSpan.FromSeconds(130));
            //return new ChromeDriver(path);
        }


        private IWebDriver GetFirefoxDriver()
        {
            //var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";
            return new FirefoxDriver(service);
            //return new FirefoxDriver(path);
        }
    }
   }

