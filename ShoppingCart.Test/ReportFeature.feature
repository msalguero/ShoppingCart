Feature: ReportFeature
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Print Report
	Given I have a list of stock movements
	 | Id | ProductId	   | EntryDate | Quantity | TransactionType |
	 | 1  | 1              | '1/1/16'  | 10       | "Ingreso"		|
	 | 1  | 2              | '5/1/16'  | 5        | "Ingreso"		|
	 | 2  | 1              | '12/2/16' | 3        | "Ingreso"		|
	 | 3  | 1              | '5/4/16'  | 8        | "Venta"			|
	When I press print the report
	Then the total amount of products with id 2 must be 5
