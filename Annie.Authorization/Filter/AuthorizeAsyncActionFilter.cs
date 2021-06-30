using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Net;
using Annie.Model;
using Annie.Authorization.JWToken;
using Annie.Data;

namespace Annie.Authorization.Filter
{
    public class AuthorizeAsyncActionFilter : Attribute, IAsyncActionFilter
    {
        public Roles[] Roles { get; set; }
        public RoleLevels[] RoleLevels { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;

            if (!IsAuthenticated)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            List<int> roleIds = Roles?.GetValues();
            List<int> roleLevelIds = RoleLevels?.GetValues();

            //var userData = GetUserDataFromIdentity();
            //var userRoleIds = userData.Roles.Select(p => p.Id).ToList();
            //var userRoleLevelIds = userData.Roles.Select(p => p.RoleLevelId).ToList();

            bool roleLevelAccessDenied = true;

            // TODO: заменить roleLevelIds на RoleLevels
            if (roleLevelIds != null && roleLevelIds.Any())
            {
                //if (roleLevelIds.Intersect<int>(userRoleLevelIds).Any())
                if (context.HttpContext.User.HavePermission(RoleLevels))
                {
                    roleLevelAccessDenied = false;
                }
                else
                {
                    // TODO: заменить roleIds на Roles
                    if (roleIds == null || !roleIds.Any())
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }
                }
            }

            // TODO: заменить roleIds на Roles
            if (roleIds != null && roleIds.Any() && roleLevelAccessDenied)
            {
                //if (!roleIds.Intersect<int>(userRoleIds).Any())
                if (!context.HttpContext.User.HavePermission(Roles))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            //выполнение метода
            await next.Invoke();
            //после выполнения метода
        }


        //public UserDataIdentity GetUserDataFromIdentity()
        //{
        //    var userClaims = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

        //    Claim claimsPayload = userClaims.Claims.FirstOrDefault(claim => claim.Type == JwtSettings.ClaimName.Claim);

        //    if (claimsPayload == null)
        //        return null;

        //    JwtSettings.Data payloadData = null;

        //    try
        //    {
        //        var scopes = claimsPayload.Value;

        //        #warning This code is deprecated. Need to use System.Text.Json. But this library does not yet support Dictionary<int, TValue>. Only Dictionary<string, TValue> is supported today
        //        payloadData = Newtonsoft.Json.JsonConvert.DeserializeObject<JwtSettings.Data>(scopes);
        //        //payloadData = System.Text.Json.JsonSerializer.Deserialize<JwtSettings.Data>(scopes);
        //    }
        //    catch { }

        //    var userDataIdentity = new UserDataIdentity(payloadData);

        //    List<Role> roles = _staticRepository.Roles.Where(p => payloadData.Roles.Keys.Contains(p.Id)).ToList();
        //    userDataIdentity.Roles = roles;

        //    return userDataIdentity;
        //}

        //public class UserDataIdentity
        //{
        //    public int UserId { get; set; }
        //    public List<Role> Roles { get; set; }

        //    public UserDataIdentity(JwtSettings.Data jwtData)
        //    {
        //        UserId = jwtData.UserId;
        //    }
        //}


    }
}
