using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.Asserts;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Dto;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.GetBooksBySeries
{
    [TestFixture]
    public class WhenSearchingBooksBySeriesAndTitle
        : LibraryTest<Functions.Library.Books.GetBooks>
    {
        private OkObjectResult _response;
        private PagingAssert<BookView> _assert;
        private SeriesDataBuilder _seriesBuilder;
        private SeriesDto _series;
        private IEnumerable<BookDto> _seriesBooks;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _seriesBuilder = Container.GetService<SeriesDataBuilder>();
            var _bookBuilder = Container.GetService<BooksDataBuilder>();
            _series = _seriesBuilder.WithLibrary(LibraryId).Build();
            _seriesBooks = _bookBuilder.WithLibrary(LibraryId).WithSeries(_series).IsPublic().Build(25);
            _seriesBuilder.WithLibrary(LibraryId).WithBooks(3).Build();

            var request = new RequestBuilder()
                          .WithQueryParameter("pageNumber", 2)
                          .WithQueryParameter("pageSize", 10)
                          .WithQueryParameter("query", "itle")
                          .WithQueryParameter("seriesid", _series.Id)
                          .Build();

            _response = (OkObjectResult)await handler.Run(request, LibraryId, AuthenticationBuilder.ReaderClaim, CancellationToken.None);

            _assert = new PagingAssert<BookView>(_response, Library);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldReturnOk()
        {
            _response.ShouldBeOk();
        }

        [Test]
        public void ShouldHaveSelfLink()
        {
            _assert.ShouldHaveSelfLink($"/api/library/{LibraryId}/books", 2, 10, "query", "itle");
            _assert.ShouldHaveSelfLink($"/api/library/{LibraryId}/books", 2, 10, "seriesid", _series.Id.ToString());
        }

        [Test]
        public void ShouldNotHaveCreateLink()
        {
            _assert.ShouldNotHaveCreateLink();
        }

        [Test]
        public void ShouldNotHaveNextLink()
        {
            _assert.ShouldHaveNextLink($"/api/library/{LibraryId}/books", 3, 10, "query", "itle");
            _assert.ShouldHaveNextLink($"/api/library/{LibraryId}/books", 3, 10, "seriesid", _series.Id.ToString());
        }

        [Test]
        public void ShouldNotHavePreviousLink()
        {
            _assert.ShouldHavePreviousLink($"/api/library/{LibraryId}/books", 1, 10, "query", "itle");
            _assert.ShouldHavePreviousLink($"/api/library/{LibraryId}/books", 1, 10, "seriesid", _series.Id.ToString());
        }

        [Test]
        public void ShouldReturnCorrectPage()
        {
            _assert.ShouldHavePage(2)
                   .ShouldHavePageSize(10)
                   .ShouldHaveTotalCount(_seriesBooks.Count())
                   .ShouldHaveItems(10);
        }

        [Test]
        public void ShouldReturnExpectedBooks()
        {
            var expectedItems = _seriesBooks.Where(b => b.Title.Contains("itle"))
                                            .OrderBy(a => a.Title)
                                            .Skip(10)
                                            .Take(10);
            foreach (var item in expectedItems)
            {
                var actual = _assert.Data.FirstOrDefault(x => x.Id == item.Id);
                actual.ShouldMatch(item, DatabaseConnection)
                            .InLibrary(LibraryId)
                            .ShouldHaveCorrectLinks()
                            .ShouldHaveSeriesLink()
                            .ShouldNotHaveEditLinks()
                            .ShouldNotHaveImageUpdateLink()
                            .ShouldNotHaveCreateChaptersLink()
                            .ShouldNotHaveAddContentLink()
                            .ShouldHaveChaptersLink()
                            .ShouldHavePublicImageLink();
            }
        }
    }
}