using Registration.API.Domains;

namespace Registration.API.Repositories.Abstration
{
    public interface IUserRepository
    {
        Task<User> GetUserEmailAddress(string emailAddress, CancellationToken cancellationToken = default);
        Task<bool> AddUserData(User user, CancellationToken cancellationToken = default);
        Task<User> GetUserById(Guid id, CancellationToken cancellationToken = default);
        Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
        Task<bool> RemoveUser(User user, CancellationToken cancellationToken = default);
        Task<bool> UpdateUser(User user, CancellationToken cancellationToken = default);
        Task<bool> UpdatePassword(User user, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmailAddress(User user, CancellationToken cancellationToken = default);
        Task<bool> AddingOTP(UserOtp userOtp, CancellationToken cancellationToken = default);
        Task<UserOtp> GetUserOTP(User user, CancellationToken cancellationToken);
    }
}
