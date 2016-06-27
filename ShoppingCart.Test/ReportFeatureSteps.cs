using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Repositories;
using TechTalk.SpecFlow;

namespace ShoppingCart.Test
{
    [Binding]
    public class ReportFeatureSteps
    {
        private List<StockMovements> _movements;
        private List<Report> _report;

        [Given(@"I have a list of stock movements")]
        public void GivenIHaveAListOfStockMovements(Table table)
        {
            List<StockMovements> movements = new List<StockMovements>();
            foreach (var row in table.Rows)
            {
                movements
                    .Add(new StockMovements
                    {
                        Id = Int32.Parse(row["Id"]),
                        TransactionType = row["TransactionType"],
                        ProductId = Int32.Parse(row["ProductId"]),
                        Quantity = Int32.Parse(row["Quantity"]),
                        EntryDate = new DateTime(2016,5,2)
                    }
                    );
            }
            _movements = movements;
        }
        
        [When(@"I press print the report")]
        public void WhenIPressPrintTheReport()
        {
            
            var moqInventoryManager = new Mock<IStockMovementsRepository>();
            moqInventoryManager.Setup(a => a.GetMovements()).Returns(_movements);
            var inventory = new InventoryManager(moqInventoryManager.Object);
            _report = inventory.GetReport();
        }
        
        [Then(@"the total amount of products with id (.*) must be (.*)")]
        public void ThenTheTotalAmountOfProductsWithIdMustBe(int p0, int p1)
        {
            foreach (var report in _report)
            {
                if (report.ProductId == p0)
                {
                    Assert.AreEqual(report.Quantity, p1);
                }
            }
        }
    }

    
}
