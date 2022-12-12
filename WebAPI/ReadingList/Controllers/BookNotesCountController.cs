using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadingList.Core.Abstractions;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;
using Serilog;

namespace ReadingList.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookNotesCountController : ControllerBase
    {
        private readonly IBookNoteService _bookNoteService;

        public BookNotesCountController(IBookNoteService bookNoteService)
        {
            _bookNoteService = bookNoteService;
        }

        /// <summary>
        /// Get book notes count from storage
        /// </summary>
        /// <param name="model">search parameters as a <see cref="GetBookNotesCountRequestModel"/></param>
        /// <returns>number of book notes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookNotesCount([FromQuery] GetBookNotesCountRequestModel model)
        {
            try
            {
                var response = await _bookNoteService
                    .GetBookNotesCountBySearchParametersAsync(model.AuthorId,
                        model.CategoryId,
                        model.Priority,
                        model.Status);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }
        }
    }
}
