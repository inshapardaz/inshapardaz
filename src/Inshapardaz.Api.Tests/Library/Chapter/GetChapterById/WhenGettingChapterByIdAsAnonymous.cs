﻿using Inshapardaz.Api.Tests.Asserts;
using Inshapardaz.Api.Tests.Dto;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inshapardaz.Api.Tests.Library.Chapter.GetChapterById
{
    [TestFixture]
    public class WhenGettingChapterByIdAsAnonymous
        : TestBase
    {
        private HttpResponseMessage _response;

        private ChapterDto _expected;

        private ChapterAssert _assert;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _expected = ChapterBuilder.WithLibrary(LibraryId).WithContents().Build(4).First();

            _response = await Client.GetAsync($"/libraries/{LibraryId}/books/{_expected.BookId}/chapters/{_expected.ChapterNumber}");
            _assert = ChapterAssert.FromResponse(_response, LibraryId);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveOkResult()
        {
            _response.ShouldBeOk();
        }

        [Test]
        public void ShouldHaveCorrectObjectRetured()
        {
            _assert.ShouldMatch(_expected);
        }

        [Test]
        public void ShouldHaveLinks()
        {
            _assert.ShouldHaveSelfLink()
                   .ShouldHaveBookLink()
                   .ShouldNotHaveContentsLink();
        }

        [Test]
        public void ShouldNotHaveEditLinks()
        {
            _assert.ShouldNotHaveUpdateLink()
                   .ShouldNotHaveDeleteLink()
                   .ShouldNotHaveAddChapterContentLink();
        }

        [Test]
        public void ShouldHaveNoContentsLink()
        {
            _assert.ShouldNotHaveContentsLink();
        }
    }
}
