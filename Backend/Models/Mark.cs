using System;
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
        public long MarkId { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
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
