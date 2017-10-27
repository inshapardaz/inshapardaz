using System;
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

namespace Inshapardaz.Api.Ports
{
    public class GetTranslationForWordDetailLanguageRequest : IRequest
    {
        public Guid Id { get; set; }

        public List<TranslationView> Result { get; set; }

        public int DictionaryId { get; set; }

        public long WordDetailId { get; set; }

        public Languages Language { get; set; }
    }

    public class GetTranslationForWordDetailLanguageRequestHandler : RequestHandlerAsync<GetTranslationForWordDetailLanguageRequest>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserHelper _userHelper;
        private readonly IRenderTranslation _translationRenderer;

        public GetTranslationForWordDetailLanguageRequestHandler(IQueryProcessor queryProcessor, IUserHelper userHelper, IRenderTranslation translationRenderer)
        {
            _queryProcessor = queryProcessor;
            _userHelper = userHelper;
            _translationRenderer = translationRenderer;
        }

        public override async Task<GetTranslationForWordDetailLanguageRequest> HandleAsync(GetTranslationForWordDetailLanguageRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = _userHelper.GetUserId();
            if (user != Guid.Empty)
            {
                var dictionary = await _queryProcessor.ExecuteAsync(new DictionaryByWordIdQuery
                {
                    WordId = command.WordDetailId
                }, cancellationToken);
                if (dictionary != null && dictionary.UserId != user)
                {
                    throw new UnauthorizedAccessException();
                }
            }

            var translations = await _queryProcessor.ExecuteAsync(new TranslationsByWordDetailAndLanguageQuery
            {
                WordDetailId = command.WordDetailId,
                Language = command.Language
            }, cancellationToken);

            command.Result = translations.Select(t => _translationRenderer.Render(t)).ToList();
            return command;
        }
    }
}