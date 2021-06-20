using System;
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

        public static void Add(Tax model)
        {
            using (var uw = new UnitOfWork())
            {
                uw.TaxRepository.Add(model);
                uw.SaveChanges();
            }
        }

        public static void UpdateTax(int id, Tax model)
        {
            using (var uw = new UnitOfWork())
            {
                if (uw.TaxRepository.GetById(id) is Tax tax)
                {
                    tax.Amount = model.Amount;
                    tax.EndDate = model.EndDate;
                    tax.Municipality_Id = model.Municipality_Id;
                    tax.StartDate = model.StartDate;
                    tax.TaxType = model.TaxType;
                    uw.SaveChanges();
                }
            }
        }

        public static void Delete(int id)
        {
            using (var uw = new UnitOfWork())
            {
                uw.TaxRepository.Delete(id);
                uw.SaveChanges();
            }
        }

        public static void ExportTax(string path)
        {
            using (var uw = new UnitOfWork())
            {
                new ExportTax(path).Write(uw.TaxRepository.Get(tax => true));
            }
        }

        public static object GetById(int id)
        {
            return new UnitOfWork().TaxRepository.GetById(id);
        }

        public static void ImportTax(string path)
        {
            using (var uw = new UnitOfWork())
            {
                var municipalities = uw.MunicipalityRepository.Get(tax => true);
                var result = new ImportTax(path).Read(municipalities);
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
                                UpdateTax(tax.Id, tax);
                            }
                        }
                    }
                    uw.SaveChanges();
                }
            }
        }

    }
}
