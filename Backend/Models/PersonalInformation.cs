using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class PersonalInformation
    {
        public PersonalInformation()
        {
            this.Birthday = DateTime.Now;
        }
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public Account Account { get; set; }
    }
}
