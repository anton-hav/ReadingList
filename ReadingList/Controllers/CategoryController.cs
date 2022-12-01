using System.Runtime.CompilerServices;
using System.Xml;
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
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        
        public CategoryController(ICategoryService categoryService, 
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a category from storage with specified id.
        /// </summary>
        /// <param name="id">a category unique identifier as a <see cref="Guid"/></param>
        /// <returns>A category with specified Id</returns>
        /// <response code="200">Returns a category corresponding to the specified identifier.</response>
        /// <response code="404">Failed to find record in the database that match the specified id.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(category);
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
        /// Add a new category to storage.
        /// </summary>
        /// <param name="model">a category</param>
        /// <returns>A newly created category</returns>
        /// <response code="201">Returns the newly created category</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">The same entry already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCategory([FromBody] AddOrUpdateCategoryRequestModel model)
        {
            try
            {
                if (model.Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isExist = await _categoryService.IsCategoryExistByNameAsync(model.Name);
                if (isExist)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var dto = _mapper.Map<CategoryDto>(model);
                dto.Id = Guid.NewGuid();
                var result = await _categoryService.CreateAsync(dto);

                return CreatedAtAction(nameof(GetCategoryById), new { id = dto.Id }, dto);
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
        /// Update or replace a category with specified Id in storage.
        /// </summary>
        /// <param name="id">a category unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a category used for update</param>
        /// <returns>A category with specified Id.</returns>
        /// <response code="200">Returns the updated category</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] AddOrUpdateCategoryRequestModel model)
        {
            try
            {

                if (model.Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                var isValid = await CheckCategoryForEditAsync(id, model.Name);

                var dto = _mapper.Map<CategoryDto>(model);
                dto.Id = id;
                var result = await _categoryService.UpdateAsync(dto);

                return Ok(dto);
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
        /// Patch a category with specified Id in storage.
        /// </summary>
        /// <param name="id">a category unique identifier as a <see cref="Guid"/></param>
        /// <param name="model">a category used for patching</param>
        /// <returns>A category with specified Id.</returns>
        /// <response code="200">Returns the updated category</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCategory(Guid id, [FromBody] AddOrUpdateCategoryRequestModel model)
        {
            try
            {
                if (model.Name.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(model), "One or more object properties are null.");

                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _categoryService.IsCategoryExistByIdAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));

                var isExistByName = await _categoryService.IsCategoryExistByNameAsync(model.Name);
                if (isExistByName)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var dto = _mapper.Map<CategoryDto>(model);
                dto.Id = id;
                var result = await _categoryService.PatchAsync(id, dto);

                return Ok(dto);
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
        /// Delete a category with specified Id from the storage.
        /// </summary>
        /// <param name="id">a category unique identifier as a <see cref="Guid"/></param>
        /// <returns></returns>
        /// <response code="204">Successful deletion</response>
        /// <response code="400">Request contains null object or invalid object type</response>
        /// <response code="409">Fail to find a record with the specified Id in the storage
        /// or the entry with the same property already exists in the storage.</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                if (id.Equals(default))
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");

                var isExistById = await _categoryService.IsCategoryExistByIdAsync(id);
                if (!isExistById)
                    throw new ArgumentException("Fail to find a record with the specified Id in the storage",
                        nameof(id));
                
                var result = await _categoryService.DeleteAsync(id);

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
        /// Validate a category model for update.
        /// </summary>
        /// <param name="id">a unique identifier that defines the category to be updated </param>
        /// <param name="categoryName">a category name</param>
        /// <returns>A boolean</returns>
        /// <exception cref="ArgumentNullException">If the id is empty.</exception>
        /// <exception cref="ArgumentException">If the same entry already exists in the storage.</exception>
        private async Task<bool> CheckCategoryForEditAsync(Guid id, string categoryName)
        {
            var isExist = await _categoryService
                .IsCategoryExistByNameAsync(categoryName);

            if (isExist)
            {
                if (!id.Equals(default))
                {
                    var isCategoryTheSame = await IsCategoryTheSameAsync(id, categoryName);

                    if (isCategoryTheSame)
                    {
                        return true;
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "A non-empty Id is required.");
                }

                throw new ArgumentException("The same entry already exists in the storage.", nameof(categoryName)); ;
            }
            return true;
        }

        /// <summary>
        /// Check if the existing category is the same.
        /// </summary>
        /// <remarks>
        /// This check is necessary to ensure idempotent behavior of the PUT method.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private async Task<bool> IsCategoryTheSameAsync(Guid id, string categoryName)
        {
            var dto = await _categoryService.GetByIdAsync(id);
            return dto.Name.Equals(categoryName);
        }
    }
}
