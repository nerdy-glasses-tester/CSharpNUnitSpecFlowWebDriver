Feature: FilterByBedPage
	To Filter the search results by number of beds
	I will search for a location and then click filter by beds and select a number

Scenario Outline: FilterByBed
	Given I enter <city> <state> to search for rentals by bed in <browser> browser
	When I click filter search results by bed and select <bed> to filter by in <browser> browser
	Then I should be able to see search results with at least mininum <bed> bed in the first two search results in <browser> browser
Examples:
| city   | state | browser | bed  |
| Irvine | CA    | Chrome  | 3    |
| Irvine | CA    | Firefox | 3    |
| Irvine | CA    | Edge	   | 3    |
