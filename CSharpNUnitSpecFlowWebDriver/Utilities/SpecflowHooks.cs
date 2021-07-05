using NUnit.Framework;
using OpenQA.Selenium;
using NLog;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using NLog.Web;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using System.IO;
using System.Collections.Concurrent;

//[assembly: Parallelizable(ParallelScope.Fixtures)]
//[assembly: LevelOfParallelism(3)]
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
        public static Logger logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

        [ThreadStatic]
        public readonly IObjectContainer _objectContainer;
        [ThreadStatic]
        public readonly FeatureContext _featureContext;
        [ThreadStatic]
        public readonly ScenarioContext _scenarioContext;
        [ThreadStatic]
        public static ExtentTest featureName;
        [ThreadStatic]
        public static ExtentTest scenario;
      
        public static ExtentReports extent;
        public static ExtentKlovReporter klov;

        public string hubUrl;
        public static string filePath;
        public static string LatestResultsReportFolder;
        public static ConcurrentDictionary<string, ExtentTest> FeatureDictionary = new ConcurrentDictionary<string, ExtentTest>();

        
        public SpecflowHooks(IObjectContainer objectContainer, FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }


        public SpecflowHooks()
        {
        }

        [BeforeTestRun]
        public static void TestInitalize()
        {
            //Initialize Extent report before test starts

            //var htmlReporter = new ExtentHtmlReporter(@"C:\CSharpNUnitSpecFlowWebDriver\CSharpNUnitSpecFlowWebDriver\reports\XOME\index.html");
            filePath = Path.GetFullPath("C:\\CSharpNUnitSpecFlowWebDriver\\CSharpNUnitSpecFlowWebDriver\\reports");
            LatestResultsReportFolder = Path.Combine(filePath, DateTime.Now.ToString("MMdd_HHmm"));
            var htmlReporter = new ExtentHtmlReporter(@LatestResultsReportFolder+"\\index.html");
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            //Attach report to reporter
            extent = new ExtentReports();
            klov = new ExtentKlovReporter();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext _featureContext)
        {
            //Get feature Name
            featureName = extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
            FeatureDictionary.TryAdd(_featureContext.FeatureInfo.Title, featureName);
        }

        [BeforeScenario]
        [Obsolete]
        public IWebDriver SetupBeforeEveryTestMethod(FeatureContext _featureContext, ScenarioContext _scenarioContext)
        {
                
                logger.Info("*************************************** TEST STARTED");
                logger.Info("*************************************** TEST STARTED");

                hubUrl = "http://10.211.55.3:4444/wd/hub";
                TimeSpan timeSpan = new TimeSpan(0, 3, 0);

                string InBSName = _featureContext.FeatureInfo.Title;
                if (FeatureDictionary.ContainsKey(InBSName))
                {
                    scenario = FeatureDictionary[InBSName].CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
                }


                browser = (string)_scenarioContext.ScenarioInfo.Arguments["browser"];
                DesiredCapabilities caps = new DesiredCapabilities();

                if (browser == "Chrome")
                {
                    ChromeOptions chromeOptions = new ChromeOptions();
                    caps.SetCapability(CapabilityType.BrowserName, "Chrome");
                    caps.SetCapability(CapabilityType.PlatformName, "Windows");
                    driver.Value = new RemoteWebDriver(new Uri(hubUrl), chromeOptions.ToCapabilities(), timeSpan);
                    _objectContainer.RegisterInstanceAs<RemoteWebDriver>(driver.Value);
                }
                else if(browser == "Firefox")
                {
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    caps.SetCapability(CapabilityType.BrowserName, "Firefox");
                    caps.SetCapability(CapabilityType.PlatformName, "Windows");
                    driver.Value = new RemoteWebDriver(new Uri(hubUrl), firefoxOptions.ToCapabilities(), timeSpan);
                    _objectContainer.RegisterInstanceAs<RemoteWebDriver>(driver.Value);
                 }
                else if (browser == "Edge")
                {
                    EdgeOptions edgeOptions = new EdgeOptions();
                    caps.SetCapability(CapabilityType.BrowserName, "Edge");
                    caps.SetCapability(CapabilityType.PlatformName, "Windows");
                    caps.SetCapability(CapabilityType.AcceptSslCertificates, true);
                    caps.SetCapability(CapabilityType.IsJavaScriptEnabled, true);
                    driver.Value = new RemoteWebDriver(new Uri(hubUrl), edgeOptions.ToCapabilities(), timeSpan);
                    _objectContainer.RegisterInstanceAs<RemoteWebDriver>(driver.Value);
            }


                driver.Value.Navigate().GoToUrl(baseURL);
                driver.Value.Manage().Window.Maximize();
                Thread.Sleep(2000);
                ScreenshotTaker = new ScreenshotTaker((RemoteWebDriver)driver.Value, TestContext.CurrentContext);
                return (RemoteWebDriver)driver.Value;
        }


        [AfterStep]
        public void AfterEachStep(FeatureContext _featureContext, ScenarioContext _scenarioContext)
        {
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepName = _scenarioContext.StepContext.StepInfo.Text;
            Console.WriteLine(stepName);

            if (_scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                {
                    scenario.CreateNode<Given>(stepName);
                }
                else if (stepType == "When")
                {
                    scenario.CreateNode<When>(stepName);
                }
                else if (stepType == "And")
                {
                    scenario.CreateNode<And>(stepName);
                }
                else if (stepType == "Then")
                {
                    scenario.CreateNode<Then>(stepName);
                }
            }
            else if (_scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName).Fail(_scenarioContext.StepContext.TestError.InnerException);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName).Fail(_scenarioContext.StepContext.TestError.InnerException);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName).Fail(_scenarioContext.StepContext.TestError.Message);
            }
            else if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepName).Skip("Step Definition Pending");
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepName).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepName).Skip("Step Definition Pending");
            }
        }


        [AfterScenario]
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

        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            extent.Flush();

        }

        public void TakeScreenshotForTestFailure()
            {
                if (ScreenshotTaker != null)
                {
                    ScreenshotTaker.CreateScreenshotIfTestFailed();
                    scenario.AddScreenCaptureFromPath(ScreenshotTaker.ScreenshotFilePath);
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



