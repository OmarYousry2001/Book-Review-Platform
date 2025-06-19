using Api.Controllers.Base;
using BL.Contracts.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.Constants;
using Shared.DTO.Entities;
using Shared.GeneralModels;

namespace Api.Controllers.Settings
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : BaseController
    {
        private readonly ISettingsService _settingsService;
        public SettingsController(ISettingsService settingsService, Serilog.ILogger logger)
            : base(logger)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Retrieves all settings.
        /// </summary>
        /// <remarks>
        /// Returns a list of all settings available in the system.  
        /// Requires authentication.
        /// </remarks>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var entities = _settingsService.GetAll();
                if (entities == null || !entities.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message =  NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<IEnumerable<SettingDto>>
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
        /// Retrieves a specific setting by ID.
        /// </summary>
        /// <param name="id">The ID of the setting to retrieve.</param>
        /// <remarks>
        /// Requires authentication.  
        /// Returns 404 if the setting is not found, or 400 if the ID is invalid.
        /// </remarks>
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

                var entity = _settingsService.FindById(id);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<SettingDto>
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
        /// Saves a new setting or updates an existing one.
        /// </summary>
        /// <remarks>
        /// This endpoint allows admin users to create or update a setting.
        /// The setting data must be provided in the request body.
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = SD.Roles.Admin)]
        public IActionResult Save([FromBody] SettingDto SettingDto)
        {
            try
            {
              
                var success = _settingsService.Save(SettingDto, GuidUserId);
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
        /// Deletes a specific setting by ID.
        /// </summary>
        /// <remarks>
        /// Requires admin role.  
        /// The setting ID must be passed as a query parameter.
        /// </remarks>
        [HttpDelete("Delete")]
        [Authorize(Roles = SD.Roles.Admin)]
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

                var success = _settingsService.Delete(id, GuidUserId);
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
