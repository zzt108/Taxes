using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;

namespace TaxesService
{
    public sealed class MainModule : NancyModule    
    {
        public MainModule()
        {
            Get("/", _ => "Taxes server");
        }
    }

    public sealed class TaxModule : NancyModule
    {
        public TaxModule()   :base("/tax")
        {
            var path = "/{municipality}/{year}/{month}/{day}";
            Get("/",_ => $"Usage:/tax{path}");
            Get(path, _ => GetTax(_));
            Post("/", _ =>
            {
                var model = this.Bind<Tax>();
                Taxes.Add(model);
                return model;
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
                return $"[{_}] [{e.Message}]";
            }

            try
            {
                return Taxes.GetTax(_.municipality.ToString(), new DateTime(year, month, day)).ToString();
            }
            catch (Exception e)
            {
                return $"[{string.Join("|", _)}] [{e.Message}]";
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            NancyHost host;
            string url = "http://localhost:8080";
            HostConfiguration hostConfigs = new HostConfiguration {UrlReservations = {CreateAutomatically = true}};
            host = new NancyHost(new Uri(url), new DefaultNancyBootstrapper(), hostConfigs);
            host.Start();

            //Debug code
            if (!Environment.UserInteractive)
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[]
                {
                //    //new Service1()
                };
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                // forces debug to keep VS running while we debug the service
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
        }
    }
}
