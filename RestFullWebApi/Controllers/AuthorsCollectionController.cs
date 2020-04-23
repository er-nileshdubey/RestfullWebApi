using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestFullWebApi.Helper;
using RestFullWebApi.Models;
using RestFullWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestFullWebApi.Controllers
{
    [Route("api/authorsCollection")]
    [ApiController]
    public class AuthorsCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public AuthorsCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();

            var authorEntities = _courseLibraryRepository.GetAuthors(ids);

            if (ids.Count() != authorEntities.Count())
                return NotFound();

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorsDto>>(authorEntities);

            return Ok(authorsToReturn);
        }

        [HttpPost]
        public IActionResult CreateAuthors(IEnumerable<AuthorForCreationDto> authors)
        {
            if (authors == null)
                return BadRequest();

            var authorsToCreate = _mapper.Map<IEnumerable<Entities.Author>>(authors);

            foreach (var author in authorsToCreate)
            {
                _courseLibraryRepository.CreateAuthor(author);
            }
            _courseLibraryRepository.Save();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorsDto>>(authorsToCreate);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(x => x.Id));

            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);
        }
    }
}