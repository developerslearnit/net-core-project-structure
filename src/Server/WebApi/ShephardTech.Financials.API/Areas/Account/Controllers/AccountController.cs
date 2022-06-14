using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShephardTech.Financials.API.Controllers;
using ShephardTech.Financials.Application.Authentication;
using ShephardTech.Financials.Application.Contracts;
using ShephardTech.Financials.Common;
using ShephardTech.Financials.Domain;

namespace ShephardTech.Financials.API.Areas.Account.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IRepositoryManager _service;

        public AccountController(IRepositoryManager service)
        {
            _service = service;
        }



        /// <summary>
        /// This endpoint creates a new user login account
        /// </summary>
        /// <returns>Status</returns>
        /// <response code="200"></response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [Route(ApiRoutes.Accounts.Register)]
        public async Task<IActionResult> CreateAccount([FromBody] UserAccountViewModel model)
        {
            try
            {
                if (model == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ApiResponse<object> { Data = null, Message = "Bad Request" });

                if (!model.email.IsEmailAddress())
                    return StatusCode(StatusCodes.Status200OK,
                       new ApiResponse<object>
                       {
                           Data = null,
                           hasError = true,
                           Message = "A valid email address is required"
                       });

                var existingUser = await _service.AuthService.FindUserByEmail(model.email.Trim());

                if (existingUser != null) return StatusCode(StatusCodes.Status200OK,
                       new ApiResponse<object>
                       {
                           Data = null,
                           hasError = true,
                           Message = "A user with this email address already exist"
                       });

                var userWithSamePhone = await _service.AuthService.FindUserByPhone(model.mobilePhone.Trim());

                if (userWithSamePhone != null) return StatusCode(StatusCodes.Status200OK,
                       new ApiResponse<object>
                       {
                           Data = null,
                           hasError = true,
                           Message = "A user with this phone number already exist"
                       });


                var userWithSameUsername = await _service.AuthService.FindUserByName(model.username.Trim());

                if (userWithSameUsername != null) return StatusCode(StatusCodes.Status200OK,
                       new ApiResponse<object>
                       {
                           Data = null,
                           hasError = true,
                           Message = "A user with this phone number already exist"
                       });



                var passwordSalt = StringExtensions.RandomString(50);

                var hashedPassword = model.password.EncryptSha512(passwordSalt);

                model.password = hashedPassword;

                var isAccountCreated = await _service.AuthService.AddUser(model, passwordSalt);

                if (isAccountCreated)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new ApiResponse<object>
                        {
                            Data = null,
                            hasError = false,
                            Message = "Account created successfully"
                        });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new ApiResponse<object>
                        {
                            Data = null,
                            hasError = true,
                            Message = "An error occured while creating account"
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<object> { Data = null, Message = ex.Message });
            }
            

        }



    }
}
