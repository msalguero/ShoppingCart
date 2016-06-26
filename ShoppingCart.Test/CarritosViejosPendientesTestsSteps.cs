using System;
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
    public class CarritosViejosPendientesTestsSteps
    {
        [Given(@"una lista de carritos pendientes y pagados")]
        public void GivenUnaListaDeCarritosPendientesYPagados(Table table)
        {
            List<ShopCart> listaCarritos = (List<ShopCart>)table.CreateSet<ShopCart>();
            ScenarioContext.Current.Add("listaCarritos", listaCarritos);
        }
        
        [Given(@"el usuario (.*)")]
        public void GivenElUsuario(string usuario)
        {
            ScenarioContext.Current.Add("usuario", usuario);
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
            mockShopCartRepository.Setup(m => m.GetCarritosPendientes(It.IsAny<List<ShopCart>>())).Returns(returnList);

            ShopCartManager shopCartManager = new ShopCartManager(mockShopCartRepository.Object);
            var listaCarritos = (List<ShopCart>)ScenarioContext.Current["listaCarritos"];
            var returnedList = shopCartManager.GetCarritosPendientes(listaCarritos);
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
