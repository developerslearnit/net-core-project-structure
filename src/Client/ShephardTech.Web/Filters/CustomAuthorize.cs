using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Common;
using ShephardTech.Web.helpers;
using System.Security.Claims;

namespace ShephardTech.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // var token = context.HttpContext.Request.Cookies["auth_token"];

            // JwtUtils _jwtUtils = new JwtUtils();

            //var user = _jwtUtils.ValidateToken(token);

            // authorization
            var user = (UserObj)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new RedirectResult("/auth/login");
            }
            else
            {
                var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString(), ClaimValueTypes.Integer32),
                new Claim("Name", user.UserName, ClaimValueTypes.String),
                new Claim("Email", user.Email, ClaimValueTypes.String),
            }, "Custom");

                context.HttpContext.User = new ClaimsPrincipal(identity);
            }
                

           
        }
    }
}
