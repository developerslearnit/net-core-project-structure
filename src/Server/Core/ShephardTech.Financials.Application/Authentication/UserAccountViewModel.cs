namespace ShephardTech.Financials.Application.Authentication
{
    public class UserAccountViewModel
    {

        public string email { get; set; }
        
        public string username { get; set; }

        public string mobilePhone { get; set; }

        public string password { get; set; }
    }

    public class UserModel
    {
        public int userId { get; set; }
        
        public string email { get; set; }

        public string username { get; set; }
        
        public string mobilePhone { get; set; }

        public List<string> roles { get; set; }

    }
}
