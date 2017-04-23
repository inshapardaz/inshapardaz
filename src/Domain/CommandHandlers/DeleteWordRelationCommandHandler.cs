﻿using Darker;
using Inshapardaz.Domain.Commands;
using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Queries;
using paramore.brighter.commandprocessor;

namespace Inshapardaz.Domain.CommandHandlers
{
    public class DeleteWordRelationCommandHandler : RequestHandler<DeleteWordRelationCommand>
    {
        private readonly IDatabaseContext _database;
        private readonly IQueryProcessor _queryProcessor;

        public DeleteWordRelationCommandHandler(
            IDatabaseContext database,
            IQueryProcessor queryProcessor)
        {
            _database = database;
            _queryProcessor = queryProcessor;
        }

        public override DeleteWordRelationCommand Handle(DeleteWordRelationCommand command)
        {
            var relation = _queryProcessor.Execute(new RelationshipByIdQuery { Id = command.RelationId });

            if (relation == null)
            {
                throw new RecordNotFoundException();
            }

            _database.WordRelations.Remove(relation);
            _database.SaveChanges();

            return base.Handle(command);
        }
    }
}
