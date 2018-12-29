using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Clazz
    {
        public Clazz()
        {
            this.Status = ClazzStatus.Active;
        }
        [Key]
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "Please input class start date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Please input the class session (Morning, Afternoon or Evening)")]
        public ClazzSession Session { get; set; }
        public ClazzStatus Status { get; set; }
        [Required(ErrorMessage = "Please input the current subject of the class")]
        public int CurrentSubjectId { get; set; }
        public Subject Subject { get; set; }
    }

    public enum ClazzSession
    {
        Morning = 0,
        Afternoon = 1,
        Evening = 2
    }

    public enum ClazzStatus
    {
        Active = 1,
        Deactive = 0
    }
}
