using System;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace TestController
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void CreateDb()
        {
            Console.WriteLine(AppContext.BaseDirectory);

            // Create context object and then save company data.
            var uw = new UnitOfWork();
            var user = uw.MunicipalityRepository.GetById(1);

            if (user == null)
            {
                Console.WriteLine("Municipality with ID 1 is not found, generating base data");
                GenerateData(uw);
            }
            else
            {
                Console.WriteLine("Municipality with ID 1 found");
            }
        }

        private static void GenerateData(UnitOfWork uw)
        {

            var vilnius = new Municipality() { Name = "Vilnius" };
            uw.MunicipalityRepository.Add(vilnius);
            var frederiksberg = new Municipality() { Name = "Taastrup" };
            uw.MunicipalityRepository.Add(frederiksberg);

            uw.TaxRepository.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Daily, StartDate = new DateTime(2016, 1, 1), Amount = 0.1f });
            uw.TaxRepository.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Daily, StartDate = new DateTime(2016, 12, 25), Amount = 0.1f });
            uw.TaxRepository.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Monthly, StartDate = new DateTime(2016, 5, 1), 
                EndDate = new DateTime(2016, 5, 31), Amount = 0.4f });
            uw.TaxRepository.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Yearly, StartDate = new DateTime(2016, 1, 1), 
                EndDate = new DateTime(2016, 12, 31), Amount = 0.2f });
            uw.SaveChanges();
        }    }
}