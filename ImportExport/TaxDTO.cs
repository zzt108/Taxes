using System;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using Model;

namespace ImportExport
{
    [DelimitedRecord(",")]
    internal class TaxDto
    {
        public TaxDto()
        {
            
        }

        public TaxDto(Tax tax)
        {
            Id = tax.Id;
            MunicipalityName = tax.Municipality?.Name;
            MunicipalityId = tax.Municipality_Id;
            TaxType = tax.TaxType;
            StartDate = tax.StartDate;
            EndDate = tax.EndDate;
            Amount = tax.Amount;
        }

        public Tax AsTax(IEnumerable<Municipality> municipalities)
        {
            var tax = new Tax
            {
                Id = Id ?? 0,
                Municipality_Id =   MunicipalityId??0,
                TaxType = TaxType,
                StartDate = StartDate,
                EndDate = EndDate,
                Amount = Convert.ToSingle(Amount)
            };
            if (tax.Municipality_Id == 0 && !string.IsNullOrWhiteSpace(MunicipalityName) && municipalities != null )
            {
                var m = municipalities.FirstOrDefault(municipality => municipality.Name.ToLower() == MunicipalityName.ToLower());
                if (m != null)
                {
                    tax.Municipality_Id = m.Id;
                    tax.Municipality = m;
                }
            }
            return tax;
        }

        public int? Id;
        public string MunicipalityName;
        public int? MunicipalityId;
        public TaxTypeEnum TaxType;
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime StartDate;
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime? EndDate;
        [FieldConverter(ConverterKind.Double, ".")] // The decimal separator is .
        public double Amount;

    }
}
