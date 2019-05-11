﻿// <auto-generated />
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;
using Inshapardaz.Ports.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Inshapardaz.Ports.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190511125812_AddedChapterContents")]
    partial class AddedChapterContents
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Inshapardaz")
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsPublic");

                    b.Property<int>("Language");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<Guid>("UserId")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Dictionary","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.DictionaryDownload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DictionaryId");

                    b.Property<int>("FileId");

                    b.Property<string>("MimeType");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.HasIndex("FileId");

                    b.ToTable("DictionaryDownload","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Meaning", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Context");

                    b.Property<string>("Example");

                    b.Property<string>("Value");

                    b.Property<long>("WordId");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("Meaning","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Translation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsTrasnpiling");

                    b.Property<int>("Language");

                    b.Property<string>("Value");

                    b.Property<long>("WordId");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("Translation","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Word", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Attributes");

                    b.Property<string>("Description");

                    b.Property<int>("DictionaryId");

                    b.Property<int>("Language");

                    b.Property<string>("Pronunciation");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("TitleWithMovements");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.HasIndex("Title");

                    b.ToTable("Word","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.WordRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("RelatedWordId");

                    b.Property<int>("RelationType");

                    b.Property<long>("SourceWordId");

                    b.HasKey("Id");

                    b.HasIndex("RelatedWordId");

                    b.HasIndex("SourceWordId");

                    b.ToTable("WordRelation","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("MimeType");

                    b.HasKey("Id");

                    b.ToTable("File","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<int>("IssueId");

                    b.Property<int>("SequenceNumber");

                    b.Property<int?>("SeriesIndex");

                    b.Property<string>("SeriesName");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("IssueId");

                    b.ToTable("Article","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ArticleText", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArticleId");

                    b.Property<string>("Content");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId")
                        .IsUnique();

                    b.ToTable("ArticleText","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Author","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<int>("Copyrights");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<int?>("ImageId");

                    b.Property<bool>("IsPublic");

                    b.Property<int>("Language");

                    b.Property<int?>("SeriesId");

                    b.Property<int?>("SeriesIndex");

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("YearPublished");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SeriesId");

                    b.ToTable("Book","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookCategory", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("CategoryId");

                    b.HasKey("BookId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategory","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookPage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<byte[]>("Contents");

                    b.Property<int?>("PageNumber");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("BookPage","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Category","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<int>("ChapterNumber");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Chapter","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ChapterContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChapterId");

                    b.Property<string>("ContentUrl");

                    b.Property<string>("MimeType");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("ChapterContent","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ChapterText", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChapterId");

                    b.Property<string>("Content");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId")
                        .IsUnique();

                    b.ToTable("ChapterText","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.FavoriteBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("FavoriteBooks","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ImageId");

                    b.Property<DateTime>("IssueDate");

                    b.Property<int>("IssueNumber");

                    b.Property<int>("MagazineId");

                    b.Property<int>("VolumeNumber");

                    b.HasKey("Id");

                    b.HasIndex("MagazineId");

                    b.ToTable("Issue","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Magazine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Category");

                    b.Property<string>("Description");

                    b.Property<int?>("ImageId");

                    b.Property<int?>("MagazineCategoryId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MagazineCategoryId");

                    b.ToTable("Magazine","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.MagazineCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("MagazineCategory","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.RecentBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<DateTime>("DateRead");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("RecentBooks","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Series","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.SeriesCategory", b =>
                {
                    b.Property<int>("SeriesId");

                    b.Property<int>("CategoryId");

                    b.HasKey("SeriesId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("SeriesCategory","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.DictionaryDownload", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Dictionary", "Dictionary")
                        .WithMany("Downloads")
                        .HasForeignKey("DictionaryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Meaning", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "Word")
                        .WithMany("Meaning")
                        .HasForeignKey("WordId")
                        .HasConstraintName("FK_Meaning_Word")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Translation", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "Word")
                        .WithMany("Translation")
                        .HasForeignKey("WordId")
                        .HasConstraintName("FK_Translation_Word")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Word", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Dictionary", "Dictionary")
                        .WithMany("Word")
                        .HasForeignKey("DictionaryId")
                        .HasConstraintName("FK_Word_Dictionary")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.WordRelation", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "RelatedWord")
                        .WithMany("WordRelationRelatedWord")
                        .HasForeignKey("RelatedWordId")
                        .HasConstraintName("FK_WordRelation_RelatedWord")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "SourceWord")
                        .WithMany("WordRelationSourceWord")
                        .HasForeignKey("SourceWordId")
                        .HasConstraintName("FK_WordRelation_SourceWord")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Article", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Author", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Issue", "Issue")
                        .WithMany("Articles")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ArticleText", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Article", "Article")
                        .WithOne("Content")
                        .HasForeignKey("Inshapardaz.Ports.Database.Entities.Library.ArticleText", "ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Book", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Series", "Series")
                        .WithMany("Books")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookCategory", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany("BookCategory")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Category", "Category")
                        .WithMany("BookCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookPage", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany("Pages")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Chapter", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany("Chapters")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ChapterContent", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Chapter", "Chapter")
                        .WithMany("Contents")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ChapterText", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Chapter", "Chapter")
                        .WithOne("Content")
                        .HasForeignKey("Inshapardaz.Ports.Database.Entities.Library.ChapterText", "ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.FavoriteBook", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Issue", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Magazine", "Magazine")
                        .WithMany("Issues")
                        .HasForeignKey("MagazineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Magazine", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.MagazineCategory", "MagazineCategory")
                        .WithMany("Magazines")
                        .HasForeignKey("MagazineCategoryId");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.RecentBook", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.SeriesCategory", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Category", "Category")
                        .WithMany("SeriesCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Series", "Series")
                        .WithMany("SeriesCategory")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
