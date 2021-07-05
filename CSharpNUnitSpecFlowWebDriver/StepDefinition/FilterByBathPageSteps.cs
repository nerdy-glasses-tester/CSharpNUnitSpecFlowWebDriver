using AventStack.ExtentReports;
using BoDi;
using CSharpNUnitSpecFlowWebDriver.Pages;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using Ninject;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
using TechTalk.SpecFlow;


namespace CSharpNUnitSpecFlowWebDriver.StepDefinition
{
    [Binding, Parallelizable]
    public class FilterByBathPageSteps
    {

        HomePageSearch homePageSearch;
        FilterByBathPage filterByBathPage;
        IWebDriver Driver;

        public FilterByBathPageSteps()
        {
            homePageSearch = new HomePageSearch();
            filterByBathPage = new FilterByBathPage();
            Driver = SpecflowHooks.driver.Value;
        }

        [Given(@"I enter (.*) (.*) to search for rentals by bath in (.*) browser")]
        public void GivenIEnterLocationToSearchForRentalsByBathInBrowser(string city, string state, string browser)
        {
            string keyword = city + ", " + state;
            SearchResultsPage searchresultspg = homePageSearch.Search(city, state, keyword);
            Assert.IsTrue(searchresultspg.CheckSearchResultsMatchKeyword(keyword), "Search results did not match keyword.");
        }
        
        [When(@"I click filter search results by bath and select (.*) to filter by in (.*) browser")]
        public void WhenIClickFilterSearchResultsByBathAndSelectToFilterByInBrowser(string bath, string browser)
        {
            filterByBathPage.FilterByBath(bath);
        }

        [Then(@"I should be able to see search results with at least mininum (.*) bath in the first two search results in (.*) browser")]
        public void ThenIShouldBeAbleToSeeSearchResultsWithAtLeastMininumNumberOfFilteredBathsInTheFirstTwoSearchResultsInBrowser(string bath, string browser)
        {
            Thread.Sleep(5000);
            bool isFiltered = filterByBathPage.VerifyIsFilterByBath(bath);
            try
            {
                Assert.IsTrue(isFiltered, "Search results are not filtered by bath.");
            }
            catch(Exception e)
            {
                SpecflowHooks.scenario.Log(Status.Fail, "Search results are not filtered by bath.");
            }

  
        }


    }
}
