using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Authentication
{
    public interface IAuthenticationRepository
    {
        public Task<User> GetUserAsync(string login, string passwordHash);
        public Task<List<UserRole>> GetUserRolesAsync(int userId, bool activeUserRole = true);
    }
}
