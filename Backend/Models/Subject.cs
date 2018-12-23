using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Subject
    {
        public Subject()
        {
            this.SubjectStatus = SubjectStatus.Active;
        }
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectStatus SubjectStatus { get; set; }
        public List<StudentClass> StudentClasses { get; set; }
    }

    public enum SubjectStatus
    {
        Active = 1,
        Deactive = 0
    }
}
