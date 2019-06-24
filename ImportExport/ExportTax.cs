using System.Collections.Generic;
using FileHelpers;
using Model;

namespace ImportExport
{
    public class ExportTax 
    {
        private readonly string _outputPath;
        private readonly FileHelperAsyncEngine<TaxDto> _engine;

        public ExportTax(string outputPath)
        {
            _outputPath = outputPath;
            _engine = new FileHelperAsyncEngine<TaxDto>();
        }

        public void Write(IEnumerable<Tax> data)
        {
            using (_engine.BeginWriteFile(_outputPath))
            {
                foreach (var record in data)
                {
                    _engine.WriteNext(new TaxDto(record));
                }
            }
        }
    }
}