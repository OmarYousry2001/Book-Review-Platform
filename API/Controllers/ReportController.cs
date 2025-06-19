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
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService, Serilog.ILogger logger)
            : base(logger)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Retrieves all Reports.
        /// </summary>
        /// <remarks>
        /// Returns a list of all available reports.
        /// </remarks>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _reportService.GetAll();
                if (entities == null || !entities.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<IEnumerable<ReportDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NotFound,
                    Data = entities
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a Report by ID.
        /// </summary>
        /// <remarks>
        /// Requires Authorization header with Bearer token.
        /// </remarks>
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid Category ID."
                    });

                var entity = _reportService.FindById(id);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Category not found."
                    });

                return Ok(new ResponseModel<ReportDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully.",
                    Data = entity
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Adds a report for a specific book.
        /// </summary>
        /// <remarks>
        /// Requires Authorization header with Bearer token.
        /// </remarks>
        /// <returns>Result of the report submission.</returns>
        [HttpPost("AddReport")]
        public IActionResult Save([FromBody] ReportDto dto)
        {
            try
            {
                if (dto == null || dto.BookId == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.InvalidData
                    });

                dto.UserId = UserId;

                var isSaved = _reportService.Save(dto, GuidUserId);

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
        /// Deletes a Report by its ID.
        /// </summary>
        /// <remarks>
        /// Requires authorization if applied on controller level.
        /// The report ID must be passed as a query parameter.
        /// </remarks>
        /// <returns>Returns success status or error message.</returns>
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

                var success = _reportService.Delete(id, GuidUserId);
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
