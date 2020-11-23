﻿using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Tests.Dto;
using Inshapardaz.Api.Views.Library;
using Inshapardaz.Domain.Adapters;
using NUnit.Framework;

namespace Inshapardaz.Api.Tests.BookPage.GetBookPages
{
    [TestFixture(Permission.Admin)]
    [TestFixture(Permission.LibraryAdmin)]
    [TestFixture(Permission.Writer)]
    public class WhenGettingBookPagesWithImagesAsWithPermission : TestBase
    {
        private BookDto _book;
        private HttpResponseMessage _response;
        private PagingAssert<BookPageView> _assert;

        public WhenGettingBookPagesWithImagesAsWithPermission(Permission permission)
            : base(permission)
        {
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            _book = BookBuilder.WithLibrary(LibraryId).WithPages(9, true).Build();

            _response = await Client.GetAsync($"/library/{LibraryId}/books/{_book.Id}/pages");

            _assert = new PagingAssert<BookPageView>(_response, Library);
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
            _assert.ShouldHaveSelfLink($"/library/{LibraryId}/books/{_book.Id}/pages");
        }

        [Test]
        public void ShouldHaveCreateLink()
        {
            _assert.ShouldHaveCreateLink($"/library/{LibraryId}/books/{_book.Id}/pages");
        }

        [Test]
        public void ShouldNotHaveNavigationLinks()
        {
            _assert.ShouldNotHaveNextLink();
            _assert.ShouldNotHavePreviousLink();
        }

        [Test]
        public void ShouldReturExpectedBookPages()
        {
            var expectedItems = BookBuilder.GetPages(_book.Id).OrderBy(p => p.SequenceNumber).Take(10);
            foreach (var item in expectedItems)
            {
                var actual = _assert.Data.FirstOrDefault(x => x.SequenceNumber == item.SequenceNumber);
                actual.ShouldMatch(item)
                    .InLibrary(LibraryId)
                            .ShouldHaveSelfLink()
                            .ShouldHaveBookLink()
                            .ShouldHaveImageLink(item.ImageId.Value)
                            .ShouldHaveUpdateLink()
                            .ShouldHaveDeleteLink()
                            .ShouldHaveImageUpdateLink()
                            .ShouldHaveImageDeleteLink();
            }
        }
    }
}