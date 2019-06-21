using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using FluentAssertions;

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
                Console.WriteLine("Municipality with ID 1 is not found, generating base data");
                GenerateData(context);
            }
            else
            {
                Console.WriteLine("Municipality with ID 1 found");
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

        [TestMethod]
        public void CanValidateSingleDay()
        {
            var tax = new Tax(){StartDate = new DateTime(2016, 1,1)};
            tax.IsDateValid(new DateTime(2016, 1, 1)).Should().BeTrue("Same date");
            tax.IsDateValid(new DateTime(2016, 1, 1, 12, 0,0)).Should().BeTrue("Same date, noon");
            tax.IsDateValid(new DateTime(2016, 1, 2)).Should().BeFalse("Different date");
        }

        [TestMethod]
        public void CanValidateDayRange()
        {
            var tax = new Tax(){StartDate = new DateTime(2016, 1,2), EndDate = new DateTime(2016, 2,2)};
            tax.IsDateValid(new DateTime(2016, 1, 2)).Should().BeTrue("On start date");
            tax.IsDateValid(new DateTime(2016, 2, 2)).Should().BeTrue("On end date");
            tax.IsDateValid(new DateTime(2016, 1, 2, 12, 0,0)).Should().BeTrue("Start date, noon");
            tax.IsDateValid(new DateTime(2016, 2, 2, 12, 0,0)).Should().BeTrue("End date, noon");
            tax.IsDateValid(new DateTime(2016, 1, 1)).Should().BeFalse("Before start date");
            tax.IsDateValid(new DateTime(2016, 2, 3)).Should().BeFalse("After start date");

        }

    }
}
