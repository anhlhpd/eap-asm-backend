﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Mark
    {
        public Mark()
        {
            this.CreatedAt = DateTime.Now;
        }
        [Key]
        [Required]
        public long MarkId { get; set; }
        [Required(ErrorMessage = "Please input account")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "Please input subject")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Please input mark value"),
            Range(0, 15, ErrorMessage = "Please input valid mark value")]
        public float Value { get; set; }
        [Required(ErrorMessage = "Please input mark type")]
        public MarkType MarkType { get; set; }
        public DateTime CreatedAt { get; set; }
        public MarkStatus MarkStatus { get; set; }
        public Account Account { get; set; }
        public Subject Subject { get; set; }
    }

    public enum MarkStatus
    {
        Fail = 1,
        Pass = 0,
        Chosen = 2
    }

    public enum MarkType
    {
        Theory = 0,
        Practice = 1,
        Assignment = 2
    }
}
