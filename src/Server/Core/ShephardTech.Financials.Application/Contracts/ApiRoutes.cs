using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Application.Contracts
{
    public static class ApiRoutes
    {
        public static class Auth
        {
            public const string Login = "login";
            public const string RefreshToken = "token/refresh";
        }

        public static class Accounts
        {
            public const string Register = "accounts";
            public const string AddToRole = "accounts/role/{userId}";
        }
    }
}
