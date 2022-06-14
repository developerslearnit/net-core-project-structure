using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Application.Authentication
{
    public class LoginModel
    {
        public string userName { get; set; }

        public string password { get; set; }
    }

    public class RefreshRequestModel
    {
        public string accessToken { get; set; }

        public string refreshToken { get; set; }
    }
}
