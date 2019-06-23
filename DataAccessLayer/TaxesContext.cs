using System.Data.Entity;
using Model;

namespace DataAccessLayer
{
    public class TaxesContext : DbContext
    {
        public TaxesContext() : base("name=TaxesModelContext")
        {
        }

        public virtual DbSet<Municipality> Municipalities { get; set; }
        public virtual DbSet<Tax> Taxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}