using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class Group
    {
        public Group() { }


        public Group(string name)
            : this()
        {
            Roles = new List<ApplicationRoleGroup>();
            Name = name;
        }


        [Key]
        [Required]
        public  int Id { get; set; }

        public  string Name { get; set; }
        public virtual IList<ApplicationRoleGroup> Roles { get; set; }

    }
}