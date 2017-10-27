﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Api.Helpers;
using Inshapardaz.Domain.Commands;
using Inshapardaz.Domain.Exception;
using Inshapardaz.Domain.Queries;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;
using Paramore.Darker;

namespace Inshapardaz.Api.Ports
{
    public class DeleteDictionaryRequest : IRequest
    {
        public Guid Id { get; set; }

        public int DictionaryId { get; set; }
    }

    public class DeleteDictionaryRequestHandler : RequestHandlerAsync<DeleteDictionaryRequest>
    {
        private readonly IUserHelper _userHelper;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly ILogger<DeleteDictionaryRequestHandler> _logger;

        public DeleteDictionaryRequestHandler(IAmACommandProcessor commandProcessor, IQueryProcessor queryProcessor, IUserHelper userHelper, ILogger<DeleteDictionaryRequestHandler> logger)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
            _userHelper = userHelper;
            _logger = logger;
        }

        public override async Task<DeleteDictionaryRequest> HandleAsync(DeleteDictionaryRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = _userHelper.GetUserId();
            var existingDictionary = await _queryProcessor.ExecuteAsync(new DictionaryByIdQuery { UserId = userId, DictionaryId = command.DictionaryId }, cancellationToken);

            if (existingDictionary == null)
            {
                _logger.LogDebug("Dictionary with id '{0}' not found. Nothing deleted", command.DictionaryId);
                throw new NotFoundException();
            }

            _logger.LogDebug("Deleting dictionary with id '{0}'", command.DictionaryId);

            await _commandProcessor.SendAsync(new DeleteDictionaryCommand
            {
                UserId = userId,
                DictionaryId = command.DictionaryId
            }, cancellationToken: cancellationToken);

            return command;
        }
    }
}