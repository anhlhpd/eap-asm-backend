using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Mark
    {
        public long MarkId { get; set; }
        public string AccountId { get; set; }
        public int SubjectId { get; set; }
        public float Value { get; set; }
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
