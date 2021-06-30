using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Authentication
{
    public interface IAuthenticationService
    {
        public Task<string> CreateJWTokenAsync(int userId, bool rememberMe);
    }
}
