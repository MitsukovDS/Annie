using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Authentication
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IAuthenticationService _authenticationService;

        public GreeterService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public override async Task<AuthenticationResponce> GetToken(AuthenticationRequest request, ServerCallContext context)
        {
            #region comment
            //var user = await _authenticationRepository.GetUserAsync(request.Login, request.Password);
            //if (user == null)
            //{
            //    return new AuthenticationResponce
            //    {
            //        Token = null
            //    };
            //}
            #endregion

            var token = await _authenticationService.CreateJWTokenAsync(request.UserId, request.RememberMe);

            return new AuthenticationResponce
            {
                Token = token
            };
        }
    }
}
