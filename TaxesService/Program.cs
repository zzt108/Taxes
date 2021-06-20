using System;
using System.ServiceProcess;
using Nancy;
using Nancy.Hosting.Self;

namespace TaxesService
{
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
