﻿using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Entities.Library;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports.Library
{
    public class GetChapterByIdRequest : BookRequest
    {
        public GetChapterByIdRequest(int bookId, int chapterId)
            : base(bookId)
        {
            ChapterId = chapterId;
        }

        public Chapter Result { get; set; }
        public int ChapterId { get; }
    }

    public class GetChapterByIdRequestHandler : RequestHandlerAsync<GetChapterByIdRequest>
    {
        private readonly IChapterRepository _chapterRepository;

        public GetChapterByIdRequestHandler(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        [BookRequestValidation(1, HandlerTiming.Before)]
        public override async Task<GetChapterByIdRequest> HandleAsync(GetChapterByIdRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var chapter = await _chapterRepository.GetChapterById(command.ChapterId, cancellationToken);
            chapter.HasContents = await _chapterRepository.HasChapterContents(command.BookId, command.ChapterId, cancellationToken);
            command.Result = chapter;
            
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
