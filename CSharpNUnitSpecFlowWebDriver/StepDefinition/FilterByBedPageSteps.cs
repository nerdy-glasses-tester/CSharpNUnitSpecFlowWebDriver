using CSharpNUnitSpecFlowWebDriver.Pages;
using NUnit.Framework;
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

        public FilterByBedPageSteps()
        {
            homePageSearch = new HomePageSearch();
            filterByBedPage = new FilterByBedPage();
        }

        [Given(@"I enter (.*) (.*) to search for rentals by bed in (.*) browser")]
        public void GivenIEnterLocationToSearchForRentalsByBedInBrowser(string city, string state, string browser)
        {
            string keyword = city + ", " + state;
            SearchResultsPage searchresultspg = homePageSearch.Search(city, state, keyword);
            Assert.IsTrue(searchresultspg.CheckSearchResultsMatchKeyword(keyword), "Search results did not match keyword.");
        }

        [Given(@"I click filter search results by bed and select (.*) to filter by")]
        public void GivenIClickFilterSearchResultsByBedAndSelectToFilterBy(string bed)
        {
            filterByBedPage.FilterByBed(bed);
        }

        [Then(@"I should be able to see search results with at least mininum (.*) bed in the first two search results")]
        public void ThenIShouldBeAbleToSeeSearchResultsWithAtLeastMininumNumberOfFilteredBathsInTheFirstTwoSearchResults(string bed)
        {
            Thread.Sleep(5000);
            bool isFiltered = filterByBedPage.VerifyIsFilterByBed(bed);
            Assert.IsTrue(isFiltered, "Search results are not filtered by bed.");
        }



    }
}
