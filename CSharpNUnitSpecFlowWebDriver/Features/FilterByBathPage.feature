Feature: FilterByBathPage
	To Filter the search results by number of baths
	I will search for a location and then click filter by bath and select a number

@Browser_Chrome
@Browser_Firefox
Scenario Outline: FilterByBath
	Given I enter <city> <state> to search for rentals
	And I click filter search results by bath and select <bath> to filter by
	Then I should be able to see search results with at least mininum <bath> in the first two search results.
Examples:
| city   | state | bath | Tags							  |
| Irvine | CA    | 3    | Browser_Chrome, Browser_Firefox |


