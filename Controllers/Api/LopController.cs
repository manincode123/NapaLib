using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.LopDtos;
using NAPASTUDENT.Models.DTOs.SinhVienDto;
using NAPASTUDENT.Repositories;

namespace NAPASTUDENT.Controllers.Api
{
    public class LopController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly HoatDongRepository _hoatDongRepository;

        public LopController()
        {
            _context = new ApplicationDbContext();
            _hoatDongRepository = new HoatDongRepository(_context);
        }
        /*
         * Quy trình để tạo lớp hoàn chỉnh:
         * Tạo lớp tại trang quản lý chung  (SaveLop)
         * Thêm danh sách sinh viên (sinh viên đã được tạo sẵn - xem SinhVienController) vào lớp tại trang quản lý sinh viên lớp
         * Quản lý chức vụ lớp (ChinhSuaChucVu)
         */
        [HttpGet]
        [Route("api/Lop/SelectList")]
        public IHttpActionResult LayDanhSachLopSelectList()
        {
            var lopSelectList = _context.Lop.Where(l => l.DaTotNghiep == false).Select(l => new
            {
                id = l.Id,
                text = l.KyHieuTenLop
            });
            return Ok(lopSelectList);
        }

        [Authorize(Roles = "Admin, QuanLyLop")]
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/QuanLyChung")]      
        public IHttpActionResult QuanLyChungLop()
        {
            var danhSachLop = _context.Lop.Select(lop => new
            {
                lop.Id,
                lop.TenLop,
                lop.KyHieuTenLop,
                lop.KhoaHocId,
                lop.LopChuyenNganh,
                tenKhoa = lop.KhoaHoc.TenKhoa,
                soLuongSinhVien=lop.DanhSachSinhVien.Count,
                soLuongDoanVien = lop.DanhSachSinhVien.Count(sv => sv.SinhVien.DoanVien != null),
                soLuongHoiVien = lop.DanhSachSinhVien.Count(sv => sv.SinhVien.HoiVien != null)
            });

            return Ok(danhSachLop);
        }


        [HttpGet]
        [Route("api/Lop/ChiTiet/{lopId}")]
        public IHttpActionResult ChiTietLop(int lopId)
        {
            var danhSachSinhVienLop = _context.DanhSachSinhVienLop
                .Where(svl => svl.LopId == lopId)
                .Select(svl => new SinhVienDtoForTable
                {
                    AnhDaiDien = svl.SinhVien.AnhDaiDien,
                    GioiTinh = svl.SinhVien.GioiTinh.TenGioiTinh,
                    HoVaTenLot = svl.SinhVien.HoVaTenLot,
                    Ten = svl.SinhVien.Ten,
                    MSSV = svl.SinhVien.MSSV,
                    NgaySinh = svl.SinhVien.NgaySinh,
                    Id = svl.SinhVienId
                })
                .ToList();
            var lopDto = new LopDto();
            lopDto.DanhSachSinhVien = danhSachSinhVienLop;
            MapSoLieuSinhVienLop(lopDto);
            return Ok(lopDto);
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/LayDanhSachSinhVien/{lopId}")]
        public IHttpActionResult LayDanhSachSinhVien(int lopId)
        {
            var danhSachSinhVienLop = _context.DanhSachSinhVienLop
                .Where(svl => svl.LopId == lopId)
                .Select(svl => new SinhVienDtoForTable
                {
                    AnhDaiDien = svl.SinhVien.AnhDaiDien,
                    GioiTinh = svl.SinhVien.GioiTinh.TenGioiTinh,
                    HoVaTenLot = svl.SinhVien.HoVaTenLot,
                    Ten = svl.SinhVien.Ten,
                    MSSV = svl.SinhVien.MSSV,
                    NgaySinh = svl.SinhVien.NgaySinh,
                    Id = svl.SinhVienId
                })
                .ToList();
            return Ok(danhSachSinhVienLop);
        }   

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/LayDanhSachChucVu/{lopId}")]
        public IHttpActionResult LayDanhSachChucVu(int lopId)
        {
            var danhSachChucVu = _context.ChucVuLop
                .Where(cvl => cvl.LopId == lopId)
                .Select(cvl => new ChucVuLopDto
                {
                    SinhVien = new TTSinhVienCBNhatDto
                    {
                        AnhDaiDien = cvl.SinhVien.AnhDaiDien,
                        HoVaTenLot = cvl.SinhVien.HoVaTenLot,
                        Id = cvl.SinhVien.Id,
                        MSSV = cvl.SinhVien.MSSV,
                        Ten = cvl.SinhVien.Ten,
                    },
                    ChucVu = cvl.ChucVu.TenChucVu,
                    ChucVuId = cvl.ChucVuId
                });
            return Ok(danhSachChucVu);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/GetLopDataForSave/{lopId}")]
        public IHttpActionResult GetLopDataForSave(int lopId)
        {
            var dataLop = _context.Lop.SingleOrDefault(l => l.Id == lopId);
            if (dataLop == null) return NotFound();
            return Ok(Mapper.Map<Lop,SaveLopDto>(dataLop));
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/SaveLop")]
        public IHttpActionResult SaveLop(SaveLopDto lopDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            Lop lop;
            var inRoleQuanLyLop = User.IsInRole("Admin") || User.IsInRole("QuanLyLop");
            //Thêm lớp mới

            bool coTrungTen;
            if (lopDto.Id == 0)
            {
                if (!inRoleQuanLyLop) return BadRequest(); //Chỉ có người quản lý mới được thêm lớp mới
                coTrungTen = _context.Lop.Any(l => l.TenLop == lopDto.TenLop || l.KyHieuTenLop == lopDto.KyHieuTenLop);
                if (coTrungTen) return BadRequest("Lỗi. Đã có lớp với tên và ký hiệu này.");
                lop = Mapper.Map<SaveLopDto, Lop>(lopDto);
                _context.Lop.Add(lop);
                _context.SaveChanges();
                return Ok();
            }
            //Thay đổi thông tin lớp
            lop = _context.Lop.Include(l => l.ChucVuLop).SingleOrDefault(l => l.Id == lopDto.Id);
            if (lop == null) return NotFound();
            //Kiểm tra xem coi có quyền không (chỉ có lớp trưởng (ChucVuId=1) và người có role Admin,QuanLyLop)
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!inRoleQuanLyLop && !lop.ChucVuLop.Any(cvl => cvl.ChucVuId == 1 && cvl.SinhVienId == userSinhVienId)) 
                return BadRequest();
            coTrungTen = _context.Lop.Any(l => l.Id != lopDto.Id  &&
                                               (l.TenLop == lopDto.TenLop || l.KyHieuTenLop == lopDto.KyHieuTenLop));
            if (coTrungTen) return BadRequest("Lỗi. Đã có lớp với tên và ký hiệu này.");
            Mapper.Map(lopDto,lop);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/ThemSinhVien")]
        public IHttpActionResult ThemSinhVien(ThemXoaSinhVienDto themSinhVienDto)
        {
            //Check quyền
            IHttpActionResult status;
            if (CheckQuyenQuanLySinhVienLop(themSinhVienDto, out status)) return status;
            //Tìm sinh viên
            var sinhVien = _context.SinhVien.Include(sv => sv.DanhSachLop)
                                   .SingleOrDefault(sv => sv.Id == themSinhVienDto.SinhVienId);
            if (sinhVien == null) return NotFound();
            if (sinhVien.DanhSachLop.Any(svl => svl.LopId == themSinhVienDto.LopId))
                return BadRequest("Sinh viên này đã có trong danh sách lớp.");
            //Để tính trường hợp chuyển lớp (một sinh viên có thể có nhiều lớp), nên mình đã comment(bỏ) phần dưới
            //if (sinhVien.DanhSachLop.Any(svl => svl.LopChuyenNganh == themSinhVienDto.LopChuyenNganh))
            //    return BadRequest("Sinh viên này đang thuộc một lớp khác.");
            var sinhVienLop = new SinhVienLop
            {
                LopChuyenNganh = themSinhVienDto.LopChuyenNganh,
                LopId = themSinhVienDto.LopId
            };
            sinhVien.DanhSachLop.Add(sinhVienLop);
            sinhVien.SetLopDangHoc(themSinhVienDto.LopId);
            _context.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/XoaSinhVien")]
        public IHttpActionResult XoaSinhVien(ThemXoaSinhVienDto xoaSinhVienDto)
        {
            //Check quyền
            IHttpActionResult status;
            if (CheckQuyenQuanLySinhVienLop(xoaSinhVienDto, out status)) return status;
            //Tìm sinh viên
            var sinhVien = _context.SinhVien.Include(sv => sv.DanhSachLop).Include(sv => sv.ChucVuLop)
                                   .SingleOrDefault(sv => sv.Id == xoaSinhVienDto.SinhVienId);
            if (sinhVien == null) return NotFound();
            //Xóa tất cả các chức vụ sinh viên đang giữ tại lớp này
            var chucVuSinhVienGiu = sinhVien.ChucVuLop.Where(cvl => cvl.LopId == xoaSinhVienDto.LopId);
            _context.ChucVuLop.RemoveRange(chucVuSinhVienGiu);
            //Xóa lớp này khỏi danh sách lớp của sinh viên
            var lop = sinhVien.DanhSachLop.SingleOrDefault(svl => svl.LopId == xoaSinhVienDto.LopId);
            if (lop == null) return NotFound();
            sinhVien.DanhSachLop.Remove(lop);
            //Reset lại lớp sinh viên đang học
            sinhVien.ResetLopDangHoc();
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/XoaChucVu")]
        public IHttpActionResult XoaChucVu(ChinhSuaChucVuLopDto chucVuLopDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Lấy chức vụ lớp
            var lop = _context.Lop.Include(l => l.ChucVuLop).SingleOrDefault(l => l.Id == chucVuLopDto.LopId);
            if (lop == null) return NotFound();
            //Kiểm tra xem coi có quyền không
            IHttpActionResult httpActionResult;
            if (CheckQuyenChinhSuaChucVu(chucVuLopDto, lop, out httpActionResult)) return httpActionResult;
            //Lấy chức vụ muốn xóa
            var chucVu = lop.ChucVuLop.SingleOrDefault(cvl =>
                cvl.ChucVuId == chucVuLopDto.ChucVuId && cvl.SinhVienId == chucVuLopDto.IdSinhVienGiuChucVuCu);
            if (chucVu == null) return NotFound();
            lop.ChucVuLop.Remove(chucVu);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/ChinhSuaChucVu")]
        public IHttpActionResult ChinhSuaChucVu(ChinhSuaChucVuLopDto chucVuLopDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Lấy chức vụ lớp
            var lop = _context.Lop.Include(l => l.ChucVuLop).Include(l => l.DanhSachSinhVien)
                .SingleOrDefault(l => l.Id == chucVuLopDto.LopId);
            if (lop == null) return NotFound();
            //Kiểm tra xem coi có quyền không
            IHttpActionResult httpActionResult;
            if (CheckQuyenChinhSuaChucVu(chucVuLopDto, lop, out httpActionResult)) return httpActionResult;
            //Nếu không phải sinh viên lớp thì không được giữ chức vụ
            var sinhVien = lop.DanhSachSinhVien.Any(svl => svl.SinhVienId == chucVuLopDto.IdSinhVienGiuChucVuMoi);
            if (!sinhVien) return BadRequest();
            //Xóa chức vụ cũ (nếu có)
            var chucVuCu = lop.ChucVuLop.SingleOrDefault(cvl => cvl.ChucVuId == chucVuLopDto.ChucVuId 
                                                             && cvl.SinhVienId == chucVuLopDto.IdSinhVienGiuChucVuCu);
            if (chucVuCu != null) lop.ChucVuLop.Remove(chucVuCu);
            //Đối với chức vụ BCH Chi đoàn, chi hội (id =6,9) chỉ có 3 người được giữ chức vụ, các chức vụ còn lại chỉ 1 người
            if (chucVuLopDto.ChucVuId == 6 || chucVuLopDto.ChucVuId == 9)
            {
                if (lop.ChucVuLop.Count(cvl => cvl.ChucVuId == chucVuLopDto.ChucVuId) >= 3)
                    return BadRequest();
            }
            else
            {
                if (lop.ChucVuLop.Count(cvl => cvl.ChucVuId == chucVuLopDto.ChucVuId) != 0)
                    return BadRequest();
            }
            //Thêm chức vụ mới
            lop.ChucVuLop.Add(new ChucVuLop
            {
                ChucVuId = chucVuLopDto.ChucVuId,
                SinhVienId = chucVuLopDto.IdSinhVienGiuChucVuMoi
            });
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/Lop/HoatDongDangToChuc/{lopId}")]
        public IHttpActionResult HoatDongDangToChuc(int lopId)
        {
            var userSinhVienId = 0;
            if (User.Identity.IsAuthenticated)
            {
                userSinhVienId = User.Identity.GetSinhVienId();
            }
            var danhSachHoatDong = _context.DanhSachHoatDong
                .Where(hd => hd.DuocPheDuyet && !hd.DaKetThuc && !hd.BiHuy && hd.DanhSachLopToChuc.Any(l => l.LopId == lopId))
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

        [HttpPost]
        [Route("api/Lop/HoatDongToChuc/{lopId}")]
        public IHttpActionResult LayHoatDongLopToChuc(int lopId, PagingSearchingSorting pagingSearchingSorting)
        {
            if (!ModelState.IsValid) return BadRequest();
            List<HoatDongResultSetForDataTable> hoatDongResultSet;
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var donViIdParameter = new SqlParameter("@DonViId", lopId);
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                hoatDongResultSet = _context.Database
                    .SqlQuery<HoatDongResultSetForDataTable>("LayHoatDongLopASC @DonViId, @SearchTerm, @ColumnName, @RecordStart, @PageSize",
                        donViIdParameter, searchTerm, columnName, recordStart, pageSize).ToList();
            }
            else
            {
                hoatDongResultSet = _context.Database
                    .SqlQuery<HoatDongResultSetForDataTable>("LayHoatDongLopDESC @DonViId, @SearchTerm, @ColumnName, @RecordStart, @PageSize",
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
                recordsTotal = _context.DanhSachHoatDongLop.Count(hd => hd.LopId == lopId);
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

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/LuotThamGiaHd/{lopId}/{namHocLay}")]
        public IHttpActionResult LayLuotThamGiaHdSvLop(int lopId,int namHocLay = 0)
        {
            if (!ModelState.IsValid) return BadRequest();
            List<LuotThamGiaHoatDongLopForDataTable> luotThamGiaResultSet;
            var lopIdSqlParameter = new SqlParameter("@LopId", lopId);
            var namHocLaySqlParameter = new SqlParameter("@NamHocLay", namHocLay);
            luotThamGiaResultSet = _context.Database
                .SqlQuery<LuotThamGiaHoatDongLopForDataTable>("LayLuotSinhVienLopThamGiaHoatDong @LopId, @NamHocLay",
                                                                lopIdSqlParameter, namHocLaySqlParameter).ToList();

            var tongLuotThamGiaTrongNam = luotThamGiaResultSet.Sum(tg => tg.SoLuotThamGiaLop);
            return Ok(new
            {
                danhSachHdVaSoLuotThamGia = luotThamGiaResultSet,
                tongLuotThamGiaTrongNam
            }) ;

        }

        [HttpGet]
        [Route("api/Lop/ThongKeHoatDong/{lopId}/{namHocLay}")]
        public IHttpActionResult ThongKeHoatDong(int lopId,int namHocLay)
        {

            //^ Mốc thời gian để dừng lấy  số lượt tham gia hoạt động
            //Nếu lấy số lượt tham gia hoạt động của một năm học cụ thể thì đó là từ 8/1/năm học đó - 31/7/năm học sau

            //var hoatDongHocVienQuery = _context.DanhSachHoatDong
            //                                    .Where(_hoatDongRepository.LayHoatDongNam_HoatDongFunc(namHocLay))
            //                                     .Where(hd => hd.DuocPheDuyet && !hd.BiHuy) //Chỉ lấy hoạt động được phê duyệt và không bị hủy
            //                                    .Select(hd => new 
            //                                    {
            //                                        hd.Id,
            //                                        hd.NgayBatDau,
            //                                        hd.NgayKetThuc,
            //                                        DanhSachLopToChuc = hd.DanhSachLopToChuc.Select(dv => dv.LopId).ToList()
            //                                    }).ToList();

            //var hoatDongHocVienToChuc = hoatDongHocVienQuery.Select(ob => new HoatDongDtoForTable
            //                                                    {
            //                                                        Id = ob.Id,
            //                                                        NgayBatDau = ob.NgayBatDau,
            //                                                        NgayKetThuc = ob.NgayKetThuc,
            //                                                    }).ToList();
            //var danhSachHoatDongLop = hoatDongHocVienQuery.Where(ob => ob.DanhSachLopToChuc.Contains(lopId))
            //                                               .Select(ob => new HoatDongDtoForTable
            //                                                {
            //                                                    Id = ob.Id,
            //                                                    NgayBatDau = ob.NgayBatDau,
            //                                                    NgayKetThuc = ob.NgayKetThuc,
            //                                                })
            //                                                .ToList();
            var lopIdSqlParameter = new SqlParameter("@LopId", lopId);
            var namHocLaySqlParameter = new SqlParameter("@NamHocLay", namHocLay);
            List<HoatDongToChucDto> danhSachHoatDongToChucResultSet;
            List<LuotThamGiaHoatDongLopForDataTable> luotThamGiaResultSet;

            //Lấy số hoạt động tổ chức theo từng tháng
            danhSachHoatDongToChucResultSet = _context.Database
                .SqlQuery<HoatDongToChucDto>("LayHoatDongHocVienVaLopToChuc @LopId, @NamHocLay",
                    lopIdSqlParameter, namHocLaySqlParameter).ToList();
            var soHoatDongToChucTungThang = _hoatDongRepository.LaySoHdTcTungThangChoLop_DonVi(danhSachHoatDongToChucResultSet, namHocLay);


            //Lấy số lượt tham gia hoạt động theo từng tháng
            lopIdSqlParameter = new SqlParameter("@LopId", lopId);
            namHocLaySqlParameter = new SqlParameter("@NamHocLay", namHocLay);
            luotThamGiaResultSet = _context.Database
                .SqlQuery<LuotThamGiaHoatDongLopForDataTable>("LayLuotThamGiaHdLopVaHocVien @LopId, @NamHocLay",
                    lopIdSqlParameter, namHocLaySqlParameter).ToList();
            var soLuotThamGiaHdTungThang = _hoatDongRepository.LaySoLuotTgTungThangChoLop(luotThamGiaResultSet, namHocLay);

            return Ok(new
            {
                soHoatDongToChucTungThang,
                soLuotThamGiaHdTungThang
            });
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/Lop/HoatDongChoPheDuyet/{lopId}")]
        public IHttpActionResult HoatDongChoPheDuyet(int lopId)
        {
            var hoatDong = _context.DanhSachHoatDong
                .Where(hd => !hd.DuocPheDuyet && hd.DanhSachLopToChuc.Any(l => l.LopId == lopId))
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
            return Ok(hoatDong);
        }

        private LopDto LayFullChiTietLop(int lopId)
        {
            return _context.Lop
                .Where(lop => lop.Id == lopId)
                .Select(lop => new LopDto
                {
                  Id = lop.Id,
                  TenLop = lop.TenLop,
                  KyHieuTenLop = lop.KyHieuTenLop,
                  KhoaHocId = lop.KhoaHocId,
                  KhoaHoc = lop.KhoaHoc.TenKhoa,
                  AnhBia = lop.AnhBia,
                  DanhSachSinhVien = lop.DanhSachSinhVien.Select(dssv => new SinhVienDtoForTable
                  {
                      Id = dssv.SinhVien.Id,
                      NgaySinh = dssv.SinhVien.NgaySinh,
                      AnhDaiDien = dssv.SinhVien.AnhDaiDien,
                      GioiTinh = dssv.SinhVien.GioiTinh.TenGioiTinh,
                      HoVaTenLot = dssv.SinhVien.HoVaTenLot,
                      Ten = dssv.SinhVien.Ten
                  }).ToList(),
                  ChucVuLop = lop.ChucVuLop.Select(cvl => new ChucVuLopDto
                  {
                      SinhVien = new TTSinhVienCBNhatDto
                      {
                          AnhDaiDien = cvl.SinhVien.AnhDaiDien,
                          HoVaTenLot = cvl.SinhVien.HoVaTenLot,
                          Id = cvl.SinhVien.Id,
                          MSSV = cvl.SinhVien.MSSV,
                          Ten = cvl.SinhVien.Ten,
                      },
                      ChucVu = cvl.ChucVu.TenChucVu,
                      ChucVuId = cvl.ChucVuId
                  }).ToList()
                }).FirstOrDefault();
        }
        
        private void MapSoLieuSinhVienLop(LopDto lopDto)
        {
            if(lopDto.DanhSachSinhVien == null) return;
            lopDto.soLuongSV = lopDto.DanhSachSinhVien.Count;
            lopDto.soNu = lopDto.DanhSachSinhVien.Count(sv => sv.GioiTinh == "Nữ");
            lopDto.soNam = lopDto.DanhSachSinhVien.Count(sv => sv.GioiTinh == "Nam");
            lopDto.khac = lopDto.soLuongSV - (lopDto.soNam + lopDto.soNu);
        }
        private bool CheckQuyenChinhSuaChucVu(ChinhSuaChucVuLopDto chucVuLopDto, Lop lop,
            out IHttpActionResult httpActionResult)
        {
            //Kiểm tra xem coi có quyền không (chỉ có lớp trưởng,bí thư, chi hội trưởng và người có role Admin,QuanLyLop)
            var chucVuBanCanSu = new List<int> { 1, 2, 3 }.AsReadOnly();
            var chucVuDoan = new List<int> { 4, 5, 6 }.AsReadOnly();
            var chucVuHoi = new List<int> { 7, 8, 9 }.AsReadOnly();
            var userSinhVienId = User.Identity.GetSinhVienId();
            var roleQuanLyLop = User.IsInRole("Admin") || User.IsInRole("QuanLyLop");

            var canEditBanCanSu = roleQuanLyLop || lop.ChucVuLop.Any(cvl => cvl.ChucVuId == 1 && cvl.SinhVienId == userSinhVienId);
            var canEditDoan = roleQuanLyLop || lop.ChucVuLop.Any(cvl => cvl.ChucVuId == 4 && cvl.SinhVienId == userSinhVienId);
            var canEditHoi = roleQuanLyLop || lop.ChucVuLop.Any(cvl => cvl.ChucVuId == 7 && cvl.SinhVienId == userSinhVienId);

            if (!canEditBanCanSu && !canEditDoan && !canEditHoi)
            {
                httpActionResult = BadRequest();
                return true;
            }

            if (chucVuBanCanSu.Contains(chucVuLopDto.ChucVuId) && !canEditBanCanSu)
            {
                httpActionResult = BadRequest();
                return true;
            }

            if (chucVuDoan.Contains(chucVuLopDto.ChucVuId) && !canEditDoan)
            {
                httpActionResult = BadRequest();
                return true;
            }

            if (chucVuHoi.Contains(chucVuLopDto.ChucVuId) && !canEditHoi)
            {
                httpActionResult = BadRequest();
                return true;
            }

            httpActionResult = Ok();
            return false;
        }
        private bool CheckQuyenQuanLySinhVienLop(ThemXoaSinhVienDto themXoaSinhVienDto, out IHttpActionResult status)
        {
            //Kiểm tra xem coi có quyền không (chỉ có lớp trưởng (ChucVuId=1) và người có role Admin,QuanLyLop)
            var userSinhVienId = User.Identity.GetSinhVienId();
            var giuChucLopTruong = _context.ChucVuLop.Any(cvl => cvl.ChucVuId == 1
                                                                 && cvl.LopId == themXoaSinhVienDto.LopId
                                                                 && cvl.SinhVienId == userSinhVienId);
            if (!User.IsInRole("Admin") && !User.IsInRole("QuanLyLop") && !giuChucLopTruong)
            {
                status = BadRequest();
                return true;
            }
            status = Ok();
            return false;
        }

    }

    public class HoatDongToChucDto
    {
        public int HoatDongId { get; set; }
        public bool CoThamGiaToChuc { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

    }
}
