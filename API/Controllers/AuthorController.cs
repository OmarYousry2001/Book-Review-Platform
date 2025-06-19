using Api.Controllers.Base;
using BL.Contracts.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTO.Entities;
using Shared.GeneralModels;

namespace Api.Controllers.Author
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthorController : BaseController
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService, Serilog.ILogger logger)
            : base(logger)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Retrieves all authors.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of all authors available in the system.
        /// </remarks>
        /// <returns>A list of authors wrapped in a standard response model.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _authorService.GetAll();
                if (entities == null || !entities.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<IEnumerable<AuthorDto>>
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
        /// Retrieves a specific author by ID.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the author details based on the provided author ID.
        /// </remarks>
        /// <returns>A single author wrapped in a standardized response model.</returns>
        [HttpGet("{id}")]
        [Authorize]
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

                var entity = _authorService.FindById(id);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<AuthorDto>
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
        /// Saves an author (create or update).
        /// </summary>
        /// <remarks>
        /// This endpoint allows creating a new author or updating an existing one based on the data provided.
        /// </remarks>
        /// <returns>Operation result with success message or error.</returns>
        [HttpPost("save")]
        public IActionResult Save([FromBody] AuthorDto AuthorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid Author data."
                    });

                var success = _authorService.Save(AuthorDto, GuidUserId);
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
        /// Deletes an author by ID.
        /// </summary>
        /// <remarks>
        /// This endpoint deletes an existing author by their unique identifier.
        /// </remarks>
        /// <returns>Status of the delete operation.</returns>
        [HttpDelete("Delete")]
        //[Authorize]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInput
                    });

                var success = _authorService.Delete(id , GuidUserId);
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
