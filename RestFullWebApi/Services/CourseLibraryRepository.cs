using RestFullWebApi.DbContexts;
using RestFullWebApi.Entities;
using RestFullWebApi.Helper;
using RestFullWebApi.Models;
using RestFullWebApi.ReferanceParameter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestFullWebApi.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        private readonly CourseLibraryContext _courseLibraryContext;
        private readonly IPropertyMappingService _mappingService;

        public CourseLibraryRepository(CourseLibraryContext courseLibraryContext, IPropertyMappingService mappingService)
        {
            _courseLibraryContext = courseLibraryContext ?? throw new ArgumentNullException(nameof(courseLibraryContext));

            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        public void AddCourses(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
                throw new ArgumentNullException(nameof(authorId));

            if (course == null)
                throw new ArgumentNullException(nameof(course));

            course.AuthorId = authorId;
            course.Id = Guid.NewGuid();
            _courseLibraryContext.Courses.Add(course);
        }

        public bool AuthorExist(Guid authorId)
        {
            return _courseLibraryContext.Authors.Any(x => x.Id == authorId);
        }

        public void CreateAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
            }
            _courseLibraryContext.Authors.Add(author);
        }

        public void DeleteCourse(Course course)
        {
            _courseLibraryContext.Courses.Remove(course);
        }

        public Author GetAuthors(Guid authorID)
        {
            return _courseLibraryContext.Authors.FirstOrDefault(x => x.Id == authorID);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _courseLibraryContext.Authors.OrderBy(x => x.FirstName).ToList();
        }

        public PagedList<Author> GetAuthors(AuthorsParam authorsParam)
        {
            if (authorsParam == null)
                throw new ArgumentNullException(nameof(authorsParam));

            var authorsCollection = _courseLibraryContext.Authors as IQueryable<Author>;

            if (!string.IsNullOrWhiteSpace(authorsParam.MainCategory))
            {
                var mainCategory = authorsParam.MainCategory.Trim();
                authorsCollection = authorsCollection.Where(x => x.MainCategory.Contains(mainCategory));
            }

            if (!string.IsNullOrEmpty(authorsParam.SearchQuery))
            {
                var searchQuery = authorsParam.SearchQuery.Trim();
                authorsCollection = authorsCollection.Where(x => x.FirstName.Contains(searchQuery)
                || x.LastName.Contains(searchQuery) || x.MainCategory.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(authorsParam.OrderBy))
            {

                var authorPropertyMappingDictionary = _mappingService.GetPropertyMapping<AuthorsDto, Author>();

                authorsCollection = authorsCollection.ApplySort(authorsParam.OrderBy, authorPropertyMappingDictionary);
            }

            return PagedList<Author>.Create(authorsCollection, authorsParam.PageNumber, authorsParam.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> ids)
        {
            return _courseLibraryContext.Authors.Where(x => ids.Contains(x.Id)).ToList();
        }

        public IEnumerable<Course> GetCourses(Guid authorId)
        {
            return _courseLibraryContext.Courses.Where(x => x.AuthorId == authorId).ToList();
        }

        public Course GetCourses(Guid authorId, Guid courseId)
        {
            return _courseLibraryContext.Courses.FirstOrDefault(x => x.AuthorId == authorId && x.Id == courseId);
        }

        public bool Save()
        {
            return (_courseLibraryContext.SaveChanges() >= 0);
        }

        public void UpdateCourse(Guid authorId, Course course)
        {

        }
    }
}
