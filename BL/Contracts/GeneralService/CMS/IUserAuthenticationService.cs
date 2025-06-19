using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;
using System.Security.Claims;

namespace BL.Contracts.GeneralService.CMS
{
    public interface IUserAuthenticationService
    {
        Task<ApplicationUser> GetAuthenticatedUserAsync(ClaimsPrincipal principal);
        Task<AuthenticatedUserResult> GetAuthenticatedUserAsync(string email, string password);
        Task<BaseResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> IsUserAuthorizedAsync(ApplicationUser user, string policy);
        public Task<BaseResult<string>> LoginUserAsync(LoginDto loginDto);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
        Task<BaseResult> ResetPasswordAsync(PasswordResetDto resetDto);
        public  Task<IList<string>> GetClaimsByEmailAsync(string email);
        public  Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        public  Task<IList<string>> GetRolesByIdlAsync(string id);
        public  Task<LoginDto> GetUserByEmailAsync(string email);
        Task SignOutAsync();
    }
}
