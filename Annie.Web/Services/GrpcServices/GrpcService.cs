using Annie.Web.Services.GrpcServices;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Annie.Authentication;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Annie.Web.Services.GrpcServices
{
    public class GrpcService : IGrpcService
    {
        private readonly Greeter.GreeterClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GrpcService(Greeter.GreeterClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthenticationResponce> GetTokenAsync(AuthenticationRequest request)
        {
            using var responce = _client.GetTokenAsync(request);
            return await responce.ResponseAsync;

            //using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            //var client = new Greeter.GreeterClient(channel);
            //return await client.LoginAsync(new AuthenticationRequest()
            //{
            //    Login = request.Login,
            //    Password = request.Password,
            //    RememberMe = true
            //});
        }
    }
}
