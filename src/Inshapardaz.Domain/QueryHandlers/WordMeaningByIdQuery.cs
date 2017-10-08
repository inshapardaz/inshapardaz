﻿using Paramore.Darker;
using Inshapardaz.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Database;
using Inshapardaz.Domain.Database.Entities;

namespace Inshapardaz.Domain.QueryHandlers
{
    public class WordMeaningByIdQueryHandler : QueryHandlerAsync<WordMeaningByIdQuery, Meaning>
    {
        private readonly IDatabaseContext _database;

        public WordMeaningByIdQueryHandler(IDatabaseContext database)
        {
            _database = database;
        }

        public override async Task<Meaning> ExecuteAsync(WordMeaningByIdQuery query,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _database.Meaning.SingleOrDefaultAsync(t => t.Id == query.Id, cancellationToken);
        }
    }
}