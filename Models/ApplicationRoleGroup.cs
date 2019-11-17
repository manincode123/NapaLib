using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NAPASTUDENT.Models
{
    public class ApplicationRoleGroup
    {
        public virtual string RoleId { get; set; }
        public virtual int GroupId { get; set; }

        public virtual IdentityRole Role { get; set; }
        public virtual Group Group { get; set; }

    }
}