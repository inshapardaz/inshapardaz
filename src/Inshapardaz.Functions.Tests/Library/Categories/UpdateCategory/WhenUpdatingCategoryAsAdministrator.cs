﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Functions.Views.Library;
using Inshapardaz.Ports.Database.Entities.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Inshapardaz.Functions.Tests.Library.Categories.UpdateCategory
{
    [TestFixture]
    public class WhenUpdatingCategoryAsAdministrator : FunctionTest
    {
        private OkObjectResult _response;
        private CategoriesDataBuilder _categoriesBuilder;
        private IEnumerable<Category> _categories;
        private Category _selectedCategory;
        private CategoryView _expectedCategory;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _categoriesBuilder = Container.GetService<CategoriesDataBuilder>();

            var handler = Container.GetService<Functions.Library.Categories.UpdateCategory>();
            _categories = _categoriesBuilder.WithBooks(3).Build(4);

            _selectedCategory = _categories.First();

            _expectedCategory = new CategoryView { Name = new Faker().Name.FullName()};
            var request = new RequestBuilder()
                                            .WithJsonBody(_expectedCategory)
                                            .Build();
            _response = (OkObjectResult) await handler.Run(request, _selectedCategory.Id, AuthenticationBuilder.AdminClaim, CancellationToken.None);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Cleanup();
        }

        [Test]
        public void ShouldHaveCreatedResult()
        {
            Assert.That(_response, Is.Not.Null);
            Assert.That(_response.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }

        [Test]
        public void ShouldHaveUpdatedTheCategory()
        {
            var createdCategory = _response.Value as CategoryView;
            Assert.That(createdCategory, Is.Not.Null);

            var cat = _categoriesBuilder.GetById(createdCategory.Id);
            Assert.That(cat.Name, Is.EqualTo(_expectedCategory.Name), "Category should be created.");

        }
    }
}