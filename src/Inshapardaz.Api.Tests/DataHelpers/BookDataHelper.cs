﻿using Dapper;
using Inshapardaz.Api.Tests.Dto;
using Inshapardaz.Api.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Inshapardaz.Api.Tests.DataHelpers
{
    public static class BookDataHelper
    {
        public static void AddBook(this IDbConnection connection, BookDto book)
        {
            var sql = @"Insert Into Book (Title, Description, AuthorId, ImageId, IsPublic, IsPublished, Language, Status, SeriesId, SeriesIndex, Copyrights, YearPublished, DateAdded, DateUpdated, LibraryId)
                        Output Inserted.Id
                        Values (@Title, @Description, @AuthorId, @ImageId, @IsPublic, @IsPublished, @Language, @Status, @SeriesId, @SeriesIndex, @Copyrights, @YearPublished, @DateAdded, @DateUpdated, @LibraryId)";
            var id = connection.ExecuteScalar<int>(sql, book);
            book.Id = id;
        }

        public static void AddBooks(this IDbConnection connection, IEnumerable<BookDto> books)
        {
            foreach (var book in books)
            {
                AddBook(connection, book);
            }
        }

        public static void AddBookToFavorites(this IDbConnection connection, int libraryId, int bookId, int accountId)
        {
            var sql = @"Insert into FavoriteBooks (LibraryId, BookId, AccountId, DateAdded)
                        Values (@LibraryId, @BookId, @AccountId, GETDATE())";
            connection.ExecuteScalar<int>(sql, new { LibraryId = libraryId, bookId, accountId });
        }

        public static void AddBooksToFavorites(this IDbConnection connection, int libraryId, IEnumerable<int> bookIds, int accountId)
        {
            bookIds.ForEach(id => connection.AddBookToFavorites(libraryId, id, accountId));
        }

        public static void AddBooksToRecentReads(this IDbConnection connection, int libraryId, IEnumerable<int> bookIds, int accountId)
        {
            bookIds.ForEach(id => connection.AddBookToRecentReads(libraryId, id, accountId));
        }

        public static void AddBookToRecentReads(this IDbConnection connection, int libraryId, int bookId, int accountId, DateTime? timestamp = null)
        {
            var sql = @"Insert into RecentBooks (LibraryId, BookId, AccountId, DateRead)
                        Values (@LibraryId, @BookId, @AccountId, @DateRead)";
            connection.ExecuteScalar<int>(sql, new { LibraryId = libraryId, bookId, accountId, DateRead = timestamp ?? DateTime.Now });
        }

        public static void AddBookToRecentReads(this IDbConnection connection, RecentBookDto dto)
        {
            var sql = @"Insert into RecentBooks (LibraryId, BookId, AccountId, DateRead)
                        Values (@LibraryId, @BookId, @AccountId, @DateRead)";
            connection.ExecuteScalar<int>(sql, dto);
        }

        public static void AddBookFiles(this IDbConnection connection, int bookId, IEnumerable<BookContentDto> contentDto) =>
            contentDto.ForEach(f => connection.AddBookFile(bookId, f));

        public static void AddBookFile(this IDbConnection connection, int bookId, BookContentDto contentDto)
        {
            var sql = @"Insert Into BookContent (BookId, FileId, Language)
                        Output Inserted.Id
                        Values (@BookId, @FileId, @Language)";
            var id = connection.ExecuteScalar<int>(sql, new { BookId = bookId, FileId = contentDto.FileId, Language = contentDto.Language });
            contentDto.Id = id;
        }

        public static int GetBookCountByAuthor(this IDbConnection connection, int id)
        {
            return connection.ExecuteScalar<int>("Select Count(*) From Book Where AuthorId = @Id", new { Id = id });
        }

        public static BookDto GetBookById(this IDbConnection connection, int bookId)
        {
            return connection.QuerySingleOrDefault<BookDto>("Select * From Book Where Id = @Id", new { Id = bookId });
        }

        public static IEnumerable<BookDto> GetBooksByAuthor(this IDbConnection connection, int id)
        {
            return connection.Query<BookDto>("Select * From Book Where AuthorId = @Id", new { Id = id });
        }

        public static IEnumerable<BookDto> GetBooksByCategory(this IDbConnection connection, int categoryId)
        {
            return connection.Query<BookDto>(@"Select b.* From Book b
                Inner Join BookCategory bc ON b.Id = bc.BookId
                Where bc.CategoryId = @CategoryId", new { CategoryId = categoryId });
        }

        public static IEnumerable<BookDto> GetBooksBySeries(this IDbConnection connection, int seriesId)
        {
            return connection.Query<BookDto>(@"Select * From Book Where SeriesId = @SeriesId ", new { SeriesId = seriesId });
        }

        public static string GetBookImageUrl(this IDbConnection connection, int bookId)
        {
            var sql = @"Select f.FilePath from [File] f
                        Inner Join Book b ON f.Id = b.ImageId
                        Where b.Id = @Id";
            return connection.QuerySingleOrDefault<string>(sql, new { Id = bookId });
        }

        public static FileDto GetBookImage(this IDbConnection connection, int bookId)
        {
            var sql = @"Select f.* from [File] f
                        Inner Join Book b ON f.Id = b.ImageId
                        Where b.Id = @Id";
            return connection.QuerySingleOrDefault<FileDto>(sql, new { Id = bookId });
        }

        public static void DeleteBooks(this IDbConnection connection, IEnumerable<BookDto> books)
        {
            var sql = "Delete From Book Where Id IN @Ids";
            connection.Execute(sql, new { Ids = books.Select(f => f.Id) });
        }

        //TODO : Add user id.
        public static List<RecentBookDto> GetRecentBooks(this IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public static bool DoesBookExistsInFavorites(this IDbConnection connection, int bookId, int accountId) =>
            connection.QuerySingle<bool>(@"Select Count(1) From FavoriteBooks Where BookId = @BookId And AccountId = @AccountId", new
            {
                BookId = bookId,
                AccountId = accountId
            });

        //TODO : Add user id.
        public static bool DoesBookExistsInRecent(this IDbConnection connection, int bookId) =>
            connection.QuerySingle<bool>(@"Select Count(1) From RecentBooks Where BookId = @BookId", new
            {
                BookId = bookId
            });

        public static IEnumerable<BookContentDto> GetBookContents(this IDbConnection connection, int bookId)
        {
            string sql = @"Select bc.*, f.MimeType From BookContent bc
                           INNER Join Book b ON b.Id = bc.BookId
                           INNER Join [File] f ON f.Id = bc.FileId
                           Where b.Id = @BookId";

            return connection.Query<BookContentDto>(sql, new
            {
                BookId = bookId
            });
        }

        public static BookContentDto GetBookContent(this IDbConnection connection, int bookId, string language, string mimetype)
        {
            string sql = @"Select * From BookContent bc
                           INNER Join Book b ON b.Id = bc.BookId
                           INNER Join [File] f ON f.Id = bc.FileId
                           Where b.Id = @BookId AND bc.Language = @Language AND f.MimeType = @MimeType";

            return connection.QuerySingleOrDefault<BookContentDto>(sql, new
            {
                BookId = bookId,
                Language = language,
                MimeType = mimetype
            });
        }

        public static string GetBookContentPath(this IDbConnection connection, int bookId, string language, string mimetype)
        {
            string sql = @"Select f.FilePath From BookContent bc
                           INNER Join Book b ON b.Id = bc.BookId
                           INNER Join [File] f ON f.Id = bc.FileId
                           Where b.BookId = @BookId AND bc.Language = @Language AND f.MimeType = @MimeType";

            return connection.QuerySingleOrDefault<string>(sql, new
            {
                BookId = bookId,
                Language = language,
                MimeType = mimetype
            });
        }
    }
}
