using Api.Controllers.Base;
using BL.Contracts.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTO.Entities;
using Shared.GeneralModels;

namespace Api.Controllers.Category
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, Serilog.ILogger logger)
            : base(logger)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <remarks>
        /// Returns a list of all available categories.
        /// </remarks>
        /// <returns>A list of categories wrapped in a response model.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _categoryService.GetAll();
                if (entities == null || !entities.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<IEnumerable<CategoryDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.Successful,
                    Data = entities
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <remarks>
        /// Provide a valid category ID to retrieve its details.
        /// </remarks>
        /// <returns>Category details wrapped in a response model.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInput
                    });

                var entity = _categoryService.FindById(id);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<CategoryDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.Successful,
                    Data = entity
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Saves a category (create or update).
        /// </summary>
        /// <remarks>
        /// Provide category data to create a new category or update an existing one.
        /// </remarks>
        /// <returns>Result indicating success or failure.</returns>
        [HttpPost("save")]
        public IActionResult Save([FromBody] CategoryDto CategoryDto)
        {
            try
            {
                var success = _categoryService.Save(CategoryDto, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <remarks>
        /// Provide a valid category ID as a query parameter to delete the category.
        /// </remarks>
        /// <returns>Result indicating success or failure of the delete operation.</returns>
        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInput
                    });

                var success = _categoryService.Delete(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.DeleteFailed
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DeletedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
