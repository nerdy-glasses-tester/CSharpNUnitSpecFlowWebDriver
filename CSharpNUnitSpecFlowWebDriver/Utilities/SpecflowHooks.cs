using NUnit.Framework;
using OpenQA.Selenium;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using NLog;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using BoDi;
using Ninject;
using CSharpNUnitSpecFlowWebDriver.Pages;
using NLog.Web;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Cqrs.Hosts;
using Microsoft.Extensions.Logging;
using System.Collections;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Reflection;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace CSharpNUnitSpecFlowWebDriver.Utilities
{
    [Binding, Parallelizable]
    public class SpecflowHooks : TechTalk.SpecFlow.Steps
    {
        private static NUnit.Framework.TestContext TestContext { get; set; }

        private static ScreenshotTaker ScreenshotTaker { get; set; }

        public static ThreadLocal<RemoteWebDriver> driver = new ThreadLocal<RemoteWebDriver>();

        public string browser = "";

        public static string baseURL = "https://www.xome.com/";

        public ScenarioContext _scenarioContext;

        public static Logger logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

        
        public SpecflowHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        

        public SpecflowHooks()
        {
        }

        
        [BeforeScenario]
        [SetUp]
        public IWebDriver SetupBeforeEveryTestMethod(ScenarioContext scenarioContext)
            {
                
                logger.Info("*************************************** TEST STARTED");
                logger.Info("*************************************** TEST STARTED");
                Reporter.AddTestCaseMetadataToHtmlReport(TestContext.CurrentContext);
                var factory = new WebDriverFactory();

                browser = (string)_scenarioContext.ScenarioInfo.Arguments["browser"];

            
               if(browser == "Chrome")
                {
                    driver.Value = (RemoteWebDriver)factory.Create(BrowserType.Chrome);
                }
                else if(browser == "Firefox")
                {
                    driver.Value = (RemoteWebDriver)factory.Create(BrowserType.Firefox);
                }
                else if (browser == "Edge")
                {
                    driver.Value = (RemoteWebDriver)factory.Create(BrowserType.Edge);
                }

                driver.Value.Navigate().GoToUrl(baseURL);
                driver.Value.Manage().Window.Maximize();
                Thread.Sleep(2000);
                ScreenshotTaker = new ScreenshotTaker((RemoteWebDriver)driver.Value, TestContext.CurrentContext);
                return (RemoteWebDriver)driver.Value;
        }

            [AfterScenario]
            [TearDown]
            public void TearDownForEverySingleTestMethod()
            {
                try
                {
                    TakeScreenshotForTestFailure();
                }
                catch (Exception e)
                {
                    logger.Error(e.Source);
                    logger.Error(e.StackTrace);
                    logger.Error(e.InnerException);
                    logger.Error(e.Message);
                }
                finally
                {
                    StopBrowser();
                    logger.Info(TestContext.CurrentContext.Test.Name);
                    logger.Info("*************************************** TEST STOPPED");
                    logger.Info("*************************************** TEST STOPPED");
                }
            }
        

        public void TakeScreenshotForTestFailure()
            {
                if (ScreenshotTaker != null)
                {
                    ScreenshotTaker.CreateScreenshotIfTestFailed();
                    Reporter.ReportTestOutcome(ScreenshotTaker.ScreenshotFilePath);
                }
                else
                {
                    Reporter.ReportTestOutcome("");
                }
            }

            public void StopBrowser()
            {
                if (driver.Value == null)
                    return;
                driver.Value.Quit();
                driver.Value.Dispose();
                driver.Value = null;
                logger.Trace("Browser stopped successfully.");
            }


    }


}



