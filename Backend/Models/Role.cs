using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Role
    {
        public Role()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.DeletedAt = DateTime.Now;
            this.RoleStatus = RoleStatus.Active;
        }
        [Key]
        public string RoleId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public RoleStatus RoleStatus { get; set; }
    }
    public enum RoleStatus
    {
        Active = 1,
        Deactive = 0
    }
}
