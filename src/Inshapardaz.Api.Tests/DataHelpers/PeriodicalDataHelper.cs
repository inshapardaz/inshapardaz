﻿using Dapper;
using Inshapardaz.Api.Tests.Dto;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Inshapardaz.Api.Tests.DataHelpers
{
    public static class PeriodicalDataHelper
    {
        public static void AddPeriodical(this IDbConnection connection, PeriodicalDto periodical)
        {
            var id = connection.ExecuteScalar<int>("Insert Into Periodical (Title, [Description], CategoryId, ImageId, LibraryId) OUTPUT Inserted.Id VALUES (@Name, @Description, @CategoryId, @ImageId, @LibraryId)", periodical);
            periodical.Id = id;
        }

        public static void AddPeriodicals(this IDbConnection connection, IEnumerable<PeriodicalDto> Periodicals)
        {
            foreach (var periodical in Periodicals)
            {
                AddPeriodical(connection, periodical);
            }
        }

        public static void DeletePeridical(this IDbConnection connection, IEnumerable<PeriodicalDto> serieses)
        {
            var sql = "Delete From Periodicals Where Id IN @Ids";
            connection.Execute(sql, new { Ids = serieses.Select(a => a.Id) });
        }

        public static PeriodicalDto GetPeriodicalById(this IDbConnection connection, int id)
        {
            return connection.QuerySingleOrDefault<PeriodicalDto>("Select * From Periodical Where Id = @Id", new { Id = id });
        }

        public static string GetPeriodicalImageUrl(this IDbConnection connection, int id)
        {
            var sql = @"Select f.FilePath from [File] f
                        Inner Join Periodical p ON f.Id = p.ImageId
                        Where p.Id = @Id";
            return connection.QuerySingleOrDefault<string>(sql, new { Id = id });
        }

        public static FileDto GetPeriodicalImage(this IDbConnection connection, int id)
        {
            var sql = @"Select f.* from [File] f
                        Inner Join Periodical p ON f.Id = p.ImageId
                        Where p.Id = @Id";
            return connection.QuerySingleOrDefault<FileDto>(sql, new { Id = id });
        }
    }
}
