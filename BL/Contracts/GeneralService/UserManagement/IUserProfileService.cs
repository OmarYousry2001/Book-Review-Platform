using Domains.Identity;
using Domains.Views;
using Shared.DTO.Views;
using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IUserProfileService
    {

        Task<UserProfileViewDto> GetUserProfileAsync(string userId);
        Task<BaseResult> UpdateUserProfileAsync(string userId, UserProfileViewDto userProfileDto);
    }
}
