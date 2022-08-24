using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShephardTech.Web.helpers
{
    public interface IJwtUtils
    {
        public UserObj? ValidateToken(string token);
    }

    public class UserObj
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class JwtUtils : IJwtUtils
    {
        public UserObj? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(StaticKeys.JWT_SECRET_KEY);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                var userName = jwtToken.Claims.First(x => x.Type == "name").Value;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "groupsid").Value);
                // return user id from JWT token if validation successful
                return new UserObj
                {
                    Email = userEmail,
                    UserName = userName,
                    UserId = userId
                };
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
