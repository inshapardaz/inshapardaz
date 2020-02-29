﻿using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Models.Library;
using Inshapardaz.Domain.Ports.Handlers.Library;
using Inshapardaz.Domain.Repositories.Library;
using Paramore.Brighter;

namespace Inshapardaz.Domain.Ports.Library
{
    public class AddSeriesRequest : LibraryBaseCommand
    {
        public AddSeriesRequest(int libraryId, SeriesModel series)
            : base(libraryId)
        {
            Series = series;
        }

        public SeriesModel Series { get; }
        public SeriesModel Result { get; set; }
    }

    public class AddSeriesRequestHandler : RequestHandlerAsync<AddSeriesRequest>
    {
        private readonly ISeriesRepository _seriesRepository;

        public AddSeriesRequestHandler(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        public override async Task<AddSeriesRequest> HandleAsync(AddSeriesRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            command.Result = await _seriesRepository.AddSeries(command.LibraryId, command.Series, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
