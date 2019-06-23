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
        public static float GetTax(string municipality, DateTime date)
        {
            using (var uw = new UnitOfWork())
            {
                var id = Municipalities.GetMunicipalityId(municipality);
                if (id==0)
                {
                    throw new ArgumentException($"Municipality {municipality} not found!");
                }
                return GetTax(id, date);
            }
        }

        public static float GetTax(int municipalityId, DateTime date)
        {
            using (var uw = new UnitOfWork())
            {
                var tax = uw.TaxRepository.Get(t => t.Municipality_Id == municipalityId).OrderBy(tt => tt.TaxType).FirstOrDefault(t =>t.IsDateValid(date));
                if (tax == null)
                {
                    throw new ArgumentException($"Tax data for {date} not found!");
                }
                return tax.Amount;
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
