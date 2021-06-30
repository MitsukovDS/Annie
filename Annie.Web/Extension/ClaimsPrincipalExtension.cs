using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Annie.Model;
using Annie.Authorization.JWToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Annie.Web
{
    public static class ClaimsPrincipalExtension
    {
        public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (!(claimsPrincipal.Identity is ClaimsIdentity userClaims) || !userClaims.IsAuthenticated)
                return null;
            var userData = GetJwtData(claimsPrincipal);
            return userData.UserId;
        }

        public static bool HavePermission(this ClaimsPrincipal claimsPrincipal, params int[] userIds)
        {
            if (!(claimsPrincipal.Identity is ClaimsIdentity userClaims) || !userClaims.IsAuthenticated)
                return false;
            var userData = GetJwtData(claimsPrincipal);
            bool havePer = userIds.Contains(userData.UserId);
            return havePer;
        }

        public static bool HavePermission(this ClaimsPrincipal claimsPrincipal, params Roles[] roles)
        {
            if (!(claimsPrincipal.Identity is ClaimsIdentity userClaims) || !userClaims.IsAuthenticated)
                return false;
            var userData = GetJwtData(claimsPrincipal);
            bool havePer = userData.Roles.Keys.Intersect(roles.GetValues()).Any();
            return havePer;
        }

        public static bool HavePermission(this ClaimsPrincipal claimsPrincipal, params RoleLevels[] roleLevels)
        {
            if (!(claimsPrincipal.Identity is ClaimsIdentity userClaims) || !userClaims.IsAuthenticated)
                return false;
            var userData = GetJwtData(claimsPrincipal);
            bool havePer = userData.Roles.Values.Intersect(roleLevels.GetValues()).Any();
            return havePer;
        }

        private static JwtSettings.Data GetJwtData(ClaimsPrincipal claimsPrincipal)
        {
            Claim claimsPayload = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == JwtSettings.ClaimName.Claim);

            if (claimsPayload == null)
                return null;

            JwtSettings.Data payloadData = null;

            try
            {
                var scopes = claimsPayload.Value;
                payloadData = JsonConvert.DeserializeObject<JwtSettings.Data>(scopes);
            }
            catch
            {
                return null;
            }

            return payloadData;
        }
    }
}
