using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class ApplicationUserGroup
    {
        [Required]
        public virtual string UserId { get; set; }
        [Required]
        public virtual int GroupId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
    }
}