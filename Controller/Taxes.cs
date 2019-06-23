using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace Controller
{
    public static class Taxes
    {
        public static Tax GetTax(string municipality, DateTime date)
        {
                var m = Municipalities.GetByName(municipality);
                if (m == null)
                {
                    throw new ArgumentException($"Municipality {municipality} not found!");
                }
                return GetTax(m.Id, date);
        }

        public static Tax GetTax(int municipalityId, DateTime date)
        {
            using (var uw = new UnitOfWork())
            {
                var tax = uw.TaxRepository.Get(t => t.Municipality_Id == municipalityId).OrderBy(tt => tt.TaxType).FirstOrDefault(t =>t.IsDateValid(date));
                if (tax == null)
                {
                    throw new ArgumentException($"Tax data for {date} not found!");
                }
                return tax;
            }
        }

        public static void Add(Tax tax)
        {
            using (var uw = new UnitOfWork())
            {
                uw.TaxRepository.Add(tax);
                uw.SaveChanges();
            }

        }
    }
}
