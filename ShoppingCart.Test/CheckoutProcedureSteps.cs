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
                State = "Unknown",
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
            moq_inventoryManager.Setup(manager => manager.getOutOfStockItems(It.IsAny<List<ShoppingCartItem>>())).Returns(new List<Product>());
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
            SaleManager saleManager = new SaleManager(inventoryManager.Object,cartManager.Object);
            var Total = saleManager.CheckOut(cart.Id);
            ScenarioContext.Current.Add("Total",Total);       
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

        [Given(@"any Product is out of Stock")]
        public void GivenAnyProductIsOutOfStock(Table table)
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

            var outOfStockItems = new List<Product>();
            outOfStockItems.Add(new Product { Id = 1, Code = "A", Description = "Description", Name = "Alpha", Price = 1.01f });

            var moq_inventoryManager = new Mock<IInventoryManager>();
            moq_inventoryManager.Setup(manager => manager.getOutOfStockItems(It.IsAny<List<ShoppingCartItem>>())).Returns(outOfStockItems);

            ScenarioContext.Current.Add("moq_cartManager", moq_cartManager);
            ScenarioContext.Current.Add("moq_inventoryManager", moq_inventoryManager);
        }

        [When(@"I Checkout the Products from Empty Cart")]
        public void WhenICheckoutTheProductsFromAnEmptyCart()
        {
            var inventoryManager = ScenarioContext.Current.Get<Mock<IInventoryManager>>("moq_inventoryManager");
            var cartManager = ScenarioContext.Current.Get<Mock<IShoppingCartManager>>("moq_cartManager");

            SaleManager saleManager = new SaleManager(inventoryManager.Object, cartManager.Object);
            ScenarioContext.Current.Add("saleManager", saleManager);

        }

        [Then(@"throw an Out of Stock error")]
        public void ThenThrowAnOutOfStockError()
        {

            var cart = ScenarioContext.Current.Get<ShopCart>("currentCart");
            SaleManager saleManager = ScenarioContext.Current.Get<SaleManager>("saleManager");
            try
            {
                saleManager.CheckOut(cart.Id);
            }
            catch (Exception exemp)
            {
                Assert.AreEqual(typeof(OutOfStockException), exemp.GetType());
            }


        }

        [Then(@"verify that a Email was sent indicating what Product must be restocked")]
        public void ThenVerifyThatAEmailWasSentIndicatingWhatProductMustBeRestocked()
        {
            var inventoryManager = ScenarioContext.Current.Get<Mock<IInventoryManager>>("moq_inventoryManager");
            inventoryManager.Verify(manager => manager.sendEmail(It.IsAny<List<Product>>()), Times.Exactly(1));
        }

        [Given(@"that cart is already Paid")]
        public void GivenThatCartIsAlreadyPaid()
        {

            var moq_cartManager = new Mock<IShoppingCartManager>();
            moq_cartManager.Setup(manager => manager.isCartPaid(It.IsAny<int>())).Returns(true);

            var moq_inventoryManager = new Mock<IInventoryManager>();
  
            ScenarioContext.Current.Add("moq_cartManager", moq_cartManager);
            ScenarioContext.Current.Add("moq_inventoryManager", moq_inventoryManager);
        }

        [When(@"I Checkout the Products from a Paid Cart")]
        public void WhenICheckoutTheProductsFromAPaidCart()
        {
            var inventoryManager = ScenarioContext.Current.Get<Mock<IInventoryManager>>("moq_inventoryManager");
            var cartManager = ScenarioContext.Current.Get<Mock<IShoppingCartManager>>("moq_cartManager");

            SaleManager saleManager = new SaleManager(inventoryManager.Object, cartManager.Object);
            ScenarioContext.Current.Add("saleManager", saleManager);
        }

        [Then(@"throw an Already Paid error")]
        public void ThenThrowAnAlreadyPaidError()
        {
            var cart = ScenarioContext.Current.Get<ShopCart>("currentCart");
            SaleManager saleManager = ScenarioContext.Current.Get<SaleManager>("saleManager");
            try
            {
                saleManager.CheckOut(cart.Id);
            }
            catch (Exception exemp)
            {
                Assert.AreEqual(typeof(CartAlreadyPaidException), exemp.GetType());
            }
        }


    }
}
