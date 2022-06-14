namespace ShephardTech.Financials.Application.Authentication
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
}
