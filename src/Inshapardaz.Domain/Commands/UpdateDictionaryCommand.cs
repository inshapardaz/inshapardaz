﻿using Inshapardaz.Domain.Database.Entities;

namespace Inshapardaz.Domain.Commands
{
    public class UpdateDictionaryCommand : Command
    {
        public UpdateDictionaryCommand(Dictionary dictionary)
        {
            Dictionary = dictionary;
        }

        public Dictionary Dictionary { get; }
    }
}
