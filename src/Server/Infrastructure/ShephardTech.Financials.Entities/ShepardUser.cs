using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Entities
{
    public partial class ShepardUser : BasicBaseEntity
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string MobilePhone { get; set; } 

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public bool IsLockedOut { get; set; }

        public bool IsActive { get; set; }

        public int FailPasswordCount { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? LastPasswordChangeDate { get; set; }
        
        public DateTime? NextPasswordChangeDate { get; set; }

        
    }
}
