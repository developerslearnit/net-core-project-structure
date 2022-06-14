using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Application
{
    public class JWTSettings
    {
        public string validIssuer { get; set; }

        public string validAudience { get; set; }

        public int tokenExipration { get; set; }
    }
}
