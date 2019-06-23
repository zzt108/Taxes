using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;

namespace IntegrationTest
{
    [TestClass]
    public class NancyFxTests
    {
        [TestMethod]
        public void CanAccessNancy()
        {
            // Given
            var browser = new Browser(with => with.Module<TaxesService.MainModule>());

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
            var browser = new Browser(with => with.Module<TaxesService.TaxModule>());

            // When
            var result = browser.Get("/tax/vilnius/2016/1/1", with =>
            {
                with.HttpRequest();
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.AsString().Should().Be("0.1");
        }

        [TestMethod]
        public void NoTaxData()
        {
            // Given
            var browser = new Browser(with => with.Module<TaxesService.TaxModule>());

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
            b.Should().Be("[Tax data for 16/01/01 00:00:00 not found!]");
        }
    }
}
