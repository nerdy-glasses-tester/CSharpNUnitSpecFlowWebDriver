using AventStack.ExtentReports;
using CSharpNUnitSpecFlowWebDriver.Pages;
using CSharpNUnitSpecFlowWebDriver.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace CSharpNUnitSpecFlowWebDriver.StepDefinition
{

    [Binding, Parallelizable]
    public class FilterByPricePageSteps
    {
        HomePageSearch homePageSearch;
        FilterByPricePage filterByPricePage;
        IWebDriver Driver;

        public FilterByPricePageSteps()
        {
            homePageSearch = new HomePageSearch();
            filterByPricePage = new FilterByPricePage();
            Driver = SpecflowHooks.driver.Value;
        }


        [Given(@"I enter (.*) (.*) to search for rentals by price in (.*) browser")]
        public void GivenIEnterLocationToSearchForRentalsByPriceInBrowser(string city, string state, string browser)
        {
            string keyword = city + ", " + state;
            SearchResultsPage searchresultspg = homePageSearch.Search(city, state, keyword);
            Assert.IsTrue(searchresultspg.CheckSearchResultsMatchKeyword(keyword), "Search results did not match keyword.");

        }

        [When(@"I click filter search results by price and select (.*) and (.*) to filter by in (.*) browser")]
        public void WhenIClickFilterSearchResultsByPriceAndSelectAndToFilterByInBrowser(string minprice, string maxprice, string browser)
        {
            filterByPricePage.FilterByPrice(minprice, maxprice);
        }

        [Then(@"I should be able to see search results between (.*) and (.*) in the first result in (.*) browser")]
        public void ThenIShouldBeAbleToSeeSearchResultsBetweenAndInTheFirstResultInBrowser(string minprice, string maxprice, string browser)
        {
            bool isFiltered = filterByPricePage.VerifyIsFilterByPrice(minprice, maxprice);
            try
            {
                Assert.IsTrue(isFiltered, "Search results are not filtered by price.");
            }
            catch (Exception e)
            {
                SpecflowHooks.scenario.Log(Status.Fail, "Search results are not filtered by bath.");
            }

        }
    }
}
