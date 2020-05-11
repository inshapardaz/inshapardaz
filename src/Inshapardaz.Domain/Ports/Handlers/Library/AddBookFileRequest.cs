﻿using Inshapardaz.Domain.Ports.Handlers.Library;
using Inshapardaz.Domain.Repositories;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FileModel = Inshapardaz.Domain.Models.FileModel;

namespace Inshapardaz.Domain.Ports.Library
{
    public class AddBookFileRequest : LibraryAuthorisedCommand
    {
        public AddBookFileRequest(ClaimsPrincipal claims, int libraryId, int bookId)
            : base(claims, libraryId)
        {
            BookId = bookId;
        }

        public int BookId { get; }

        public FileModel Content { get; set; }

        public FileModel Result { get; set; }
    }

    public class AddBookFileRequestHandler : RequestHandlerAsync<AddBookFileRequest>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IFileStorage _fileStorage;

        public AddBookFileRequestHandler(IBookRepository bookRepository, IFileRepository fileRepository, IFileStorage fileStorage)
        {
            _bookRepository = bookRepository;
            _fileRepository = fileRepository;
            _fileStorage = fileStorage;
        }

        [Authorise(step: 1, HandlerTiming.Before)]
        public override async Task<AddBookFileRequest> HandleAsync(AddBookFileRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var book = await _bookRepository.GetBookById(command.LibraryId, command.BookId, command.UserId, cancellationToken);

            if (book != null)
            {
                var url = await StoreFile(book.Id, command.Content.FileName, command.Content.Contents, cancellationToken);
                command.Content.FilePath = url;
                command.Content.IsPublic = true;
                var file = await _fileRepository.AddFile(command.Content, cancellationToken);
                await _bookRepository.AddBookFile(book.Id, file.Id, cancellationToken);

                command.Result = file;
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<string> StoreFile(int bookId, string fileName, byte[] contents, CancellationToken cancellationToken)
        {
            var filePath = GetUniqueFileName(bookId, fileName);
            return await _fileStorage.StoreFile(filePath, contents, cancellationToken);
        }

        private static string GetUniqueFileName(int bookId, string fileName)
        {
            var fileNameWithoutExtension = Path.GetExtension(fileName).Trim('.');
            return $"books/{bookId}/{Guid.NewGuid():N}.{fileNameWithoutExtension}";
        }
    }
}