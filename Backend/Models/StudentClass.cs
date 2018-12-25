using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        [Required]
        public string StudentClassId { get; set; }
        [Required(ErrorMessage = "Please input class start date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Please input the class session (Morning, Afternoon or Evening)")]
        public StudentClassSession Session { get; set; }
        public StudentClassStatus StudentClassStatus { get; set; }
        [Required(ErrorMessage = "Please input the current subject of the class")]
        public int CurrentSubjectId { get; set; }
        public Subject Subject { get; set; }
    }

    public enum StudentClassSession
    {
        Morning = 0,
        Afternoon = 1,
        Evening = 2
    }

    public enum StudentClassStatus
    {
        Active = 1,
        Deactive = 0
    }
}
