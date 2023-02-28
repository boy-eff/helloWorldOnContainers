﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Words.DataAccess;

#nullable disable

namespace Words.DataAccess.Migrations
{
    [DbContext(typeof(WordsDbContext))]
    partial class WordsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Words.DataAccess.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("EnglishLevel")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Words.DataAccess.Models.UserWord", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WordId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "WordId");

                    b.HasIndex("WordId");

                    b.ToTable("UserWords");
                });

            modelBuilder.Entity("Words.DataAccess.Models.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WordCollectionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WordCollectionId");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("DailyViews")
                        .HasColumnType("int");

                    b.Property<int>("EnglishLevel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalViews")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordCollectionRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CollectionId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("UserId");

                    b.ToTable("WordCollectionRatings");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Translation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("WordTranslations");
                });

            modelBuilder.Entity("Words.DataAccess.Models.UserWord", b =>
                {
                    b.HasOne("Words.DataAccess.Models.User", "User")
                        .WithMany("DictionaryWords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Words.DataAccess.Models.Word", "Word")
                        .WithMany("UserDictionaries")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Words.DataAccess.Models.Word", b =>
                {
                    b.HasOne("Words.DataAccess.Models.WordCollection", "WordCollection")
                        .WithMany("Words")
                        .HasForeignKey("WordCollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordCollection");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordCollection", b =>
                {
                    b.HasOne("Words.DataAccess.Models.User", "User")
                        .WithMany("Collections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordCollectionRating", b =>
                {
                    b.HasOne("Words.DataAccess.Models.WordCollection", "Collection")
                        .WithMany("Ratings")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Words.DataAccess.Models.User", "User")
                        .WithMany("CollectionRatings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Collection");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordTranslation", b =>
                {
                    b.HasOne("Words.DataAccess.Models.Word", "Word")
                        .WithMany("Translations")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("Words.DataAccess.Models.User", b =>
                {
                    b.Navigation("CollectionRatings");

                    b.Navigation("Collections");

                    b.Navigation("DictionaryWords");
                });

            modelBuilder.Entity("Words.DataAccess.Models.Word", b =>
                {
                    b.Navigation("Translations");

                    b.Navigation("UserDictionaries");
                });

            modelBuilder.Entity("Words.DataAccess.Models.WordCollection", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Words");
                });
#pragma warning restore 612, 618
        }
    }
}
