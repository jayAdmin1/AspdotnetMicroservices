using eshopFrontEnd.Models;

namespace eshopFrontEnd.Services.Interfaces
{
    public interface IUserService
    {
        Task<(UserAddModel, UserErrorModel)> CreateUser(UserAddModel userAddModel);
        Task<(string,Guid, UserErrorModel)> Login(UserLoginModel userLoginModel);
        Task<(string,UserErrorModel)>ChangePassword(UserChangePasswordModel userChangePasswordModel, string token);
        Task<(string, UserErrorModel)> ChangeEmailAddress(UserChangeEmailAddressModel userChangeEmailAddressModel, string token);
        Task<(UserAddModel, UserErrorModel)> UpdateUser(UserUpdateModel userUpdate,Guid userId, string token);
        Task<(string, UserErrorModel)> SendOTP(string token, string userEmailAddress);
        Task<(string, UserErrorModel)> VerifyOTP(string token, string userEmailAddress, string OTP);
        Task<(string, UserErrorModel)> ResendOTP(string token, string userEmailAddress);
    }
}
