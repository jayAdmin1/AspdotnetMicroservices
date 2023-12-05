using eshopFrontEnd.Models;

namespace eshopFrontEnd.Services.Interfaces
{
    public interface IUserService
    {
        Task<(UserAddModel, UserErrorModel)> CreateUser(UserAddModel userAddModel);
    }
}
