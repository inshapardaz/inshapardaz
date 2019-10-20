using System.Linq;
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
    public class WhenGettingBooksBySeriesAdministrator : FunctionTest
    {
        private OkObjectResult _response;
        private PageView<BookView> _view;
        
        [OneTimeSetUp]
        public async Task Setup()
        {
            var request = TestHelpers.CreateGetRequest();

            var builder = Container.GetService<BooksDataBuilder>();
            builder.HavingSeries().Build(3);
            
            var seriesDataBuilder = Container.GetService<SeriesDataBuilder>();
            var series = seriesDataBuilder.Build();
            builder.WithSeries(series).Build(4);
            
            var handler = Container.GetService<Functions.Library.Books.GetBooksBySeries>();
            _response = (OkObjectResult) await handler.Run(request, series.Id,  AuthenticationBuilder.AdminClaim, CancellationToken.None);

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
        public void ShouldNotHaveCreateLink()
        {
            _view.Links.AssertLink("create")
                 .ShouldBePost()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldReturnCorrectPage()
        {
            Assert.That(_view.PageCount, Is.EqualTo(1));
            Assert.That(_view.PageSize, Is.EqualTo(10));
            Assert.That(_view.TotalCount, Is.EqualTo(4));
            Assert.That(_view.CurrentPageIndex, Is.EqualTo(1));
        }

        [Test]
        public void ShouldHaveCorrectBookData()
        {
            var actual = _view.Data.FirstOrDefault();
            Assert.That(actual, Is.Not.Null, "Should contain at-least one book");
            Assert.That(actual.Title, Is.Not.Empty, "Book name should have a value");
            Assert.That(actual.Description, Is.Not.Empty, "Book should have some description.");

            actual.Links.AssertLink("update")
                      .ShouldBePut()
                      .ShouldHaveSomeHref();
                actual.Links.AssertLink("delete")
                      .ShouldBeDelete()
                      .ShouldHaveSomeHref();
        }
    }
}