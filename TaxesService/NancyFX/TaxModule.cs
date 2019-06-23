using System;
using System.Collections.Generic;
using Controller;
using Model;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public class BaseModule : NancyModule
    {
        protected BaseModule(string path) : base(path)
        {
            
        }

        protected static Response ErrorResponse(Exception e, HttpStatusCode httpStatusCode)
        {
            var r = (Response) $"[{e.Message}]";
            r.StatusCode = httpStatusCode;
            return r;
        }
    }

    public sealed class TaxModule : BaseModule
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
                    //model.Municipality_Id = Municipalities.GetByName(model.Municipality.Name)?.Id;
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
                return ErrorResponse(e,HttpStatusCode.InternalServerError);
            }

            try
            {
                return Taxes.GetTax(_.municipality.ToString(), new DateTime(year, month, day)).ToString();
            }
            catch (Exception e)
            {
                return ErrorResponse(e,HttpStatusCode.InternalServerError);
            }
        }
    }

    public sealed class MunicipalityModule : BaseModule
    {
        public MunicipalityModule()   :base("/municipality")
        {
            var path = "/{name}";
            Get("/",_ => GetAll(_));
            Get("/name/{name}", _ => GetMunicipality(_));
            Post("/", _ =>
            {
                try
                {
                    var model = this.Bind<Municipality>();
                    Municipalities.Add(model);
                    return model;
                }
                catch (Exception e)
                {
                    return ErrorResponse(e,HttpStatusCode.InternalServerError);
                }
            });
        }

        private static object GetAll(dynamic _)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorResponse(e,HttpStatusCode.InternalServerError);
            }
        }

        private static object GetMunicipality(dynamic _)
        {

            try
            {
                return Municipalities.GetByName(_.name.ToString()).ToString();
            }
            catch (Exception e)
            {
                return ErrorResponse(e,HttpStatusCode.InternalServerError);
            }
        }

    }
}