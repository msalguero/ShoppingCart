Feature: SpecFlowFeature1
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Normal Checkout
	Given a shopping cart with ID 1
	And Products in Shopping Cart Exist in Stock
	 | ID | ShoppingCartId | ProductId | Quantity |
	 | 1  | 1              | 1         | 10        |
	 | 2  | 1              | 2         | 3        |
	 | 3  | 1              | 3         | 8        |
	Given the following Product Table
	 | ID | Code | Name  | Description | Price |
	 | 1  | A    | Alpha | Prime       | 1.00  |
	 | 2  | B    | Beta  | Secondo     | 2.00  |
	 | 3  | C    | Gamma | Tercia      | 3.00  |
	When I Checkout the Products from Cart
	Then the Ammount I pay is 40
	Then the Producst must have Reduced in Stock
	Then Shopping Cart status must have changed to Paid