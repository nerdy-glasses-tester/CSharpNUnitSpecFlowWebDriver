I just finished Selenium WebDriver Masterclass with C# course and decided to practice. 
Also big thanks to https://github.com/executeautomation/SeleniumWithSpecflow teacher for teaching Extent Reports with parallel testing.
C# is similiar to Java. Using www.xome.com as a sample project, I created a Page Object Model based automation framework to run Selenium C# automation tests on Chrome and Firefox. 
This uses NUnit, NLog, and Extent Reports. As of Jun 7 2021, the test suite has been updated and is passing.

1. In SpecflowHooks.cs change filePath = Path.GetFullPath("C:\\CSharpNUnitSpecFlowWebDriver\\CSharpNUnitSpecFlowWebDriver\\reports");
to a path on your computer to save the reports to
2. Review NLogs at Bin>Debug>net5.0>CSharpNUnitCoreXOME>Logs>Logs.txt folder
3. If you are creating your project create in Visual Studio Community Edition a NUnit3 C# project and choose .NET5.0
4. Install Nuget Packages 
CQRS,DotNetSeleniumExtras.WaitHelpers, ExtentReports, ExtentReports.Core, Microsoft.Net.Test.SDK, NLOG, NLOG.Config, NLOG.Extensions.Logging,
NLog.Schema, NLog.Web.AspNetCore, NUnit, NUnit3TestAdapter, Selenium.WebDriver, Selenium.Support, Microsoft.Edge.SeleniumTool, Selenium.WebDriver.ChromeDriver, Selenium.WebDriver.GeckoDriver,
SpecFlow and SpecFlow.NUnit

5. Download NLog config from https://www.nuget.org/packages/NLog.config and put this in the config file

	<targets>

		<target xsi:type="File"
				name="default"
				layout="${longdate} - ${threadid} - ${callsite:className=true:fileName=false:includeSourcePath=false:methodName=true} -
				${level:uppercase=true}: ${message} ${onexception:${newline}EXCEPTION\: ${exception:format=ToString}}"
				fileName="C:\CSharpNUnitSpecFlowWebDriver\CSharpNUnitSpecFlowWebDriver\Logs\Logs.txt"
				keepFileOpen="false"
	   />
	</targets>

	<rules>

		<logger name="*" writeTo="default" minlevel="Info" />

	</rules>

6. Change properties for NLog.config and NLog.xsd to have these settings
   Build Action -> Content
   Copy to Output Directory -> Copy always

7. WebDriverFactory.cs class must have these settings so browser will run faster and not stall
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


8. Add this to C# project settings to turn off autogenerated assemblyinfo.cs so then you can copy the assemblyinfo.cs to your root folder and add in for parallel testing 
using NUnit.Framework;[assembly: Parallelizable(ParallelScope.Fixtures)], [assembly: LevelOfParallelism(6)]

        <target xsi:type="File"
        <PropertyGroup>
            <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
        </PropertyGroup>
9.  
        Use public static ThreadLocal<RemoteWebDriver> driver = new ThreadLocal<RemoteWebDriver>(); for parallel testing
10.     Use ThreadStatic for extent reports fields
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
11.     Use SeleniumGrid see https://medium.com/maestral-solutions/selenium-grid-setup-the-complete-guide-cf000a2be50f 




Angela Tong