﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Dictionary;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace Inshapardaz.Api.IntegrationTests.Dictionary.Meaning
{
    [TestFixture]
    public class WhenGettingMeaningsForWordWithNoAccess : IntegrationTestBase
    {
        private IEnumerable<MeaningView> _view;
        private Domain.Entities.Dictionary.Dictionary _dictionary;
        private Domain.Entities.Dictionary.Word _word;
        private readonly Guid _userId = Guid.NewGuid();

        [OneTimeSetUp]
        public async Task Setup()
        {
            _dictionary = new Domain.Entities.Dictionary.Dictionary
            {
                IsPublic = false,
                UserId = _userId,
                Name = "Test1"
            };

            _word = new Domain.Entities.Dictionary.Word
            {
                Title = "abc",
                TitleWithMovements = "xyz",
                Language = Languages.Bangali,
                Pronunciation = "pas",
                Attributes = GrammaticalType.FealImdadi & GrammaticalType.Male,
                DictionaryId = _dictionary.Id
            };

            _dictionary = DictionaryDataHelper.CreateDictionary(_dictionary);
            _word = WordDataHelper.CreateWord(_dictionary.Id, _word);

            var meaning1 = MeaningDataHelper.CreateMeaning(_dictionary.Id, _word.Id, new Domain.Entities.Dictionary.Meaning {Context = "ctx", Value = "sdsd1", Example = "None"});
            var meaning2 = MeaningDataHelper.CreateMeaning(_dictionary.Id, _word.Id, new Domain.Entities.Dictionary.Meaning {Context = "ctx", Value = "sdsd2", Example = "None"});

            Response = await GetContributorClient(Guid.NewGuid()).GetAsync($"/api/dictionaries/{_dictionary.Id}/words/{_word.Id}/meanings");
            _view = JsonConvert.DeserializeObject<IEnumerable<MeaningView>>(await Response.Content.ReadAsStringAsync());
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DictionaryDataHelper.DeleteDictionary(_dictionary.Id);
        }

        [Test]
        public void ShouldReturnUnauthorised()
        {
            Response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void ShouldReturnNoMeanings()
        {
            _view.ShouldBeNull();
        }
    }
}