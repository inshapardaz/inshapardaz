﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Library.Authors;
using Inshapardaz.Functions.Library.Books;
using Inshapardaz.Functions.Library.Files;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Views.Library;

namespace Inshapardaz.Functions.Converters
{
    public static class AuthorsRenderer 
    {
        public static PageView<AuthorView> Render(this PageRendererArgs<Author> source, ClaimsPrincipal principal)
        {
            var page = new PageView<AuthorView>(source.Page.TotalCount, source.Page.PageSize, source.Page.PageNumber)
            {
                Data = source.Page.Data?.Select(x => x.Render(principal))
            };

            
            var links = new List<LinkView>
            {
                source.LinkFunc(page.CurrentPageIndex, page.PageSize, RelTypes.Self)
            };

            if (principal.IsWriter())
            {
                links.Add(AddAuthor.Link(RelTypes.Create));
            }

            if (page.CurrentPageIndex < page.PageCount)
            {
                links.Add(source.LinkFunc(page.CurrentPageIndex + 1, page.PageSize, RelTypes.Next));
            }

            if (page.PageCount > 1 && page.CurrentPageIndex > 1 && page.CurrentPageIndex <= page.PageCount)
            {
                links.Add(source.LinkFunc(page.CurrentPageIndex - 1, page.PageSize, RelTypes.Previous));
            }

            page.Links = links;
            return page;
        }

        public static AuthorView Render(this Author source, ClaimsPrincipal principal)
        {
            var result = source.Map();

            var links = new List<LinkView>
            {
                GetAuthorById.Link(source.Id, RelTypes.Self),
                GetBooksByAuthor.Link(source.Id, RelTypes.Books)
            };

            if (source.ImageId > 0)
            {
                links.Add(GetFileById.Link(source.ImageId, RelTypes.Image));
            }

            if (principal.IsWriter())
            {
                links.Add(UpdateAuthor.Link(source.Id, RelTypes.Update));
                links.Add(DeleteAuthor.Link(source.Id, RelTypes.Delete));
                links.Add(UpdateAuthorImage.Link(source.Id, RelTypes.ImageUpload));
            }

            result.Links = links;
            return result;
        }
    }
}