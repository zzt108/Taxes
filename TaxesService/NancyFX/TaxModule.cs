using System;
using Controller;
using DataAccessLayer;
using Model;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public sealed class TaxModule : NancyModule
    {
        public TaxModule()   :base("/tax")
        {
            var path = "/{municipality}/{year}/{month}/{day}";
            Get("/",_ => $"Usage:/tax{path}");
            Get(path, _ => GetTax(_));
            Post("/", _ =>
            {
                try
                {
                    var model = this.Bind<Tax>();
                    model.Municipality_Id = Municipalities.GetMunicipalityId(model.Municipality.Name);
                    Taxes.Add(model);
                    return model;
                }
                catch (Exception e)
                {
                    var r = (Response) $"[{e.Message}]";
                    r.StatusCode = HttpStatusCode.InternalServerError;
                    return r;
                }
            });
        }

        private static object GetTax(dynamic _)
        {
            int year;
            int month;
            int day;
            try
            {
                year = int.Parse(_.year);
                month = int.Parse(_.month);
                day = int.Parse(_.day);
            }
            catch (Exception e)
            {
                var r = (Response) $"[{e.Message}]";
                r.StatusCode = HttpStatusCode.InternalServerError;
                return r;
            }

            try
            {
                return Taxes.GetTax(_.municipality.ToString(), new DateTime(year, month, day)).ToString();
            }
            catch (Exception e)
            {
                var r = (Response) $"[{e.Message}]";
                r.StatusCode = HttpStatusCode.InternalServerError;
                return r;
            }
        }
    }
}