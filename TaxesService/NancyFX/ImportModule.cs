using System;
using Controller;
using DataAccessLayer;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public sealed class ImportModule : NancyModule
    {
        public ImportModule() : base("/import")
        {
            Get("/tax", _ =>
            {
                try
                {
                    var request = this.Bind<RequestObject>();
                    if (string.IsNullOrWhiteSpace(request.Path))
                    {
                        return Helper.ErrorResponse(new ArgumentNullException("path", "Please specify source file as query parameter"), HttpStatusCode.BadRequest);
                    }
                    Taxes.ImportTax(request.Path, new UnitOfWork().MunicipalityRepository.Get(tax => true));
                    return HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
                }
            });
        }
    }
}