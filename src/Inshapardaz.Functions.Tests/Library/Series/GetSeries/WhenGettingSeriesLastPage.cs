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

namespace Inshapardaz.Functions.Tests.Library.Series.GetSeries
{
    [TestFixture]
    public class WhenGettingSeriesLastPage : LibraryTest<Functions.Library.Series.GetSeries>
    {
        private SeriesDataBuilder _builder;
        private OkObjectResult _response;
        private PagingAssert<SeriesView> _assert;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var request = new RequestBuilder()
                .WithQueryParameter("pageNumber", 5)
                .WithQueryParameter("pageSize", 10)
                .Build();

            _builder = Container.GetService<SeriesDataBuilder>();
            _builder.WithLibrary(LibraryId).WithBooks(3).Build(50);

            _response = (OkObjectResult)await handler.Run(request, LibraryId, AuthenticationBuilder.Unauthorized, CancellationToken.None);

            _assert = new PagingAssert<SeriesView>(_response);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _builder.CleanUp();
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
            _assert.ShouldHaveSelfLink($"/api/library/{LibraryId}/series");
        }

        [Test]
        public void ShouldHavePreviousLink()
        {
            _assert.ShouldHavePreviousLink($"/api/library/{LibraryId}/series", 4);
        }

        [Test]
        public void ShouldNotHaveNextLink()
        {
            _assert.ShouldNotHaveNextLink();
        }

        [Test]
        public void ShouldHaveCorrectSeriesData()
        {
            var expectedItems = _builder.Series.OrderBy(a => a.Name).Skip(4 * 10).Take(10);
            foreach (var item in expectedItems)
            {
                var actual = _assert.Data.FirstOrDefault(x => x.Id == item.Id);
                actual.ShouldMatch(item)
                      .InLibrary(LibraryId)
                      .WithBookCount(3)
                      .WithReadOnlyLinks()
                      .ShouldHavePublicImageLink();
            }
        }
    }
}
