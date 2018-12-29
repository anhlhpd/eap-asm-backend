using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ClazzSubject
    {
        public ClazzSubject()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Status = ClazzSubjectStatus.Studying;
        }
        [Key]
        public int Id { get; set; }
        public string ClazzId { get; set; }
        public string SubjectId { get; set; }
        public Clazz Clazz { get; set; }
        public Subject Subject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public ClazzSubjectStatus Status { get; set; }
    }

    public enum ClazzSubjectStatus
    {
        Studying = 1,
        Done = 2,
        NoMark = 0
    }
}
