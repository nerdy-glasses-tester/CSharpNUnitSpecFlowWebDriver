using System;
using System.IO;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace CSharpNUnitSpecFlowWebDriver
{
    public class ScreenshotTaker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RemoteWebDriver _driver;
        private readonly NUnit.Framework.TestContext _testContext;

        public ScreenshotTaker(RemoteWebDriver driver, NUnit.Framework.TestContext testContext)
        {
            if (driver == null)
                return;
            _driver = driver;
            _testContext = testContext;
            ScreenshotFileName = _testContext.Test.Name;
        }

        public string ScreenshotFilePath { get; private set; }
        private string ScreenshotFileName { get; set; }

        public void CreateScreenshotIfTestFailed()
        {
            if (_testContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                TakeScreenshotForFailure();

            if (_testContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Inconclusive)
                TakeScreenshotForInconclusive();
        }

        public string TakeScreenshot(string screenshotFileName)
        {
            var ss = GetScreenshot();
            var successfullySaved = TryToSaveScreenshot(screenshotFileName, ss);

            return successfullySaved ? ScreenshotFilePath : "";
        }

        public bool TakeScreenshotForFailure()
        {
            ScreenshotFileName = $"{ScreenshotFileName}";

            var ss = GetScreenshot();
            var successfullySaved = TryToSaveScreenshot(ScreenshotFileName, ss);
            if (successfullySaved)
                Logger.Error($"Screenshot Of Error=>{ScreenshotFilePath}");
            return successfullySaved;
        }

        
        public bool TakeScreenshotForInconclusive()
        {
            ScreenshotFileName = $"{ScreenshotFileName}";

            var ss = GetScreenshot();
            var successfullySaved = TryToSaveScreenshot(ScreenshotFileName, ss);
            if (successfullySaved)
                Logger.Error($"Screenshot Of Error=>{ScreenshotFilePath}");
            return successfullySaved;
        }
        

        private Screenshot GetScreenshot()
        {
            return ((ITakesScreenshot) _driver)?.GetScreenshot();
        }

        private bool TryToSaveScreenshot(string screenshotFileName, Screenshot ss)
        {
            try
            {
                SaveScreenshot(screenshotFileName, ss);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.InnerException);
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                return false;
            }
        }

        private void SaveScreenshot(string screenshotName, Screenshot ss)
        {
            if (ss == null)
                return;
            //var filePath = Path.GetFullPath("C:\\CSharpNUnitSpecFlowWebDriver\\CSharpNUnitSpecFlowWebDriver\\reports");
            //var LatestResultsReportFolder = Path.Combine(filePath, DateTime.Now.ToString("MMdd_HHmm"));
            var reportfolder = SpecflowHooks.LatestResultsReportFolder;
            ScreenshotFilePath = $"{reportfolder}\\{screenshotName}.jpg";

            ScreenshotFilePath = ScreenshotFilePath.Replace('/', ' ').Replace('"', ' ');
            ss.SaveAsFile(ScreenshotFilePath, ScreenshotImageFormat.Png);
        }


    }
}