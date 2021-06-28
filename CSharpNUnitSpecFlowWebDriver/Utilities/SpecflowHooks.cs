using NUnit.Framework;
using OpenQA.Selenium;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using NLog;
using System;
using System.Threading;
using System.IO;
using CSharpNUnitSpecFlowWebDriver.PageObject;
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

namespace CSharpNUnitSpecFlowWebDriver.Utilities
{
    [Binding, Parallelizable]
    //[TestFixture]
    public class SpecflowHooks : TechTalk.SpecFlow.Steps
    {
        private static NUnit.Framework.TestContext TestContext { get; set; }

        private static ScreenshotTaker ScreenshotTaker { get; set; }

        public static IWebDriver driver;

        private static string baseURL = "https://www.xome.com/";

        public static ScenarioContext _scenarioContext;

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
      
        public IWebDriver SetupBeforeEveryTestMethod()
            {
                
                logger.Info("*************************************** TEST STARTED");
                logger.Info("*************************************** TEST STARTED");
                Reporter.AddTestCaseMetadataToHtmlReport(TestContext.CurrentContext);
                var factory = new WebDriverFactory();


               if (_scenarioContext.ScenarioInfo.Tags.Contains("Browser_Chrome"))
                {
                    driver = factory.Create(BrowserType.Chrome);
                }
               
               if (_scenarioContext.ScenarioInfo.Tags.Contains("Browser_Firefox"))
                {
                    driver = factory.Create(BrowserType.Firefox);
                }



                driver.Navigate().GoToUrl(baseURL);
                driver.Manage().Window.Maximize();
                Thread.Sleep(2000);
                ScreenshotTaker = new ScreenshotTaker(driver, TestContext.CurrentContext);
                return driver;
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

            [AfterScenario]
            [TearDown]
            public void StopBrowser()
            {
                if (driver == null)
                    return;
                driver.Quit();
                driver.Dispose();
                driver = null;
                logger.Trace("Browser stopped successfully.");
            }

        }


    }



