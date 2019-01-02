using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ClazzAccount
    {
        [Key]
        public long Id { get; set; }
        public string ClazzId { get; set; }
        public string AccountId { get; set; }
        public Clazz Clazz { get; set; }
        public Account Account { get; set; }
    }
}
