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
    }
}
