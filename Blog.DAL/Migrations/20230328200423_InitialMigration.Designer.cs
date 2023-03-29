﻿// <auto-generated />
using System;
using Blog.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blog.DAL.Migrations
{
    [DbContext(typeof(BlogDataContext))]
    [Migration("20230328200423_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("Blog.DAL.Models.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RubricId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RubricId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Blog.DAL.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ArticleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CommentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Blog.DAL.Models.Rubric", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ParentID")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentID")
                        .IsUnique();

                    b.ToTable("Rubrics");
                });

            modelBuilder.Entity("Blog.DAL.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Blog.DAL.Models.Article", b =>
                {
                    b.HasOne("Blog.DAL.Models.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blog.DAL.Models.Rubric", "Rubric")
                        .WithMany("Articles")
                        .HasForeignKey("RubricId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Author");

                    b.Navigation("Rubric");
                });

            modelBuilder.Entity("Blog.DAL.Models.Comment", b =>
                {
                    b.HasOne("Blog.DAL.Models.Article", null)
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId");

                    b.HasOne("Blog.DAL.Models.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blog.DAL.Models.Comment", null)
                        .WithMany("Comments")
                        .HasForeignKey("CommentId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Blog.DAL.Models.Rubric", b =>
                {
                    b.HasOne("Blog.DAL.Models.Rubric", "Parent")
                        .WithOne()
                        .HasForeignKey("Blog.DAL.Models.Rubric", "ParentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Blog.DAL.Models.Article", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Blog.DAL.Models.Comment", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Blog.DAL.Models.Rubric", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("Blog.DAL.Models.User", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
