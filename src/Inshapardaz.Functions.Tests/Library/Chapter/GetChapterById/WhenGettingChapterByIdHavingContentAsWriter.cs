﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Chapter.GetChapterById
{
    [TestFixture]
    public class WhenGettingChapterByIdHavingContentAsWriter : FunctionTest
    {
        private OkObjectResult _response;
        private ChapterView _view;
        private Ports.Database.Entities.Library.Chapter _expected;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var dataBuilder = Container.GetService<ChapterDataBuilder>();
            _expected = dataBuilder.AsPublic().WithContents().Build(4).First();
            
            var handler = Container.GetService<Functions.Library.Books.Chapters.GetChapterById>();
            _response = (OkObjectResult) await handler.Run(null, _expected.BookId, _expected.Id, NullLogger.Instance, AuthenticationBuilder.WriterClaim, CancellationToken.None);

            _view = _response.Value as ChapterView;
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
        public void ShouldNotHaveAddChapterContentLink()
        {
            _view.Links.AssertLinkNotPresent("add-contents");
        }

        [Test]
        public void ShouldHaveUpdateChapterContentLink()
        {
            _view.Links.AssertLink("update-contents")
                 .ShouldBePut()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldHaveDeleteChapterContentLink()
        {
            _view.Links.AssertLink("delete-contents")
                 .ShouldBeDelete()
                 .ShouldHaveSomeHref();
        }

        [Test]
        public void ShouldReturnCorrectAuthorData()
        {
            Assert.That(_view, Is.Not.Null, "Should return chapter");
            Assert.That(_view.Id, Is.EqualTo(_expected.Id), "Chapter id does not match");
            Assert.That(_view.Title, Is.EqualTo(_expected.Title), "Chapter name does not match");
        }
    }
}
