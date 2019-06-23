using System;
using DataAccessLayer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTest
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void CanGetMunicipalityId()
        {
            using(var uw = new UnitOfWork()  )
            {
                Controller.Municipalities.GetMunicipalityId("Vilnius").Should().Be(1);
                Controller.Municipalities.GetMunicipalityId("vilnius").Should().Be(1);
            }
        }


        [TestMethod]
        public void CanGetTaxValue()
        {
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,1,1)).Should().Be(0.1f);
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,5,2)).Should().Be(0.4f);
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,7,10)).Should().Be(0.2f);
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,3,16)).Should().Be(0.2f);
        }

    }
}
