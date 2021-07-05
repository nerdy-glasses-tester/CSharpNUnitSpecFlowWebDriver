using NLog;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Threading;
using NUnit.Framework;
using TechTalk.SpecFlow;
using BoDi;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using OpenQA.Selenium.Remote;

namespace CSharpNUnitSpecFlowWebDriver.Pages
{
    [Binding, Parallelizable]
    public class HomePageSearch
    {
        IWebDriver Driver;

        public HomePageSearch()
        {
            Driver = SpecflowHooks.driver.Value;
        }
        public WebDriverWait Wait => new WebDriverWait(Driver, System.TimeSpan.FromSeconds(45));

        private IWebElement SearchField => Driver.FindElement(By.Id("homepage-search-field"));

        private IWebElement SearchBtn => Driver.FindElement(By.CssSelector("button.call-to-action.search-field-button"));

        private IWebElement SpecificPlaceLoaded => Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".property-address")));
        


        public SearchResultsPage Search(string city, string state, string keyword)
        {
            SearchField.SendKeys(keyword);
            SearchBtn.Click();
            Thread.Sleep(6000);
            //Reporter.LogTestStepForBugLogger(AventStack.ExtentReports.Status.Info, "Search for " + $"{keyword}.");
            return new SearchResultsPage();
        }

        public SearchResultsPage SearchSpecificPlace (string place)
        {
            SearchField.SendKeys(place);
            SearchBtn.Click();
            Thread.Sleep(6000);
            //Reporter.LogTestStepForBugLogger(AventStack.ExtentReports.Status.Info, "Search for " + $"{place}.");
            Driver.Navigate().Refresh();
            Thread.Sleep(6000);
            if (SpecificPlaceLoaded.GetAttribute("innerHTML").Contains("12512 Brighton Pl"))
            {
                //Reporter.LogTestStepForBugLogger(AventStack.ExtentReports.Status.Info, "Search Results Page Loaded.");
            }

            return new SearchResultsPage();
        }
    }
}
