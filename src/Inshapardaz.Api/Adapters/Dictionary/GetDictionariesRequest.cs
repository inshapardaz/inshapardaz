﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Api.Helpers;
using Inshapardaz.Api.Renderers;
using Inshapardaz.Api.View;
using Inshapardaz.Domain.Database.Entities;
using Inshapardaz.Domain.Queries;
using Paramore.Brighter;
using Paramore.Darker;

namespace Inshapardaz.Api.Adapters.Dictionary
{
    public class GetDictionariesRequest : IRequest
    {
        public Guid Id { get; set; }

        public DictionariesView Result { get; set; }
    }

    public class GetDictionariesRequestHandler : RequestHandlerAsync<GetDictionariesRequest>
    {
        private readonly IUserHelper _userHelper;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IRenderDictionaries _dictionariesRenderer;

        public GetDictionariesRequestHandler(IQueryProcessor queryProcessor, IUserHelper userHelper, IRenderDictionaries dictionariesRenderer)
        {
            _queryProcessor = queryProcessor;
            _userHelper = userHelper;
            _dictionariesRenderer = dictionariesRenderer;
        }

        public override async Task<GetDictionariesRequest> HandleAsync(GetDictionariesRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = _userHelper.GetUserId();

            var results = await _queryProcessor.ExecuteAsync(new DictionariesByUserQuery { UserId = userId }, cancellationToken);

            var wordCounts = new Dictionary<int, int>();
            var downloads = new Dictionary<int, IEnumerable<DictionaryDownload>>();
            foreach (var dictionary in results)
            {
                wordCounts.Add(
                    dictionary.Id,
                    await _queryProcessor.ExecuteAsync(new GetDictionaryWordCountQuery {DictionaryId = dictionary.Id}, cancellationToken)
                );

                var download = await _queryProcessor.ExecuteAsync(new GetDictionaryDownloadsQuery {DictionaryId = dictionary.Id, UserId = _userHelper.GetUserId()}, cancellationToken);
                if (download != null && download.Any())
                {
                    downloads.Add(dictionary.Id, download);
                }

            }
            command.Result = _dictionariesRenderer.Render(results, wordCounts, downloads);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
