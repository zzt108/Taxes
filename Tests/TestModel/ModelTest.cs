using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using FluentAssertions;

namespace TestModel
{
    [TestClass]
    public class ModelTest
    {


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
