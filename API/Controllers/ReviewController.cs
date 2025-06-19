using Api.Controllers.Base;
using BL.Contracts.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.Constants;
using Shared.DTO.Entities;
using Shared.GeneralModels;

namespace Api.Controllers.Review
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService, Serilog.ILogger logger)
            : base(logger)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <remarks>
        /// Returns a list of all available reviews.  
        /// If no reviews are found, returns 404.
        /// </remarks>
        /// <returns>List of reviews.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _reviewService.GetAll();
                if (entities == null || !entities.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<IEnumerable<ReviewDto>>
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
        /// Retrieves a review by its ID.
        /// </summary>
        /// <remarks>
        /// Provide a valid review ID as a route parameter.  
        /// Returns 404 if the review does not exist, or 400 if the ID is invalid.
        /// </remarks>
        /// <returns>A single review if found.</returns>
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

                var entity = _reviewService.FindById(id);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<ReviewDto>
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
        /// Adds a review and rating to a book.
        /// </summary>
        /// <remarks>
        /// Requires authentication.  
        /// Returns a success message if saved correctly.
        /// </remarks>
        [HttpPost("AddReview")]
        public async Task<IActionResult> Save([FromBody] ReviewDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Comment) || !Enum.IsDefined(typeof(EnRating), dto.Rating))
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.InvalidData
                    });
                }

                dto.UserId = UserId;

                var isSaved = _reviewService.Save(dto, GuidUserId);
                if (!isSaved)
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });
                }

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
        /// Deletes a review by its ID.
        /// </summary>
        /// <remarks>
        /// Returns success message if deletion is successful.
        /// </remarks>
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

                var success = _reviewService.Delete(id, GuidUserId);
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
