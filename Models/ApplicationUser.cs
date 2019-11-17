using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NAPASTUDENT.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        //Custom properties
        public int? SinhVienId { get;private set; }

        public string TenNguoiDung { get; set; }

        public ApplicationUser()
        {
            Groups = new HashSet<ApplicationUserGroup>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("SinhVienId", SinhVienId.ToString()));
            userIdentity.AddClaim(new Claim("TenNguoiDung", TenNguoiDung));
            return userIdentity;
        }

        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

        public void SetSinhVienId(int sinhVienId)
        {
            SinhVienId = sinhVienId;
        }
    }
}