using Registration.API.Domains;

namespace Registration.API.Services.Abstration
{
    public interface IHelperService
    {
        (string, byte[]) PasswordSalt(string password);
        bool VerifyPassword(string storedPassword, byte[] passwordSalt, string enteredPassword);
        string Authenticate(User user);
    }
}
