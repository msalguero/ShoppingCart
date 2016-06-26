using System;
using TechTalk.SpecFlow;

namespace ShoppingCart.Test
{
    [Binding]
    public class CarritosViejosPendientesTestsSteps
    {
        [Given(@"una lista de carritos pendientes y pagados")]
        public void GivenUnaListaDeCarritosPendientesYPagados(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"el usuario (.*)")]
        public void GivenElUsuario(string usuario)
        {
            ScenarioContext.Current.Add("usuario", usuario);
        }
        
        [When(@"Al ejecutar el reporte de carritos pendientes")]
        public void WhenAlEjecutarElReporteDeCarritosPendientes()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"devolvera los carritos que tengan mas de un mes de creados y que aun esten pendientes")]
        public void ThenDevolveraLosCarritosQueTenganMasDeUnMesDeCreadosYQueAunEstenPendientes()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
