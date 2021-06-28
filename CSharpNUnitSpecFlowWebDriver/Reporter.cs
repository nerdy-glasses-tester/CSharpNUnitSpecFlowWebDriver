using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NLog;
using Ext= AventStack.ExtentReports.ExtentReports;


namespace CSharpNUnitSpecFlowWebDriver
{
    public static class Reporter
    {
        private static readonly Logger TheLogger = LogManager.GetCurrentClassLogger();
        private static Ext ReportManager { get; set; }
        private static string ApplicationDebuggingFolder => "C:\\CSharpNUnitSpecFlowWebDriver\\CSharpNUnitSpecFlowWebDriver\\Reports\\CSharpNUnitSpecFlowWebDriver";

        private static string HtmlReportFullPath { get; set; }

        public static string LatestResultsReportFolder { get; set; }

        private static NUnit.Framework.TestContext MyTestContext { get; set; }

        private static ExtentTest CurrentTestCase { get; set; }

        public static void StartReporter()
        {
            TheLogger.Trace("Starting a one time setup for the entire" +
                            " .LoggingFramework namespace." +
                            "Going to initialize the reporter next...");
            CreateReportDirectory();
            var htmlReporter = new ExtentHtmlReporter(HtmlReportFullPath);
            ReportManager = new Ext();
            ReportManager.AttachReporter(htmlReporter);
        }

        private static void CreateReportDirectory()
        {
            var filePath = Path.GetFullPath(ApplicationDebuggingFolder);
            LatestResultsReportFolder = Path.Combine(filePath, DateTime.Now.ToString("MMdd_HHmm"));
            Directory.CreateDirectory(LatestResultsReportFolder);

            HtmlReportFullPath = $"{LatestResultsReportFolder}\\TestResults.html";
            TheLogger.Trace("Full path of HTML report=>" + HtmlReportFullPath);
        }

        public static void AddTestCaseMetadataToHtmlReport(NUnit.Framework.TestContext testContext)
        {
            MyTestContext = testContext;
            CurrentTestCase = ReportManager.CreateTest(MyTestContext.Test.Name);
        }

        public static void LogPassingTestStepToBugLogger(string message)
        {
            TheLogger.Info(message);
            CurrentTestCase.Log(Status.Pass, message);
        }

        public static void ReportTestOutcome(string screenshotPath)
        {
            var status = MyTestContext.Result.Outcome.Status;

            switch (status)
            {
                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    TheLogger.Error($"Test Failed=>{MyTestContext.Test.FullName}");
                    CurrentTestCase.AddScreenCaptureFromPath(screenshotPath);
                    CurrentTestCase.Fail("Fail");
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Inconclusive:
                    CurrentTestCase.AddScreenCaptureFromPath(screenshotPath);
                    CurrentTestCase.Warning("Inconclusive");
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Skipped:
                    CurrentTestCase.Skip("Test skipped");
                    break;
                default:
                    CurrentTestCase.Pass("Pass");
                    break;
            }

            ReportManager.Flush();
        }

        //Log test steps in extent report
        public static void LogTestStepForBugLogger(Status status, string message)
        {
            TheLogger.Info(message);
            CurrentTestCase.Log(status, message);
        }
    }
}