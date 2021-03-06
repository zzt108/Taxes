﻿using System;
using Controller;
using Model;
using Nancy;
using Nancy.ModelBinding;

namespace TaxesService.NancyFX
{
    public sealed class TaxModule : NancyModule
    {

        public TaxModule() : base("/tax")
        {
            Get("/id/{id}", _ => GetTaxById(_));
            Get("/{municipality}/{year}/{month}/{day}", _ => GetTax(_));
            Post("/add", _ =>
            {
                try
                {
                    var model = this.Bind<Tax>();
                    Taxes.Add(model);

                    var r = Response.AsJson(model).WithHeader("Location", $"/tax/id/{model.Id}");
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
                    Taxes.UpdateTax(request.Id, model);

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
                    var request = this.Bind<RequestObject>();
                    Taxes.Delete(request.Id);

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
                return Taxes.GetById(request.Id);
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
                return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
            }

            try
            {
                return Taxes.GetTax(_.municipality.ToString(), new DateTime(year, month, day));
            }
            catch (Exception e)
            {
                return Helper.ErrorResponse(e, HttpStatusCode.InternalServerError);
            }
        }
    }

}