Feature: FilterByBathPage
	To Filter the search results by number of baths
	I will search for a location and then click filter by bath and select a number

Scenario Outline: FilterByBath
	Given I enter <city> <state> to search for rentals by bath in <browser> browser
	When I click filter search results by bath and select <bath> to filter by in <browser> browser
	Then I should be able to see search results with at least mininum <bath> bath in the first two search results in <browser> browser
Examples:
| city   | state | browser | bath |
| Irvine | CA    | Chrome  | 3    |
| Irvine | CA    | Firefox | 3    |
| Irvine | CA    | Edge	   | 3    |