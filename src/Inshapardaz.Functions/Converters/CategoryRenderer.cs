﻿using Inshapardaz.Domain.Models.Library;
using Inshapardaz.Functions.Extensions;
using Inshapardaz.Functions.Library.Books;
using Inshapardaz.Functions.Library.Categories;
using Inshapardaz.Functions.Mappings;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Views.Library;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Inshapardaz.Functions.Converters
{
    public static class CategoryRenderer
    {
        public static ListView<CategoryView> Render(this IEnumerable<CategoryModel> categories, int libraryId, ClaimsPrincipal user)
        {
            var items = categories.Select(g => g.Render(libraryId, user));
            var view = new ListView<CategoryView> { Data = items };
            view.Links.Add(GetCategories.Link(libraryId));

            if (user.IsAdministrator())
            {
                view.Links.Add(AddCategory.Link(libraryId, RelTypes.Create));
            }

            return view;
        }

        public static CategoryView Render(this CategoryModel category, int libraryId, ClaimsPrincipal principal)
        {
            var view = category.Map();

            view.Links.Add(GetCategoryById.Link(libraryId, category.Id));
            view.Links.Add(GetBooks.Link(libraryId, relType: RelTypes.Books, filter: new BookFilter { CategoryId = category.Id }));

            if (principal.IsAdministrator())
            {
                view.Links.Add(UpdateCategory.Link(libraryId, category.Id, RelTypes.Update));
                view.Links.Add(DeleteCategory.Link(libraryId, category.Id, RelTypes.Delete));
            }

            return view;
        }
    }
}
