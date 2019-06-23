using System;
using Model;

namespace DataAccessLayer
{
    public sealed class UnitOfWork : IDisposable
    {
        private readonly TaxesContext _context = new TaxesContext();
        private GenericRepository<Municipality> _municipalityRepository;
        private GenericRepository<Tax> _taxRepository;
        private bool _disposed;

        public GenericRepository<Municipality> MunicipalityRepository => _municipalityRepository ?? (_municipalityRepository = new GenericRepository<Municipality>(_context));

        public GenericRepository<Tax> TaxRepository => _taxRepository ?? (_taxRepository = new GenericRepository<Tax>(_context));

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}