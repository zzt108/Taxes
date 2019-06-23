using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;
using TaxesService;
using TaxesService.NancyFX;

namespace IntegrationTest
{
    [TestClass]
    public class NancyFxTests
    {
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
            var browser = new Browser(with => with.Module<TaxModule>());

            // When
            var result = browser.Get("/tax/vilnius/2016/1/1", with =>
            {
                with.HttpRequest();
            });
            var response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.AsString().Should().Be("0.1");
            // When
            result = browser.Get("/tax/vilnius/2016/5/2", with =>
            {
                with.HttpRequest();
            });
            response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.AsString().Should().Be("0.4");
            // When
            result = browser.Get("/tax/vilnius/2016/7/10", with =>
            {
                with.HttpRequest();
            });
            response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.AsString().Should().Be("0.2");
            // When
            result = browser.Get("/tax/vilnius/2016/3/16", with =>
            {
                with.HttpRequest();
            });
            response = result.Result;
            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body.AsString().Should().Be("0.2");
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
            b.Should().Be("[Tax data for 16/01/01 00:00:00 not found!]");
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
