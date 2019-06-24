using System.Collections.Generic;
using System.Linq;
using FileHelpers;
using Model;

namespace ImportExport
{
    public class ImportTax
    {
        private readonly string _path;
        private readonly FileHelperAsyncEngine<TaxDto> _engine;

        public ImportTax(string path)
        {
            _path = path;
            _engine = new FileHelperAsyncEngine<TaxDto>();
        }
        public List<Tax> Read(IEnumerable<Municipality> municipalities)
        {
            var result = new List<Tax>();
            using (_engine.BeginReadFile(_path))
            {
                var enumerable = municipalities as Municipality[] ?? municipalities.ToArray();
                foreach (var dto in _engine)
                {
                    result.Add(dto.AsTax(enumerable));
                }
            }

            return result;
        }

    }
}