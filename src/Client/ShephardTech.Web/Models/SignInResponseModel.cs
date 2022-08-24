namespace ShephardTech.Web.Models
{
    public class SignInResponseModel
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int userId { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string mobilePhone { get; set; }
        public List<string> roles { get; set; }
    }

    public class LoginResponse
    {
        public SignInResponseModel Data { get; set; }

        public int StatusCode { get; set; } 

        public bool hasError { get; set; }

        public string Message { get; set; }
    }
}
