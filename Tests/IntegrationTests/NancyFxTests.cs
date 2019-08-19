using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Nancy;
using Nancy.Testing;
using TaxesService;
using TaxesService.NancyFX;

namespace IntegrationTest
{
    [TestClass]
    public class NancyFxTests
    {
        private const string InaccessibleFile = @"c:\taxdata.csv";

        [TestMethod]
        public void CanAccessNancy()
        {
            // Given
            var browser = new Browser(with => with.Module<MainModule>());

            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
        }

        [TestMethod]
        public void CanGetHappyPathValues()
        {
            // Given
            var browser = new Browser(with =>
            {
                with.Module<TaxModule>();
            });

            // When
            var result = browser.Get("/tax/vilnius/2016/1/1", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var t = response.Body.DeserializeJson<Tax>().Amount.Should().Be(0.1f);

            // When
            result = browser.Get("/tax/vilnius/2016/5/2", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });
            response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.DeserializeJson<Tax>().Amount.Should().Be(0.4f);
            // When
            result = browser.Get("/tax/vilnius/2016/7/10", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });
            response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.DeserializeJson<Tax>().Amount.Should().Be(0.2f);
            // When
            result = browser.Get("/tax/vilnius/2016/3/16", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });
            response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.DeserializeJson<Tax>().Amount.Should().Be(0.2f);
        }

        [TestMethod]
        public void CanGetTaxById()
        {
            // Given
            var browser = new Browser(with =>
            {
                with.Module<TaxModule>();
            });

            // When
            var result = browser.Get("/tax/id/1", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var t = response.Body.DeserializeJson<Tax>().Amount.Should().Be(0.1f);

        }

        [TestMethod]
        public void CanExport()
        {
            Assert.Inconclusive("Export Test is not functional");
            // Given
            var browser = new Browser(with =>
            {
                with.Module<ExportModule>();
            });

            // When
            var path = System.Web.HttpUtility.UrlEncode(InaccessibleFile);
            var result = browser.Get($"/export/tax", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
                //passing query string in url is not supported
                with.Query("path", path);
            });
            var response = result.Result;
            // Then
            // response.StatusCode.Should().Be(HttpStatusCode.InternalServerError, $"{InaccessibleFile} access should be denied");
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"{InaccessibleFile} access error");

        }

        [TestMethod]
        public void CanHandleExportMissingPath()
        {
            // Given
            var browser = new Browser(with =>
            {
                with.Module<ExportModule>();
            });

            // When
            var result = browser.Get($"/export/tax", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [TestMethod]
        public void CanImport()
        {
            Assert.Inconclusive("Import Test is not functional");

            // Given
            var browser = new Browser(with =>
            {
                with.Module<ImportModule>();
            });

            // When
            var path = System.Web.HttpUtility.UrlEncode(InaccessibleFile);
            var result = browser.Get($"/import/tax", with =>
            {
                with.HttpRequest();
                with.Header("accept", "application/json");
                //passing query string in url is not supported
                with.Query("path", path);
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError, $"{InaccessibleFile} should not exist"); 

        }

        [TestMethod]
        public void NoTaxData()
        {
            // Given
            var browser = new Browser(with => with.Module<TaxModule>());

            // When
            var result = browser.Get("/tax/taastrup/2016/1/1", with =>
            {
                with.HttpRequest();
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var b = response.Body.AsString();
            Console.WriteLine(b);
            b.Should().Be("[Tax data not found!]");
        }

        [TestMethod]
        public void NoMunicipality()
        {
            // Given
            var browser = new Browser(with => with.Module<TaxModule>());

            // When
            var result = browser.Get("/tax/Berlin/2016/1/1", with =>
            {
                with.HttpRequest();
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var b = response.Body.AsString();
            Console.WriteLine(b);
            b.Should().Be("[Municipality Berlin not found!]");
        }
    }
}
