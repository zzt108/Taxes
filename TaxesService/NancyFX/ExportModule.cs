using System;
using Controller;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public sealed class ExportModule : NancyModule
    {
        public ExportModule() : base("/export")
        {
            Get("/tax", _ =>
            {
                try
                {
                    var request = this.Bind<RequestObject>();
                    if (string.IsNullOrWhiteSpace(request.Path))
                    {
                        return Helper.ErrorResponse(new ArgumentNullException("path", "Please specify destination file as query parameter"), HttpStatusCode.BadRequest);
                    }
                    Taxes.ExportTax(request.Path);
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