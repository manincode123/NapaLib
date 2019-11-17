using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.ViewModels;

namespace NAPASTUDENT.Controllers
{
    public class LopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LopController()
        {
            _context = new ApplicationDbContext();
        }
        /*Các trang quản lý chung của admin*/
        [Authorize(Roles = "Admin, QuanLyLop")]
        [Route("Lop/QuanLyChung")]
        public ActionResult QuanLyChungLop()
        {
            return View();
        }

        [Authorize(Roles = "Admin , QuanLyLop")]
        [Route("QuanLyKhoaHoc")]
        public ActionResult QuanLyKhoaHoc()
        {
            return View();
        }

        /*Các trang của từng lớp (admin vẫn có thể truy cập)*/
        [Route("Lop/ChiTiet/{lopId}")]
        public ActionResult ChiTietLop(int lopId)
        {
            var lop = _context.Lop
                .Include(l => l.ChucVuLop)
                .Include(l => l.KhoaHoc)
                .SingleOrDefault(l => l.Id == lopId);
            if (lop == null) return View("Error");
            LopViewModel lopViewModel;
            if (!User.Identity.IsAuthenticated)
            {
                lopViewModel = new LopViewModel(lop);
                return View("ChiTietLop",lopViewModel);
            }
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (lop.ChucVuLop.Any(cvl => cvl.ChucVuId == 1 && cvl.SinhVienId == userSinhVienId) 
                || User.IsInRole("Admin") || User.IsInRole("QuanLyLop"))
            {
                var quanLyLopViewModel = new QuanLyLopViewModel(lop);
                quanLyLopViewModel.SetChucVuLopTruong();
                return View("QuanLyLop", quanLyLopViewModel);
            }
            if (lop.ChucVuLop.Any(cvl => cvl.SinhVienId == userSinhVienId && (cvl.ChucVuId == 4 || cvl.ChucVuId == 7)))
            {
                var quanLyLopViewModel = new QuanLyLopViewModel(lop);
                quanLyLopViewModel.SetChucVuBiThuChiHoiTruong();
                return View("QuanLyLop", quanLyLopViewModel);
            }
            lopViewModel = new LopViewModel(lop);
            return View("ChiTietLop", lopViewModel);
        }

        [Authorize]
        [Route("Lop/QuanLySinhVienLop/{lopId}")]  //Quản lý sinh viên lớp
        public ActionResult QuanLySinhVienLop(int lopId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var lop = _context.Lop.Include(l => l.ChucVuLop).SingleOrDefault(l => l.Id == lopId);
            if (lop == null) return View("Error");
            var quanLyChucVuLopViewModel = new QuanLyChucVuLopViewModel(lop);
            var laLopTruong = lop.ChucVuLop.Any(cvl => cvl.LopId == lopId 
                                                    && cvl.SinhVienId == userSinhVienId
                                                    && cvl.ChucVuId == 1);
            if (laLopTruong || User.IsInRole("Admin") || User.IsInRole("QuanLyLop"))
                return View("QuanLySinhVienLop", quanLyChucVuLopViewModel);
            ViewBag.Message = "Bạn không quyền truy cập trang này.";
            return View("Error");
        }

        [Authorize]
        [Route("Lop/QuanLyChucVuLop/{lopId}")]   //Quản lý chức vụ lớp
        public ActionResult QuanLyChucVuLop(int lopId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var lop = _context.Lop.Include(l => l.ChucVuLop).SingleOrDefault(l => l.Id == lopId);
            if (lop == null) return View("Error");
            var quanLyChucVuLopViewModel = new QuanLyChucVuLopViewModel(lop);
            if (User.IsInRole("Admin") || User.IsInRole("QuanLyLop"))
            {
                quanLyChucVuLopViewModel.SetQuyenAdmin();
                return View("QuanLyChucVuLop", quanLyChucVuLopViewModel);
            }
            if (lop.ChucVuLop.Any(cvl => cvl.ChucVuId == 1 && cvl.SinhVienId == userSinhVienId))
                quanLyChucVuLopViewModel.SetQuyenBanCanSu();

            if (lop.ChucVuLop.Any(cvl => cvl.SinhVienId == userSinhVienId && cvl.ChucVuId == 4))
                quanLyChucVuLopViewModel.SetQuyenDoan();

            if (lop.ChucVuLop.Any(cvl => cvl.SinhVienId == userSinhVienId && cvl.ChucVuId == 7))
                quanLyChucVuLopViewModel.SetQuyenHoi();
            if (quanLyChucVuLopViewModel.CheckQuyen()) return View("QuanLyChucVuLop", quanLyChucVuLopViewModel);
            ViewBag.Message = "Bạn không quyền truy cập trang này.";
            return View("Error");
        }


        [Route("Lop/HoatDong/{lopId}")]  //Xem hoặc quản lý hoạt động lớp (với người có quyền)
        public ActionResult HoatDong(int lopId) 
        {
            var lop = _context.Lop
                              .Include(l => l.KhoaHoc)
                              .Include(l => l.ChucVuLop)
                              .SingleOrDefault(l => l.Id == lopId);
            if (lop == null)
            {
                ViewBag.Message = "Yêu cầu không hợp lệ";
                return View("Error");
            }
            var lopViewModel = new LopViewModel(lop);
            //Nếu không đăng nhập
            if (!User.Identity.IsAuthenticated) return View("HoatDongLop", lopViewModel);
            //Nếu có đăng nhập
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coChucVu = lop.ChucVuLop.Any(cvl => cvl.LopId == lopId && cvl.SinhVienId == userSinhVienId 
                                                && (cvl.ChucVuId == 1 || cvl.ChucVuId == 4 || cvl.ChucVuId == 7))
                           || User.IsInRole("Admin") || User.IsInRole("QuanLyHoatDong");
            return View(coChucVu ? "HoatDongLop_QuanLy" : "HoatDongLop", lopViewModel);
        } 

        [Authorize]
        [Route("Lop/HoatDongChoPheDuyet/{lopId}")] //Quản lý hoạt động chờ phê duyệt của lớp
        public ActionResult HoatDongChoPheDuyet(int lopId)
        {
            var sinhVienId = User.Identity.GetSinhVienId();
            var lop = _context.Lop
                .Include(l => l.ChucVuLop)
                .Include(l => l.KhoaHoc)
                .SingleOrDefault(l => l.Id == lopId);
            if (lop == null)
            {
                ViewBag.Message = "Yêu cầu không hợp lệ";
                return View("Error");
            }
            var coChucVu = lop.ChucVuLop.Any(cvl =>  cvl.LopId == lopId 
                                                 &&  cvl.SinhVienId == sinhVienId
                                                 && (cvl.ChucVuId == 1 || cvl.ChucVuId == 4 || cvl.ChucVuId == 7))
                           || User.IsInRole("Admin") || User.IsInRole("QuanLyHoatDong");
            if (coChucVu)
            {
                var lopViewModel = new LopViewModel(lop);
                return View("HoatDongChoPheDuyet", lopViewModel);
            }
            ViewBag.Message = "Bạn không có quyền xem trang này";
            return View("Error");
        }

        [Authorize]
        [Route("Lop/LopCuaToi")]
        public ActionResult LopCuaToi()
        {
            return View();
        }

        [Authorize]
        [Route("Lop/HoatDongLopCuaToi")]
        public ActionResult HoatDongLopCuaToi()
        {
            return View();
        }
    }

    public class QuanLyChucVuLopViewModel
    {
        public QuanLyChucVuLopViewModel()
        {
            
        }
        public QuanLyChucVuLopViewModel(Lop lop)
        {
            LopId = lop.Id;
            TenLop = lop.TenLop;
            AnhBia = lop.AnhBia;
            LopChuyenNganh = lop.LopChuyenNganh;
        }

        public bool LopChuyenNganh { get; set; }
        public string TenLop { get;private set; }
        public string AnhBia { get; private set; }
        public int LopId { get; private set; }
        public bool SuaBanCanSu { get;private set; }  //Có thể sửa chức vụ ban cán sự
        public bool SuaDoan { get;private set; }  //Có thể sửa chức vụ đoàn
        public bool SuaHoi { get;private set; }   //Có thể sửa chức vụ hội

        public void SetQuyenAdmin()
        {
            SuaBanCanSu = true;
            SuaDoan = true;
            SuaHoi = true;
        }

        public void SetQuyenBanCanSu()
        {
            SuaBanCanSu = true;
        }

        public void SetQuyenDoan()
        {
            SuaDoan = true;
        }

        public void SetQuyenHoi()
        {
            SuaHoi = true;  
        }

        public bool CheckQuyen()
        {
            //Kiểm tra xem có quyền nào không, nếu ko có quyền nào, trả false
            return SuaBanCanSu || SuaDoan || SuaHoi;
        }
    }
}