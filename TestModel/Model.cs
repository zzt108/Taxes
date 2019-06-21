using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace TestModel
{
    [TestClass]
    public class Model
    {
        [TestMethod]
        public void CreateDb()
        {
            Console.WriteLine(AppContext.BaseDirectory);

            // Create context object and then save company data.
            var context = new ModelContext();
            var user = context.Municipalities.Find(1);

            if (user == null)
            {
                Console.WriteLine("Default user with ID 1 is not found, creating it");
                GenerateData(context);
            }
            else
            {
                Console.WriteLine("Default user with ID 1 is found");
            }
        }

        private static void GenerateData(ModelContext context)
        {

            var vilnius = new Municipality() { Name = "Vilnius" };
            context.Municipalities.Add(vilnius);
            var frederiksberg = new Municipality() { Name = "Taastrup" };
            context.Municipalities.Add(frederiksberg);

            context.Taxes.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Daily, StartDate = new DateTime(2016, 1, 1), Amount = 0.1f });
            context.Taxes.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Daily, StartDate = new DateTime(2016, 12, 25), Amount = 0.1f });
            context.Taxes.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Monthly, StartDate = new DateTime(2016, 5, 1), 
                EndDate = new DateTime(2016, 5, 31), Amount = 0.4f });
            context.Taxes.Add(new Tax() { Municipality = vilnius, TaxType = TaxTypeEnum.Yearly, StartDate = new DateTime(2016, 1, 1), 
                EndDate = new DateTime(2016, 12, 31), Amount = 0.2f });
            context.SaveChanges();
        }

    }
}
