﻿using Paramore.Darker;

namespace Inshapardaz.Domain.Ports.Handlers.Library
{
    public abstract class LibraryBaseQuery<T> : IQuery<T>
    {
        public LibraryBaseQuery(int libraryId)
        {
            LibraryId = libraryId;
        }

        public int LibraryId { get; private set; }
    }
}
