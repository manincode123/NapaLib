using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.SinhVienDto;
using NAPASTUDENT.Repositories;

namespace NAPASTUDENT.Controllers.Api
{
    public class HoatDongController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly HoatDongRepository _hoatDongRepository;
        public HoatDongController()
        {
            _context = new ApplicationDbContext();
            _hoatDongRepository = new HoatDongRepository(_context);

        }

        /*Các API Quản lý hoạt động*/

            //Lấy thông tin cho trang quản lý hoạt động chung
        [Authorize]
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/QuanLyChung")]
        public IHttpActionResult LayThongTinChoTrangQuanLy()
        {
            var hoatDong = _context.DanhSachHoatDong
                .Where(_hoatDongRepository.LayHoatDongNam_HoatDongFunc())
                .Select(hd => new HoatDongDtoForTable
                {
                    NgayBatDau = hd.NgayBatDau,
                    NgayKetThuc = hd.NgayKetThuc,
                    TenHoatDong = hd.TenHoatDong,
                    Id = hd.Id,
                    DaKetThuc = hd.DaKetThuc,
                    BiHuy = hd.BiHuy,
                    CapHoatDong = hd.CapHoatDong,
                    DuocPheDuyet = hd.DuocPheDuyet,
                    DanhSachDonViToChuc = hd.DanhSachDonViToChuc.Select(dv => dv.DonVi.TenDonVi).ToList(),
                    DanhSachLopToChuc = hd.DanhSachLopToChuc.Select(l => l.Lop.TenLop).ToList(),
                    SoLuotThamGia = hd.SoLuotThamGia
                })
                .OrderByDescending(hd => hd.DaKetThuc)
                .ThenByDescending(hd => hd.NgayBatDau)
                .ToList();
            //Lấy số hoạt động chờ phê duyệt
            var soHoatDongChoPheDuyet = _context.DanhSachHoatDong.Count(hd => !hd.DuocPheDuyet);
            //Lấy danh sách hoạt động đang diễn ra
            var hoatDongDangDienRa = hoatDong.Where(hd => !hd.DaKetThuc && !hd.BiHuy && hd.DuocPheDuyet).ToList();
            //Lấy hoạt động tổ chức trong tháng
            var hoatDongThangNay = hoatDong.Where(hd => hd.NgayBatDau.Month == DateTime.Now.Month
                                                     || hd.NgayKetThuc.Month == DateTime.Now.Month
                                                     && !hd.BiHuy
                                                     && hd.DuocPheDuyet).ToList();
            //Lấy số hoạt động tổ chức năm nay
            var soHoatDongTcNamNay = hoatDong.Count(hd => !hd.BiHuy);

            var soHoatDongTcThangNay = hoatDongThangNay.Count();
            var soHoatDongDangDienRa = hoatDongDangDienRa.Count();

            var luotThamGiaThangNay = hoatDongThangNay.Sum(hd => hd.SoLuotThamGia);

            var luotThamGiaNamNay = hoatDong.Sum(hd => hd.SoLuotThamGia);

            return Ok(new
            {
                hoatDong,
                hoatDongDangDienRa,
                hoatDongThangNay,
                luotThamGiaThangNay,
                luotThamGiaNamNay,
                soHoatDongTcNamNay,
                soHoatDongTcThangNay,
                soHoatDongDangDienRa,
                soHoatDongChoPheDuyet

            });

        }

            //Save (tạo mới, chỉnh sửa) hoạt động
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/SaveHoatDong")]
        public IHttpActionResult SaveHoatDong(HoatDongDtoForSave hoatDongDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var hoatDong = new HoatDong();
            var danhSachLop = _context.Lop.ToList();
            var danhSachDonVi = _context.DanhSachDonVi.ToList();
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (hoatDongDto.Id == 0)
            {
                hoatDong.TaoHoatDong(hoatDongDto, userSinhVienId, danhSachLop, danhSachDonVi);
                _context.DanhSachHoatDong.Add(hoatDong);
                _context.SaveChanges();
                hoatDongDto.Id = hoatDong.Id;
                return Created(new Uri(Request.RequestUri + "/" + hoatDongDto.Id), hoatDongDto);
            }
            hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachSinhVienTheoDoi.Select(td => td.SinhVien))
                .Include(hd => hd.DanhSachDonViToChuc)
                .Include(hd => hd.DanhSachLopToChuc)
                .SingleOrDefault(cthd => cthd.Id == hoatDongDto.Id);
            if (hoatDong == null) return NotFound();
            //Check quyền
            if (hoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                return BadRequest();
            hoatDong.ThayDoi(hoatDongDto, danhSachLop, danhSachDonVi);
            _context.SaveChanges();
            return Ok(hoatDongDto);
        }

            //Lấy thông tin cho trang phê duyệt hoạt động
        [Authorize(Roles = "Admin,QuanLyHoatDong")]
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/TrangPheDuyetHoatDong")]
        public IHttpActionResult LayThongTinChoTrangPheDuyet()
        {
            var hoatDong = _context.DanhSachHoatDong
                .Where(hd => !hd.DuocPheDuyet)
                .Select(hd => new
                {
                    hd.NgayBatDau,
                    hd.NgayKetThuc,
                    hd.TenHoatDong,
                    hd.Id,
                    hd.CapHoatDong,
                    DanhSachDonViToChuc = hd.DanhSachDonViToChuc.Select(dv => dv.DonVi.TenDonVi).ToList(),
                    DanhSachLopToChuc = hd.DanhSachLopToChuc.Select(dv => dv.Lop.TenLop).ToList()
                }).ToList();
            return Ok(hoatDong);
        }

            //Phê duyệt hoạt động
        [Authorize(Roles = "Admin,QuanLyHoatDong")]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/PheDuyetHoatDong/{hoatDongId}")]
        public IHttpActionResult PheDuyetHoatDong(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.Include(hd => hd.SinhVienTaoHd)
                .SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();
            hoatDong.PheDuyetHoatDong();
            _context.SaveChanges();
            return Ok();
        }

            //Hủy hoạt động
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/HuyHoatDong/{hoatDongId}")]
        public IHttpActionResult HuyHoatDong(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachSinhVienTheoDoi.Select(svtd => svtd.SinhVien))
                .Include(hd => hd.SinhVienTaoHd)
                .SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (hoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                return BadRequest("Bạn không có quyền hủy hoạt động này. Chỉ có sinh viên tạo mới có quyền hủy.");
            hoatDong.HuyHoatDong();
            _context.SaveChanges();
            return Ok();
        }
            
            //Hủy tham gia tổ chức hoạt động
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/HuyThamGiaToChucHoatDong")]
        public IHttpActionResult HuyThamGiaToChucHoatDong(HuyToChucHoatDong huyHoatDong)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            bool coQuyenHuyHoatDong;
            HoatDong hoatDong;
            if (huyHoatDong.LopId != 0)
            {
                hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachLopToChuc.Select(l => l.Lop.ChucVuLop))
                    .SingleOrDefault(hd => hd.Id == huyHoatDong.HoatDongId);
                if (hoatDong == null) return NotFound();
                coQuyenHuyHoatDong = hoatDong.DanhSachLopToChuc.Any(l =>
                    l.Lop.ChucVuLop.Any(cvl => cvl.LopId == huyHoatDong.LopId && cvl.SinhVienId == userSinhVienId
                                                                              && (cvl.ChucVuId == 1 || cvl.ChucVuId == 4 || cvl.ChucVuId == 7)));
                if (!coQuyenHuyHoatDong && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                    return BadRequest("Bạn không có quyền làm điều này");
                hoatDong.XoaLopToChuc(huyHoatDong.LopId);
                _context.SaveChanges();
                return Ok();
            }

            if (huyHoatDong.DonViId != 0)
            {
                hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachDonViToChuc.Select(dv => dv.DonVi.ChucVuDonVi))
                    .SingleOrDefault(hd => hd.Id == huyHoatDong.HoatDongId);
                if (hoatDong == null) return NotFound();
                coQuyenHuyHoatDong = hoatDong.DanhSachDonViToChuc.Any(dv => dv.DonVi.ChucVuDonVi
                                                    .Any(cvdv => cvdv.DonViId == huyHoatDong.DonViId 
                                                     && cvdv.SinhVienId == userSinhVienId
                                                     && (cvdv.ChucVuId == 10 || cvdv.QuanLyHoatDong)));
                if (!coQuyenHuyHoatDong && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                    return BadRequest("Bạn không có quyền làm điều này");
                hoatDong.XoaDonViToChuc(huyHoatDong.DonViId);
                _context.SaveChanges();
                return Ok();

            }

            return BadRequest();
        }

            //Kết thúc hoạt động
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/KetThucHoatDong/{hoatDongId}")]
        public IHttpActionResult KetThucHoatDong(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.Include(td => td.DanhSachSinhVienTheoDoi.Select(svtd => svtd.SinhVien))
                .Include(hd => hd.SinhVienTaoHd)
                .SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (hoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                return BadRequest("Bạn không có quyền kết thúc hoạt động này.");
            hoatDong.KetThucHoatDong();
            _context.SaveChanges();
            return Ok();
        }

            //Mở lại hoạt động
        [Authorize(Roles = "Admin,QuanLyHoatDong")]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/MoLaiHoatDong/{hoatDongId}")]
        public IHttpActionResult MoLaiHoatDong(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();
            hoatDong.MoLaiHoatDong();
            _context.SaveChanges();
            return Ok();
        }

            //Khôi phục hoạt động
        [Authorize(Roles = "Admin,QuanLyHoatDong")]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/KhoiPhucHoatDong/{hoatDongId}")]
        public IHttpActionResult KhoiPhucHoatDong(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();
            hoatDong.KhoiPhucHoatDong();
            _context.SaveChanges();
            return Ok();
        }

            //Save (tạo mới, chỉnh sửa) chương trình
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/SaveChuongTrinhHoatDong")]
        public IHttpActionResult SaveChuongTrinhHoatDong(ChuongTrinhHoatDongDto chuongTrinhDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var userSinhVienId = User.Identity.GetSinhVienId();
            ChuongTrinhHoatDong chuongTrinh;
            if (chuongTrinhDto.Id == 0)
            {
                //Lấy hoạt động
                var hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachSinhVienTheoDoi.Select(tg => tg.SinhVien))
                                                .SingleOrDefault(hd => hd.Id == chuongTrinhDto.HoatDongId);
                if (hoatDong == null) return NotFound();
                //Check quyền
                if (hoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                    return BadRequest("Bạn không có thêm chương trình cho hoạt động này.");
                //Thêm hoạt động
                hoatDong.ThemChuongTrinh(chuongTrinhDto);
                _context.SaveChanges();
                chuongTrinhDto.Id = hoatDong.DanhSachChuongTrinhHoatDong.Last().Id;
                return Created(new Uri(Request.RequestUri + "/" + chuongTrinhDto.Id), chuongTrinhDto);
            }
            //Lấy chương trình
            chuongTrinh = _context.DanhSachChuongTrinhHoatDong
                                  .Include(ct => ct.HoatDong)
                                  .SingleOrDefault(cthd => cthd.Id == chuongTrinhDto.Id);
            if (chuongTrinh == null) return NotFound();
            //Check quyền
            if (chuongTrinh.HoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                return BadRequest("Bạn không có quyền thay đổi chương trình cho hoạt động này.");
            //Thay đổi chương trình
            Mapper.Map(chuongTrinhDto, chuongTrinh);
            _context.SaveChanges();
            return Ok(chuongTrinhDto);
        }

            //Xóa chương trình hoạt động
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/DeleteChuongTrinhHoatDong/{ctrId}")]
        public IHttpActionResult DeleteChuongTrinhHoatDong(int ctrId)
        {
            var chuongTrinh = _context.DanhSachChuongTrinhHoatDong
                .Include(ct => ct.HoatDong)
                .SingleOrDefault(cthd => cthd.Id == ctrId);
            if (chuongTrinh == null) return NotFound();
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (chuongTrinh.HoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong"))
                return BadRequest("Bạn không có quyền xóa chương trình của hoạt động này.");
            //Xóa chuowgn trình
            _context.DanhSachChuongTrinhHoatDong.Remove(chuongTrinh);
            _context.SaveChanges();
            return Ok();
        }

            //Điểm danh hoạt động
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/DiemDanh")]
        public IHttpActionResult DiemDanh(LuotThamGiaDto luotThamGiaDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachSinhVienThamGia)
                                            .SingleOrDefault(hd => hd.Id == luotThamGiaDto.HoatDongId);
            if (hoatDong == null) return NotFound();
            if (hoatDong.BiHuy) return BadRequest("Hoạt động đã bị hủy, không thể điểm danh.");
            if (!hoatDong.DuocPheDuyet) return BadRequest("Hoạt động chưa được duyệt, không thể điểm danh.");
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (hoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong") && !User.IsInRole("DiemDanhHoatDong"))
                return BadRequest("Bạn không có quyền điểm danh hoạt động này.");
            //Lấy sinh viên
            var sinhVien = _context.SinhVien.SingleOrDefault(sv => sv.Id == luotThamGiaDto.SinhVienId);
            if (sinhVien == null) return NotFound();
            if (sinhVien.LopDangHocId == null)
                return BadRequest("Sinh viên phải đăng kí lớp trước khi tham gia hoạt động");

            var luotThamGia = hoatDong.DanhSachSinhVienThamGia
                                      .SingleOrDefault(tghd => tghd.SinhVienId == sinhVien.Id);

            if (luotThamGia != null)
            {
                if (luotThamGia.DuocPheDuyet) return BadRequest("Đã điểm danh người này");
                return BadRequest("Hãy phê duyệt lượt đăng kí của người này ở trên.");
            }

            luotThamGia = new ThamGiaHoatDong(luotThamGiaDto.HoatDongId, sinhVien);
            hoatDong.ThemLuotThamGia(luotThamGia, sinhVien);
            _context.SaveChanges();
            return Ok();
        }

            //Xóa điểm danh hoạt động
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/XoaDiemDanh")]
        public IHttpActionResult XoaDiemDanh(LuotThamGiaDto luotThamGiaDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var luotThamGia = _context.DanhSachThamGiaHoatDong
                                      .Include(tghd => tghd.HoatDong)
                                      .Include(tghd => tghd.SinhVien)
                                      .SingleOrDefault(dstghd => dstghd.HoatDongId == luotThamGiaDto.HoatDongId
                                                              && dstghd.SinhVienId == luotThamGiaDto.SinhVienId);
            if (luotThamGia == null) return NotFound();
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (luotThamGia.HoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong") && !User.IsInRole("DiemDanhHoatDong"))
                return BadRequest("Bạn không có quyền xóa điểm danh hoạt động này.");
            luotThamGia.Xoa();
            _context.DanhSachThamGiaHoatDong.Remove(luotThamGia);
            _context.SaveChanges();
            return Ok();
        }

            //Phê duyệt lượt đăng kí
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/PheDuyetLuotDangKi")]
        public IHttpActionResult PheDuyetLuotDangKi(LuotThamGiaDto luotThamGiaDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var luotDangKi = _context.DanhSachThamGiaHoatDong
                .Include(tg => tg.HoatDong)
                .Include(tg => tg.SinhVien)
                .SingleOrDefault(tg => tg.SinhVienId == luotThamGiaDto.SinhVienId);

            if (luotDangKi == null) return BadRequest();
            if (luotDangKi.HoatDong.BiHuy) return BadRequest("Hoạt động đã bị hủy, không thể điểm danh.");
            if (!luotDangKi.HoatDong.DuocPheDuyet) return BadRequest("Hoạt động chưa được duyệt, không thể điểm danh.");


            if (luotDangKi.DuocPheDuyet) return BadRequest("Đã điểm danh người này.");
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (luotDangKi.HoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong") && !User.IsInRole("DiemDanhHoatDong"))
                return BadRequest("Bạn không có quyền phê duyệt đăng kí tham gia hoạt động này.");
            luotDangKi.PheDuyetLuotDangKi();
            _context.SaveChanges();
            return Ok();

        }

        [HttpGet]
        [Route("api/HoatDong/TimKiem")]
        public IHttpActionResult TimKiemHoatDong(string searchTerm, int pageIndex = 1)
        {
            if (searchTerm.Length <= 2) return Ok();
            searchTerm = searchTerm.Trim();
            var param1 = new SqlParameter("@SearchTerm", searchTerm);
            var param2 = new SqlParameter("@CurrentPage", pageIndex);
            var result = _context.Database.SqlQuery<HoatDongDtoForSearch>("SearchHoatDong @SearchTerm, @CurrentPage", param1, param2).ToList();

            return Ok(result);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/ChiTiet/{hoatDongId}")]    
        public IHttpActionResult XemChiTietHoatDong(int hoatDongId)
        {
            var sinhVienId=0;
            if (User.Identity.IsAuthenticated)
            {
                sinhVienId = User.Identity.GetSinhVienId();
            }
            
            var hoatDong = _context.DanhSachHoatDong
                .Where(hd => hd.Id == hoatDongId)
                .Select(hd => new
            {
                hd.TenHoatDong,
                hd.AnhBia,
                hd.Id,
                hd.NgayBatDau,
                hd.NgayKetThuc,
                hd.SoNgayTinhNguyen,
                hd.DiaDiem,
                hd.SoLuoc,
                hd.NoiDung,
                hd.DaKetThuc,
                hd.CapHoatDong,
                hd.BiHuy,
                hd.HoatDongNgoaiHocVien,
                hd.DuocPheDuyet,
                soLuotThamGia = hd.DanhSachSinhVienThamGia.Count(tg => tg.DuocPheDuyet),
                danhSachChuongTrinhHoatDong = hd.DanhSachChuongTrinhHoatDong.Select(cthd => 
                    new { 
                        cthd.Id,
                        cthd.TieuDe, 
                        cthd.LoaiHienThi, 
                        cthd.NoiDungChuongTrinh,
                        cthd.TgDienRa,  
                    }).OrderBy(cthd => cthd.TgDienRa),
                donViToChuc = hd.DanhSachDonViToChuc.Select(dv => new {dv.DonViId, dv.DonVi.TenDonVi}),
                lopToChuc = hd.DanhSachLopToChuc.Select(l => new {l.LopId, l.Lop.TenLop}),
                CoThamGia = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == sinhVienId && tg.DuocPheDuyet),
                CoTheoDoi = hd.DanhSachSinhVienTheoDoi.Any(tg => tg.SinhVienId == sinhVienId),
                CoDangKi = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == sinhVienId && !tg.DuocPheDuyet)
            }).FirstOrDefault();
            
            return Ok(hoatDong);
        }

        [HttpGet]
        [Authorize]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/LayThongTinTrangDiemDanh/{hoatDongId}")]
        public IHttpActionResult LayThongTinTrangDiemDanh(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong
                .Where(hd => hd.Id == hoatDongId)
                .Select(hd => new DanhSachDiemDanhDto
                {
                    TenHoatDong = hd.TenHoatDong,
                    Id = hd.Id,
                    NgayBatDau = hd.NgayBatDau,
                    NgayKetThuc = hd.NgayKetThuc,
                    DaKetThuc = hd.DaKetThuc,
                    DuocPheDuyet = hd.DuocPheDuyet,
                    BiHuy = hd.BiHuy,
                    IdSinhVienTao = hd.IdSinhVienTaoHd,
                    DanhSachSinhVienDangKi = hd.DanhSachSinhVienThamGia.Where(tg => !tg.DuocPheDuyet)
                        .Select(dstghd => 
                        new ThamGiaHoatDongDto
                        {
                            Id = dstghd.SinhVien.Id,
                            HoVaTenLot = dstghd.SinhVien.HoVaTenLot,
                            Ten = dstghd.SinhVien.Ten,
                            MSSV = dstghd.SinhVien.MSSV,
                            KyHieuTenLop = dstghd.SinhVien.LopDangHoc.KyHieuTenLop
                        }).ToList()
                    //Không query DanhSachSinhVienThamGia vì nó sẽ được lấy qua ajax datatables (xem TrangDiemDanh View)
                }).FirstOrDefault();

            if (hoatDong == null) return NotFound();
            //Kiểm tra xem đay phải là người có quyền điểm danh không (là người tạo hoạt động hoặc có role phù hợp)
            var sinhVienId = User.Identity.GetSinhVienId();
            if (hoatDong.IdSinhVienTao != sinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong") && !User.IsInRole("DiemDanhHoatDong")) 
                return BadRequest();
            return Ok(hoatDong);
        }

        //Func này để lấy danh sách tham gia cho trang điểm danh (chỉ bao gồm người đã phê duyệt), khác với XemDanhSachSinhVienThamGia
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/LayDanhSachDaDiemDanh/{hoatDongId}")]
        public IHttpActionResult LayDanhSachDaDiemDanh(int hoatDongId)
        {
            var danhSachThamGia = _context.DanhSachThamGiaHoatDong
                .Where(dstghd => dstghd.HoatDongId == hoatDongId && dstghd.DuocPheDuyet)
                .Select(dstghd =>
                    new ThamGiaHoatDongDto
                    {
                        Id = dstghd.SinhVien.Id,
                        HoVaTenLot = dstghd.SinhVien.HoVaTenLot,
                        Ten = dstghd.SinhVien.Ten,
                        MSSV = dstghd.SinhVien.MSSV,
                        KyHieuTenLop = dstghd.SinhVien.LopDangHoc.KyHieuTenLop,
                        NgayThamGia = dstghd.NgayThamGia
                    }).ToList();
            return Ok(danhSachThamGia);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/DanhSachThamGia/{hoatDongId}")]
        public IHttpActionResult XemDanhSachSinhVienThamGia(int hoatDongId)
        {
            //Danh sách tham gia này bao gồm cả những người chỉ mới đăng kí tham gia
            var sinhVienId = 0;
            if (User.Identity.IsAuthenticated)
            {
                sinhVienId = User.Identity.GetSinhVienId();
            }
            var nguoiTaoHoatDong = _context.DanhSachHoatDong.Any(hd => hd.Id == hoatDongId && hd.IdSinhVienTaoHd == sinhVienId);
            IList<ThamGiaHoatDongDto> danhSachThamGia;
            //Nếu là người tạo hoạt động thì lấy thêm sđt của người tham gia
            if (nguoiTaoHoatDong)
            {
                danhSachThamGia = _context.DanhSachThamGiaHoatDong
                    .Where(dstghd => dstghd.HoatDongId == hoatDongId)
                    .Select(dstghd =>
                        new ThamGiaHoatDongDto
                        {
                            Id = dstghd.SinhVien.Id,
                            HoVaTenLot = dstghd.SinhVien.HoVaTenLot,
                            Ten = dstghd.SinhVien.Ten,
                            MSSV = dstghd.SinhVien.MSSV,
                            KyHieuTenLop = dstghd.SinhVien.LopDangHoc.KyHieuTenLop,
                            Sdt = dstghd.SinhVien.SDT.Select(sdt => sdt.SoDienThoai).FirstOrDefault(),
                            NgayThamGia = dstghd.NgayThamGia,
                            DuocPheDuyet = dstghd.DuocPheDuyet
                        }).ToList();

                return Ok(danhSachThamGia);
            }
            danhSachThamGia = _context.DanhSachThamGiaHoatDong
                .Where(dstghd => dstghd.HoatDongId == hoatDongId)
                .Select(dstghd =>
                    new ThamGiaHoatDongDto
                    {
                        Id = dstghd.SinhVien.Id,
                        HoVaTenLot = dstghd.SinhVien.HoVaTenLot,
                        Ten = dstghd.SinhVien.Ten,
                        MSSV = dstghd.SinhVien.MSSV,
                        KyHieuTenLop = dstghd.SinhVien.LopDangHoc.KyHieuTenLop,
                        DuocPheDuyet = dstghd.DuocPheDuyet
                    }).ToList();

            return Ok(danhSachThamGia);
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/DangKi/{hoatDongId}")]
        public IHttpActionResult DangKiThamGia(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachSinhVienThamGia)
                .Include(hd => hd.DanhSachSinhVienTheoDoi)
                .SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();
            if (hoatDong.BiHuy) return BadRequest("Hoạt động đã bị hủy, không thể đăng kí tham gia.");
            if (!hoatDong.DuocPheDuyet) return BadRequest("Hoạt động chưa được duyệt, không thể đăng kí tham gia.");

            var userId = User.Identity.GetUserId();
            var sinhVien = _context.SinhVien.SingleOrDefault(sv => sv.ApplicationUserId == userId);
            if (sinhVien == null) return NotFound();
            if (sinhVien.LopDangHocId == null)
                return BadRequest("Sinh viên phải đăng kí lớp trước khi đăng kí tham gia hoạt động");

            var daCoLuotDangKi = hoatDong.DanhSachSinhVienThamGia.Any(tghd => tghd.SinhVienId == sinhVien.Id);
            if (daCoLuotDangKi) return BadRequest("Bạn đã đăng kí, hãy chờ xác nhận");

            var daCoTheoDoi = hoatDong.DanhSachSinhVienTheoDoi.Any(tghd => tghd.SinhVienId == sinhVien.Id);
            if (!daCoTheoDoi)
            {
                var luotTheoDoi = new TheoDoiHoatDong(hoatDongId, sinhVien.Id);
                hoatDong.ThemLuotTheoDoi(luotTheoDoi);
            }

            var luotDangKi = new ThamGiaHoatDong(hoatDongId, sinhVien, true);
            hoatDong.ThemLuotDangKi(luotDangKi);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/XoaDangKi/{hoatDongId}/{sinhVienId:int?}")]
        public IHttpActionResult XoaDangKiThamGia(int hoatDongId,int? sinhVienId = 0)
        {
            ThamGiaHoatDong luotDangKi;
            if (sinhVienId == 0)
            {
                sinhVienId = User.Identity.GetSinhVienId();
                luotDangKi = _context.DanhSachThamGiaHoatDong.SingleOrDefault(dstghd =>
                    dstghd.HoatDongId == hoatDongId && dstghd.SinhVien.Id == sinhVienId && dstghd.DuocPheDuyet==false);

                if (luotDangKi == null) return NotFound();

                _context.DanhSachThamGiaHoatDong.Remove(luotDangKi);

                //Bỏ theo dõi khi bỏ đăng kí hoạt động
                var luotTheoDoi = _context.DanhSachTheoDoiHoatDong.SingleOrDefault(td =>
                    td.HoatDongId == hoatDongId && td.SinhVien.Id == sinhVienId);
                if (luotTheoDoi != null) _context.DanhSachTheoDoiHoatDong.Remove(luotTheoDoi);

                _context.SaveChanges();
                return Ok();
            }
            //Nếu không phải sinh viên đó (có cung cấp sinhVienId), mà là người quản lý hoạt động 
            luotDangKi = _context.DanhSachThamGiaHoatDong
                                 .Include(tghd => tghd.HoatDong)
                                 .Include(tghd => tghd.SinhVien)
                                 .SingleOrDefault(dstghd => dstghd.HoatDongId == hoatDongId 
                                                         && dstghd.SinhVien.Id == sinhVienId 
                                                         && !dstghd.DuocPheDuyet);
            if (luotDangKi == null) return NotFound();
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (luotDangKi.HoatDong.IdSinhVienTaoHd != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyHoatDong") && !User.IsInRole("DiemDanhHoatDong"))
                return BadRequest();
            luotDangKi.HuyLuotDangKi();
            _context.DanhSachThamGiaHoatDong.Remove(luotDangKi);
            _context.SaveChanges();
            return Ok();
        }
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/TheoDoi/{hoatDongId}")]
        public IHttpActionResult DangKiTheoDoi(int hoatDongId)
        {
            var hoatDong = _context.DanhSachHoatDong.Include(hd => hd.DanhSachSinhVienTheoDoi)
                .SingleOrDefault(hd => hd.Id == hoatDongId);
            if (hoatDong == null) return NotFound();

            var sinhVienId = User.Identity.GetSinhVienId();
            if (sinhVienId == 0) return NotFound();

            var daCoTheoDoi = hoatDong.DanhSachSinhVienTheoDoi.Any(tghd => tghd.SinhVienId == sinhVienId);
            if (daCoTheoDoi) return BadRequest("Bạn đã theo dõi hoạt động này.");

            var luotTheoDoi = new TheoDoiHoatDong(hoatDongId,sinhVienId);
            hoatDong.ThemLuotTheoDoi(luotTheoDoi);
            _context.SaveChanges();
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/XoaTheoDoi/{hoatDongId}")]
        public IHttpActionResult XoaTheoDoi(int hoatDongId)
        {
                var sinhVienId = User.Identity.GetSinhVienId();
                var luotTheoDoi = _context.DanhSachTheoDoiHoatDong.SingleOrDefault(td =>
                    td.HoatDongId == hoatDongId && td.SinhVien.Id == sinhVienId);

                if (luotTheoDoi == null) return NotFound();
                _context.DanhSachTheoDoiHoatDong.Remove(luotTheoDoi);
                _context.SaveChanges();
                return Ok();
        
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/ThongKeChung/{namHocLay}")]
        public IHttpActionResult LayThongKeHoatDongChung(int namHocLay)
        {
            var danhSachHoatDong = _context.DanhSachHoatDong
                .Where(_hoatDongRepository.LayHoatDongNam_HoatDongFunc(namHocLay)) 
                .Where(hd => hd.DuocPheDuyet && !hd.BiHuy)  //Chỉ lấy hoạt động được phê duyệt và không bị hủy
                .Select(hd => new HoatDongDtoForTable
                {
                    NgayBatDau = hd.NgayBatDau,
                    NgayKetThuc = hd.NgayKetThuc,
                    TenHoatDong = hd.TenHoatDong,
                    Id = hd.Id,
                    DaKetThuc = hd.DaKetThuc,
                    BiHuy = hd.BiHuy,
                    CapHoatDong = hd.CapHoatDong,
                    DuocPheDuyet = hd.DuocPheDuyet,
                    DanhSachDonViToChuc = hd.DanhSachDonViToChuc.Select(dv => dv.DonVi.TenDonVi).ToList(),
                    DanhSachLopToChuc = hd.DanhSachLopToChuc.Select(l => l.Lop.TenLop).ToList(),
                    SoLuotThamGia = hd.SoLuotThamGia
                })
                .OrderByDescending(hd => hd.DaKetThuc)
                .ThenByDescending(hd => hd.NgayBatDau)
                .ToList();
            var soHoatDongToChucTrongNam = danhSachHoatDong.Count();

            var soLuotThamGiaTrongNam = danhSachHoatDong.Sum(hd => hd.SoLuotThamGia);

            var soHoatDongToChucTungThang = _hoatDongRepository.LaySoHdTcTungThangChoHocVien(danhSachHoatDong,namHocLay);

            var soLuotThamGiaTungThang = _hoatDongRepository.LaySoLuotTgTungThangChoHocVien(danhSachHoatDong, namHocLay);

            var soHoatDongTungCapHoatDong = _hoatDongRepository.LaySoHoatDongTungCap(danhSachHoatDong);
            return Ok(new
            {
                danhSachHoatDong,
                soHoatDongToChucTungThang,
                soLuotThamGiaTungThang,
                soLuotThamGiaTrongNam,
                soHoatDongToChucTrongNam,
                soHoatDongTungCapHoatDong
            });
        }
        [Authorize]
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/TrangCaNhan")]
        public IHttpActionResult LayThongTinHoatDongCaNhan()
        {
            var sinhVienId = User.Identity.GetSinhVienId();
            var danhSachHoatDong = _context.DanhSachHoatDong.Where(hd => !hd.DaKetThuc)
                .Select(hd => new
                {
                    hd.Id,
                    hd.AnhBia,
                    hd.TenHoatDong,
                    hd.SoLuoc,
                    hd.CapHoatDong,
                    hd.NgayBatDau,
                    hd.DiaDiem,
                    hd.BiHuy,
                    hd.DuocPheDuyet,
                    hd.IdSinhVienTaoHd,
                    donViToChuc = hd.DanhSachDonViToChuc.Select(dv => new { dv.DonViId, dv.DonVi.TenDonVi }),
                    lopToChuc = hd.DanhSachLopToChuc.Select(l => new { l.LopId, l.Lop.KyHieuTenLop }),
                    CoThamGia = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == sinhVienId && tg.DuocPheDuyet),
                    CoTheoDoi = hd.DanhSachSinhVienTheoDoi.Any(tg => tg.SinhVienId == sinhVienId),
                    CoDangKi = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == sinhVienId && !tg.DuocPheDuyet),
                    soLuotThamGia = hd.DanhSachSinhVienThamGia.Count(tg => tg.DuocPheDuyet),
                    soLuotTheoDoi = hd.DanhSachSinhVienTheoDoi.Count
                });
            var danhSachHoatDongThamGia = danhSachHoatDong.Where(hd => hd.CoThamGia && hd.DuocPheDuyet).ToList();
            var danhSachHoatDongDangKi = danhSachHoatDong.Where(hd => hd.CoDangKi && hd.DuocPheDuyet).ToList();
            var danhSachHoatDongTheoDoi = danhSachHoatDong.Where(hd => hd.CoTheoDoi
                                                                   && hd.DuocPheDuyet
                                                                   && !hd.CoThamGia
                                                                   && !hd.CoDangKi).ToList();
            var danhSachHoatDongToChuc = danhSachHoatDong.Where(hd => hd.IdSinhVienTaoHd == sinhVienId);
            return Ok(new
            {
                danhSachHoatDongThamGia,
                danhSachHoatDongDangKi,
                danhSachHoatDongTheoDoi,
                danhSachHoatDongToChuc
            });
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/HoatDongSinhVienThamGia/{sinhVienId}")]
        public IHttpActionResult LayHoatDongSinhVienThamGia(int sinhVienId)
        {
            var thongTinSinhVien = _context.SinhVien.Where(sv => sv.Id == sinhVienId)
                .Select(sv => new
                {
                    sv.Id,
                    sv.HoVaTenLot,
                    sv.Ten,
                    sv.MSSV,
                    sv.AnhDaiDien,
                    sv.LopDangHoc.KyHieuTenLop,
                    KhoaHoc = new KhoaHocDto
                    {
                        NamBatDau = sv.KhoaHoc.NamBatDau,
                        NamKetThuc = sv.KhoaHoc.NamKetThuc
                    },
                    DanhSachHoatDongThamGia = sv.DanhSachHoatDongThamGia
                        .Where(tg => tg.DuocPheDuyet)
                        .Select(tghd => new HoatDongDtoForTable
                        {
                            Id = tghd.HoatDongId,
                            TenHoatDong = tghd.HoatDong.TenHoatDong,
                            NgayBatDau = tghd.HoatDong.NgayBatDau,
                            NgayKetThuc = tghd.HoatDong.NgayKetThuc
                        }).ToList()
                }).SingleOrDefault();
            //var hoatDongThamGia = _context.DanhSachThamGiaHoatDong
            //    .Where(tghd => tghd.SinhVienId == sinhVienId && tghd.DuocPheDuyet)
            //    .Select(tghd => new HoatDongDtoForTable
            //    {
            //       Id = tghd.HoatDongId,
            //       TenHoatDong = tghd.DanhSachHoatDong.TenHoatDong,
            //       NgayBatDau = tghd.DanhSachHoatDong.NgayBatDau,
            //       NgayKetThuc = tghd.DanhSachHoatDong.NgayKetThuc
            //    })
            //    .ToList();
            return Ok(thongTinSinhVien);
            
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/ThongKeHoatDongSinhVien/{sinhVienId}/{namHocLay}")]
        public IHttpActionResult ThongKeHoatDongSinhVien(int sinhVienId,int namHocLay)
        {
            var danhSachHoatDongThamGia = _context.DanhSachThamGiaHoatDong
                .Where(tghd => tghd.SinhVienId == sinhVienId) //&& tghd.DuocPheDuyet được để ở _hoatDongRepository.LayHoatDongNam_ThamGiaHoatDongFunc
                .Where(_hoatDongRepository.LayHoatDongNam_ThamGiaHoatDongFunc(namHocLay))
                .Select(tghd => new HoatDongDtoForTable
                {
                    NgayBatDau = tghd.HoatDong.NgayBatDau,
                    NgayKetThuc = tghd.HoatDong.NgayKetThuc
                })
                .ToList();
            var soHoatDongThamGiaTungThang = _hoatDongRepository.LaySoHdTcTungThangChoHocVien(danhSachHoatDongThamGia,namHocLay);
            //Vì dùng chung function LaySoHdTcTungThangChoHocVien và sử dụng Dto SoHoatDongToChucTrongThang
            //nên số hoạt động sinh viên tham gia sẽ lưu ở biến SoHoatDongHocVienToChuc
            return Ok(soHoatDongThamGiaTungThang);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/HoatDong/LayDanhSachDonViToChuc")]
        public IHttpActionResult LayDanhSachDonViToChuc()
        {
            var danhSachLop = _context.Lop.Select(lop => new
            {
                id= lop.Id,
                text = lop.KyHieuTenLop
            }).ToList();

            var danhSachDonVi = _context.DanhSachDonVi.Select(dv => new
            {
                id =dv.Id,
                text = dv.TenDonVi
            }).ToList();

            return Ok(new
            {
                danhSachLop,
                danhSachDonVi
            });
        }

        [HttpGet]
        [Route("api/HoatDong/TrangChu")]
        public IHttpActionResult LayDanhSachHoatDongTrangChu()
        {
            int? sinhVienId = 0;
            if (User.Identity.IsAuthenticated)
            {
                sinhVienId = User.Identity.GetSinhVienId();
            }

            var danhSachHoatDong = _context.DanhSachHoatDong.Where(hd => !hd.DaKetThuc 
                                                                 && hd.DuocPheDuyet 
                                                                 && !hd.BiHuy)
                .Select(hd => new
            {
               hd.Id,
               hd.AnhBia,
               hd.TenHoatDong,
               hd.SoLuoc,
               hd.CapHoatDong,
               hd.NgayBatDau,
               hd.DiaDiem,
               donViToChuc = hd.DanhSachDonViToChuc.Select(dv => new { dv.DonViId, dv.DonVi.TenDonVi }),
               lopToChuc = hd.DanhSachLopToChuc.Select(l => new { l.LopId, l.Lop.KyHieuTenLop }),
               CoThamGia = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == sinhVienId && tg.DuocPheDuyet),
               CoTheoDoi = hd.DanhSachSinhVienTheoDoi.Any(tg => tg.SinhVienId == sinhVienId),
               CoDangKi = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == sinhVienId && !tg.DuocPheDuyet),
               soLuotThamGia = hd.DanhSachSinhVienThamGia.Count(tg => tg.DuocPheDuyet),
               soLuotTheoDoi = hd.DanhSachSinhVienTheoDoi.Count
            });
            var dsHdCapPhanVien = danhSachHoatDong.Where(hd => hd.CapHoatDong == (CapHoatDong) 4).ToList();
            var dsHdCapKhoa = danhSachHoatDong.Where(hd => hd.CapHoatDong == (CapHoatDong) 3).ToList();
            var dsHdCapLienChiHoi = danhSachHoatDong.Where(hd => hd.CapHoatDong == (CapHoatDong) 2).ToList();
            var dsHdCapChiHoi = danhSachHoatDong.Where(hd => hd.CapHoatDong == (CapHoatDong) 1).ToList();
            return Ok(new
            {
                dsHdCapPhanVien,
                dsHdCapKhoa,
                dsHdCapLienChiHoi,
                dsHdCapChiHoi
            });
        }
        
    }

    public class HuyToChucHoatDong
    {
        [Required]
        public int HoatDongId { get; set; }

        public int LopId { get; set; }

        public int DonViId { get; set; }
    }
}
