using System;
using System.Collections.Generic;
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
                Console.WriteLine($"Name: {tax.Municipality_Id}\t{tax.TaxType.ToString()}\tDate: {tax.StartDate}-{tax.EndDate}\tAmount:{tax.Amount}");
            }

            static async Task<Uri> CreateTaxAsync(Tax tax)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    "/tax/add", tax);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.
                return response.Headers.Location;
            }

            static async Task<List<Municipality>> GetMunicipalitiesAsync(string path)
            {
                List<Municipality> municipalities = null;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    municipalities = await response.Content.ReadAsAsync<List<Municipality>>();
                }
                return municipalities;
            }

            static async Task<Tax> GetTaxAsync(string path)
            {
                Tax tax = null;
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    tax = await response.Content.ReadAsAsync<Tax>();
                }
                return tax;
            }

            static async Task<Tax> UpdateTaxAsync(Tax tax)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    $"/tax/{tax.Id}", tax);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated tax from the response body.
                tax = await response.Content.ReadAsAsync<Tax>();
                return tax;
            }

            static async Task<HttpStatusCode> DeleteTaxAsync(int id)
            {
                HttpResponseMessage response = await client.DeleteAsync(
                    $"/tax/{id}");
                return response.StatusCode;
            }

            static void Main()
            {
                // Update port # in the following line.
                client.BaseAddress = new Uri("http://localhost:8080/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {

                    var tax1 = GetTaxAsync("/tax/vilnius/2016/1/1");
                    ShowTax(tax1.Result);
                    tax1 = GetTaxAsync("/tax/vilnius/2016/5/2");
                    ShowTax(tax1.Result);
                    tax1 = GetTaxAsync("/tax/vilnius/2016/7/10");
                    ShowTax(tax1.Result);
                    tax1 = GetTaxAsync("/tax/vilnius/2016/3/16");
                    ShowTax(tax1.Result);

                    var municipalities = GetMunicipalitiesAsync("/municipality").Result;

                // Create a new tax
                Tax tax = new Tax
                {
                    Municipality_Id = municipalities[1].Id,
                    StartDate = new DateTime(2017, 1, 1),
                    EndDate = new DateTime(2017, 12, 31),
                    Amount = 0.15f,
                    TaxType = TaxTypeEnum.Yearly
                };

                var url = CreateTaxAsync(tax).Result;
                Console.WriteLine($"Created at {url}");

                // Get the tax
                tax = GetTaxAsync(url.ToString()).Result;
                ShowTax(tax);

                // Update the tax
                Console.WriteLine("Updating price...");
                tax.Amount = 0.3f;
                tax = UpdateTaxAsync(tax).Result;
                ShowTax(tax);

                // Get the updated tax
                tax = GetTaxAsync(url.ToString()).Result;
                ShowTax(tax);

                // Delete the tax
                var statusCode = DeleteTaxAsync(tax.Id).Result;
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
