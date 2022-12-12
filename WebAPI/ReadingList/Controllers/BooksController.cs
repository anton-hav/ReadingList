using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;
using Serilog;

namespace ReadingList.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;


        public BooksController(IMapper mapper, 
            IBookService bookService)
        {
            _mapper = mapper;
            _bookService = bookService;
        }

        /// <summary>
        /// Get a book from storage with specified id.
        /// </summary>
        /// <param name="id">a book unique identifier as a <see cref="Guid"/></param>
        /// <returns>A book with specified Id</returns>
        /// <response code="200">Returns a book corresponding to the specified identifier.</response>
        /// <response code="404">Failed to find record in the database that match the specified id.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetBookResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            try
            {
                var book = await _bookService.GetByIdAsync(id);
                var response = _mapper.Map<GetBookResponseModel>(book);
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
        /// Get books from storage.
        /// </summary>
        /// <returns>all books that match the search parameters</returns>
        /// <response code="200">Returns all books that match the search parameters.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetBookResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks([FromQuery] GetBooksRequestModel model)
        {
            try
            {
                if (model.PageSize == 0)
                    model.PageSize = 10;

                var books = await _bookService
                    .GetBooksBySearchParametersAsync(model.Title, 
                        model.AuthorId, 
                        model.CategoryId,
                        model.BookNoteId,
                        model.PageNumber, 
                        model.PageSize);
                var response = _mapper.Map<IEnumerable<GetBookResponseModel>>(books);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }

        }

        /// <summary>
        /// Add a new book to storage.
        /// </summary>
        /// <param name="model">a book</param>
        /// <returns>A newly created book</returns>
        /// <response code="201">Returns the newly created book</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">The same entry already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetBookResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBook([FromBody] AddOrUpdateBookRequestModel model)
        {
            try
            {
                if (model.Title.IsNullOrEmpty()
                    || model.AuthorId.Equals(default)
                    || model.CategoryId.Equals(default))
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isExist = await _bookService.IsBookExistAsync(model.Title, model.AuthorId);
                if (isExist)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var dto = _mapper.Map<BookDto>(model);
                dto.Id = Guid.NewGuid();
                var result = await _bookService.CreateAsync(dto);
                var response = _mapper.Map<GetBookResponseModel>(dto);

                return CreatedAtAction(nameof(GetBookById), new { id = response.Id }, response);
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
        /// Update or replace a book with specified Id in storage.
        /// </summary>
        /// <param name="id">a book unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a book used for update</param>
        /// <returns>A book with specified Id.</returns>
        /// <response code="200">Returns the updated book</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GetBookResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] AddOrUpdateBookRequestModel model)
        {
            try
            {

                if (model.Title.IsNullOrEmpty()
                    || model.AuthorId.Equals(default)
                    || model.CategoryId.Equals(default))
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isValid = await CheckBookForEditAsync(id, model.Title, model.AuthorId);

                var dto = _mapper.Map<BookDto>(model);
                dto.Id = id;
                var result = await _bookService.UpdateAsync(dto);
                var response = _mapper.Map<GetBookResponseModel>(dto);

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
        /// Patch a book with specified Id in storage.
        /// </summary>
        /// <param name="id">a book unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a book used for patching</param>
        /// <returns>A book with specified Id.</returns>
        /// <response code="200">Returns the updated book</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(GetBookResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBook(Guid id, [FromBody] AddOrUpdateBookRequestModel model)
        {
            try
            {
                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _bookService.IsBookExistAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));
                
                var isValid = await CheckBookForEditAsync(id, model.Title, model.AuthorId);
                var dto = _mapper.Map<BookDto>(model);
                dto.Id = id;
                var result = await _bookService.PatchAsync(id, dto);
                var response = _mapper.Map<GetBookResponseModel>(dto);

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
        /// Delete a book with specified Id from the storage.
        /// </summary>
        /// <param name="id">a book unique identifier as a <see cref="Guid"/></param>
        /// <returns></returns>
        /// <response code="204">Successful deletion</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(GetBookResponseModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            try
            {
                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _bookService.IsBookExistAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));

                var result = await _bookService.DeleteAsync(id);

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
        /// Validate a book model for update.
        /// </summary>
        /// <param name="id">a unique identifier that defines the book to be updated </param>
        /// <param name="bookTitle">a book title</param>
        /// <param name="bookAuthorId">unique identifier of the book's author</param>
        /// <returns>A boolean</returns>
        /// <exception cref="ArgumentNullException">If the id is empty.</exception>
        /// <exception cref="ArgumentException">If the same entry already exists in the storage.</exception>
        private async Task<bool> CheckBookForEditAsync(Guid id, string bookTitle, Guid bookAuthorId)
        {
            var isExist = await _bookService
                .IsBookExistAsync(bookTitle, bookAuthorId);

            if (isExist)
            {
                if (!id.Equals(default))
                {
                    var isBookTheSame = await IsBookTheSameAsync(id, bookTitle, bookAuthorId);

                    if (isBookTheSame)
                    {
                        return true;
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");
                }

                throw new ArgumentException("The same entry already exists in the storage.", nameof(bookTitle)); ;
            }
            return true;
        }

        /// <summary>
        /// Check if the existing book is the same.
        /// </summary>
        /// <remarks>
        /// This check is necessary to ensure idempotent behavior of the PUT method.
        /// </remarks>
        /// <param name="id">book unique identifier</param>
        /// <param name="bookTitle">book title</param>
        /// <param name="bookAuthorId">unique identifier of the book's author</param>
        /// <returns></returns>
        private async Task<bool> IsBookTheSameAsync(Guid id, string bookTitle, Guid bookAuthorId)
        {
            var dto = await _bookService.GetByIdAsync(id);
            return dto.Title.Equals(bookTitle)
                   && dto.AuthorId.Equals(bookAuthorId);
        }
    }
}
