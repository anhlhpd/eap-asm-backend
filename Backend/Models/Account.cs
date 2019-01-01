using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Account
    {
        public Account()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Status = AccountStatus.Active;
        }
        [Key]
        [Required]
<<<<<<< HEAD
        [ForeignKey("PersonalInformation")]
        public string Id { get; set; }
        [Required]
=======
        public string Id { get; set; }
>>>>>>> a8a634cef5637871d0e89db869da0bbcff2fe170
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public AccountStatus Status { get; set; }
<<<<<<< HEAD
        public PersonalInformation PersonalInformation { get; set; }
=======
        public GeneralInformation GeneralInformation { get; set; }
>>>>>>> a8a634cef5637871d0e89db869da0bbcff2fe170
    }

    public enum AccountStatus
    {
        Active = 1,
        Deactive = 0
    }
}
