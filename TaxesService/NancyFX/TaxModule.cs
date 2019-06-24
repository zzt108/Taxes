using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Controller;
using DataAccessLayer;
using Model;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public sealed class TaxModule : NancyModule
    {
        public class RequestObject
        {
            public int Id;
            public string Path;
            public string Municipality;
            public int Year;
            public int Month;
            public int Day;
        }

        public TaxModule()   :base("/tax")
        {
            Get("/export", _ =>
            {
                try
                {
                    var request = this.Bind<RequestObject>();
                    if (string.IsNullOrWhiteSpace(request.Path))
                    {
                        return Helper.ErrorResponse(new ArgumentNullException("path","Please specify destination file as query parameter"), HttpStatusCode.BadRequest);
                    }
                    Taxes.ExportTax(request.Path, new UnitOfWork().TaxRepository.Get(tax => true));
                    return HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
                }
            });
            Get("/import", _ =>
            {
                try
                {
                    var request = this.Bind<RequestObject>();
                    if (string.IsNullOrWhiteSpace(request.Path))
                    {
                        return Helper.ErrorResponse(new ArgumentNullException("path","Please specify source file as query parameter"), HttpStatusCode.BadRequest);
                    }
                    Taxes.ImportTax(request.Path, new UnitOfWork().MunicipalityRepository.Get(tax => true));
                    return HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
                }
            });
            Get("/id/{id}", _ => GetTaxById(_));
            Get("/{municipality}/{year}/{month}/{day}", _ => GetTax(_));
            Get("/{municipality}/{year}/{month}/{day}", _ => GetTax(_));
            Post("/add", _ =>
            {
                try
                {
                    var model = this.Bind<Tax>();
                    //model.Municipality_Id = Municipalities.GetByName(model.Municipality.Name)?.Id;
                    using (var uw = new UnitOfWork())
                    {
                        uw.TaxRepository.Add(model);
                        uw.SaveChanges();
                    }

                    var r = Response.AsJson(model).WithHeader("Location", $"/tax/id/{model.Id}");
                    ;
                    return r;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
                }
            });
            Put("/{id}", _ =>
            {
                try
                {
                    var request = this.Bind<RequestObject>();
                    var model = this.Bind<Tax>();
                    using (var uw = new UnitOfWork())
                    {
                        Taxes.UpdateTax(uw, request.Id, model);
                        uw.SaveChanges();
                    }
                    return model;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
                }
            });
            Delete("/{id}", _ =>
            {
                try
                {
                    using (var uw = new UnitOfWork())
                    {
                        var request = this.Bind<RequestObject>();
                        uw.TaxRepository.Delete(request.Id);
                        uw.SaveChanges();
                    }
                    return HttpStatusCode.NoContent;
                }
                catch (Exception e)
                {
                    return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
                }
            });
        }

        private object GetTaxById(dynamic _)
        {
            try
            {
                var request = this.Bind<RequestObject>();
                return new UnitOfWork().TaxRepository.GetById(request.Id);
            }
            catch (Exception e)
            {
                return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
            }

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
                return Helper.ErrorResponse(e,HttpStatusCode.InternalServerError);
            }

            try
            {
                return Taxes.GetTax(_.municipality.ToString(), new DateTime(year, month, day));
            }
            catch (Exception e)
            {
                return Helper.ErrorResponse(e,HttpStatusCode.InternalServerError);
            }
        }
    }
}