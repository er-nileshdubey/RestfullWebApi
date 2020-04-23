﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestFullWebApi.DbContexts;

namespace RestFullWebApi.Migrations
{
    [DbContext(typeof(CourseLibraryContext))]
    [Migration("20200213131332_initialMigration")]
    partial class initialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RestFullWebApi.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateOfBirth")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MainCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                            DateOfBirth = new DateTimeOffset(new DateTime(1991, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)),
                            FirstName = "Nilesh",
                            LastName = "Dubey",
                            MainCategory = "Fiction"
                        },
                        new
                        {
                            Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247483"),
                            DateOfBirth = new DateTimeOffset(new DateTime(1989, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)),
                            FirstName = "Ratnesh",
                            LastName = "Dubey",
                            MainCategory = "Fiction"
                        },
                        new
                        {
                            Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247484"),
                            DateOfBirth = new DateTimeOffset(new DateTime(1990, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)),
                            FirstName = "Pradeep",
                            LastName = "Kanti",
                            MainCategory = "Fiction"
                        });
                });

            modelBuilder.Entity("RestFullWebApi.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1500)")
                        .HasMaxLength(1500);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Courses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247485"),
                            AuthorId = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                            Description = "Motivational Book",
                            Title = "The Monk who sold his ferary"
                        });
                });

            modelBuilder.Entity("RestFullWebApi.Entities.Course", b =>
                {
                    b.HasOne("RestFullWebApi.Entities.Author", "Author")
                        .WithMany("Courses")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
