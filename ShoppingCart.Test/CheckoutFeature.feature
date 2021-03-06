﻿Feature: CheckingOut a purchase
	At the finalization of a purchase, I want to be able to check out.

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

Scenario: Out of Stock Scenario
Given a shopping cart with ID 1
And any Product is out of Stock
	 | ID | ShoppingCartId | ProductId | Quantity |
	 | 1  | 1              | 1         | 10        |
	 | 2  | 1              | 2         | 3        |
	 | 3  | 1              | 3         | 8        |
When I Checkout the Products from Empty Cart
Then throw an Out of Stock error
Then verify that a Email was sent indicating what Product must be restocked

Scenario: Already Paid Cart
Given a shopping cart with ID 1
And that cart is already Paid
When I Checkout the Products from a Paid Cart
Then throw an Already Paid error