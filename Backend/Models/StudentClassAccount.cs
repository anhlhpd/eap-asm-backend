using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class StudentClassAccount
    {
        public string StudentClassId { get; set; }
        public string AccountId { get; set; }
        public StudentClass StudentClass { get; set; }
        public Account Account { get; set; }
    }
}
