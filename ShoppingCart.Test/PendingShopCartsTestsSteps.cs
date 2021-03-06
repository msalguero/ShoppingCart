﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingCart.Data;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Repositories;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ShoppingCart.Test
{
    [Binding]
    public class PendingShopCartsTestsSteps
    {
        [Given(@"una lista de carritos pendientes y pagados")]
        public void GivenUnaListaDeCarritosPendientesYPagados(Table table)
        {
            List<ShopCart> shopCartsList = (List<ShopCart>)table.CreateSet<ShopCart>();
            ScenarioContext.Current.Add("shopCartsList", shopCartsList);
        }
        
        [Given(@"el usuario (.*)")]
        public void GivenElUsuario(string user)
        {
            ScenarioContext.Current.Add("user", user);
        }
        
        [When(@"Al ejecutar el reporte de carritos pendientes")]
        public void WhenAlEjecutarElReporteDeCarritosPendientes()
        {
            var returnList = new List<ShopCart>()
            {
                new ShopCart(){Id = 1, User = "victor", State = "sinPagar"},
                new ShopCart(){Id = 2, User = "ricardo", State = "sinPagar"}
            };
            var mockShopCartRepository = new Mock<IShoppingCartRepository>();
            mockShopCartRepository.Setup(m => m.GetPendingShopCarts(It.IsAny<List<ShopCart>>())).Returns(returnList);

            ShopCartManager shopCartManager = new ShopCartManager(mockShopCartRepository.Object);
            var shopCartsList = (List<ShopCart>)ScenarioContext.Current["shopCartsList"];
            var returnedList = shopCartManager.GetPendingShopCarts(shopCartsList);
            ScenarioContext.Current.Add("returnedList", returnedList);
        }
        [Then(@"devolvera los carritos que tengan mas de un mes de creados y que aun esten pendientes")]
        public void ThenDevolveraLosCarritosQueTenganMasDeUnMesDeCreadosYQueAunEstenPendientes(Table table)
        {
            var expectedReturnList = (List<ShopCart>)table.CreateSet<ShopCart>();
            var returnedList = (List<ShopCart>)ScenarioContext.Current["returnedList"];
            for (int i = 0; i < returnedList.Count; i++)
            {
                Assert.AreEqual(expectedReturnList[i].Id, returnedList[i].Id);
                Assert.AreEqual(expectedReturnList[i].User, returnedList[i].User);
                Assert.AreEqual(expectedReturnList[i].State, returnedList[i].State);
                Assert.AreEqual(expectedReturnList[i].CreationDate, returnedList[i].CreationDate);
            }
        }
    }
}
