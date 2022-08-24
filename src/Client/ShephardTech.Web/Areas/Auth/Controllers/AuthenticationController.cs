using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using ShephardTech.Web.helpers;
using ShephardTech.Web.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ShephardTech.Web.Areas.Auth.Controllers
{
    [Area("auth")]
    [Route("auth")]
    public class AuthenticationController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CookieHelper _cookieHelper;
        private readonly IJwtUtils _jwtUtils;
        public AuthenticationController(IHttpClientFactory httpClientFactory, CookieHelper cookieHelper, IJwtUtils jwtUtils)
        {
            _httpClientFactory = httpClientFactory;
            _cookieHelper = cookieHelper;
            _jwtUtils = jwtUtils;
        }

        [Route("login")]
        public IActionResult login()
        {
            return View();
        }
        
        [Route("signout")]
        public IActionResult signout()
        {
            _cookieHelper.remove("auth_token");
            return RedirectToActionPermanent("login");
        }


        [Route("user/login")]
        public async Task<IActionResult> DoLogin([FromBody] LoginModel model)
        {
            var _client = _httpClientFactory.CreateClient("shepardClient");
            
          
            var requestBody = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = httpContent,
                RequestUri = new Uri($"{_client.BaseAddress}auth/login"),
            };
            
            var loginResponse = new LoginResponse();

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {

                var jsonRes = await response.Content.ReadAsStringAsync();

                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonRes);

                if (!loginResponse.hasError)
                {
                    var token = loginResponse.Data.access_token;
                    _cookieHelper.Set("auth_token", token, 1);                   
                    
                   return Json(new { hasError = false, message = "" });
                }
                else
                {
                    return Json(new { hasError = true, message = loginResponse.Message });
                }
            }

            return View();
        }
    }
}
