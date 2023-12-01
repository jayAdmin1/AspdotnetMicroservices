using Registration.API.Domains;
using Registration.API.ViewModels;

namespace Registration.API.Builder
{
    public class UserBuilder
    {
        public static User Convert(UserAddModel userAdd, byte[] salt, string encryptedPassword)
        {
            var user = new User(userAdd.Name, userAdd.EmailAddress, userAdd.MobileNo, userAdd.Address, encryptedPassword, salt, false);
            return user;
        }
    }
}
