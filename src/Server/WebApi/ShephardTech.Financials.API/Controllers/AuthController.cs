using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShephardTech.Financials.API.Filters;
using ShephardTech.Financials.Application;
using ShephardTech.Financials.Application.Authentication;
using ShephardTech.Financials.Application.Contracts;
using ShephardTech.Financials.Common;
using ShephardTech.Financials.Domain;
using ShephardTech.Financials.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ShephardTech.Financials.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/auth")]
    [ApiController]
    [APIKeyAuth]
    public class AuthController : ControllerBase
    {
        private readonly JWTSettings _jWTSettings;
        private readonly IRepositoryManager _service;
        public AuthController(IRepositoryManager service, IOptions<JWTSettings> jWTSettings)
        {
            _service = service;
            _jWTSettings = jWTSettings.Value;
        }

        [HttpPost(ApiRoutes.Auth.Login)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse<SignInResponseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null) return BadRequest();

            var user = await _service.AuthService.FindUserByName(loginModel.userName);

            if(user==null)
                user = await _service.AuthService.FindUserByEmail(loginModel.userName);


            if (user == null) return StatusCode(StatusCodes.Status200OK,
                new ApiResponse<object>
                {
                    Data = null,
                    Message = "Wrong username or password",
                    StatusCode = StatusCodes.Status400BadRequest,
                    hasError = true
                });


            var passwordSalt = await _service.AuthService.FindUserPasswordSalt(user.userId);

            var hashedPassword = loginModel.password.EncryptSha512(passwordSalt);

            var signInResult = await _service.AuthService.LoginWithPassword(loginModel.userName, hashedPassword);

            if (signInResult.status != LoginStatus.Success)
            {
                return StatusCode(StatusCodes.Status200OK,
                new ApiResponse<object> { Data = null, Message = signInResult.message, StatusCode = StatusCodes.Status200OK, hasError = true });

            }


            var refreshToken = ComputeRefreshToken();
            refreshToken.username = user.username;

            _service.AuthService.LogToken(refreshToken.authToken, refreshToken.expiryDate, refreshToken.username);


            List<string> roles = signInResult.user.roles;

            var jwtToken = await ComputeToken(loginModel.userName, roles);

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<SignInResponseModel>
            {
                Data = new SignInResponseModel
                {
                    access_token = jwtToken,
                    refresh_token = refreshToken.authToken,
                    userId = signInResult.user.userId,
                    email = signInResult.user.email,
                    userName = signInResult.user.username,
                    mobilePhone = signInResult.user.mobilePhone,
                    roles = signInResult.user.roles
                },
                hasError = false,
            });


        }



        private LoginTokenModel ComputeRefreshToken()
        {
            var randomNumber = new byte[32];
            var refreshToken = new LoginTokenModel();
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.authToken = Convert.ToBase64String(randomNumber);
            }
            refreshToken.expiryDate = DateTime.Now.AddMonths(6);

            return refreshToken;

        }



        private async Task<string> ComputeToken(string username, List<string> roles)
        {

            var user = await _service.AuthService.FindUserByName(username);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppConstants.Settings.JWT_SECRET_KEY);

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, username),
                 new Claim(ClaimTypes.GroupSid,user.userId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.Email, user.email),
            };

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jWTSettings.tokenExipration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
