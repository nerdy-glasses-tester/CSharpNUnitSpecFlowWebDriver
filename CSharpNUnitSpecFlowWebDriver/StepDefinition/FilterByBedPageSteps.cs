using AventStack.ExtentReports;
using CSharpNUnitSpecFlowWebDriver.Pages;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace CSharpNUnitSpecFlowWebDriver.StepDefinition
{
    [Binding, Parallelizable]
    public class FilterByBedPageSteps
    {
        HomePageSearch homePageSearch;
        FilterByBedPage filterByBedPage;
        IWebDriver Driver;

        public FilterByBedPageSteps()
        {
            homePageSearch = new HomePageSearch();
            filterByBedPage = new FilterByBedPage();
            Driver = SpecflowHooks.driver.Value;
        }

        [Given(@"I enter (.*) (.*) to search for rentals by bed in (.*) browser")]
        public void GivenIEnterLocationToSearchForRentalsByBedInBrowser(string city, string state, string browser)
        {
            string keyword = city + ", " + state;
            SearchResultsPage searchresultspg = homePageSearch.Search(city, state, keyword);
            Assert.IsTrue(searchresultspg.CheckSearchResultsMatchKeyword(keyword), "Search results did not match keyword.");
        }

        [When(@"I click filter search results by bed and select (.*) to filter by in (.*) browser")]
        public void WhenIClickFilterSearchResultsByBedAndSelectToFilterByInBrowser(string bed, string browser)
        {
            filterByBedPage.FilterByBed(bed);
        }

        [Then(@"I should be able to see search results with at least mininum (.*) bed in the first two search results in (.*) browser")]
        public void ThenIShouldBeAbleToSeeSearchResultsWithAtLeastMininumNumberOfFilteredBathsInTheFirstTwoSearchResultsInBrowser(string bed, string browser)
        {
            Thread.Sleep(5000);
            bool isFiltered = filterByBedPage.VerifyIsFilterByBed(bed);

            try
            {
                Assert.IsTrue(isFiltered, "Search results are not filtered by bed.");
            }
            catch (Exception e)
            {
                SpecflowHooks.scenario.Log(Status.Fail, "Search results are not filtered by bath.");
            }
        }



    }
}
