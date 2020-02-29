﻿namespace Inshapardaz.Domain
{
    public class Settings
    {
        public bool RunDBMigrations { get; set; }

        public int DefaultDictionaryId { get; set; }

        public string DatabaseConnectionString { get; set; }

        public string Audience { get; set; }

        public string Authority { get; set; }

        public string FileStorageConnectionString { get; set; }
    }
}
