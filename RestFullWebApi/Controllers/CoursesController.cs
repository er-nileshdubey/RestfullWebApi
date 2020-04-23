using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestFullWebApi.Models;
using RestFullWebApi.Services;
using System;
using System.Collections.Generic;

namespace RestFullWebApi.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CoursesDto>> GetCourses(Guid authorId)
        {
            var authors = _courseLibraryRepository.AuthorExist(authorId);
            if (!authors)
                return NotFound();

            var coursesFromRepo = _courseLibraryRepository.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CoursesDto>>(coursesFromRepo));
        }

        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        public ActionResult<CoursesDto> GetCourses(Guid authorId, Guid courseId)
        {
            var authors = _courseLibraryRepository.AuthorExist(authorId);
            if (!authors)
                return NotFound();

            var coursesFromRepo = _courseLibraryRepository.GetCourses(authorId, courseId);
            if (coursesFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<CoursesDto>(coursesFromRepo));
        }

        [HttpPost]
        public ActionResult<CoursesDto> CreateCourse(Guid authorId, CourseForCreationDto course)
        {
            if (!_courseLibraryRepository.AuthorExist(authorId))
                return NotFound();

            var courseEntities = _mapper.Map<Entities.Course>(course);
            _courseLibraryRepository.AddCourses(authorId, courseEntities);
            _courseLibraryRepository.Save();

            var courseToReturn = _mapper.Map<CoursesDto>(courseEntities);

            return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);
        }

        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdationDto course)
        {
            if (!_courseLibraryRepository.AuthorExist(authorId))
                return NotFound();

            var courseFromRepo = _courseLibraryRepository.GetCourses(authorId, courseId);
            if (courseFromRepo == null)
                return NotFound();

            if (course == null)
            {
                //Upserting Using Put request
                var courseEntities = _mapper.Map<Entities.Course>(course);
                courseEntities.Id = courseId;
                _courseLibraryRepository.AddCourses(authorId, courseEntities);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CoursesDto>(courseEntities);

                return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }

            _mapper.Map(course, courseFromRepo);

            _courseLibraryRepository.UpdateCourse(authorId, courseFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId,
            JsonPatchDocument<CourseForUpdationDto> patchDocument)
        {
            if (!_courseLibraryRepository.AuthorExist(authorId))
                return NotFound();

            var courseFromRepo = _courseLibraryRepository.GetCourses(authorId, courseId);
            if (courseFromRepo == null)
            {
                //Upserting Using Patch request
                var courseDto = new CourseForUpdationDto();
                patchDocument.ApplyTo(courseDto, ModelState);

                if (!TryValidateModel(courseDto))
                    return BadRequest();

                var courseToAdd = _mapper.Map<Entities.Course>(courseDto);
                courseToAdd.Id = courseId;
                _courseLibraryRepository.AddCourses(authorId, courseToAdd);
                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CoursesDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }

            var courseToPatch = _mapper.Map<CourseForUpdationDto>(courseFromRepo);
            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!TryValidateModel(courseToPatch))
                return BadRequest();

            _mapper.Map(courseToPatch, courseFromRepo);

            _courseLibraryRepository.UpdateCourse(authorId, courseFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();

        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

    }
}