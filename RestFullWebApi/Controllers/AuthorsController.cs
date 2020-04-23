using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestFullWebApi.Entities;
using RestFullWebApi.Helper;
using RestFullWebApi.Models;
using RestFullWebApi.ReferanceParameter;
using RestFullWebApi.Services;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RestFullWebApi.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _mappingService;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper, IPropertyMappingService mappingService)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _mappingService = mappingService ??
                throw new ArgumentNullException(nameof(mappingService));
        }

        [HttpGet(Name ="GetAuthors")]
        [HttpHead]
        public IActionResult GetAuthors([FromQuery] AuthorsParam authorsParam)
        {
            if (!_mappingService.ValidMappingExistFor<AuthorsDto, Author>(authorsParam.OrderBy))
                return BadRequest();

            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsParam);
            if (authorsFromRepo == null)
                return NotFound();

            var previousPageLink = authorsFromRepo.HasPreviousPage ?
                CreateAuthorsRecourceUri(authorsParam, ResourceUriType.PreviousPage) : null;

            var nextPageLink = authorsFromRepo.HasNextPage ?
                CreateAuthorsRecourceUri(authorsParam, ResourceUriType.NextPage) : null;
            var paginationMataDate = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPage = authorsFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(paginationMataDate));

            return Ok(_mapper.Map<IEnumerable<AuthorsDto>>(authorsFromRepo));
        }

        [HttpGet("{Id}")]
        [HttpHead("{Id}", Name = "GetAuthor")]
        public IActionResult GetAuthors(Guid id)
        {
            var author = _courseLibraryRepository.GetAuthors(id);
            if (author == null)
                return NotFound();

            return Ok(_mapper.Map<AuthorsDto>(author));
        }

        [HttpPost]
        public ActionResult<AuthorsDto> CreateAuthor(AuthorForCreationDto authorForCreationDto)
        {
            var authorEntity = _mapper.Map<Entities.Author>(authorForCreationDto);
            _courseLibraryRepository.CreateAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorsDto>(authorEntity);

            return CreatedAtRoute("GetAuthors", new { authorId = authorToReturn.Id }, authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "Get, Options, Post, Head");
            return Ok();
        }

        private string CreateAuthorsRecourceUri(AuthorsParam authorsParam, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors", new
                    {
                        orderBy = authorsParam.OrderBy,
                        pageNumber = authorsParam.PageNumber - 1,
                        pageSize = authorsParam.PageSize,
                        mainCategory = authorsParam.MainCategory,
                        searchQuery = authorsParam.SearchQuery
                    });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors", new
                    {
                        orderBy = authorsParam.OrderBy,
                        pageNumber = authorsParam.PageNumber + 1,
                        pageSize = authorsParam.PageSize,
                        mainCategory = authorsParam.MainCategory,
                        searchQuery = authorsParam.SearchQuery
                    });

                default:
                    return Url.Link("GetAuthors", new
                    {
                        orderBy = authorsParam.OrderBy,
                        pageNumber = authorsParam.PageNumber,
                        pageSize = authorsParam.PageSize,
                        mainCategory = authorsParam.MainCategory,
                        searchQuery = authorsParam.SearchQuery
                    });
            }
        }
    }
}