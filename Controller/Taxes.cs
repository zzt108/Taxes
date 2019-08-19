using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using ImportExport;
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
                var tax = uw.TaxRepository.Get(t => t.Municipality_Id == municipalityId).OrderBy(tt => tt.TaxType).FirstOrDefault(t => t.IsDateValid(date));
                if (tax == null)
                {
                    throw new ArgumentException($"Tax data not found!");
                }
                return tax;
            }
        }

        public static void UpdateTax(UnitOfWork uw, int id, Tax model)
        {
            if (uw.TaxRepository.GetById(id) is Tax tax)
            {
                tax.Amount = model.Amount;
                tax.EndDate = model.EndDate;
                tax.Municipality_Id = model.Municipality_Id;
                tax.StartDate = model.StartDate;
                tax.TaxType = model.TaxType;
            }
        }


        public static void ExportTax(string path, IEnumerable<Tax> data)
        {
            new ExportTax(path).Write(data);
        }

        public static void ImportTax(string path, IEnumerable<Municipality> municipalities)
        {
            var result = new ImportTax(path).Read(municipalities);
            using (var uw = new UnitOfWork())
            {
                foreach (var tax in result)
                {
                    if (tax.Id == 0)
                    {
                        uw.TaxRepository.Add(tax);
                    }
                    else
                    {
                        var existingTax = uw.TaxRepository.GetById(tax.Id);
                        if (existingTax == null)
                        {
                            uw.TaxRepository.Add(tax);
                        }
                        else
                        {
                            UpdateTax(uw, tax.Id, tax);
                        }
                    }
                }
                uw.SaveChanges();
            }
        }
    }
}
