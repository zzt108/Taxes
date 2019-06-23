using System;
using Model;

namespace DataAccessLayer
{
    public class UnitOfWork : IDisposable
    {
        private readonly TaxesContext _context = new TaxesContext();
        private GenericRepository<Municipality> _municipalityRepository;
        private GenericRepository<Tax> _taxRepository;
        private bool _disposed;

        public GenericRepository<Municipality> MunicipalityRepository
        {
            get
            {
                return this._municipalityRepository ?? (_municipalityRepository = new GenericRepository<Municipality>(_context));
            }
        }

        public GenericRepository<Tax> TaxRepository
        {
            get
            {
                return this._taxRepository ?? (_taxRepository = new GenericRepository<Tax>(_context));
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}