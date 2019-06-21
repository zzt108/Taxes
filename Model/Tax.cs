using System;

public enum TaxTypeEnum
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}

namespace Model
{
    public class Tax
    {
        public int Id { get; set; }
        public TaxTypeEnum TaxType { get; set; }
        public Municipality Municipality { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float Amount { get; set; }
    }
}