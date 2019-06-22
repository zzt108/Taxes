using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace TestController
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void CanGetMunicipalityId()
        {
            var context = new ModelContext();
            Controller.Municipalities.GetMunicipalityId(context.Municipalities, "Vilnius").Should().Be(1);
            Controller.Municipalities.GetMunicipalityId(context.Municipalities, "vilnius").Should().Be(1);
        }


        [TestMethod]
        public void CanGetTaxValue()
        {
            var context = new ModelContext();
            Controller.Taxes.GetTax(context.Municipalities, context.Taxes, "Vilnius", new DateTime(2016,1,1)).Should().Be(0.1f);
            Controller.Taxes.GetTax(context.Municipalities, context.Taxes, "Vilnius", new DateTime(2016,5,2)).Should().Be(0.4f);
            Controller.Taxes.GetTax(context.Municipalities, context.Taxes, "Vilnius", new DateTime(2016,7,10)).Should().Be(0.2f);
            Controller.Taxes.GetTax(context.Municipalities, context.Taxes, "Vilnius", new DateTime(2016,3,16)).Should().Be(0.2f);
        }

    }
}
