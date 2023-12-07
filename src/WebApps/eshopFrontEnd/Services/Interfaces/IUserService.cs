using eshopFrontEnd.Models;

namespace eshopFrontEnd.Services.Interfaces
{
    public interface IUserService
    {
        Task<(UserAddModel, UserErrorModel)> CreateUser(UserAddModel userAddModel);
        Task<(string, UserErrorModel)> Login(UserLoginModel userLoginModel);
        Task<(string,UserErrorModel)>ChangePassword(UserChangePasswordModel userChangePasswordModel, string token);
        Task<(string, UserErrorModel)> ChangeEmailAddress(UserChangeEmailAddressModel userChangeEmailAddressModel, string token);
    }
}
