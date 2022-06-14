using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Application.Authentication
{
    public class LoginTokenModel
    {
        public string username { get; set; }

        public string authToken { get; set; }

        public DateTime expiryDate { get; set; }
    }
}
