using Microsoft.AspNetCore.Mvc;
using Shared.GeneralModels;
using System.Security.Claims;

namespace Api.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        public BaseController(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        protected Guid GuidUserId =>
       Guid.TryParse(UserId, out var guid) ? guid : Guid.NewGuid();

        // Centralized error handling
        internal IActionResult HandleException(Exception ex)
        {
            _logger.Error(ex, "An error occurred while processing your request.", ex.Message);
            var response = new ResponseModel<object>
            {
                Success = false,
                Message = "An error occurred while processing your request.",
                Errors = new List<string> { ex.Message },
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return StatusCode(response.StatusCode, response);
        }
    }
}
