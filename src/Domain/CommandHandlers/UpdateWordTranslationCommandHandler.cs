﻿using Darker;
using Inshapardaz.Domain.Commands;
using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Queries;
using paramore.brighter.commandprocessor;

namespace Inshapardaz.Domain.CommandHandlers
{
    public class UpdateWordTranslationCommandHandler : RequestHandler<UpdateWordTranslationCommand>
    {
        private readonly IDatabaseContext _database;
        private readonly IQueryProcessor _queryProcessor;

        public UpdateWordTranslationCommandHandler(
            IDatabaseContext database, 
            IQueryProcessor queryProcessor)
        {
            _database = database;
            _queryProcessor = queryProcessor;
        }

        public override UpdateWordTranslationCommand Handle(UpdateWordTranslationCommand command)
        {
            var translation = _queryProcessor.Execute(new TranslationByIdQuery {Id = command.Translation.Id});

            if (translation == null)
            {
                throw new RecordNotFoundException();
            }

            translation.Language = command.Translation.Language;
            translation.Value = command.Translation.Value;

            _database.SaveChanges();

            return base.Handle(command);
        }
    }
}
