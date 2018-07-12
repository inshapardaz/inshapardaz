using System.Net;
using System.Net.Http;
using Inshapardaz.Api.View;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Inshapardaz.Api.IntegrationTests.Entry
{
    [TestFixture]
    public class EntryTestsGettingEntryAsAnonymousUser : IntegrationTestBase
    {
        private HttpResponseMessage _response;
        private EntryView _view;

        [OneTimeSetUp]
        public void Setup()
        {
            _response = GetClient().GetAsync("/api").Result;
            _view = JsonConvert.DeserializeObject<EntryView>(_response.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public void ShouldReturn200()
        {
            _response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldHaveCorrectResponseBody()
        {
            _view.ShouldNotBeNull();
        }

        [Test]
        public void ShouldHaveSelfLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Self && l.Href != null);
        }

        [Test]
        public void ShouldHaveDictionariesLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Dictionaries && l.Href != null);
        }

        [Test]
        public void ShouldHaveLanguagesLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Languages && l.Href != null);
        }

        [Test]
        public void ShouldHaveAttributesLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Attributes && l.Href != null);
        }

        [Test]
        public void ShouldHaveRelationshipTypesLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.RelationshipTypes && l.Href != null);
        }

        [Test]
        public void ShouldHaveThesaurusLink()
        {
            _view.Links.ShouldContain(l => l.Rel == RelTypes.Thesaurus && l.Href != null);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (isDisposing)
            {
                _response?.Dispose();
            }
        }
    }
}