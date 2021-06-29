using CSharpNUnitSpecFlowWebDriver.Pages;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace CSharpNUnitSpecFlowWebDriver.StepDefinition
{
    [Binding, Parallelizable]
    public class FilterByPricePageSteps
    {
        HomePageSearch homePageSearch;
        FilterByPricePage filterByPricePage;

        public FilterByPricePageSteps()
        {
            homePageSearch = new HomePageSearch();
            filterByPricePage = new FilterByPricePage();
        }


        [Given(@"I enter (.*) (.*) to search for rentals by price in (.*) browser")]
        public void GivenIEnterLocationToSearchForRentalsByPriceInBrowser(string city, string state, string browser)
        {
            string keyword = city + ", " + state;
            SearchResultsPage searchresultspg = homePageSearch.Search(city, state, keyword);
            Assert.IsTrue(searchresultspg.CheckSearchResultsMatchKeyword(keyword), "Search results did not match keyword.");

        }

        [Given(@"I click filter search results by price and select (.*) and (.*) to filter by")]
        public void GivenIClickFilterSearchResultsByPriceAndSelectAndToFilterBy(string minprice, string maxprice)
        {
            filterByPricePage.FilterByPrice(minprice, maxprice);
        }

        [Then(@"I should be able to see search results between (.*) and (.*) in the first result")]
        public void ThenIShouldBeAbleToSeeSearchResultsBetweenAndInTheFirstResult(string minprice, string maxprice)
        {
            bool isFiltered = filterByPricePage.VerifyIsFilterByPrice(minprice, maxprice);
            Assert.IsTrue(isFiltered, "Search results are not filtered by price.");
        }
    }
}
