using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Models;
using NAPASTUDENT.Repositories;

namespace NAPASTUDENT.Controllers
{
    public class HoatDongController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HoatDongController()
        {
            _context = new ApplicationDbContext();
        }

        /*Các view quản lý hoạt động*/
            //Trang quản lý hoạt động học viện
        [Authorize(Roles = "Admin,QuanLyHoatDong")]
        public ActionResult QuanLyHoatDong()
        {
            return View();
        }

            //Trang phê duyệt hoạt động
        [Authorize(Roles = "Admin,QuanLyHoatDong")]
        [Route("HoatDong/PheDuyetHoatDong")]
        public ActionResult PheDuyetHoatDong()
        {
            return View();
        }

            //Trang điều hướng điểm danh
        [Authorize(Roles = "Admin,QuanLyHoatDong,DiemDanhHoatDong")]
        [Route("HoatDong/DiemDanh/")]
        public ActionResult DieuHuongDiemDanh()
        {
            if (User.IsInRole("Admin")|| User.IsInRole("QuanLyHoatDong") || User.IsInRole("DiemDanhHoatDong"))
                return View();
            ViewBag.Message = "Bạn không có quyền truy cập trang này";
            return View("Error");
        }


            //Trang điểm danh hoạt động
        [Authorize]
        [Route("HoatDong/DiemDanh/{hoatDongId}")]
        public ActionResult DiemDanhHoatDong(int hoatDongId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var hoatDong = _context.DanhSachHoatDong.SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null)
            {
                ViewBag.Message = "Không tìm thấy hoạt động này";
                return View("Error");
            }
            if ( hoatDong.IdSinhVienTaoHd == userSinhVienId || User.IsInRole("Admin") 
                         || User.IsInRole("QuanLyHoatDong") || User.IsInRole("DiemDanhHoatDong"))
                return View(hoatDongId);
            ViewBag.Message = "Bạn không có quyền truy cập trang này";
            return View("Error");
        }

        [Route("HoatDong/ChiTiet/{hoatDongId}")]
        public ActionResult XemChiTietHoatDong(int hoatDongId)
        {
            if (!User.Identity.IsAuthenticated) return View("ChiTietHoatDong - Khach", hoatDongId);
            var userSinhVienId = User.Identity.GetSinhVienId();
            var nguoiTaoHoatDong = _context.DanhSachHoatDong.Any(hd => hd.Id == hoatDongId && hd.IdSinhVienTaoHd == userSinhVienId);
            var quyenQuanLy = nguoiTaoHoatDong || User.IsInRole("Admin") || User.IsInRole("QuanLyHoatDong");
            return View(nguoiTaoHoatDong ? "ChiTietHoatDong - QuanLy" : "ChiTietHoatDong", hoatDongId);
        }

        [Route("HoatDong")]
        public ActionResult TrangHoatDong()
        {
            return View();
        }


        [Route("HoatDong/CaNhan")]
        [Authorize]
        public ActionResult TrangHoatDongCaNhan()
        {
            return View();
        }

        [Route("HoatDong/ThongKeSinhVien/{sinhVienId}")]
        public ActionResult TrangThongKeSinhVien(int sinhVienId)
        {

            return View(sinhVienId); 
        }
        
        [Route("HoatDong/ThongKeCaNhan")]
        public ActionResult TrangThongKeCaNhan()
        {
            var userId = User.Identity.GetUserId();
            var sinhVienId = _context.SinhVien.Where(sv => sv.ApplicationUserId == userId)
                .Select(sv => sv.Id).SingleOrDefault();
            return View("TrangThongKeSinhVien",sinhVienId); 
        }

        [Route("HoatDong/ThongKeChung")]
        public ActionResult ThongKeChung()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        [Route("HoatDong/SaveHoatDong")]
        public ActionResult SaveHoatDong()
        {
            return PartialView();
        }

        [Route("HoatDong/DanhSachDonVi")]
        public ActionResult DanhSachHoatDongDonVi()
        {
            return View();
        }
    }
}