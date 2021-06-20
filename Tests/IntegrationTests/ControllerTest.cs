using System;
using System.IO;
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
                Controller.Municipalities.GetByName("Vilnius").Id.Should().Be(1);
                Controller.Municipalities.GetByName("vilnius").Id.Should().Be(1);
        }


        [TestMethod]
        public void CanGetTaxValue()
        {
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,1,1)).Amount.Should().Be(0.1f);
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,5,2)).Amount.Should().Be(0.4f);
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,7,10)).Amount.Should().Be(0.2f);
            Controller.Taxes.GetTax( "Vilnius", new DateTime(2016,3,16)).Amount.Should().Be(0.2f);
        }

        [TestMethod]
        public void CanExport()
        {
            Controller.Taxes.ExportTax("taxdata.csv");
        }

        [TestMethod]
        public void CanImport()
        {
            const string taxdataCsv = "taxdata.csv";
            if (File.Exists(taxdataCsv))
            {
                Controller.Taxes.ImportTax(taxdataCsv);
            }
            else
            {
                    Assert.Inconclusive($"{taxdataCsv} does not exist!");
            }
        }
    }
}
