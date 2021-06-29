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

        public FilterByBathPageSteps()
        {
            homePageSearch = new HomePageSearch();
            filterByBathPage = new FilterByBathPage();
        }

        [Given(@"I enter (.*) (.*) to search for rentals by bath in (.*) browser")]
        public void GivenIEnterLocationToSearchForRentalsByBathInBrowser(string city, string state, string browser)
        {
            string keyword = city + ", " + state;
            SearchResultsPage searchresultspg = homePageSearch.Search(city, state, keyword);
            Assert.IsTrue(searchresultspg.CheckSearchResultsMatchKeyword(keyword), "Search results did not match keyword.");
        }
        
        [Given(@"I click filter search results by bath and select (.*) to filter by")]
        public void GivenIClickFilterSearchResultsByBathAndSelectToFilterBy(string bath)
        {
            filterByBathPage.FilterByBath(bath);
        }

        [Then(@"I should be able to see search results with at least mininum (.*) bath in the first two search results")]
        public void ThenIShouldBeAbleToSeeSearchResultsWithAtLeastMininumNumberOfFilteredBathsInTheFirstTwoSearchResults(string bath)
        {
            Thread.Sleep(5000);
            bool isFiltered = filterByBathPage.VerifyIsFilterByBath(bath);
            Assert.IsTrue(isFiltered, "Search results are not filtered by bath.");
        }


    }
}
