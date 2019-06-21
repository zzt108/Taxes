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

        public bool IsDateValid(DateTime date)
        {
            if (EndDate == null)
            {
                return date.Year == StartDate.Year && date.DayOfYear == StartDate.DayOfYear;
            }
            else
            {
                return date.CompareTo(StartDate) >= 0 && date.CompareTo(EndDate.Value.AddDays(1)) < 0;
            }
        }
    }
}