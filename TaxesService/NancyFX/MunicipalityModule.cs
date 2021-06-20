using System;
using Controller;
using Model;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public sealed class MunicipalityModule : NancyModule
    {
        public MunicipalityModule()   :base("/municipality")
        {
            var path = "/{name}";
            Get("/",_ => GetAll(_));
            Get("/name/{name}", _ => GetMunicipality(_));
            Post("/add", _ =>
            {
                try
                {
                    var model = this.Bind<Municipality>();
                    Municipalities.Add(model);
                    return model;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e,HttpStatusCode.InternalServerError);
                }
            });
        }

        private static object GetAll(dynamic _)
        {
            try
            {
                return Municipalities.GetAll();
             }
            catch (Exception e)
            {
                return Helper.ErrorResponse(e,HttpStatusCode.InternalServerError);
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
                return Helper.ErrorResponse(e,HttpStatusCode.InternalServerError);
            }
        }

    }
}