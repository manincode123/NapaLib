using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Models;

namespace NAPASTUDENT.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SinhVienController()
        {
            _context = new ApplicationDbContext();
        }
        [Route("CaNhan")]
        public ActionResult TrangCaNhan()
        {
            var sinhVienId = User.Identity.GetSinhVienId();
            return View(sinhVienId);
        }
        

        [Route("SinhVien/{sinhVienId}")]
        public ActionResult ChiTietSinhVien(int sinhVienId)
        {
             return View(sinhVienId);
        }

        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [Route("SinhVien/QuanLySinhVien")]
        public ActionResult QuanLySinhVien()
        {
            return View();
        }

        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [Route("SinhVien/ThemLoSinhVien")]
        public ActionResult ThemLoSinhVien()
        {
            return View();
        }

        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [Route("SinhVien/QuanLyHoiVien")]
        public ActionResult QuanLyHoiVien()
        {
            return View();
        }

        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [Route("SinhVien/QuanLyDoanVien")]
        public ActionResult QuanLyDoanVien()
        {
            return View();
        }
        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [HttpGet]
        [Route("SinhVien/DownLoadDanhSachSv")]
        public FileResult DownLoadDanhSachSv()
        {
            const string virtualFilePath = "~/Content/File/DanhSachSV(mau).xlsx";
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, "DanhSachSvMau.xlsx");
        }
        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [HttpGet]
        [Route("SinhVien/DownLoadFileDangKiHoiVien")]
        public FileResult DownLoadFileDangKiHoiVien()
        {
            const string virtualFilePath = "~/Content/File/DanhSachDangKiHoiVien.xlsx";
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, "DanhSachDangKiHoiVien.xlsx");
        }
        [Authorize(Roles = "Admin,QuanLySinhVien")]
        [HttpGet]
        [Route("SinhVien/DownLoadFileDangKiDoanVien")]
        public FileResult DownLoadFileDangKiDoanVien()
        {
            const string virtualFilePath = "~/Content/File/DanhSachDangKiDoanVien.xlsx";
            return File(virtualFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, "DanhSachDangKiDoanVien.xlsx");
        }
    }
}