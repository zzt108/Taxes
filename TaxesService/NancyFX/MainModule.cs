using Nancy;

namespace TaxesService
{
    public sealed class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/", _ => "Taxes server");
        }
    }
}