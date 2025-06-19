//using BL.Contracts.GeneralService;
//using BL.Contracts.GeneralService.UserManagement;
//using Domains.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Resources;
//using Shared.GeneralModels.ResultModels;

//namespace BL.GeneralService.UserManagement
//{
//    public class UserActivationService : IUserActivationService
//    {
//        private readonly IVerificationCodeService _verificationCodeService;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly ILogger<UserActivationService> _logger; 

//        public UserActivationService(
//            IVerificationCodeService verificationCodeService,
//            UserManager<ApplicationUser> userManager,
//            ILogger<UserActivationService> logger)
//        {
//            _verificationCodeService = verificationCodeService;
//            _userManager = userManager;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Sends a new activation code to the user's mobile.
//        /// </summary>
//        //public async Task<bool> SendActivationCodeAsync(string mobile)
//        //{
//        //    return await _verificationCodeService.SendCodeAsync(mobile);
//        //}

//        /// <summary>
//        /// Verifies the activation code and activates the user.
//        /// </summary>
//        public async Task<BaseResult> VerifyActivationCodeAsync(string mobile, string code)
//        {
//            // Verify the code
//            var isCodeValid = _verificationCodeService.VerifyCode(mobile, code);

//            if (!isCodeValid)
//            {
//                return new BaseResult
//                {
//                    Success = false,
//                    Message = UserResources.InvalidOrExpiredCode
//                };
//            }

//            // Find the user by mobile number
//            var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == mobile);

//            if (user == null || user.CurrentState == 0)
//            {
//                return new BaseResult
//                {
//                    Success = false,
//                    Message = UserResources.UserNotFound
//                };
//            }

//            // Activate the user
//            user.PhoneNumberConfirmed = true; // Assuming there’s an `IsActive` property
//            var updateResult = await _userManager.UpdateAsync(user);

//            if (!updateResult.Succeeded)
//            {
//                return new BaseResult
//                {
//                    Success = false,
//                    Message = UserResources.UserActivationFailed,
//                    Errors = updateResult.Errors.Select(e => e.Description).ToList()
//                };
//            }

//            // Remove the used code from cache
//            _verificationCodeService.DeleteCode(mobile);

//            return new BaseResult
//            {
//                Success = true,
//                Message = UserResources.UserActivatedSuccessfully
//            };
//        }

//        /// <summary>
//        /// Resends the activation code to the user's mobile if the cooldown period has elapsed.
//        /// </summary>
//        //public async Task<bool> ResendActivationCodeAsync(string mobile)
//        //{
//        //    return await _verificationCodeService.SendCodeAsync(mobile);
//        //}
//    }
//}
