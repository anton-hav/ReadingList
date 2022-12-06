using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReadingList.Core;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;
using Serilog;

namespace ReadingList.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookNotesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookNoteService _bookNoteService;


        public BookNotesController(IMapper mapper, 
            IBookNoteService bookNoteService)
        {
            _mapper = mapper;
            _bookNoteService = bookNoteService;
        }

        /// <summary>
        /// Get a book note from storage with specified id.
        /// </summary>
        /// <param name="id">a book note unique identifier as a <see cref="Guid"/></param>
        /// <returns>A book note with specified Id</returns>
        /// <response code="200">Returns a book note corresponding to the specified identifier.</response>
        /// <response code="404">Failed to find record in the database that match the specified id.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetBookNoteResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookNoteById(Guid id)
        {
            try
            {
                var bookNote = await _bookNoteService.GetByIdAsync(id);
                var response = _mapper.Map<GetBookNoteResponseModel>(bookNote);
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
        /// Get book notes from storage.
        /// </summary>
        /// <returns>all the book notes that match the search parameters</returns>
        /// <response code="200">Returns all book notes that match the search parameters.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetBookNoteResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookNotes([FromQuery] GetBookNotesRequestModel model)
        {
            try
            {
                if (model.PageSize == 0)
                    model.PageSize = 10;

                var bookNotes = await _bookNoteService
                    .GetBookNotesBySearchParametersAsync(model.BookId, 
                        model.Priority, 
                        model.Status, 
                        model.PageNumber, 
                        model.PageSize);
                var response = _mapper.Map<IEnumerable<GetBookNoteResponseModel>>(bookNotes);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }

        }

        /// <summary>
        /// Add a new book note to storage.
        /// </summary>
        /// <param name="model">a bookNote</param>
        /// <returns>A newly created bookNote</returns>
        /// <response code="201">Returns the newly created bookNote</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">The same entry already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetBookNoteResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBookNote([FromBody] AddOrUpdateBookNoteRequestModel model)
        {
            try
            {
                if (Guid.Empty.Equals(model.BookId)
                    || !Enum.IsDefined(typeof(ReadingPriority), model.Priority)
                    || !Enum.IsDefined(typeof(ReadingStatus), model.Status))
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isExist = await _bookNoteService.IsBookNoteExistByBookIdAsync(model.BookId);
                if (isExist)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var dto = _mapper.Map<BookNoteDto>(model);
                dto.Id = Guid.NewGuid();
                var result = await _bookNoteService.CreateAsync(dto);
                var response = _mapper.Map<GetBookNoteResponseModel>(dto);

                return CreatedAtAction(nameof(GetBookNoteById), new { id = response.Id }, response);
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
        /// Update or replace a book note with specified Id in storage.
        /// </summary>
        /// <param name="id">a book note unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a book note used for update</param>
        /// <returns>A book note with specified Id.</returns>
        /// <response code="200">Returns the updated book note</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GetBookNoteResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBookNote(Guid id, [FromBody] AddOrUpdateBookNoteRequestModel model)
        {
            try
            {

                if (Guid.Empty.Equals(model.BookId)
                    || !Enum.IsDefined(typeof(ReadingPriority), model.Priority)
                    || !Enum.IsDefined(typeof(ReadingStatus), model.Status))
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isValid = await CheckBookNoteForEditAsync(id, model.BookId);

                var dto = _mapper.Map<BookNoteDto>(model);
                dto.Id = id;
                var result = await _bookNoteService.UpdateAsync(dto);
                var response = _mapper.Map<GetBookNoteResponseModel>(dto);

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
        /// Patch a bookNote with specified Id in storage.
        /// </summary>
        /// <param name="id">a bookNote unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a bookNote used for patching</param>
        /// <returns>A bookNote with specified Id.</returns>
        /// <response code="200">Returns the updated bookNote</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(GetBookNoteResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBookNote(Guid id, [FromBody] AddOrUpdateBookNoteRequestModel model)
        {
            try
            {
                //if (model.Title.IsNullOrEmpty())
                //    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _bookNoteService.IsBookNoteExistAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));

                //var isExistByFullName = await _bookNoteService.IsBookNoteExistAsync(model.Title, model.AuthorId);
                //if (isExistByFullName)
                //    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var isValid = await CheckBookNoteForEditAsync(id, model.BookId);

                var dto = _mapper.Map<BookNoteDto>(model);
                dto.Id = id;
                var result = await _bookNoteService.PatchAsync(id, dto);
                var response = _mapper.Map<GetBookNoteResponseModel>(dto);

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
        /// Delete a book note with specified Id from the storage.
        /// </summary>
        /// <param name="id">a book note unique identifier as a <see cref="Guid"/></param>
        /// <returns></returns>
        /// <response code="204">Successful deletion</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(GetBookNoteResponseModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookNote(Guid id)
        {
            try
            {
                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _bookNoteService.IsBookNoteExistAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));

                var result = await _bookNoteService.DeleteAsync(id);

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
        /// Validate a book note model for update.
        /// </summary>
        /// <param name="id">a unique identifier that defines the bookNote to be updated </param>
        /// <param name="bookId">book unique identifier</param>
        /// <returns>A boolean</returns>
        /// <exception cref="ArgumentNullException">If the id is empty.</exception>
        /// <exception cref="ArgumentException">If the same entry already exists in the storage.</exception>
        private async Task<bool> CheckBookNoteForEditAsync(Guid id, Guid bookId)
        {
            var isExist = await _bookNoteService
                .IsBookNoteExistByBookIdAsync(bookId);

            if (isExist)
            {
                if (!id.Equals(default))
                {
                    var isBookNoteTheSame = await IsBookNoteTheSameAsync(id, bookId);

                    if (isBookNoteTheSame)
                    {
                        return true;
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");
                }

                throw new ArgumentException("The same entry already exists in the storage.", nameof(bookId)); ;
            }
            return true;
        }

        /// <summary>
        /// Check if the existing book note is the same.
        /// </summary>
        /// <remarks>
        /// This check is necessary to ensure idempotent behavior of the PUT method.
        /// </remarks>
        /// <param name="id">book note unique identifier</param>
        /// <param name="bookId">book unique identifier</param>
        /// <returns>A boolean</returns>
        private async Task<bool> IsBookNoteTheSameAsync(Guid id, Guid bookId)
        {
            var dto = await _bookNoteService.GetByIdAsync(id);
            return dto.BookId.Equals(bookId);
        }
    }
}
