﻿using Inshapardaz.Domain.Models;
using Inshapardaz.Domain.Models.Library;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Adapters.Repositories.Library
{
    public interface IBookPageRepository
    {
        Task<BookPageModel> GetPageBySequenceNumber(int libraryId, int bookId, int sequenceNumber, CancellationToken cancellationToken);

        Task<int> GetLastPageNumberForBook(int libraryId, int bookId, CancellationToken cancellationToken);

        Task<BookPageModel> AddPage(int libraryId, int bookId, int sequenceNumber, string text, int imageId, CancellationToken cancellationToken);

        Task<BookPageModel> UpdatePage(int libraryId, int bookId, int sequenceNumber, string text, int imageId, PageStatuses status, int? accountId, CancellationToken cancellationToken);

        Task DeletePage(int libraryId, int bookId, int sequenceNumber, CancellationToken cancellationToken);

        Task<BookPageModel> UpdatePageImage(int libraryId, int bookId, int sequenceNumber, int imageId, CancellationToken cancellationToken);

        Task DeletePageImage(int libraryId, int bookId, int sequenceNumber, CancellationToken cancellationToken);

        Task<Page<BookPageModel>> GetPagesByBook(int libraryId, int bookId, int pageNumber, int pageSize, PageStatuses status, CancellationToken cancellationToken);

        Task<BookPageModel> UpdatePageAssignment(int libraryId, int bookId, int sequenceNumber, PageStatuses status, int? assignedAccountId, CancellationToken cancellationToken);
    }
}
