using Nancy;

namespace TaxesService.NancyFX
{
    public sealed class MainModule : NancyModule
    {
        public MainModule()
        {
            Get("/", _ => "Taxes server");
        }
    }
}