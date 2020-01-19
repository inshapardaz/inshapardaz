﻿using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Book.Files.UpdateBookFile
{
    [TestFixture]
    public class WhenUpdatingBookFileForNonExistingBook : FunctionTest
    {
        private BadRequestResult _response;

        private BooksDataBuilder _dataBuilder;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _dataBuilder = Container.GetService<BooksDataBuilder>();

            var newContents = new Faker().Image.Random.Bytes(50);
            var request = new RequestBuilder().WithBytes(newContents).BuildRequestMessage();
            var handler = Container.GetService<Functions.Library.Books.Files.UpdateBookFile>();
            _response = (BadRequestResult)await handler.Run(request, Random.Number, Random.Number, AuthenticationBuilder.WriterClaim, CancellationToken.None);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldReturnBadRequestResult()
        {
            Assert.That(_response, Is.Not.Null);
        }
    }
}