using System.Data.Entity;

namespace Model
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("name=TaxesModelContext")
        //public TimeRegContext() : base("TimeRegDb")
        {
        }

        public virtual DbSet<Municipality> Municipalities { get; set; }
        public virtual DbSet<Tax> Taxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}