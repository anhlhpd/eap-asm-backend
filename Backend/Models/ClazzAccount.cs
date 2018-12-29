using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ClazzAccount
    {
        public string ClazzId { get; set; }
        public string AccountId { get; set; }
        public Clazz Clazz { get; set; }
        public Account Account { get; set; }
    }
}
