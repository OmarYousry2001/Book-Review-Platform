using Microsoft.AspNetCore.Identity;
using Shared.DTOs.User;
using Shared.GeneralModels.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts.GeneralService.UserManagement
{
    public interface IUserRegistrationService
    {
        Task<BaseResult> RegisterUserAsync(UserRegistrationDto user);
        Task<BaseResult> UpdateUserAsync(UserRegistrationDto user, bool resetPassword);
    }
}
