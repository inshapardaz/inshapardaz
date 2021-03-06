﻿using Inshapardaz.Domain.Adapters.Repositories.Library;
using Inshapardaz.Domain.Models.Handlers.Library;
using Paramore.Darker;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Models.Library
{
    public class GetArticlesByIssueQuery : LibraryBaseQuery<IEnumerable<ArticleModel>>
    {
        public GetArticlesByIssueQuery(int libraryId, int periodicalId, int issueId)
            : base(libraryId)
        {
            PeriodicalId = periodicalId;
            IssueId = issueId;
        }

        public int PeriodicalId { get; }
        public int IssueId { get; }
    }

    public class GetArticlesByIssueQuerytHandler : QueryHandlerAsync<GetArticlesByIssueQuery, IEnumerable<ArticleModel>>
    {
        private readonly IArticleRepository _articleRepository;

        public GetArticlesByIssueQuerytHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public override async Task<IEnumerable<ArticleModel>> ExecuteAsync(GetArticlesByIssueQuery command, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _articleRepository.GetArticlesByIssue(command.LibraryId, command.PeriodicalId, command.IssueId, cancellationToken);
        }
    }
}
