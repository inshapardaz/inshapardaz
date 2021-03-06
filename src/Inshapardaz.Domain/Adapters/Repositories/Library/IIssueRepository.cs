﻿using Inshapardaz.Domain.Models;
using Inshapardaz.Domain.Models.Library;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Repositories.Library
{
    public interface IIssueRepository
    {
        Task<IssueModel> GetIssueById(int libraryId, int periodicalId, int issueId, CancellationToken cancellationToken);

        Task<Page<IssueModel>> GetIssues(int libraryId, int periodicalId, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<IssueModel> AddIssue(int libraryId, int periodicalId, IssueModel issue, CancellationToken cancellationToken);

        Task UpdateIssue(int libraryId, int periodicalId, IssueModel issue, CancellationToken cancellationToken);

        Task DeleteIssue(int libraryId, int issueId, CancellationToken cancellationToken);

        Task AddIssueContent(int libraryId, int periodicalId, int issueId, int fileId, string language, string mimeType, CancellationToken cancellationToken);

        Task<IssueContentModel> GetIssueContent(int libraryId, int periodicalId, int issueId, string language, string mimeType, CancellationToken cancellationToken);

        Task UpdateIssueContentUrl(int libraryId, int periodicalId, int issueId, string language, string mimeType, string url, CancellationToken cancellationToken);

        Task DeleteIssueContent(int libraryId, int periodicalId, int issueId, string language, string mimeType, CancellationToken cancellationToken);
        Task UpdateIssueContent(int libraryId, int periodicalId, int issueId, int articleId, string language, string mimeType, string actualUrl, CancellationToken cancellationToken);
    }
}
