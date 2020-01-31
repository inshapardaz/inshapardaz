﻿using System;
using Inshapardaz.Domain.Repositories;
using Inshapardaz.Functions.Dictionaries;
using Inshapardaz.Functions.Library.Authors;
using Inshapardaz.Functions.Library.Books;
using Inshapardaz.Functions.Library.Books.Chapters;
using Inshapardaz.Functions.Library.Books.Chapters.Contents;
using Inshapardaz.Functions.Library.Books.Files;
using Inshapardaz.Functions.Library.Categories;
using Inshapardaz.Functions.Library.Series;
using Inshapardaz.Functions.Tests.DataBuilders;
using Inshapardaz.Functions.Tests.Fakes;
using Inshapardaz.Functions.Tests.Helpers;
using Inshapardaz.Ports.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inshapardaz.Functions.Tests
{
    public abstract class FunctionTest
    {
        private readonly TestHostBuilder _builder;
        private readonly Startup _startup;
        private SqliteConnection _connection;

        protected FunctionTest()
        {
            _builder = new TestHostBuilder();
            _startup = new Startup();

            DatabaseContext = CreateDbContext();

            _builder.Services.AddSingleton<IDatabaseContext>(sp => DatabaseContext);
            _builder.Services.AddTransient<CategoriesDataBuilder>()
                             .AddTransient<SeriesDataBuilder>()
                             .AddTransient<AuthorsDataBuilder>()
                             .AddTransient<BooksDataBuilder>()
                             .AddTransient<ChapterDataBuilder>()
                             .AddTransient<DictionaryDataBuilder>()
                             .AddSingleton<IFileStorage>(new FakeFileStorage())
                             .AddTransient<DictionaryDataHelper>();

            RegisterHandlers(_builder);
            _startup.Configure(_builder);
        }

        protected IServiceProvider Container => _builder.ServiceProvider;

        protected IDatabaseContext DatabaseContext { get; private set; }

        private IDatabaseContext CreateDbContext()
        {
            if (_connection != null)
            {
                throw new Exception("connection already created");
            }

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                          .UseSqlite(_connection)
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors()
                          .Options;

            var context = new DatabaseContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        protected void Cleanup()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        private void RegisterHandlers(TestHostBuilder builder)
        {
            builder.Services.AddTransient<GetEntry>()
                   .AddTransient<GetLanguages>()
                   .AddTransient<GetCategories>()
                   .AddTransient<AddCategory>()
                   .AddTransient<GetCategoryById>()
                   .AddTransient<UpdateCategory>()
                   .AddTransient<DeleteCategory>()
                   .AddTransient<GetSeries>()
                   .AddTransient<AddSeries>()
                   .AddTransient<GetSeriesById>()
                   .AddTransient<UpdateSeries>()
                   .AddTransient<DeleteSeries>()
                   .AddTransient<GetAuthors>()
                   .AddTransient<GetAuthorById>()
                   .AddTransient<AddAuthor>()
                   .AddTransient<UpdateAuthor>()
                   .AddTransient<DeleteAuthor>()
                   .AddTransient<UpdateAuthorImage>()
                   .AddTransient<GetBooks>()
                   .AddTransient<GetBookById>()
                   .AddTransient<GetBooksByAuthor>()
                   .AddTransient<GetBooksByCategory>()
                   .AddTransient<GetBooksBySeries>()
                   .AddTransient<GetFavoriteBooks>()
                   .AddTransient<GetRecentReadBooks>()
                   .AddTransient<GetLatestBooks>()
                   .AddTransient<AddBook>()
                   .AddTransient<UpdateBook>()
                   .AddTransient<DeleteBook>()
                   .AddTransient<UpdateBookImage>()
                   .AddTransient<AddBookFile>()
                   .AddTransient<UpdateBookFile>()
                   .AddTransient<DeleteBookFile>()
                   .AddTransient<GetBookFiles>()
                   .AddTransient<GetChapterById>()
                   .AddTransient<AddChapter>()
                   .AddTransient<UpdateChapter>()
                   .AddTransient<DeleteChapter>()
                   .AddTransient<GetChaptersByBook>()
                   .AddTransient<GetChapterContents>()
                   .AddTransient<AddChapterContents>()
                   .AddTransient<UpdateChapterContents>()
                   .AddTransient<DeleteChapterContents>()
                   .AddTransient<GetDictionaries>()
                   .AddTransient<GetDictionaryById>()
                   .AddTransient<AddDictionary>();
        }
    }
}
