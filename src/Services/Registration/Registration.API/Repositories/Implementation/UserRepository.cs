using Microsoft.EntityFrameworkCore;
using Registration.API.Data;
using Registration.API.Domains;
using Registration.API.Repositories.Abstration;
using System.Runtime.CompilerServices;

namespace Registration.API.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddUserData(User user, CancellationToken cancellationToken = default)
        {
            await _dataContext.Users.AddAsync(user, cancellationToken);
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0 ? true : false;
        }

        public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var users = await _dataContext.Users.Where(u => !u.IsDeleted).ToListAsync(cancellationToken);
            return users;
        }

        public async Task<User> GetUserById(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
            return user;
        }

        public async Task<User> GetUserEmailAddress(string emailAddress, CancellationToken cancellationToken = default)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.EmailAddress.Equals(emailAddress) && !u.IsDeleted, cancellationToken);
            return user;
        }

        public async Task<bool> RemoveUser(User user, CancellationToken cancellationToken = default)
        {
            if (!user.IsDeleted)
            {
                user.IsDeleted = true;
                _dataContext.Entry(user).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateEmailAddress(User user, CancellationToken cancellationToken = default)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }

        public async Task<bool> UpdatePassword(User user, CancellationToken cancellationToken = default)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }

        public async Task<bool> UpdateUser(User user, CancellationToken cancellationToken = default)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
            var count = await _dataContext.SaveChangesAsync(cancellationToken);
            return count > 0;
        }
    }
}
