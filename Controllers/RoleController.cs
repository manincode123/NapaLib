using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NAPASTUDENT.Models;

namespace NAPASTUDENT.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController()
        {
            _context = new ApplicationDbContext();
            var roleStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(roleStore);
        }

        // GET: Role
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Index()
        {
            var listSinhVienRole = new List<SinhVienRole>();
            var userRoles = _context.UserRoles.Where(ur => ur.RoleId != "1").ToList();
            var listUserId = userRoles.Select(ur => ur.UserId).ToList();
            var danhSachSinhVien = _context.SinhVien
                .Where(sv => listUserId.Contains(sv.ApplicationUserId))
                .Select(sv => new
                {
                    sv.Id,
                    sv.ApplicationUserId,
                    sv.MSSV,
                    sv.HoVaTenLot,
                    sv.Ten
                })
                .ToList();
            var roles = _context.Roles.Where(r => r.Name != "SuperAdmin").ToList();
            foreach (var identityUserRole in userRoles)
            {
                var sinhVien = danhSachSinhVien.SingleOrDefault(sv => sv.ApplicationUserId == identityUserRole.UserId);
                if (sinhVien == null) continue;
                listSinhVienRole.Add(new SinhVienRole
                {
                    HoVaTenLot = sinhVien.HoVaTenLot,
                    Ten = sinhVien.Ten,
                    MSSV = sinhVien.MSSV,
                    SinhVienId = sinhVien.Id,
                    RoleName = roles.Single(r => r.Id == identityUserRole.RoleId).Name
                });
            }
            return View(listSinhVienRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public string AddSinhVienToRole(SinhVienRole sinhVienRole)
        {
            if (sinhVienRole.RoleName == "SuperAdmin")   //Super Admin chỉ có 1
            {
                //if (!User.IsInRole("SuperAdmin"))
                return "Đã có lỗi xảy ra";
            }
            var sinhVien = _context.SinhVien.SingleOrDefault(sv => sv.Id == sinhVienRole.SinhVienId);
            if (sinhVien == null) return "Đã có lỗi xảy ra";
            _userManager.AddToRole(sinhVien.ApplicationUserId, sinhVienRole.RoleName);
            _context.SaveChanges();
            return "Đã thêm chức vụ cho sinh viên.";
        }       
        
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public string RemoveSinhVienFromRole(SinhVienRole sinhVienRole)
        {
            if (sinhVienRole.RoleName == "SuperAdmin")  //Super Admin chỉ có 1
            {
                //if (!User.IsInRole("SuperAdmin"))
                return "Đã có lỗi xảy ra";
            }
            var sinhVien = _context.SinhVien.SingleOrDefault(sv => sv.Id == sinhVienRole.SinhVienId);
            if (sinhVien == null) return "Đã có lỗi xảy ra.";
            if (sinhVienRole.RoleName == "Admin")
            {
                var adminLeft = _context.UserRoles.Count(ur => ur.RoleId == "2");
                if (adminLeft <= 1) return "Không thể xóa Admin cuối cùng.";
            }
            _userManager.RemoveFromRole(sinhVien.ApplicationUserId, sinhVienRole.RoleName);
            _context.SaveChanges();
            return "Đã xóa chức vụ.";
        }       
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AddUserToRole(string userId, string roleName)
        {
            if (roleName == "SuperAdmin")
            {
                if (!User.IsInRole("SuperAdmin")) return View("Error");
            }

            var isUserExist = _context.Users.Any(u => u.Id == userId);
            if (!isUserExist) return View("Error");
            _userManager.AddToRole(userId, roleName);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    public class SinhVienRole
    {
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public string MSSV { get; set; }
        public int SinhVienId { get; set; }
        public string RoleName { get; set; }
    }
}