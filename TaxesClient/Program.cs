using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Model;

namespace TaxesClient
{


    namespace HttpClientSample
    {


        class Program
        {
            static HttpClient client = new HttpClient();

            static void ShowTax(Tax tax)
            {
                Console.WriteLine($"Name: {tax.Municipality.Name}\t{tax.TaxType.ToString()}\tDate: {tax.StartDate}-{tax.EndDate}\tAmount:{tax.Amount}");
            }

            static async Task<Uri> CreateTaxAsync(Tax tax)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    "/", tax);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.
                return response.Headers.Location;
            }

            static async Task<Tax> GetTaxAsync(string path)
            {
                Tax product = null;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadAsAsync<Tax>();
                }
                return product;
            }

            static async Task<Tax> UpdateTaxAsync(Tax tax)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    $"/{tax.Id}", tax);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated tax from the response body.
                tax = await response.Content.ReadAsAsync<Tax>();
                return tax;
            }

            static async Task<HttpStatusCode> DeleteTaxAsync(int id)
            {
                HttpResponseMessage response = await client.DeleteAsync(
                    $"/{id}");
                return response.StatusCode;
            }

            static void Main()
            {
                RunAsync().GetAwaiter().GetResult();
            }

            static async Task RunAsync()
            {
                // Update port # in the following line.
                client.BaseAddress = new Uri("http://localhost:8080/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    // Create a new tax
                    Tax tax = new Tax
                    {
                        Municipality = new Municipality() { Name = "New York" },
                        StartDate = new DateTime(2017, 1, 1),
                        EndDate = new DateTime(2017, 12, 31),
                        Amount = 0.15f,
                        TaxType = TaxTypeEnum.Yearly
                    };

                    var url = await CreateTaxAsync(tax);
                    Console.WriteLine($"Created at {url}");

                    // Get the tax
                    tax = await GetTaxAsync(url.PathAndQuery);
                    ShowTax(tax);

                    // Update the tax
                    Console.WriteLine("Updating price...");
                    tax.Amount = 0.3f;
                    await UpdateTaxAsync(tax);

                    // Get the updated tax
                    tax = await GetTaxAsync(url.PathAndQuery);
                    ShowTax(tax);

                    // Delete the tax
                    var statusCode = await DeleteTaxAsync(tax.Id);
                    Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.ReadLine();
            }
        }
    }
}
