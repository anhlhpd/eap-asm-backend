﻿using System;
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
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        [Key]
        [Required]
        public string AccountId { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Account Account { get; set; }
    }
}
