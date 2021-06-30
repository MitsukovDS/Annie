using Annie.Authorization.JWToken;
using Annie.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Annie.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;

        public AuthenticationService(IAuthenticationRepository authenticationRepository, IJwtSigningEncodingKey signingEncodingKey)
        {
            _authenticationRepository = authenticationRepository;
            _signingEncodingKey = signingEncodingKey;
        }

        public async Task<string> CreateJWTokenAsync(int userId, bool rememberMe)
        {
            var signingCredentials = new SigningCredentials(
                _signingEncodingKey.GetKey(),
                _signingEncodingKey.SigningAlgorithm);

            DateTime expirationTime = rememberMe ? JwtSettings.Parameters.LongExpirationTime : JwtSettings.Parameters.ShortExpirationTime;

            var payload = new JwtPayload(
                JwtSettings.Parameters.ValidIssuer,
                JwtSettings.Parameters.ValidAudience,
                new List<Claim>(),
                JwtSettings.Parameters.NotBefore,
                expirationTime);

            var claims = await GetClaimsAsync(userId);

            payload.Add(JwtSettings.ClaimName.Claim, claims);

            var jwtToken = new JwtSecurityToken(new JwtHeader(signingCredentials), payload);
            var jwtTokenHandler = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return jwtTokenHandler;
        }

        private async Task<Dictionary<string, object>> GetClaimsAsync(int userId)
        {
            var claims = new Dictionary<string, object>();
            JwtSettings.Data jwtData = new JwtSettings.Data();

            jwtData.UserId = userId;
            claims.Add(JwtSettings.ClaimName.UserId, jwtData.UserId);

            List<UserRole> userRoles = await _authenticationRepository.GetUserRolesAsync(userId);

            var roleToRoleLevelList = userRoles.Select(ur => (ur.RoleId, ur.RoleLevelId)).ToList();

            if (roleToRoleLevelList.Any())
            {
                foreach (var (roleId, roleLevelId) in roleToRoleLevelList)
                {
                    jwtData.Roles.Add(roleId, roleLevelId);
                }

                claims.Add(JwtSettings.ClaimName.Roles, jwtData.Roles);
            }

            return claims;
        }
    }
}
