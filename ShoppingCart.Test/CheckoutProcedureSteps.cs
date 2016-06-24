using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Moq;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using TechTalk.SpecFlow.Assist;

namespace ShoppingCart.Test
{
    [Binding]
    public class CheckoutProcedureSteps
    {
        [Given(@"a shopping cart with ID (.*)")]
        public void GivenAShoppingCartWithID(int p0)
        {
            var Cart = new ShopCart
            {
                Id = p0,
                CreationDate = DateTime.Today,
                State = "Unpaid",
                User = "Fabian"
            };
            ScenarioContext.Current.Add("currentCart", Cart);
        }
        
        [Given(@"Products in Shopping Cart Exist in Stock")]
        public void GivenProductsInShoppingCartExistInStock(Table table)
        {
            var itemsInCart = table;
            
            List<ShoppingCartItem> cartItems = new List<ShoppingCartItem>();
            foreach (var row in itemsInCart.Rows)
            {
                cartItems
                    .Add(new ShoppingCartItem
                        {
                            Id = Int32.Parse(row["ID"]),
                            ShoppingCartId = Int32.Parse(row["ShoppingCartId"]),
                            ProductId = Int32.Parse(row["ProductId"]),
                            Quantity = Int32.Parse(row["Quantity"]),
                        }
                    );
            }
            var moq_cartManager = new Mock<IShoppingCartManager>();
            moq_cartManager.Setup(manager => manager.getShoppingCartItems(It.IsAny<int>())).Returns(cartItems);
            moq_cartManager.Setup(manager => manager.setCartAsPaid(It.IsAny<int>()));

            var moq_inventoryManager = new Mock<IInventoryManager>();
            moq_inventoryManager.Setup(manager => manager.InStock(It.IsAny<List<ShoppingCartItem>>())).Returns(true);
            moq_inventoryManager.Setup(manager => manager.getProductPrice(1)).Returns(1.00f);
            moq_inventoryManager.Setup(manager => manager.getProductPrice(2)).Returns(2.00f);
            moq_inventoryManager.Setup(manager => manager.getProductPrice(3)).Returns(3.00f);
            moq_inventoryManager.Setup(manager => manager.decreaseStock(It.IsAny<int>(), It.IsAny<int>()));

            ScenarioContext.Current.Add("moq_cartManager", moq_cartManager);
            ScenarioContext.Current.Add("moq_inventoryManager",moq_inventoryManager);


        }
        
        [Given(@"the following Product Table")]
        public void GivenTheFollowingProductTable(Table table)
        {
        }
        
        [When(@"I Checkout the Products from Cart")]
        public void WhenICheckoutTheProductsFromCart()
        {
            var cart = ScenarioContext.Current.Get<ShopCart>("currentCart");
            var inventoryManager = ScenarioContext.Current.Get<Mock<IInventoryManager>>("moq_inventoryManager");
            var cartManager = ScenarioContext.Current.Get<Mock<IShoppingCartManager>>("moq_cartManager");

            ScenarioContext.Current.Remove("moq_inventoryManager");
            ScenarioContext.Current.Remove("moq_cartManager");

            SaleManager saleManager = new SaleManager(inventoryManager.Object,cartManager.Object);

            var Total = saleManager.CheckOut(cart.Id);
            ScenarioContext.Current.Add("Total",Total);

            ScenarioContext.Current.Add("moq_inventoryManager", inventoryManager);
            ScenarioContext.Current.Add("moq_cartManager", cartManager);

        }
        
        [Then(@"the Ammount I pay is (.*)")]
        public void ThenTheAmmountIPayIs(int p0)
        {
            var expectedResult = p0;
            var Total = ScenarioContext.Current.Get<float>("Total");
            Assert.AreEqual(expectedResult,Total,Total + " is not " + expectedResult);
        }
        
        [Then(@"the Producst must have Reduced in Stock")]
        public void ThenTheProducstMustHaveReducedInStock()
        {
            var inventoryManager = ScenarioContext.Current.Get<Mock<IInventoryManager>>("moq_inventoryManager");
            inventoryManager.Verify(manager => manager.decreaseStock(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(3));
        }
        
        [Then(@"Shopping Cart status must have changed to Paid")]
        public void ThenShoppingCartStatusMustHaveChangedToPaid()
        {
            var cartManager = ScenarioContext.Current.Get<Mock<IShoppingCartManager>>("moq_cartManager");
            cartManager.Verify(manager => manager.setCartAsPaid(It.IsAny<int>()),Times.Exactly(1));
        }
    }
}
