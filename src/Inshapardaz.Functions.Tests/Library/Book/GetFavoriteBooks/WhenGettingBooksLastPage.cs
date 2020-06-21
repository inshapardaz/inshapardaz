﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.Asserts;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.GetFavoriteBooks
{
    [TestFixture]
    public class WhenGettingBooksLastPage
            : LibraryTest<Functions.Library.Books.GetBooks>
    {
        private OkObjectResult _response;
        private PagingAssert<BookView> _assert;
        private BooksDataBuilder _builder;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var claim = AuthenticationBuilder.ReaderClaim;

            var request = new RequestBuilder()
                          .WithQueryParameter("pageNumber", 3)
                          .WithQueryParameter("pageSize", 10)
                          .WithQueryParameter("favorite", bool.TrueString)
                          .Build();

            _builder = Container.GetService<BooksDataBuilder>();
            _builder.WithLibrary(LibraryId).IsPublic().AddToFavorites(claim.GetUserId()).Build(25);

            _response = (OkObjectResult)await handler.Run(request, LibraryId, claim, CancellationToken.None);

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
            _assert.ShouldHaveSelfLink($"/api/library/{LibraryId}/books");
        }

        [Test]
        public void ShouldNotHaveNextLink()
        {
            _assert.ShouldNotHaveNextLink();
        }

        [Test]
        public void ShouldHavePreviousLink()
        {
            _assert.ShouldHavePreviousLink($"/api/library/{LibraryId}/books", 2);
        }

        [Test]
        public void ShouldReturnCorrectPage()
        {
            _assert.ShouldHavePage(3)
                   .ShouldHavePageSize(10)
                   .ShouldHaveTotalCount(_builder.Books.Count())
                   .ShouldHaveItems(5);
        }

        [Test]
        public void ShouldReturnExpectedBooks()
        {
            var expectedItems = _builder.Books.OrderBy(a => a.Title).Skip(2 * 10).Take(10);
            foreach (var item in expectedItems)
            {
                var actual = _assert.Data.FirstOrDefault(x => x.Id == item.Id);
                actual.ShouldMatch(item, DatabaseConnection)
                            .InLibrary(LibraryId)
                            .ShouldHaveCorrectLinks()
                            .ShouldNotHaveEditLinks()
                            .ShouldNotHaveImageUpdateLink()
                            .ShouldNotHaveCreateChaptersLink()
                            .ShouldNotHaveAddContentLink()
                            .ShouldHaveChaptersLink()
                            .ShouldHaveRemoveFavoriteLink()
                            .ShouldHavePublicImageLink();
            }
        }
    }
}
