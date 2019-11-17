using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.BaiVietDtos;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.LopDtos;
using NAPASTUDENT.Repositories;

namespace NAPASTUDENT.Controllers.Api
{
    public class DonViController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly HoatDongRepository _hoatDongRepository;

        public DonViController()
        {
            _context = new ApplicationDbContext();
            _hoatDongRepository = new HoatDongRepository(_context);
        }

        [Authorize]
        [Route("api/DonVi/SaveDonVi")]
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public IHttpActionResult SaveDonVi(DonViDto donViDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var donVi = new DonVi();
            var inRoleQuanLyDonVi = User.IsInRole("Admin") || User.IsInRole("QuanLyDonVi");
            //Thêm lớp mới

            bool coTrungTen;
            if (donViDto.Id == 0)
            {
                if (!inRoleQuanLyDonVi) return BadRequest(); //Chỉ có người quản lý mới được thêm đơn vị mới
                coTrungTen = _context.DanhSachDonVi.Any(l => l.TenDonVi == donViDto.TenDonVi);
                if (coTrungTen) return BadRequest("Lỗi. Đã có đơn vị với tên này.");
                donVi.TaoDonViMoi(donViDto);
                _context.DanhSachDonVi.Add(donVi);
                _context.SaveChanges();
                return Ok();
            }
            //Thay đổi thông tin lớp
            donVi = _context.DanhSachDonVi.Include(dv => dv.ChucVuDonVi).SingleOrDefault(l => l.Id == donViDto.Id);
            if (donVi == null) return NotFound();
            //Kiểm tra xem coi có quyền không (chỉ có lớp trưởng (ChucVuId=1) và người có role Admin,QuanLyLop)
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!inRoleQuanLyDonVi && !donVi.ChucVuDonVi.Any(cvl => cvl.QuanLyThongTin && cvl.SinhVienId == userSinhVienId))
                return BadRequest();
            coTrungTen = _context.DanhSachDonVi.Any(dv => dv.TenDonVi == donViDto.TenDonVi && dv.Id != donVi.Id);
            if (coTrungTen) return BadRequest("Lỗi. Đã có đơn vị với tên này.");
            donVi.ChinhSuaThongTin(donViDto);
            _context.SaveChanges();
            return Ok();
        }        
        
        [Authorize(Roles = "Admin,QuanLyDonVi")]
        [Route("api/DonVi/XoaDonVi/{donViId}")]
        [ApiValidateAntiForgeryToken]
        [HttpDelete]
        public IHttpActionResult XoaDonVi(int donViId)
        {
            var donVi = _context.DanhSachDonVi
                .Include(dv => dv.ChucVuDonVi)
                .Include(dv => dv.DanhSachThanhVienDonVi)
                .Include(dv => dv.DanhSachBaiVietDonVi)
                .Include(dv => dv.DanhSachHoatDongToChuc)
                .SingleOrDefault(l => l.Id == donViId);
            if (donVi == null) return NotFound();
            donVi.XoaDonVi();
            _context.DanhSachDonVi.Remove(donVi);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ThemThanhVien")]
        public IHttpActionResult ThemThanhVien(ThemXoaThanhVienDonViDto themThanhVienDto)
        {

            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyThanhVien = _context.DanhSachChucVuDonVi.Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                                   && cvdv.DonViId == themThanhVienDto.DonViId
                                                                   && (cvdv.QuanLyThanhVien || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyThanhVien && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();
            //Tìm sinh viên
            var sinhVien = _context.SinhVien
                                   .Include(sv => sv.DanhSachDonViThamGia)
                                   .SingleOrDefault(sv => sv.Id == themThanhVienDto.SinhVienId);
            if (sinhVien == null) return NotFound();
            //Tìm xem có là thành viên không
            var thanhVien = sinhVien.DanhSachDonViThamGia.SingleOrDefault(tv => tv.DonViId == themThanhVienDto.DonViId);
            if (thanhVien != null)
            {
                //Nếu đã là thành viên được phê duyệt
                if (thanhVien.DuocPheDuyet) return BadRequest("Sinh viên này đã có trong danh sách thành viên đơn vị.");
                //Nếu chưa được phê duyệt thì phê duyệt
                thanhVien.PheDuyetDangKi();
            }
            else
            {
                //Nếu sinh viên chưa đăng kí thì thêm vào
                thanhVien = new ThanhVienDonVi();
                thanhVien.ThemThanhVien(themThanhVienDto);
                sinhVien.DanhSachDonViThamGia.Add(thanhVien);
            }
            _context.SaveChanges();
            return Ok();
        }

        //Đăng kí làm thành viên đơn vị
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/DangKiThanhVien")]
        public IHttpActionResult DangKiThanhVien(DangKiThanhVienDto dangKiDto)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var thanhVien = _context.DanhSachThanhVienDonVi.SingleOrDefault(tv => tv.DonViId == dangKiDto.DonViId
                                                                          && tv.SinhVienId == userSinhVienId);
            if (thanhVien != null)
                return BadRequest("Đã đăng kí/là thành viên của đơn vị.");
            thanhVien = new ThanhVienDonVi();
            thanhVien.DangKiThanhVien(dangKiDto, userSinhVienId);
            _context.DanhSachThanhVienDonVi.Add(thanhVien);
            _context.SaveChanges();
            return Ok();
        }
        //Hủy đăng kí thành viên (do chính sinh viên đăng kí tự hủy)
        //Còn xóa đăng kí thành viên (do quản lý sử dụng) thì dùng chung API với XoaThanhVien
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/HuyDangKiThanhVien/{donViId}")]
        public IHttpActionResult HuyDangKiThanhVien(int donViId)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            var thanhVien = _context.DanhSachThanhVienDonVi.SingleOrDefault(tv => tv.DonViId == donViId
                                                                                  && tv.SinhVienId == userSinhVienId);
            if (thanhVien == null) return NotFound();
            if (thanhVien.DuocPheDuyet) return BadRequest("Đã là thành viên chính thức của đơn vị.");
            _context.DanhSachThanhVienDonVi.Remove(thanhVien);
            _context.SaveChanges();
            return Ok();
        }

        //Phê duyệt đăng kí thành viên
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/PheDuyetDangKiThanhVien")]
        public IHttpActionResult PheDuyetDangKiThanhVien(ThemXoaThanhVienDonViDto pheDuyetDto)
        {
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyThanhVien = _context.DanhSachChucVuDonVi
                                                 .Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                           && cvdv.DonViId == pheDuyetDto.DonViId
                                                           && (cvdv.QuanLyThanhVien || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyThanhVien && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();
            //Tìm thành viên
            var thanhVien = _context.DanhSachThanhVienDonVi
                                    .SingleOrDefault(tv => tv.SinhVienId == pheDuyetDto.SinhVienId
                                                           && tv.DonViId == pheDuyetDto.DonViId
                                                           && !tv.DuocPheDuyet);
            if (thanhVien == null) return NotFound();

            thanhVien.PheDuyetDangKi();
            _context.SaveChanges();
            return Ok();
        }

        //Thay đổi thông tin thành viên
        [Authorize]
        [HttpPut]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ThayDoiThanhVien")]
        public IHttpActionResult ThayDoiThanhVien(ThayDoiThanhVienDto thayDoiDto)
        {
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyThanhVien = _context.DanhSachChucVuDonVi
                                                 .Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                           && cvdv.DonViId == thayDoiDto.DonViId
                                                           && (cvdv.QuanLyThanhVien || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyThanhVien && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();

            //Tìm thành viên
            var thanhVien = _context.DanhSachThanhVienDonVi
                .SingleOrDefault(tv => tv.SinhVienId == thayDoiDto.SinhVienId
                                       && tv.DonViId == thayDoiDto.DonViId
                                       && tv.DuocPheDuyet);
            if (thanhVien == null) return NotFound();

            thanhVien.ThayDoi(thayDoiDto);
            _context.SaveChanges();
            return Ok();
        }        
        
        //Xóa thành viên (xóa hẳn luôn á)
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/XoaThanhVien")]
        public IHttpActionResult XoaThanhVien(ThemXoaThanhVienDonViDto xoaThanhVienDto)
        {
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyThanhVien = _context.DanhSachChucVuDonVi.Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                                       && cvdv.DonViId == xoaThanhVienDto.DonViId
                                                                      && (cvdv.QuanLyThanhVien || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyThanhVien && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();
            //Tìm thành viên
            var thanhVien = _context.DanhSachThanhVienDonVi
                                    .Include(tv => tv.DanhSachChucVuDonVi)
                                    .SingleOrDefault(tv => tv.SinhVienId == xoaThanhVienDto.SinhVienId
                                                          && tv.DonViId == xoaThanhVienDto.DonViId);
            if (thanhVien == null) return NotFound();
            //Nếu có chức vụ thì không thể xóa
            if (thanhVien.DanhSachChucVuDonVi.Count > 0) 
                return BadRequest("Hãy bỏ tất cả các chức vụ của sinh viên này trước khi xóa khỏi danh sách thành viên ban");
            //Xóa thành viên này
            _context.DanhSachThanhVienDonVi.Remove(thanhVien);
            _context.SaveChanges();
            return Ok();
        }

        //Tốt nghiệp thành viên
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/TotNghiepThanhVien")]
        public IHttpActionResult TotNghiepThanhVien(TotNghiepThanhVienDto totNghiepThanhVienDto)
        {
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyThanhVien = _context.DanhSachChucVuDonVi.Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                                      && cvdv.DonViId == totNghiepThanhVienDto.DonViId
                                                                      && (cvdv.QuanLyThanhVien || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyThanhVien && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();


            //Tìm thành viên
            var thanhVien = _context.DanhSachThanhVienDonVi
                .Include(tv => tv.DanhSachChucVuDonVi)
                .SingleOrDefault(tv => tv.SinhVienId == totNghiepThanhVienDto.SinhVienId
                                       && tv.DonViId == totNghiepThanhVienDto.DonViId
                                       && tv.DuocPheDuyet);
            if (thanhVien == null) return NotFound();
            //Nếu có chức vụ thì không thể xóa
            if (thanhVien.DanhSachChucVuDonVi.Count > 0)
                return BadRequest("Hãy bỏ tất cả các chức vụ của sinh viên này trước khi tốt nghiệp");
            thanhVien.SetTotNghiep(totNghiepThanhVienDto);
            _context.SaveChanges();
            return Ok();
        }

        //Lưu chức vụ
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/SaveChucVu")]
        public IHttpActionResult SaveChucVu(ChucVuDonViDto chucVuDonViDto)
        {
            //Nếu không phải thành viên chính thức của đơn vị thì không được giữ chức vụ
            var thanhVien = _context.DanhSachThanhVienDonVi
                .Include(tv => tv.DanhSachChucVuDonVi)
                .SingleOrDefault(tv => tv.SinhVienId == chucVuDonViDto.SinhVienId
                                    && tv.DonViId == chucVuDonViDto.DonViId
                                    && tv.DuocPheDuyet);
            if (thanhVien == null) return BadRequest("Chỉ thành viên chính thức của đơn vị mới được giữ chức vụ.");

            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyChucVu = _context.DanhSachChucVuDonVi.Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                                && cvdv.DonViId == chucVuDonViDto.DonViId
                                                                && (cvdv.QuanLyChucVu || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyChucVu && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();
            //Nếu sinh viên đã giữ chức vụ này (trùng SinhVienId, ChucVuId) nghĩa là muốn sửa thông tin chức vụ
            var chucVu = thanhVien.DanhSachChucVuDonVi.SingleOrDefault(cvl => cvl.ChucVuId == chucVuDonViDto.ChucVuId);
            //Tạo chức vụ mới
            if (chucVu == null)
            {
                chucVu = new ChucVuDonVi();
                chucVu.TaoMoiChucVu(chucVuDonViDto);
                thanhVien.DanhSachChucVuDonVi.Add(chucVu);
            }
            //Chỉnh sửa thông tin chức vụ
            else
            {
                chucVu.ThayDoiChucVu(chucVuDonViDto);
            }
            _context.SaveChanges();
            return Ok();
        }

        //Xóa chức vụ
        [Authorize]
        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/XoaChucVu")]
        public IHttpActionResult XoaChucVu(ChucVuDonViDto chucVuDonViDto)
        {
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyChucVu = _context.DanhSachChucVuDonVi.Any(cvdv => cvdv.SinhVienId == userSinhVienId
                                                                      && cvdv.DonViId == chucVuDonViDto.DonViId
                                                                      && (cvdv.QuanLyChucVu || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyChucVu && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();

            //Nếu chức vụ là Trưởng ban (Chức vụ Id = 10) thì phải có ít nhất 1 trưởng ban khác trong danh sách chức vụ
            if (chucVuDonViDto.ChucVuId == 10)
            {
                var chucVuTruongBan = _context.DanhSachChucVuDonVi.Where(cv => cv.DonViId == chucVuDonViDto.DonViId
                                                                       && cv.ChucVuId == 10);
                if (chucVuTruongBan.Count() <= 1)
                    return BadRequest("Không thể xóa chức vụ trưởng ban duy nhất.");
                var chucVuDonVi = chucVuTruongBan.SingleOrDefault(cv => cv.SinhVienId == chucVuDonViDto.SinhVienId);
                if (chucVuDonVi == null) return NotFound();
                _context.DanhSachChucVuDonVi.Remove(chucVuDonVi);
            }
            //Nếu là chức vụ khác thì cứ xóa
            else
            {
                //Tìm chức vụ
                var chucVuDonVi = _context.DanhSachChucVuDonVi.SingleOrDefault(cv => cv.DonViId == chucVuDonViDto.DonViId
                                                                             && cv.SinhVienId == chucVuDonViDto.SinhVienId
                                                                             && cv.ChucVuId == chucVuDonViDto.ChucVuId);
                if (chucVuDonVi == null) return NotFound();
                _context.DanhSachChucVuDonVi.Remove(chucVuDonVi);
            }
            _context.SaveChanges();
            return Ok();
        }

        //Lấy danh sách đơn vị
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/DanhSachDonVi")]
        public IHttpActionResult LayDanhSachDonVi()
        {
            var danhSachDonVi = _context.DanhSachDonVi.Select(dv => new
            {
                DonViId=dv.Id,
                dv.AnhBia,
                dv.TenDonVi,
                dv.LoaiDonVi,
                dv.NgayThanhLap,
                SoThanhVien = dv.DanhSachThanhVienDonVi.Count(tv => tv.DuocPheDuyet && !tv.NgungThamGia) 
            });
            return Ok(danhSachDonVi);
        }

        //Lấy thông tin đơn vị
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ThongTin/{donViId}")]
        public IHttpActionResult ThongTinDonVi(int donViId)
        {
            var donViDto = _context.DanhSachDonVi.Where(dv => dv.Id == donViId).Select(dv => new
            {
                dv.Id,
                dv.AnhBia,
                dv.TenDonVi,
                dv.NgayThanhLap,
                dv.LoaiDonVi,
                dv.GioiThieu
            }).SingleOrDefault();
            return Ok(donViDto);
        }

        //Lấy danh sách thành viên đơn vị
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ThanhVien/{donViId}")]
        public IHttpActionResult LayDanhSachThanhVienDonVi(int donViId)
        {
            var danhSachThanhVien = _context.DanhSachThanhVienDonVi
                .Where(tv => tv.DonViId == donViId && tv.DuocPheDuyet && !tv.NgungThamGia)
                .Select(tv => new
                {
                    tv.NgayGiaNhap,
                    tv.SinhVien.AnhDaiDien,
                    tv.SinhVien.Id,
                    tv.SinhVien.HoVaTenLot,
                    tv.SinhVien.Ten,
                    tv.SinhVien.MSSV
                });
            return Ok(danhSachThanhVien);
        }

        //Lấy danh sách cựu thành viên đơn vị
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/CuuThanhVien/{donViId}")]
        public IHttpActionResult LayDanhSachCuuThanhVienDonVi(int donViId)
        {
            var danhSachCuuThanhVien = _context.DanhSachThanhVienDonVi
                .Where(tv => tv.DonViId == donViId && tv.DuocPheDuyet && tv.NgungThamGia)
                .Select(tv => new
                {
                    tv.NgayGiaNhap,
                    tv.NgayRoi,
                    tv.SinhVien.AnhDaiDien,
                    tv.SinhVien.Id,
                    tv.SinhVien.HoVaTenLot,
                    tv.SinhVien.Ten,
                    tv.SinhVien.MSSV,
                    tv.GhiChu
                });
            return Ok(danhSachCuuThanhVien);
        }

        //Lấy danh sách chức vụ đơn vị
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ChucVu/{donViId}")]
        public IHttpActionResult LayDanhSachChucVu(int donViId)
        {
            var danhSachChucVu = _context.DanhSachChucVuDonVi.Where(cv => cv.DonViId == donViId).Select(cv => new
            {
                cv.SinhVien.AnhDaiDien,
                cv.SinhVien.Id,
                cv.SinhVien.HoVaTenLot,
                cv.SinhVien.Ten,
                cv.SinhVien.MSSV,
                cv.TenChucVu
            });
            return Ok(danhSachChucVu);
        }

        //Lấy danh sách chức vụ đơn vị nhưng dành cho quản lý (có kèm theo quyền của từng chức vụ)
        [HttpGet]
        [Authorize]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ChucVuQuanLy/{donViId}")]
        public IHttpActionResult LayDanhSachChucVu_QuanLy(int donViId)
        {
            
            var danhSachChucVu = _context.DanhSachChucVuDonVi.Where(cv => cv.DonViId == donViId).Select(cv => new
            {
                cv.SinhVien.AnhDaiDien,
                cv.SinhVien.Id,
                cv.SinhVien.HoVaTenLot,
                cv.SinhVien.Ten,
                cv.SinhVien.MSSV,
                cv.TenChucVu,
                cv.ChucVuId,
                cv.QuanLyChucVu,
                cv.QuanLyHoatDong,
                cv.QuanLyThanhVien,
                cv.QuanLyThongTin
            });
            //Check quyền
            var userSinhVienId = User.Identity.GetSinhVienId();
            var coQuyenQuanLyThanhVien = danhSachChucVu.Any(cvdv => cvdv.Id == userSinhVienId
                                                                && (cvdv.QuanLyChucVu || cvdv.ChucVuId == 10));
            if (!coQuyenQuanLyThanhVien && !User.IsInRole("Admin") && !User.IsInRole("QuanLyDonVi"))
                return BadRequest();
            //Trả kết quả
            return Ok(danhSachChucVu);
        }

        //Lấy danh sách đăng kí làm thành viên đơn vị
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/DanhSachDangKi/{donViId}")]
        public IHttpActionResult LayDanhSachDangKiThanhVien(int donViId)
        {
            var danhSachThanhVien = _context.DanhSachThanhVienDonVi
                .Where(tv => tv.DonViId == donViId && !tv.DuocPheDuyet && !tv.NgungThamGia)
                .Select(tv => new
                {
                    tv.NgayGiaNhap,
                    tv.SinhVien.AnhDaiDien,
                    tv.SinhVien.Id,
                    tv.SinhVien.HoVaTenLot,
                    tv.SinhVien.Ten,
                    tv.SinhVien.MSSV,
                    tv.GhiChu
                });
            return Ok(danhSachThanhVien);
        }

        //Bài viết đơn vị sẽ được lấy ở BaiVietController

        //Lấy hoạt động đơn vị đang tổ chức
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/HoatDongDangToChuc/{donViId}")]
        public IHttpActionResult HoatDongDangToChuc(int donViId)
        {
            var userSinhVienId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userSinhVienId = User.Identity.GetSinhVienId();
            }
            var danhSachHoatDong = _context.DanhSachHoatDong
                .Where(hd => hd.DuocPheDuyet && !hd.DaKetThuc && !hd.BiHuy && hd.DanhSachDonViToChuc.Any(dv => dv.DonViId == donViId))
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
                    donViToChuc = hd.DanhSachDonViToChuc.Select(dv => new { dv.DonViId, dv.DonVi.TenDonVi }),
                    lopToChuc = hd.DanhSachLopToChuc.Select(l => new { l.LopId, l.Lop.KyHieuTenLop }),
                    CoThamGia = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == userSinhVienId && tg.DuocPheDuyet),
                    CoTheoDoi = hd.DanhSachSinhVienTheoDoi.Any(tg => tg.SinhVienId == userSinhVienId),
                    CoDangKi = hd.DanhSachSinhVienThamGia.Any(tg => tg.SinhVienId == userSinhVienId && !tg.DuocPheDuyet),
                    soLuotThamGia = hd.DanhSachSinhVienThamGia.Count(tg => tg.DuocPheDuyet),
                    soLuotTheoDoi = hd.DanhSachSinhVienTheoDoi.Count
                });

            return Ok(danhSachHoatDong);
        }

        //Lấy hoạt động đơn vị tổ chức từ trước tới giờ
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/HoatDongToChuc/{donViId}")]
        public IHttpActionResult LayHoatDongDonViToChuc(int donViId,PagingSearchingSorting pagingSearchingSorting)
        {
            //Tên cột để sort cho sql server
            // @ColumnName = 'SoLuotXem' THEN DanhSachTamCTE.SoLuotXem 
            // @ColumnName = 'NgayTao' THEN DanhSachTamCTE.NgayTao
            if (!ModelState.IsValid) return BadRequest();
            List<HoatDongResultSetForDataTable> hoatDongResultSet;
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var donViIdParameter = new SqlParameter("@DonViId", donViId);
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                hoatDongResultSet = _context.Database
                    .SqlQuery<HoatDongResultSetForDataTable>("LayHoatDongDonViASC @DonViId, @SearchTerm, @ColumnName, @RecordStart, @PageSize",
                        donViIdParameter,searchTerm, columnName, recordStart, pageSize).ToList();
            }
            else
            {
                hoatDongResultSet = _context.Database
                    .SqlQuery<HoatDongResultSetForDataTable>("LayHoatDongDonViDESC @DonViId, @SearchTerm, @ColumnName, @RecordStart, @PageSize",
                        donViIdParameter, searchTerm, columnName, recordStart, pageSize).ToList();
            }

            int recordsTotal, recordsFiltered;
            if (hoatDongResultSet.Count > 0)
            {
                recordsTotal = hoatDongResultSet[0].TotalCount;
                recordsFiltered = hoatDongResultSet[0].FilteredCount;
            }
            else
            {
                recordsTotal = _context.DanhSachHoatDongDonVi.Count(hd => hd.DonViId == donViId);
                recordsFiltered = 0;
            }
            //Tính số trang dựa trên pageIndex và pageSize
            var pageNumbers = (int)Math.Ceiling((double)recordsTotal / pagingSearchingSorting.PageSize);
            return Ok(new
            {
                pagingSearchingSorting.Draw,
                data = hoatDongResultSet,
                pageNumbers,
                pagingSearchingSorting.PageIndex,
                recordsTotal,
                recordsFiltered
            });
        }

        //Lấy thống kê hoạt động đơn vị tổ chức theo năm
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/ThongKeHoatDong/{donViId}/{namHocLay}")]
        public IHttpActionResult ThongKeHoatDong(int donViId, int namHocLay)
        {

            //^ Mốc thời gian để dừng lấy  số lượt tham gia hoạt động
            //Nếu lấy số lượt tham gia hoạt động của một năm học cụ thể thì đó là từ 8/1/năm học đó - 31/7/năm học sau

            //var hoatDongHocVienQuery = _context.DanhSachHoatDong
            //                                    .Where(_hoatDongRepository.LayHoatDongNam_HoatDongFunc(namHocLay))
            //                                    .Where(hd => hd.DuocPheDuyet && !hd.BiHuy) //Chỉ lấy hoạt động được phê duyệt và không bị hủy
            //                                    .Select(hd => new
            //                                    {
            //                                        hd.Id,
            //                                        hd.NgayBatDau,
            //                                        hd.NgayKetThuc,
            //                                        DanhSachDonViToChuc = hd.DanhSachDonViToChuc.Select(dv => dv.DonViId).ToList()
            //                                    }).ToList();

            //var hoatDongHocVienToChuc = hoatDongHocVienQuery
            //    .Select(ob => new HoatDongDtoForTable
            //            {
            //                Id = ob.Id,
            //                NgayBatDau = ob.NgayBatDau,
            //                NgayKetThuc = ob.NgayKetThuc,
            //            })
            //    .ToList();
            //var danhSachHoatDongDonVi = hoatDongHocVienQuery.Where(ob => ob.DanhSachDonViToChuc.Contains(donViId))
            //                                               .Select(ob => new HoatDongDtoForTable
            //                                                       {
            //                                                           Id = ob.Id,
            //                                                           NgayBatDau = ob.NgayBatDau,
            //                                                           NgayKetThuc = ob.NgayKetThuc,
            //                                                       })
            //                                               .ToList();
            //Lấy số hoạt động tổ chức theo từng tháng
            var donViIdSqlParameter = new SqlParameter("@DonViId", donViId);
            var namHocLaySqlParameter = new SqlParameter("@NamHocLay", namHocLay);

            List<HoatDongToChucDto> danhSachHoatDongToChucResultSet;
            danhSachHoatDongToChucResultSet = _context.Database
                .SqlQuery<HoatDongToChucDto>("LayHoatDongHocVienVaDonViToChuc @DonViId, @NamHocLay",
                    donViIdSqlParameter, namHocLaySqlParameter).ToList();
            var soHoatDongToChucTungThang = _hoatDongRepository.LaySoHdTcTungThangChoLop_DonVi(danhSachHoatDongToChucResultSet, namHocLay);



            return Ok(soHoatDongToChucTungThang);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/DonVi/HoatDongChoPheDuyet/{donViId}")]
        public IHttpActionResult HoatDongChoPheDuyet(int donViId)
        {
            var danhSachHoatDongChoPheDuyet = _context.DanhSachHoatDong
                .Where(hd => !hd.DuocPheDuyet && hd.DanhSachDonViToChuc.Any(dv => dv.DonViId == donViId))
                .Select(hd => new
                {
                    hd.NgayBatDau,
                    hd.NgayKetThuc,
                    hd.TenHoatDong,
                    hd.Id,
                    DanhSachDonViToChuc = hd.DanhSachDonViToChuc.Select(dv => dv.DonVi.TenDonVi).ToList(),
                    DanhSachLopToChuc = hd.DanhSachLopToChuc.Select(l => l.Lop.TenLop).ToList(),
                    hd.SinhVienTaoHd.Ten,
                    hd.SinhVienTaoHd.HoVaTenLot
                }).ToList();
            return Ok(danhSachHoatDongChoPheDuyet);
        }


        //Đơn vị của tôi được lấy ở SinhVienController
    }

    public class ThayDoiThanhVienDto
    {
        [Required]
        public int SinhVienId { get; set; }
        [Required]
        public int DonViId { get; set; }
        [Required]
        public DateTime NgayGiaNhap { get; set; }
    }

    public class DangKiThanhVienDto
    {
        [Required]
        public int DonViId { get; set; }
        [Required]
        public string GioiThieu { get; set; }
    }

    public class ChucVuDonViDto
    {
        [Required]
        public int SinhVienId { get; set; }
        [Required]
        public int DonViId { get; set; }
        [Required]
        public int ChucVuId { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenChucVu { get; set; }
        [Required]
        public bool QuanLyThongTin { get; set; }
        [Required]
        public bool QuanLyThanhVien { get; set; }
        [Required]
        public bool QuanLyChucVu { get; set; }
        [Required]
        public bool QuanLyHoatDong { get; set; }
    }

    public class TotNghiepThanhVienDto
    {
        [Required]
        public int SinhVienId { get; set; }
        [Required]
        public int DonViId { get; set; }
        [Required]
        public DateTime NgayGiaNhap { get; set; }
        [Required]
        public DateTime NgayRoi { get; set; }
        public string GhiChu { get; set; }

    }

    public class ThemXoaThanhVienDonViDto
    {
        [Required]
        public int SinhVienId { get; set; }
        [Required]
        public int DonViId { get; set; }
    }

    public class DonViDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenDonVi { get; set; }
        [Required]
        public DateTime NgayThanhLap { get; set; }
        [Required]
        public string GioiThieu { get; set; }
        [Required]
        public LoaiDonVi LoaiDonVi { get; set; }
        [Required]
        public string AnhBia { get; set; }
    }
}
