﻿using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Ports.Library
{
    public class DeleteBookToFavoriteRequest : BookRequest
    {
        public DeleteBookToFavoriteRequest(ClaimsPrincipal claims, int libraryId, int bookId, Guid userId)
            : base(claims, libraryId, bookId, userId)
        {
        }
    }

    public class DeleteBookToFavoriteRequestHandler : RequestHandlerAsync<DeleteBookToFavoriteRequest>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookToFavoriteRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [Authorise(step: 1, HandlerTiming.Before)]
        public override async Task<DeleteBookToFavoriteRequest> HandleAsync(DeleteBookToFavoriteRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            await _bookRepository.AddBookToFavorites(command.LibraryId, command.UserId, command.BookId, cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}