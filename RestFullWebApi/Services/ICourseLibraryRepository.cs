using RestFullWebApi.Entities;
using RestFullWebApi.Helper;
using RestFullWebApi.ReferanceParameter;
using System;
using System.Collections.Generic;

namespace RestFullWebApi.Services
{
    public interface ICourseLibraryRepository
    {
        IEnumerable<Author> GetAuthors();

        void CreateAuthor(Author authors);

        PagedList<Author> GetAuthors(AuthorsParam authorsParam);

        IEnumerable<Author> GetAuthors(IEnumerable<Guid> ids);

        Author GetAuthors(Guid authorID);

        IEnumerable<Course> GetCourses(Guid authorId);

        Course GetCourses(Guid authorId, Guid courseId);

        bool AuthorExist(Guid authorId);

        void AddCourses(Guid authorId, Course courseId);

        void UpdateCourse(Guid authorId, Course course);

        void DeleteCourse(Course course);

        bool Save();
    }
}
