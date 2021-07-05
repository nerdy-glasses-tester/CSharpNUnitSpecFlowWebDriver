Feature: FilterByPricePage
	To Filter the search results by number of beds
	I will search for a location and then click filter by price and select a range

Scenario Outline: FilterByPrice
	Given I enter <city> <state> to search for rentals by price in <browser> browser
	When I click filter search results by price and select <minprice> and <maxprice> to filter by in <browser> browser
	Then I should be able to see search results between <minprice> and <maxprice> in the first result in <browser> browser
Examples:
| city   | state | browser | minprice | maxprice |
| Irvine | CA    | Chrome  | 800000   | 1300000  |
| Irvine | CA    | Firefox | 800000   | 1300000  |
| Irvine | CA    | Edge    | 800000   | 1300000  |