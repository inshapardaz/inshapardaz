﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.GetBooksBySeries
{
    [TestFixture]
    public class WhenGettingBooksBySeriesLastPage : FunctionTest
    {
        private OkObjectResult _response;
        private PageView<BookView> _view;
        
        [OneTimeSetUp]
        public async Task Setup()
        {
            var request = new RequestBuilder()
                          .WithQueryParameter("pageNumber", 2)
                          .WithQueryParameter("pageSize", 10)
                          .Build();

            var builder = Container.GetService<BooksDataBuilder>();
            builder.HavingSeries().Build(20);
            
            var seriesDataBuilder = Container.GetService<SeriesDataBuilder>();
            var series = seriesDataBuilder.Build();
            builder.WithSeries(series).Build(20);
            
            var handler = Container.GetService<Functions.Library.Books.GetBooksBySeries>();
            _response = (OkObjectResult) await handler.Run(request, series.Id,  AuthenticationBuilder.Unauthorized, CancellationToken.None);

            _view = _response.Value as PageView<BookView>;
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveOkResult()
        {
            Assert.That(_response, Is.Not.Null);
            Assert.That(_response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ShouldHaveSelfLink()
        {
            _view.Links.AssertLink("self")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldNotHaveNextLink()
        {
            _view.Links.AssertLinkNotPresent("next");
        }

        [Test]
        public void ShouldHavePreviousLink()
        {
            _view.Links.AssertLink("previous")
                 .ShouldBeGet()
                 .ShouldHaveSomeHref(); ;
        }

        [Test]
        public void ShouldHaveSomeBooks()
        {
            Assert.IsNotEmpty(_view.Data, "Should return some books.");
            Assert.That(_view.Data.Count(), Is.EqualTo(10), "Should return all books on page");
        }

        [Test]
        public void ShouldReturnCorrectPage()
        {
            Assert.That(_view.PageCount, Is.EqualTo(2));
            Assert.That(_view.PageSize, Is.EqualTo(10));
            Assert.That(_view.TotalCount, Is.EqualTo(20));
            Assert.That(_view.CurrentPageIndex, Is.EqualTo(2));
        }
        
        [Test]
        public void ShouldHaveCorrectBookData()
        {
            var actual = _view.Data.FirstOrDefault();
            Assert.That(actual, Is.Not.Null, "Should contain at-least one book");
            Assert.That(actual.Title, Is.Not.Empty, "Book name should have a value");
            Assert.That(actual.Description, Is.Not.Empty, "Book should have some description.");

            actual.Links.AssertLinkNotPresent("update");
            actual.Links.AssertLinkNotPresent("delete");
        }
    }
}