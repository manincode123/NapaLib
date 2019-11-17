using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.ViewModels;

namespace NAPASTUDENT.Controllers
{
    public class BaiVietController : Controller
    {
        private ApplicationDbContext _context;

        public BaiVietController()
        {
            _context = new ApplicationDbContext();
        }

        /*Các view để quản lý bài viết*/
            //Trang quản lý bài viết chung
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [Route("BaiViet/QuanLyBaiViet")]
        public ActionResult QuanLyBaiViet()
        {
            return View();
        }

            //Trang phê duyệt bài viết
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [Route("BaiViet/PheDuyetBaiViet")]
        public ActionResult PheDuyetBaiViet()
        {
            return View();
        }

            //Trang quản lý chuyên mục
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [Route("BaiViet/QuanLyChuyenMuc")]
        public ActionResult QuanLyChuyenMuc()
        {
            return View();
        }

            //Trang thêm bài viết
        [Authorize]
        [Route("BaiViet/ThemBaiViet")]
        public ActionResult ThemBaiViet()
        {
            return View();
        }
        
            //Trang chỉnh sửa bài viết
        [Authorize]
        [Route("BaiViet/SuaBaiViet/{baiVietId}")]
        public ActionResult SuaBaiViet(int baiVietId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var baiViet = _context.DanhSachBaiViet.SingleOrDefault(bv => bv.Id == baiVietId);
            if (baiViet == null)
            {
                ViewBag.Message = "Không tìm thấy bài viết bạn yêu cầu.";
                return View("Error");
            }
            if (baiViet.NguoiTaoId == userSinhVienId || User.IsInRole("Admin") || User.IsInRole("QuanLyBaiViet"))
                return View(baiVietId);
            ViewBag.Message = "Bạn không có quyền truy cập trang này.";
            return View("Error");
        }

            //Trang quản lý bài viết cá nhân
        [Authorize]
        [Route("BaiViet/CaNhan")]
        public ActionResult BaiVietCaNhan()
        {
            return View();
        }

        /*Các view để xem bài viết*/
            //Trang xem bài viết
        [Route("BaiViet/{baiVietId}")]
        public ActionResult XemBaiViet(int baiVietId)
        {
            var userSinhVienId = 0;
            if (User.Identity.IsAuthenticated) userSinhVienId = User.Identity.GetSinhVienId();
            var baiViet = _context.DanhSachBaiViet.Where(bv => bv.Id == baiVietId).Select(bv => new
            {
                bv.DuocPheDuyet,
                bv.DaXoa,
                bv.NguoiTaoId
            }).SingleOrDefault();
            if (baiViet == null)
            {
                ViewBag.Message = "Không tìm thấy bài viết";
                return View("Error");
            }
            if (baiViet.DaXoa || !baiViet.DuocPheDuyet)
            {
                return baiViet.NguoiTaoId != userSinhVienId ? View("Error") : View(baiVietId);
            }
            return View(baiVietId);
        }
        
            //Xem bài viết Partial View
        [Route("BaiViet/PartialView")]
        public ActionResult BaiVietPartialView()
        {
           return PartialView();
        }

            //Xem bài viết theo chuyên mục
        [Route("BaiViet/ChuyenMuc/{chuyenMucId}")]
        public ActionResult XemBaiVietChuyenMuc(int chuyenMucId)
        {
            return View(chuyenMucId);
        }

            //Xem bài viết theo tác giả
        [Route("BaiViet/TacGia/{sinhVienId}")]
        public ActionResult BaiVietSinhVien(int sinhVienId)
        {
            var sinhVien = _context.SinhVien.Where(sv => sv.Id == sinhVienId)
                .Select(sv => new BaiVietSinhVienViewModel
                {
                    Id = sv.Id,
                    AnhDaiDien = sv.AnhDaiDien,
                    HoVaTenLot = sv.HoVaTenLot,
                    Ten = sv.Ten,
                    MSSV = sv.MSSV,
                    GioiThieu = sv.GioiThieu
                }).SingleOrDefault();
            if (sinhVien != null) return View(sinhVien);
            ViewBag.Message = "Yêu cầu không hợp lệ";
            return View("Error");
        }

            //Xem bài viết theo lớp
        [Route("BaiViet/Lop/{lopId}")]
        public ActionResult BaiVietLop(int lopId)
        {
            var lop = _context.Lop.Include(l => l.KhoaHoc).SingleOrDefault(l => l.Id == lopId);
            if (lop != null)
            {
                var lopViewModel = new LopViewModel(lop);
                return View(lopViewModel);
            }
            ViewBag.Message = "Yêu cầu không hợp lệ";
            return View("Error");
        }            
        
        //Xem bài viết theo đơn vị
        [Route("BaiViet/DonVi/{donViId}")]
        public ActionResult BaiVietDonVi(int donViId)
        {
            var donVi = _context.DanhSachDonVi.Where(dv => dv.Id == donViId)
                .Select(dv => new DonViViewModel
                {
                    DonViId = dv.Id,
                    AnhBia = dv.AnhBia,
                    TenDonVi = dv.TenDonVi,
                    NgayThanhLap = dv.NgayThanhLap,
                    LoaiDonVi = dv.LoaiDonVi
                }).SingleOrDefault();
            if (donVi != null) return View(donVi);
            ViewBag.Message = "Yêu cầu không hợp lệ";
            return View("Error");
        }


        [Route("BaiViet/ChuyenMucForm")]
        public ActionResult ChuyenMucForm()
        {
            return PartialView();
        }
    }

    public class DonViViewModel
    {
        public int DonViId { get; set; }
        public string TenDonVi { get; set; }
        public DateTime NgayThanhLap { get; set; }
        public string AnhBia { get; set; }
        public string GioiThieu { get; set; }
        public LoaiDonVi LoaiDonVi { get; set; }

    }
}