﻿using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Commands;
using Inshapardaz.Domain.Database;
using Inshapardaz.Domain.Exception;
using Microsoft.EntityFrameworkCore;
using paramore.brighter.commandprocessor;

namespace Inshapardaz.Domain.CommandHandlers
{
    public class UpdateWordTranslationCommandHandler : RequestHandlerAsync<UpdateWordTranslationCommand>
    {
        private readonly IDatabaseContext _database;

        public UpdateWordTranslationCommandHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<UpdateWordTranslationCommand> HandleAsync(UpdateWordTranslationCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var translation = await _database.Translation.SingleOrDefaultAsync(t => t.Id == command.Translation.Id, cancellationToken);

            if (translation == null)
            {
                throw new RecordNotFoundException();
            }

            translation.Language = command.Translation.Language;
            translation.Value = command.Translation.Value;

            await _database.SaveChangesAsync(cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
