using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.DataBase.Entities;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;
using Serilog;

namespace ReadingList.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;

        public AuthorsController(IMapper mapper,
            IAuthorService authorService)
        {
            _mapper = mapper;
            _authorService = authorService;
        }


        /// <summary>
        /// Get a author from storage with specified id.
        /// </summary>
        /// <param name="id">a author unique identifier as a <see cref="Guid"/></param>
        /// <returns>A author with specified Id</returns>
        /// <response code="200">Returns a author corresponding to the specified identifier.</response>
        /// <response code="404">Failed to find record in the database that match the specified id.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetAuthorResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            try
            {
                var author = await _authorService.GetByIdAsync(id);
                var response = _mapper.Map<GetAuthorResponseModel>(author);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return NotFound(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }
        }

        /// <summary>
        /// Get authors from storage.
        /// </summary>
        /// <returns>all authors</returns>
        /// <response code="200">Returns all authors.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetAuthorResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                var authors = await _authorService.GetAuthorsAsync();
                var response = _mapper.Map<List<GetAuthorResponseModel>>(authors);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }

        }

        /// <summary>
        /// Add a new author to storage.
        /// </summary>
        /// <param name="model">a author</param>
        /// <returns>A newly created author</returns>
        /// <response code="201">Returns the newly created author</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">The same entry already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetAuthorResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAuthor([FromBody] AddOrUpdateAuthorRequestModel model)
        {
            try
            {
                if (model.FullName.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isExist = await _authorService.IsAuthorExistByNameAsync(model.FullName);
                if (isExist)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var dto = _mapper.Map<AuthorDto>(model);
                dto.Id = Guid.NewGuid();
                var result = await _authorService.CreateAsync(dto);
                var response = _mapper.Map<GetAuthorResponseModel>(dto);

                return CreatedAtAction(nameof(GetAuthorById), new { id = response.Id }, response);
            }
            catch (ArgumentNullException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return Conflict(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }
        }

        /// <summary>
        /// Update or replace a author with specified Id in storage.
        /// </summary>
        /// <param name="id">a author unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a author used for update</param>
        /// <returns>A author with specified Id.</returns>
        /// <response code="200">Returns the updated author</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GetAuthorResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AddOrUpdateAuthorRequestModel model)
        {
            try
            {

                if (model.FullName.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isValid = await CheckAuthorForEditAsync(id, model.FullName);

                var dto = _mapper.Map<AuthorDto>(model);
                dto.Id = id;
                var result = await _authorService.UpdateAsync(dto);
                var response = _mapper.Map<GetAuthorResponseModel>(dto);

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return NotFound(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Patch a author with specified Id in storage.
        /// </summary>
        /// <param name="id">a author unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a author used for patching</param>
        /// <returns>A author with specified Id.</returns>
        /// <response code="200">Returns the updated author</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(GetAuthorResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAuthor(Guid id, [FromBody] AddOrUpdateAuthorRequestModel model)
        {
            try
            {
                if (model.FullName.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _authorService.IsAuthorExistByIdAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));

                var isExistByFullName = await _authorService.IsAuthorExistByNameAsync(model.FullName);
                if (isExistByFullName)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var dto = _mapper.Map<AuthorDto>(model);
                dto.Id = id;
                var result = await _authorService.PatchAsync(id, dto);
                var response = _mapper.Map<GetAuthorResponseModel>(dto);

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return NotFound(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete a author with specified Id from the storage.
        /// </summary>
        /// <param name="id">a author unique identifier as a <see cref="Guid"/></param>
        /// <returns></returns>
        /// <response code="204">Successful deletion</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(GetAuthorResponseModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _authorService.IsAuthorExistByIdAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));

                var result = await _authorService.DeleteAsync(id);

                return NoContent();

            }
            catch (ArgumentNullException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return NotFound(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        // todo: implement validation used FluentValidation
        /// <summary>
        /// Validate a author model for update.
        /// </summary>
        /// <param name="id">a unique identifier that defines the author to be updated </param>
        /// <param name="authorFullName">a author name</param>
        /// <returns>A boolean</returns>
        /// <exception cref="ArgumentNullException">If the id is empty.</exception>
        /// <exception cref="ArgumentException">If the same entry already exists in the storage.</exception>
        private async Task<bool> CheckAuthorForEditAsync(Guid id, string authorFullName)
        {
            var isExist = await _authorService
                .IsAuthorExistByNameAsync(authorFullName);

            if (isExist)
            {
                if (!id.Equals(default))
                {
                    var isAuthorTheSame = await IsAuthorTheSameAsync(id, authorFullName);

                    if (isAuthorTheSame)
                    {
                        return true;
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");
                }

                throw new ArgumentException("The same entry already exists in the storage.", nameof(authorFullName)); ;
            }
            return true;
        }

        /// <summary>
        /// Check if the existing author is the same.
        /// </summary>
        /// <remarks>
        /// This check is necessary to ensure idempotent behavior of the PUT method.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="authorFullName"></param>
        /// <returns></returns>
        private async Task<bool> IsAuthorTheSameAsync(Guid id, string authorFullName)
        {
            var dto = await _authorService.GetByIdAsync(id);
            return dto.FullName.Equals(authorFullName);
        }

    }
}
