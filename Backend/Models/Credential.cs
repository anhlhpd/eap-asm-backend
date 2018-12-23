using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Credential
    {
        public Credential()
        {

        }
        [Key]
        public string AccountId { get; set; }
        public string AccessToken { get; set; }
        public Account Account { get; set; }
    }
}
