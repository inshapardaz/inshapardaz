﻿using System.Collections.Generic;
using System.Linq;
using Inshapardaz.Api.View;
using Inshapardaz.Api.View.Dictionary;
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;

namespace Inshapardaz.Api.Renderers.Dictionary
{
    public interface IRenderWordPage
    {
        PageView<WordView> Render(PageRendererArgs<Word> source, int dictionaryId);
    }

    public class WordPageRenderer : IRenderWordPage
    {
        private readonly IRenderLink _linkRenderer;
        private readonly IRenderWord _wordRenderer;

        public WordPageRenderer(IRenderLink linkRenderer, IRenderWord wordRenderer)
        {
            _linkRenderer = linkRenderer;
            _wordRenderer = wordRenderer;
        }

        public PageView<WordView> Render(PageRendererArgs<Word> source, int dictionaryId)
        {
            var page = new PageView<WordView>(source.Page.TotalCount, source.Page.PageSize, source.Page.PageNumber)
            {
                Data = source.Page.Data?.Select(x => _wordRenderer.Render(x, dictionaryId))
            };

            var links = new List<LinkView>
            {
                _linkRenderer.Render(source.RouteName, RelTypes.Self,
                    CreateRouteParameters(source, dictionaryId, page.CurrentPageIndex, page.PageSize))
            };

            if (page.CurrentPageIndex < page.PageCount)
            {
                links.Add(_linkRenderer.Render(source.RouteName, RelTypes.Next,
                    CreateRouteParameters(source, dictionaryId, page.CurrentPageIndex + 1, page.PageSize)));
            }

            if (page.PageCount > 1 && page.CurrentPageIndex > 1)
            {
                links.Add(_linkRenderer.Render(source.RouteName, RelTypes.Previous,
                    CreateRouteParameters(source, dictionaryId, page.CurrentPageIndex - 1, page.PageSize)));
            }

            page.Links = links;
            return page;
        }

        private object CreateRouteParameters(PageRendererArgs<Word> source, int dictionaryId, int pageNumber, int pageSize)
        {
            var args = source.RouteArguments as DictionaryPagedRouteArgs ?? new DictionaryPagedRouteArgs();
            args.DictionaryId = dictionaryId;
            args.PageNumber = pageNumber;
            args.PageSize = pageSize;
            return args;
        }
    }
}