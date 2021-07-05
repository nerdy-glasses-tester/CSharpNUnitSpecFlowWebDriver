using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BoDi;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using Microsoft.Extensions.Logging;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CSharpNUnitSpecFlowWebDriver.Pages
{
    [Binding, Parallelizable]
    public class FilterByBathPage
    {
        IWebDriver Driver;

        public FilterByBathPage()
        {
            Driver = SpecflowHooks.driver.Value;
        }

        public WebDriverWait Wait => new WebDriverWait(Driver, System.TimeSpan.FromSeconds(45));

        private IWebElement BathFilter => Driver.FindElement(By.CssSelector("div#ddbtn-label-baths>span.dd-info"));

        private IList<IWebElement> BathFilterSelection => Wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("ul#dd-criteria-baths>li.dd-menu-item")));

        private IList<IWebElement> BathResults => Driver.FindElements(By.CssSelector(".attrib-number.attrib-number-bathrooms"));
        private IWebElement BathResults1 => BathResults[0];

        private IWebElement BathResults2 => BathResults[1];

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public void FilterByBath(string bath)
        {
            BathFilter.Click();
            BathFilterSelection[int.Parse(bath)].Click();
            Thread.Sleep(6000); //Wait for page to load
            //Reporter.LogTestStepForBugLogger(AventStack.ExtentReports.Status.Info, "Filter by " + $"{bath} baths.");
            logger.Info("Filter by " + $"{bath} baths.");
        }

        public bool VerifyIsFilterByBath(string bath)
        {
            bool isFiltered = false;

            int numofbaths = Int32.Parse(bath);
            int result1 = Int32.Parse(BathResults1.GetAttribute("innerText"));
            int result2 = Int32.Parse(BathResults2.GetAttribute("innerText"));

            if((result1 >= numofbaths) && (result2 >= numofbaths))
            {
                isFiltered = true;
                //Reporter.LogTestStepForBugLogger(AventStack.ExtentReports.Status.Info, "Verified it filtered by baths.");
                logger.Info("Verified it filtered by baths.");
            }
            else
            {
                isFiltered = false;
                //Reporter.LogTestStepForBugLogger(AventStack.ExtentReports.Status.Info, "Failed to filter by baths.");
                logger.Info("Failed to filter by baths.");
            }

  

            return isFiltered;
        }
    }
}
