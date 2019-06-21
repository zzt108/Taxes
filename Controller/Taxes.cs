using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class Municipalities
    {
        public static int GetMunicipalityId(IEnumerable<Municipality> municipalities, string name)
        {
            var first = municipalities.FirstOrDefault(municipality => municipality.Name.ToLower() == name.ToLower());
            return first?.Id ?? 0;        // zero id is not possible. better performance than throwing an error
        }
    }

    public static class Taxes
    {
        public static float GetTax(IEnumerable<Municipality> municipalities,IEnumerable<Tax> taxes, string municipality, DateTime date)
        {
            var id = Municipalities.GetMunicipalityId(municipalities, municipality);
            if (id==0)
            {
                throw new ArgumentException($"Municipality {municipality} not found!");
            }
            return GetTax(taxes, id, date);
        }

        public static float GetTax(IEnumerable<Tax> taxes, int municipalityId, DateTime date)
        {
            var tax = taxes.Where(t => t.IsDateValid(date)).OrderBy(tt => tt.TaxType).FirstOrDefault();
            if (tax == null)
            {
                throw new ArgumentException($"Tax data for {date} not found!");
            }
            return tax.Amount;
        }
    }
}
