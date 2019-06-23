using System;
using System.Collections.Generic;
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
            Get("/id/{id}", _ => GetTaxById(_));
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
                    var model = this.Bind<Tax>();
                    //model.Municipality_Id = Municipalities.GetByName(model.Municipality.Name)?.Id;
                    using (var uw = new UnitOfWork())
                    {
                        int id = _.id;
                        if (uw.TaxRepository.GetById(id) is Tax tax)
                        {
                            tax.Amount = model.Amount;
                            tax.EndDate = model.EndDate;
                            tax.Municipality_Id = model.Municipality_Id;
                            tax.StartDate = model.StartDate;
                            tax.TaxType = model.TaxType;
                            uw.SaveChanges();
                        }
                        else
                        {
                            return Helper.ErrorResponse(new ArgumentException($"{id} not found"),
                                HttpStatusCode.InternalServerError);
                        }
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
                        int id = _.id;
                        uw.TaxRepository.Delete(id);
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
            int id = _.id;
            return new UnitOfWork().TaxRepository.GetById(id);
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