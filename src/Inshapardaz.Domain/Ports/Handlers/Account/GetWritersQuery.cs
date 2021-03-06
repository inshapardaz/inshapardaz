﻿using Inshapardaz.Domain.Models;
using Inshapardaz.Domain.Models.Handlers.Library;
using Inshapardaz.Domain.Repositories;
using Paramore.Darker;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Domain.Ports.Handlers.Account
{
    public class GetWritersQuery : LibraryBaseQuery<IEnumerable<AccountModel>>
    {
        public GetWritersQuery(int libraryId)
            : base(libraryId)
        {
        }
    }

    public class GetWritersQueryHandler : QueryHandlerAsync<GetWritersQuery, IEnumerable<AccountModel>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetWritersQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public override async Task<IEnumerable<AccountModel>> ExecuteAsync(GetWritersQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _accountRepository.GetWriters(query.LibraryId, cancellationToken);
        }
    }
}
