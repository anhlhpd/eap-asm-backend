using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class StudentClass
    {
        public StudentClass()
        {
            this.StartDate = DateTime.Now;
            this.StudentClassStatus = StudentClassStatus.Active;
        }
        public string StudentClassId { get; set; }
        public DateTime StartDate { get; set; }
        public string Session { get; set; }
        public StudentClassStatus StudentClassStatus { get; set; }
        public int CurrentSubjectId { get; set; }
        public Subject Subject { get; set; }
    }

    public enum StudentClassStatus
    {
        Active = 1,
        Deactive = 0
    }
}
