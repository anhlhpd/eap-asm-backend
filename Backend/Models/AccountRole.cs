using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class AccountRole
    {
        public int RoleId { get; set; }
        public string AccountId { get; set; }
        public Role Role { get; set; }
        public Account Account { get; set; }
    }
}
