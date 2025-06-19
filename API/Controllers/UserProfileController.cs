using Api.Controllers.Base;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Services.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTO.Entities;
using Shared.DTO.Views;
using Shared.GeneralModels;
using System.Threading.Tasks;

namespace Api.Controllers.Author
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : BaseController
    {
        private readonly IUserProfileService _userProfileController;
        public UserProfileController(IUserProfileService userProfileController, Serilog.ILogger logger)
            : base(logger)
        {
            _userProfileController = userProfileController;
        }

        /// <summary>
        /// Retrieves the currently authenticated author's profile.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the profile data for the currently logged-in user based on the token.
        /// </remarks>
        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserId))
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });


                var entity = await _userProfileController.GetUserProfileAsync(UserId);
                if (entity == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound
                    });

                return Ok(new ResponseModel<UserProfileViewDto>
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
        /// Updates the profile information of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows an authenticated user to update their profile details.  
        /// The profile data should be sent in the request body.
        /// </remarks>
        [HttpPut("save")]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] UserProfileViewDto userProfile)
        {
            try
            {
                if (userProfile==null)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                var success =await _userProfileController.UpdateUserProfileAsync(UserId, userProfile);
                if (!success.Success)
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

    }
}
