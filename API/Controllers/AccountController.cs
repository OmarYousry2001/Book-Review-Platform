using Api.Controllers.Base;
using BL.Contracts.GeneralService.CMS;
using BL.Contracts.GeneralService.UserManagement;
using BL.Contracts.Services.Items;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User;
using Shared.GeneralModels;

namespace Api.Controllers.Author
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserTokenService _userTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountController(IUserAuthenticationService authorService, IUserRegistrationService userRegistrationService, IUserTokenService userTokenService, IRefreshTokenService refreshTokenService, Serilog.ILogger logger)
            : base(logger)
        {
            _userAuthenticationService = authorService;
            _userRegistrationService = userRegistrationService;
            _userTokenService = userTokenService;
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Registers a new user with the specified role.
        /// </summary>
        /// <param name="user">User registration information.</param>
        /// <param name="role">Role assigned to the new user ( "Admin", "Reader" ,"Writer).</param>
        /// <returns>Registration result.</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user, string role)
        {
            try
            {
                var entities = await _userRegistrationService.RegisterUserAsync(user, role);
                if (!entities.Success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NotFound,
                        Errors = entities.Errors
                    });

                return Ok(new ResponseModel<UserRegistrationDto>
                {
                    Success = entities.Success,
                    Message = entities.Message,
                }

                );
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Authenticates a user and returns access and refresh tokens.
        /// </summary>
        /// <param name="user">Login credentials (email and password).</param>
        /// <returns>JWT access token and refresh token.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            try
            {
                var result = await _userAuthenticationService.LoginUserAsync(user);
                if (!result.Success)
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }
                var existUSer = await _userAuthenticationService.GetUserByEmailAsync(user.Email);

                var refreshToken = _userTokenService.GenerateRefreshTokenAsync(existUSer.Id.ToString());
                _refreshTokenService.Refresh(refreshToken);

                Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true if using HTTPS
                    SameSite = SameSiteMode.Strict, // Adjust as needed
                    Expires = DateTimeOffset.UtcNow.AddDays(7) // Set expiration as needed
                });
                return Ok(new { RefreshToken = refreshToken, accsresToken = result.Data });

            }

            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <returns>New access token.</returns
        [HttpPost("RefreshAccessToken")]
        public async Task<IActionResult> RefreshAccessToken()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.RefreshTokenNotFound
                    });
                }

                var storedToken = _refreshTokenService.GetByToken(refreshToken);
                if (storedToken == null || storedToken.CurrentState == 0 || storedToken.ExpiresAt < DateTime.Now)
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.InvalidOrExpiredRefreshToken
                    });
                }

                var roles = await _userAuthenticationService.GetRolesByIdlAsync(storedToken.UserId);
                var newAsscessToken = await _userTokenService.GenerateJwtTokenAsync(storedToken.UserId, roles);

                return Ok(new { raccsresToken = newAsscessToken });

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Refreshes the refresh token and issues a new one.
        /// </summary>
        /// <returns>New refresh token stored in HTTP-only cookie.</returns>
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.RefreshTokenNotFound
                    });
                }

                var storedToken = _refreshTokenService.GetByToken(refreshToken);
                if (storedToken == null || storedToken.CurrentState == 0 || storedToken.ExpiresAt < DateTime.Now)
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = ValidationResources.InvalidOrExpiredRefreshToken
                    });
                }

                // Generate  new refreshToken
                var newRefreshToken = _userTokenService.GenerateRefreshTokenAsync(UserId);

                // refresh  refreshToken   
                _refreshTokenService.Refresh(newRefreshToken);
                Response.Cookies.Append("refreshToken", newRefreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true if using HTTPS
                    Expires = DateTimeOffset.UtcNow.AddDays(7) // Set expiration as needed
                });

                return Ok(new { RefreshToke = newRefreshToken });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
