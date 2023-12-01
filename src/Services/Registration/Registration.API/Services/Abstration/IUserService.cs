using Registration.API.ViewModels;

namespace Registration.API.Services.Abstration
{
    public interface IUserService
    {
        Task<(UserDisplayModel, RegistrationApiError)> CreateUser(UserAddModel userAddModel, CancellationToken cancellationToken);
        Task<(List<UserDisplayModel>, RegistrationApiError)> GetAllUsers(CancellationToken cancellationToken);
        Task<(UserDisplayModel, RegistrationApiError)> GetUserById(Guid id, CancellationToken cancellationToken);
        Task<(string, RegistrationApiError)> RemoveUser(Guid id, CancellationToken cancellationToken);
        Task<(UserDisplayModel, RegistrationApiError)> UpdateUser(Guid id, UserUpdateModel userUpdateModel, CancellationToken cancellationToken);
        Task<(string, RegistrationApiError)> Login(UserLoginModel userLogin, CancellationToken cancellationToken);
        Task<(string, RegistrationApiError)> ChangePassword(UserPasswordModel userPasswordModel, CancellationToken cancellationToken);
        Task<(string, RegistrationApiError)> ChangeEmail(UserEmailModel userEmailModel, CancellationToken cancellationToken);
    }
}
