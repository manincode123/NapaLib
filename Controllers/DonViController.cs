using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.ViewModels;

namespace NAPASTUDENT.Controllers
{
    public class DonViController : Controller
    {
        private ApplicationDbContext _context;

        public DonViController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Admin, QuanLyDonVi")]
        [Route("DonVi/QuanLyChung")]
        public ActionResult QuanLyChungDonVi()
        {
            return View();
        }

        /*Các trang của từng lớp (admin vẫn có thể truy cập)*/
        [Route("DonVi/ThongTin/{donViId}")]
        public ActionResult ThongTinDonVi(int donViId)
        {
            var donVi = _context.DanhSachDonVi.SingleOrDefault(dv => dv.Id == donViId);
            var quanLyDonViViewModel = new QuanLyDonViViewModel(donVi);
            //Nếu không đăng nhập
            if (!User.Identity.IsAuthenticated) return View(quanLyDonViViewModel);
            //Nếu có đăng nhập và có chức vụ
            if (User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi"))
            {
                quanLyDonViViewModel.SetQuyenCaoNhat();
                return View("QuanLyDonVi", quanLyDonViViewModel);
            }
            //Nếu có đăng nhập nhưng không có chức vụ admin
            var userSinhVienId = User.Identity.GetSinhVienId();
            var thanhVien = _context.DanhSachThanhVienDonVi
                .Include(tv => tv.DanhSachChucVuDonVi)
                .SingleOrDefault(cv => cv.DonViId == donViId && cv.SinhVienId == userSinhVienId);
            //Nếu không là thành viên
            if (thanhVien == null) return View(quanLyDonViViewModel);
            //Nếu chỉ mới đăng kí
            if (!thanhVien.DuocPheDuyet)
            {
                quanLyDonViViewModel.SetMoiDangKi();
                return View(quanLyDonViViewModel);
            }
            quanLyDonViViewModel.SetLaThanhVienChinhThuc();
            //Nếu không có chức vụ dơn vị
            if (thanhVien.DanhSachChucVuDonVi.Count == 0) return View(quanLyDonViViewModel);
            //Nếu có chức vụ đơn vị
            if (thanhVien.DanhSachChucVuDonVi.Any(cvl => cvl.ChucVuId == 10)
                                    || User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi"))
            {
                quanLyDonViViewModel.SetQuyenCaoNhat();
                return View("QuanLyDonVi", quanLyDonViViewModel);
            }
            if (thanhVien.DanhSachChucVuDonVi.Any(cv => cv.QuanLyChucVu))
                quanLyDonViViewModel.SetQuyenQuanLyChucVu();
            if (thanhVien.DanhSachChucVuDonVi.Any(cv => cv.QuanLyHoatDong))
                quanLyDonViViewModel.SetQuanLyHoatDong();
            if (thanhVien.DanhSachChucVuDonVi.Any(cv => cv.QuanLyThongTin))
                quanLyDonViViewModel.SetQuanLyThongTin();
            if (thanhVien.DanhSachChucVuDonVi.Any(cv => cv.QuanLyThanhVien))
                quanLyDonViViewModel.SetQuanLyThanhVien();
            return View("QuanLyDonVi", quanLyDonViViewModel);
        }

        [Authorize]
        [Route("DonVi/QuanLyThanhVien/{donViId}")]  //Quản lý sinh viên lớp
        public ActionResult QuanLyThanhVien(int donViId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var quyenQuanLyThanhVien = _context.DanhSachChucVuDonVi.Any(cv => cv.DonViId == donViId 
                                                                   && cv.SinhVienId == userSinhVienId
                                                                   && (cv.ChucVuId == 10 || cv.QuanLyThanhVien));
            if (quyenQuanLyThanhVien || User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi"))
            {
                var donVi = _context.DanhSachDonVi.SingleOrDefault(dv => dv.Id == donViId);
                var quanLyDonViViewModel = new QuanLyDonViViewModel(donVi);
                return View(quanLyDonViViewModel);
            }

            ViewBag.Message = "Bạn không quyền truy cập trang này.";
            return View("Error");
        }

        [Authorize]
        [Route("DonVi/QuanLyChucVu/{donViId}")]   //Quản lý chức vụ lớp
        public ActionResult QuanLyChucVu(int donViId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var quyenQuanLyThanhVien = _context.DanhSachChucVuDonVi.Any(cv => cv.DonViId == donViId
                                                                              && cv.SinhVienId == userSinhVienId
                                                                              && (cv.ChucVuId == 10 || cv.QuanLyChucVu));
            if (quyenQuanLyThanhVien || User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi"))
            {
                var donVi = _context.DanhSachDonVi.SingleOrDefault(dv => dv.Id == donViId);
                var quanLyDonViViewModel = new QuanLyDonViViewModel(donVi);
                return View(quanLyDonViViewModel);
            }

            ViewBag.Message = "Bạn không quyền truy cập trang này.";
            return View("Error");
        }


        [Route("DonVi/HoatDong/{donViId}")]  //Xem hoặc quản lý hoạt động lớp (với người có quyền)
        public ActionResult HoatDongDonVi(int donViId)
        {
            var donVi = _context.DanhSachDonVi.Include(dv => dv.ChucVuDonVi).SingleOrDefault(dv => dv.Id == donViId);
            if (donVi == null)
            {
                ViewBag.Message = "Yêu cầu không hợp lệ";
                return View("Error");
            }
            var quanLyDonViViewModel = new QuanLyDonViViewModel(donVi);
            //Nếu không đăng nhập
            if (!User.Identity.IsAuthenticated) return View("HoatDongDonVi",quanLyDonViViewModel);
            //Nếu có đăng nhập
            var userSinhVienId = User.Identity.GetSinhVienId();
            var quyenQuanLyThanhVien = donVi.ChucVuDonVi.Any(cv => cv.DonViId == donViId
                                                                && cv.SinhVienId == userSinhVienId
                                                                && (cv.ChucVuId == 10 || cv.QuanLyHoatDong));

            return quyenQuanLyThanhVien || User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi")
                ? View("HoatDongDonVi_QuanLy", quanLyDonViViewModel)
                : View("HoatDongDonVi", quanLyDonViViewModel);                        
        }

        [Authorize]
        [Route("DonVi/HoatDongChoPheDuyet/{donViId}")]  //Xem hoặc quản lý hoạt động lớp (với người có quyền)
        public ActionResult HoatDongChoPheDuyet(int donViId)
        {
            var donVi = _context.DanhSachDonVi.Include(dv => dv.ChucVuDonVi).SingleOrDefault(dv => dv.Id == donViId);
            if (donVi == null)
            {
                ViewBag.Message = "Yêu cầu không hợp lệ";
                return View("Error");
            }
            var quanLyDonViViewModel = new QuanLyDonViViewModel(donVi);
            //Nếu có đăng nhập
            var userSinhVienId = User.Identity.GetSinhVienId();
            var quyenQuanLyThanhVien = donVi.ChucVuDonVi.Any(cv => cv.DonViId == donViId
                                                                   && cv.SinhVienId == userSinhVienId
                                                                   && (cv.ChucVuId == 10 || cv.QuanLyHoatDong));

            if (quyenQuanLyThanhVien || User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi"))
                return View("HoatDongChoPheDuyet", quanLyDonViViewModel);
            //Nếu không có quyền
            ViewBag.Message = "Bạn không có quyền truy cập trang này.";
            return View("Error");
        }

        [Authorize]
        [Route("DonVi/DonViCuaToi")]
        public ActionResult DonViCuaToi()
        {
            return View();
        }

        [Route("DonVi/DanhSachDonVi")]
        public ActionResult DanhSachDonVi()
        {
            return View();
        }
    }
}