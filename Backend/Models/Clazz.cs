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
<<<<<<< HEAD:Backend/Models/Clazz.cs
            this.StartDate = DateTime.Now;
            this.ClazzStatus = ClazzStatus.Active;
=======
            this.Status = ClazzStatus.Active;
>>>>>>> a8a634cef5637871d0e89db869da0bbcff2fe170:Backend/Models/Clazz.cs
        }
        [Key]
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "Please input class start date")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Please input the class session (Morning, Afternoon or Evening)")]
        public ClazzSession Session { get; set; }
<<<<<<< HEAD:Backend/Models/Clazz.cs
        public ClazzStatus ClazzStatus { get; set; }
=======
        public ClazzStatus Status { get; set; }
>>>>>>> a8a634cef5637871d0e89db869da0bbcff2fe170:Backend/Models/Clazz.cs
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
