using Microsoft.EntityFrameworkCore;
using RestFullWebApi.Entities;
using System;

namespace RestFullWebApi.DbContexts
{
    public class CourseLibraryContext : DbContext
    {
        public CourseLibraryContext(DbContextOptions<CourseLibraryContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author()
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                    FirstName = "Nilesh",
                    LastName = "Dubey",
                    DateOfBirth = new DateTime(1991, 06, 01),
                    MainCategory = "Fiction"
                },
                 new Author()
                 {
                     Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247483"),
                     FirstName = "Ratnesh",
                     LastName = "Dubey",
                     DateOfBirth = new DateTime(1989, 06, 01),
                     MainCategory = "Fiction"
                 },
                new Author()
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247484"),
                    FirstName = "Pradeep",
                    LastName = "Kanti",
                    DateOfBirth = new DateTime(1990, 06, 01),
                    MainCategory = "Fiction"
                });

            modelBuilder.Entity<Course>().HasData(
                new Course()
                {
                    Id = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247485"),
                    Title = "The Monk who sold his ferary",
                    Description = "Motivational Book",
                    AuthorId = Guid.Parse("9245fe4a-d402-451c-b9ed-9c1a04247482")
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
