using Annie.Authentication;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.GrpcServices
{
    public interface IGrpcService
    {
        public Task<AuthenticationResponce> GetTokenAsync(AuthenticationRequest request);
    }
}
