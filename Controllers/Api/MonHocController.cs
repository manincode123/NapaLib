using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.MonHocDtos;

namespace NAPASTUDENT.Controllers.Api
{
    public class MonHocController : ApiController
    {
        /*
         *  Điểm trung bình của sinh viên sẽ được tạo khi tạo sinh viên. Xem sinh viên controller
         */
        private ApplicationDbContext _context;

        public MonHocController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("api/MonHoc/ChiTietCoBan/{monHocId}")]
        public IHttpActionResult LayChiTietCoBanMonHoc(int monHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var monHoc = _context.MonHoc.SingleOrDefault(mh => mh.Id == monHocId);
            return Ok(Mapper.Map<MonHoc, MonHocDto>(monHoc));
        }

        //Lấy danh sách môn học
        [HttpGet]
        [Route("api/MonHoc/DanhSachMonHoc")]
        public IHttpActionResult LayDanhSachMonHoc()
        {
            var dsMonHoc = _context.MonHoc.ToList();

            return Ok(dsMonHoc.Select(Mapper.Map<MonHoc,MonHocDto>));
        }

        //Save môn học (thêm hoặc chỉnh sửa)
        [HttpPost]
        [Route("api/MonHoc/SaveMonHoc")]
        public IHttpActionResult SaveMonHoc(MonHocDto monHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            MonHoc monHoc;
            if (monHocDto.Id == 0)
            {
                monHoc = Mapper.Map<MonHocDto,MonHoc>(monHocDto);
                _context.MonHoc.Add(monHoc);
                _context.SaveChanges();
                return Ok();
            }
            monHoc = _context.MonHoc.SingleOrDefault(mh => mh.Id == monHocDto.Id);
            if (monHoc == null) return NotFound();
            monHoc.MapForEdit(monHocDto);
            _context.SaveChanges();
            return Ok();
        }

        //Xóa môn học
        [HttpDelete]
        [Route("api/MonHoc/XoaMonHoc/{monHocId}")]
        public IHttpActionResult XoaMonHoc(int monHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var monHoc = _context.MonHoc.SingleOrDefault(mh => mh.Id == monHocId);
            if (monHoc == null) return NotFound();
            monHoc.DanhSachLopHocMonHoc.Clear();
            _context.MonHoc.Remove(monHoc);
            _context.SaveChanges();
            return Ok();
        }

        /*Phần cho controller quản lý lớp môn học*/
        //Lấy danh sách các lớp học môn học này
        [HttpGet]
        [Route("api/MonHoc/DanhSachLopMonHoc/{monHocId}")] 
        public IHttpActionResult LayDanhSachLopMonHoc(int monHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var danhSachLop = _context.LopMonHoc.Where(lmh => lmh.MonHocId == monHocId).Select(lmh => new
            {
                lmh.Lop.TenLop,
                lmh.LopId,
                lmh.HocKi,
                lmh.NgayThi,
                lmh.DiaDiemThi
            }).ToList();
            return Ok(danhSachLop);
        }

        //Đăng kí lớp học môn học
        [HttpPost]
        [Route("api/MonHoc/DangKiMonHoc")] 
        public IHttpActionResult DangKiMonHoc(DangKiMonHocDto dangKiMonHocDto)
        {
            var lopMonHoc = _context.LopMonHoc.SingleOrDefault(lmh =>
                lmh.LopId == dangKiMonHocDto.LopId && lmh.MonHocId == dangKiMonHocDto.MonHocId);
            if (lopMonHoc != null)
            {
                return BadRequest("Lớp được chọn đã đăng kí môn học này.");
            }
            lopMonHoc = Mapper.Map<DangKiMonHocDto, LopMonHoc>(dangKiMonHocDto);
            _context.LopMonHoc.Add(lopMonHoc);
            _context.SaveChanges();
            return Ok();
        }

        //Sửa đăng kí lớp học môn học (Sửa ngày thi,địa điểm thi). Tách ra với đăng kí vì dùng Composite Key
        [HttpPut]
        [Route("api/MonHoc/SuaLopMonHoc")]
        public IHttpActionResult SuaLopMonHoc(DangKiMonHocDto dangKiMonHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var lopMonHoc = _context.LopMonHoc.SingleOrDefault(lmh =>
                lmh.LopId == dangKiMonHocDto.LopId && lmh.MonHocId == dangKiMonHocDto.MonHocId);
            if (lopMonHoc == null)
            {
                return NotFound();
            }

            lopMonHoc.MapForEdit(dangKiMonHocDto);
            _context.SaveChanges();
            return Ok();
        }

        //Xóa đăng kí môn học của lớp
        [HttpDelete]
        [Route("api/MonHoc/XoaDangKiMonHoc")]
        public IHttpActionResult XoaDangKiMonHoc(LopMonHocDto lopMonHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var lopMonHoc = _context.LopMonHoc.Include(lmh => lmh.DanhSachDiem).SingleOrDefault(lmh =>
                lmh.LopId == lopMonHocDto.LopId && lmh.MonHocId == lopMonHocDto.MonHocId);
            if (lopMonHoc == null) return NotFound();
            lopMonHoc.XoaDiem(); 
            _context.LopMonHoc.Remove(lopMonHoc);
            _context.SaveChanges();
            return Ok();
        }

        //Lấy thông tin cho trang quản lý lớp môn học
        [HttpPost]
        [Route("api/MonHoc/QuanLyLopMonHoc")]
        public IHttpActionResult LayThongTinTrangQuanLyLopMonHoc(LopMonHocDto lopMonHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var lopMonHoc = _context.LopMonHoc.Where(lmh =>
                lmh.LopId == lopMonHocDto.LopId && lmh.MonHocId == lopMonHocDto.MonHocId)
                .Select(lmh => new
                {
                    lmh.LopId,
                    lmh.MonHocId,
                    lmh.Lop.TenLop,
                    lmh.NgayThi,
                    lmh.HocKi,
                    lmh.DiaDiemThi,
                    monHoc = new
                    {
                        lmh.MonHoc.TenMonHoc,
                        lmh.MonHoc.HaiDiemDk,
                        lmh.MonHoc.LoaiMon,
                        lmh.MonHoc.SoTiet,
                        lmh.MonHoc.SoHocPhan
                    },
                    LichHoc = lmh.DanhSachLichHoc.Select(lh => new
                    {
                        lh.Id,
                        lh.BuoiSang,
                        lh.BaTietDau,
                        lh.Thu246,
                        lh.NgayBatDau,
                        lh.NgayKetThuc,
                        lh.GiaoVienDay,
                        lh.PhongHoc
                    }).ToList()
                }).SingleOrDefault();
            if (lopMonHoc == null) return NotFound();
            return Ok(lopMonHoc);
        }

        //Đăng kí lịch học cho lớp
        [HttpPost]
        [Route("api/MonHoc/DangKiLichHoc")]
        public IHttpActionResult DangKiLichHoc(LichHocDto lichHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            LichHoc lichHoc;
            if (lichHocDto.Id == 0)
            {
                lichHoc = Mapper.Map<LichHocDto, LichHoc>(lichHocDto);
                _context.LichHoc.Add(lichHoc);
                _context.SaveChanges();
                return Ok();
            }

            lichHoc = _context.LichHoc.SingleOrDefault(lh => lh.Id == lichHocDto.Id);
            if (lichHoc == null) return NotFound();
            lichHoc.MapForEdit(lichHocDto);
            _context.SaveChanges();
            return Ok();
        }
        
        //Xóa lịch học
        [HttpDelete]
        [Route("api/MonHoc/XoaLichHoc/{lichHocId}")]
        public IHttpActionResult XoaLichHoc(int lichHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var lichHoc = _context.LichHoc.SingleOrDefault(lh => lh.Id == lichHocId);
            if (lichHoc == null) return NotFound();
            _context.LichHoc.Remove(lichHoc);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/MonHoc/LayDanhSachLichHoc")]
        public IHttpActionResult LayDanhSachLichHoc(LopMonHocDto lopMonHocDto)   //Lấy danh sách lịch học của lớp môn học
        {
            if (!ModelState.IsValid) return BadRequest();
            var lichHoc = _context.LichHoc
                .Where(lh => lh.MonHocId == lopMonHocDto.MonHocId && lh.LopId == lopMonHocDto.LopId)
                .ToList();
            return Ok(lichHoc.Select(Mapper.Map<LichHoc, LichHocDto>));
        }

        [HttpGet]
        [Route("api/MonHoc/LayLichHoc/{lichHocId}")]
        public IHttpActionResult LayLichHoc(int lichHocId)   //Lấy lịch học cụ thể của lớp môn học
        {
            var lichHoc = _context.LichHoc.SingleOrDefault(lh => lh.Id == lichHocId);
            return Ok( Mapper.Map<LichHoc, LichHocDto>(lichHoc));
        }

        /*Phần controller điểm môn học*/
        [HttpPost]
        [Route("api/MonHoc/TaoDiem")]
        public IHttpActionResult TaoDiemSinhVien(TaoDiemMonHocDto taoDiemMonHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var lopMonHoc = _context.LopMonHoc.Where(lmh => lmh.LopId == taoDiemMonHocDto.LopId
                                                            && lmh.MonHocId == taoDiemMonHocDto.MonHocId)
                                              .Select(lmh => new
                                               {
                                                    lmh.MonHocId,
                                                    lmh.LopId,
                                                    lmh.MonHoc.LoaiMon,
                                                    lmh.MonHoc.SoHocPhan,
                                                    lmh.HocKi,
                                                   DanhSachSinhVien = lmh.Lop.DanhSachSinhVien.Select(svl => svl.SinhVienId)
                                               }).SingleOrDefault();
            if (lopMonHoc == null) return NotFound();

            foreach (var sinhVienId in lopMonHoc.DanhSachSinhVien)
            {
                ThemDiem(lopMonHoc.MonHocId,lopMonHoc.LoaiMon, lopMonHoc.LopId, sinhVienId, lopMonHoc.HocKi);
            }

            //Cộng thêm học phần của môn học này vào điểm trung bình học kì
            var danhSachDiemTb = _context.DiemTrungBinhHocKi.Where(dtb =>
                lopMonHoc.DanhSachSinhVien.Contains(dtb.SinhVienId) && dtb.HocKi == lopMonHoc.HocKi).ToList();
            foreach (var diemTrungBinhHocKi in danhSachDiemTb)
            {
                diemTrungBinhHocKi.TongHocPhan += lopMonHoc.SoHocPhan;
            }

            _context.SaveChanges();
            return Ok();
        }


        [HttpPut]
        [Route("api/MonHoc/ChinhSuaDiem")]
        public IHttpActionResult ChinhSuaDiem(DiemDto diemDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //phải Include MonHoc vì tí nữa func .TinhDiemTb(); sẽ sử dụng nó
            var diemSinhVien = _context.Diem.Include(diem => diem.MonHoc).Include(diem => diem.DanhSachDiemBoSung)
                .SingleOrDefault(d => d.SinhVienId == diemDto.SinhVienId && d.MonHocId == diemDto.MonHocId);
            if (diemSinhVien == null) return NotFound();
            if (diemDto.DiemChuyenCan != null) diemSinhVien.SetDiemChuyenCan(diemDto.DiemChuyenCan);
            if (diemDto.DiemDieuKien1 != null) diemSinhVien.SetDiemDieuKien1(diemDto.DiemDieuKien1);
            if (diemDto.DiemDieuKien2 != null) diemSinhVien.SetDiemDieuKien2(diemDto.DiemDieuKien2);
            if (diemDto.DiemThi != null) diemSinhVien.SetDiemThi(diemDto.DiemThi);
            if (diemDto.DanhSachDiemBoSung != null && diemDto.DanhSachDiemBoSung.Count != 0)
            {
                foreach (var diemBoSungDto in diemDto.DanhSachDiemBoSung)
                {
                    var diemBoSung = diemSinhVien.DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.Id == diemBoSungDto.Id);
                    if (diemBoSung == null) continue;
                    if (diemBoSungDto.Diem != null) diemBoSung.SetDiem(diemBoSungDto.Diem);
                }
            }

            var diemTbMonHocCu = diemSinhVien.DiemTb.GetValueOrDefault();
            diemSinhVien.TinhDiemTb();
            if (diemSinhVien.DiemTb != null) TinhDiemTrungBinhTheoMon(diemSinhVien.SinhVienId, diemSinhVien.HocKi, diemTbMonHocCu,(byte) diemSinhVien.DiemTb, diemSinhVien.MonHoc.SoHocPhan);
            _context.SaveChanges();
            return Ok(new
            {
                diemSinhVien.DiemChuyenCan,
                diemSinhVien.DiemDieuKien1,
                diemSinhVien.DiemDieuKien2,
                diemSinhVien.DiemThi,
                diemSinhVien.DiemTb,
                DanhSachDiemBoSung = diemSinhVien.DanhSachDiemBoSung.OrderBy(dbs => dbs.LoaiDiem).ToList()
            });
        }

        private void TinhDiemTrungBinhTheoMon(int sinhVienId, HocKi hocKi, byte diemTbMonHocCu, byte diemTbMonHocMoi, byte hocPhanMonHoc)
        {
            var diemTb = _context.DiemTrungBinhHocKi.SingleOrDefault(dtb => dtb.SinhVienId == sinhVienId
                                                                            && dtb.HocKi == hocKi);
            if (diemTb != null)
            {
                diemTb.TongDiem = diemTb.TongDiem - diemTbMonHocCu * hocPhanMonHoc + diemTbMonHocMoi * hocPhanMonHoc;
                diemTb.DiemTb = (float)diemTb.TongDiem / diemTb.TongHocPhan;
            }
            _context.SaveChanges();
        }
        private void TinhDiemTrungBinhHocKi(int sinhVienId, HocKi hocKi)
        {
            var danhSachDiem = _context.Diem.Where(d => d.SinhVienId == sinhVienId && d.HocKi == hocKi)
                .Select(d => new
                {
                    d.DiemTb,
                    d.MonHoc.SoHocPhan
                });
            var diemTb = _context.DiemTrungBinhHocKi.SingleOrDefault(dtb => dtb.SinhVienId == sinhVienId 
                                                                         && dtb.HocKi == hocKi);
            if (diemTb != null)
            {
                diemTb.TongDiem = 0;
                diemTb.DiemTb = 0;
                foreach (var diem in danhSachDiem)
                {
                    diemTb.TongDiem += diem.DiemTb.GetValueOrDefault() * diem.SoHocPhan;
                    diemTb.TongHocPhan += diem.SoHocPhan;
                }

                diemTb.DiemTb = (float) diemTb.TongDiem / diemTb.TongHocPhan;
            }

            _context.SaveChanges();

        }

        [HttpPost]
        [Route("api/MonHoc/LayDiemHocKiLop")]
        public IHttpActionResult LayDiemHocKiLop(DiemLopHocKiDto diemLopHocKiDto)
        {
            var lop = _context.Lop.Where(l => l.Id == diemLopHocKiDto.LopId)
            .Select(l => new
            {
                MonHoc = l.DanhSachMonHoc.Where(lmh => lmh.HocKi == diemLopHocKiDto.HocKi)
                                         .OrderBy(lmh => lmh.MonHocId)
                                         .Select(lmh => new
                                         {
                                             lmh.MonHoc.KyHieuMonHoc,
                                             lmh.MonHoc.SoHocPhan,
                                             lmh.MonHoc.HaiDiemDk,
                                             lmh.MonHoc.TenMonHoc
                                         })
            }).SingleOrDefault();


            var sinhVienVaDiem = _context.DanhSachSinhVienLop.Where(svl => svl.LopId == diemLopHocKiDto.LopId)
                .Select(svl => new
                {
                    svl.SinhVien.MSSV,
                    svl.SinhVien.HoVaTenLot,
                    svl.SinhVien.Ten,
                    Diem = svl.SinhVien.Diem.Where(diem => diem.HocKi == diemLopHocKiDto.HocKi)
                                            .OrderBy(diem => diem.MonHocId).Select(diem => new
                                            {
                                                diem.DiemChuyenCan,
                                                diem.DiemDieuKien1,
                                                diem.DiemDieuKien2,
                                                diem.DiemThi,
                                                diem.DiemTb,
                                                diem.DanhSachDiemBoSung
                                            }).ToList()
                });
            return Ok(new
            {
                lop,
                sinhVienVaDiem
            });
        }

        [HttpPost]
        [Route("api/MonHoc/LayDiemLopMonHoc")]
        public IHttpActionResult LayDiemLopMonHoc(LopMonHocDto lopMonHocDto)
        {
            var sinhVienVaDiem = _context.DanhSachSinhVienLop.Where(svl => svl.LopId == lopMonHocDto.LopId)
                .Select(svl => new
                {
                    Id = svl.SinhVienId,
                    svl.SinhVien.MSSV,
                    svl.SinhVien.HoVaTenLot,
                    svl.SinhVien.Ten,
                    Diem = svl.SinhVien.Diem.Where(diem => diem.MonHocId == lopMonHocDto.MonHocId)
                                            .Select(diem => new
                                            {
                                                diem.DiemChuyenCan,
                                                diem.DiemDieuKien1,
                                                diem.DiemDieuKien2,
                                                diem.DiemThi,
                                                diem.DiemTb,
                                                DanhSachDiemBoSung = diem.DanhSachDiemBoSung
                                                                       .Select(dbs => new
                                                                       {
                                                                           dbs.Id,
                                                                           dbs.Diem
                                                                       })
                                            }).FirstOrDefault()
                });
            return Ok(sinhVienVaDiem);
        }

        [HttpGet]
        [Route("api/MonHoc/LayDiemSinhVienMon/{monHocId}")]
        public IHttpActionResult LayDiemMonHocSinhVien(int monHocId)
        {
            var sinhVienId = 0;
            var sinhVien = _context.SinhVien.Where(sv => sv.Id == sinhVienId).Select(sv => new
                ThongTinMonHocSinhVienDto()
            {
                Id = sv.Id,     //Lấy Id
                MSSV = sv.MSSV, //Lấy MSSV
                Ten = sv.Ten,   //Lấy tên
                HoVaTenLot = sv.HoVaTenLot, //Lấy họ tên lót
                Diem = sv.Diem.Where(diem => diem.MonHocId == monHocId).Select(diem => new
                    DiemForTableDto()  //Lấy điểm và thông tin môn học
                {
                    TenMonHoc = diem.MonHoc.TenMonHoc,
                    SoHocPhan = diem.MonHoc.SoHocPhan,
                    SoTiet = diem.MonHoc.SoTiet,
                    LoaiMon = diem.MonHoc.LoaiMon,
                    HaiDiemDk = diem.MonHoc.HaiDiemDk,
                    DiemChuyenCan = diem.DiemChuyenCan,
                    DiemDieuKien1 = diem.DiemDieuKien1,
                    DiemDieuKien2 = diem.DiemDieuKien2,
                    DiemThi = diem.DiemThi,
                    DiemTb = diem.DiemTb,
                    DanhSachDiemBoSung = diem.DanhSachDiemBoSung.Select(dbs => new DiemBoSungForTableDto()
                    {  //Lấy điểm bổ sung
                        Diem = dbs.Diem,
                        LoaiDiem = dbs.LoaiDiem
                    }).ToList()
                }).FirstOrDefault(),
                LopId = sv.Diem.FirstOrDefault(d => d.MonHocId == monHocId).LopId, //Lấy id lớp để tìm lớp môn học ở dưới
                LichThiLai = sv.Diem.Where(d => d.MonHocId == monHocId).Select(d => new
                LichThiLai4SinhVienMon
                {
                    ThoiGianThi = d.LichThiLai.ThoiGianThi,
                    DiaDiemThi = d.LichThiLai.DiaDiemThi
                }).FirstOrDefault()
            }).SingleOrDefault();
            if (sinhVien == null) return NotFound();
            sinhVien.Diem.DanhSachDiemBoSung = sinhVien.Diem.DanhSachDiemBoSung.OrderByDescending(dbs => dbs.LoaiDiem).ToList();
            var lopMonHoc = _context.LopMonHoc.Where(lmh => lmh.LopId == sinhVien.LopId
                                                         && lmh.MonHocId == monHocId)
                                              .Select(lmh => new
                                              {
                                                  lmh.Lop.TenLop,
                                                  lmh.NgayThi,
                                                  lmh.HocKi,
                                                  lmh.DiaDiemThi,
                                                  LichHoc = lmh.DanhSachLichHoc.Select(lh => new
                                                  {
                                                      lh.BuoiSang,
                                                      lh.BaTietDau,
                                                      lh.Thu246,
                                                      lh.NgayBatDau,
                                                      lh.NgayKetThuc,
                                                      lh.GiaoVienDay,
                                                      lh.PhongHoc
                                                  }).ToList()
                                              }).SingleOrDefault();
            return Ok(new
            {
                sinhVien,
                lopMonHoc
            });
        }

        [HttpGet]
        [Route("api/MonHoc/LayDiemHocKiSinhVien/{hocKi}")]
        public IHttpActionResult LayDiemHocKiSinhVien(HocKi hocKi)
        {
            var sinhVienId = 0;
            var sinhVien = _context.SinhVien.Where(sv => sv.Id == sinhVienId).Select(sv => new
            {
                sv.Id,
                sv.MSSV,
                sv.Ten,
                sv.HoVaTenLot,
                Diem = sv.Diem.Where(diem => diem.HocKi == hocKi).Select(diem => new
                {
                    diem.MonHoc.TenMonHoc,
                    diem.MonHoc.SoHocPhan,
                    diem.MonHoc.LoaiMon,
                    diem.MonHoc.HaiDiemDk,
                    diem.DiemChuyenCan,
                    diem.DiemDieuKien1,
                    diem.DiemDieuKien2,
                    diem.DiemThi,
                    diem.DiemTb,
                    DanhSachDiemBoSung = diem.DanhSachDiemBoSung.Select(dbs => dbs.Diem)
                })
            }).SingleOrDefault();
            if (sinhVien == null) return NotFound();
            return Ok(sinhVien);
        }



        //Học lại á :v
        [HttpPost]
        [Route("api/MonHoc/ResetDiem")]
        public IHttpActionResult ResetDiem(DiemDto diemDto)  
        {
            var diemSinhVien = _context.Diem.Include(diem => diem.DanhSachDiemBoSung).SingleOrDefault(d =>
                d.SinhVienId == diemDto.SinhVienId && d.MonHocId == diemDto.MonHocId);
            if (diemSinhVien == null) return NotFound();
            diemSinhVien.HocLai();
            return Ok();
        }


        /*Phần cho controller quản lý lớp môn học*/
        [HttpGet]
        [Route("api/MonHoc/LayDsSvPhaiThiLaiTatCaMon")]
        public IHttpActionResult LayDsSvPhaiThiLaiTatCaMon()
        {
           var dsSvThiLai = _context.Diem.Where(d => d.DiemTb < 4 && d.LichThiLaiId == null
                                                     || d.DiemTb == null && d.LichThiLaiId != null)
                .Select(d => new
                {
                    d.SinhVien.HoVaTenLot,
                    d.SinhVien.Ten,
                    d.SinhVienId,
                    d.SinhVien.MSSV,
                    d.LopMonHoc.Lop.TenLop,
                    d.MonHoc.TenMonHoc,
                    d.MonHocId,
                    d.DiemTb,
                    d.LichThiLaiId
                }).ToList();
            return Ok(dsSvThiLai);
        }

        [HttpGet]
        [Route("api/MonHoc/LayDsSvPhaiThiLai/{monHocId}")]
        public IHttpActionResult LayDsSvPhaiThiLai(int monHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            /*
             Lấy danh sách những sinh viên phải thi lại. Có 2 điều kiện sau đây:
             * Điểm trung bình dưới 4 và chưa thi lại
             * Đã đăng kí lịch thi lại nhưng chưa có điểm thi lại, điểm trung bình (điểm trung bình == null)
             */
            var danhSachSvThiLai = _context.Diem.Where(d => d.MonHocId == monHocId
                                                         && ((d.DiemTb < 4 && d.LichThiLaiId == null) 
                                                          || (d.DiemTb == null && d.LichThiLaiId != null)))
            .Select(d => new
            {
                d.SinhVien.HoVaTenLot,
                d.SinhVien.Ten,    
                d.SinhVienId,
                d.SinhVien.MSSV,
                d.LopMonHoc.Lop.TenLop,
                d.DiemTb,
                d.LichThiLaiId
            }).ToList();
            return Ok(danhSachSvThiLai);
        }

        [HttpGet]
        [Route("api/MonHoc/LayDsSvChuaCoLichThiLai/{monHocId}")]
        public IHttpActionResult LayDsSvChuaCoLichThiLai(int monHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Cái này là lấy cách sinh viên phải thi lạ nhưng chưa được đăng kí lịch thi lại
            var danhSachSvThiLai = _context.Diem.Where(d => d.DiemTb < 4 && d.MonHocId == monHocId 
                                                                         && d.LichThiLaiId == null)
             .Select(d => new
            {
                Id = d.SinhVienId,
                d.SinhVien.HoVaTenLot,
                d.SinhVien.Ten,
                d.SinhVien.MSSV,
                d.LopMonHoc.Lop.KyHieuTenLop,
                d.DiemTb
            }).ToList();
            return Ok(danhSachSvThiLai);
        }

        [HttpGet]
        [Route("api/MonHoc/LayDsDiemThiLaiSv/{lichThiLaiId}")]
        public IHttpActionResult LayDsDiemThiLaiSv(int lichThiLaiId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var sinhVienVaDiem = _context.Diem.Where(svl => svl.LichThiLaiId == lichThiLaiId)
                .Select(diem => new
                {
                    Id = diem.SinhVienId,
                    diem.SinhVien.MSSV,
                    diem.SinhVien.HoVaTenLot,
                    diem.SinhVien.Ten,
                    diem.LopMonHoc.Lop.KyHieuTenLop,
                    diem.DiemThi,
                    DanhSachDiemBoSung = diem.DanhSachDiemBoSung
                               .Select(dbs => new
                               {
                                    dbs.Id,
                                   dbs.Diem
                               })
                });
            return Ok(sinhVienVaDiem);
        }

        [HttpPut]
        [Route("api/MonHoc/SetDiemThiLai")]
        public IHttpActionResult SetDiemThiLai(DiemDto diemDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //phải Include MonHoc vì tí nữa func .TinhDiemTb(); sẽ sử dụng nó
            var diemSinhVien = _context.Diem.Include(diem => diem.DanhSachDiemBoSung).Include(diem => diem.MonHoc)
                .SingleOrDefault(d => d.SinhVienId == diemDto.SinhVienId && d.MonHocId == diemDto.MonHocId);
            if (diemSinhVien == null) return NotFound();
            if (diemDto.DiemThi != null) diemSinhVien.SetDiemThi(diemDto.DiemThi);
            if (diemDto.DanhSachDiemBoSung != null && diemDto.DanhSachDiemBoSung.Count != 0)
            {
                foreach (var diemBoSungDto in diemDto.DanhSachDiemBoSung)
                {
                    var diemBoSung = diemSinhVien.DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.Id == diemBoSungDto.Id);
                    if (diemBoSung == null) continue;
                    if (diemBoSungDto.Diem != null) diemBoSung.SetDiem(diemBoSungDto.Diem);
                }
            }
            diemSinhVien.TinhDiemTb();
            _context.SaveChanges();
            return Ok(new
            {
                diemSinhVien.DiemThi,
                DanhSachDiemBoSung = diemSinhVien.DanhSachDiemBoSung.OrderBy(dbs => dbs.LoaiDiem).ToList()
            });
        }

        [HttpGet]
        [Route("api/MonHoc/LayThongTinThiLai/{lichThiLaiId}")]
        public IHttpActionResult LayThongTinThiLai(int lichThiLaiId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var thongTinThiLai = _context.LichThiLai.Where(ltl => ltl.Id == lichThiLaiId).Select(ltl => new
            {
                ltl.Id,
                ltl.ThoiGianThi,
                ltl.DiaDiemThi,
                ltl.DaThiXong,
                monHoc = new
                {
                    ltl.MonHoc.Id,
                    ltl.MonHoc.TenMonHoc,
                    ltl.MonHoc.HaiDiemDk,
                    ltl.MonHoc.LoaiMon,
                    ltl.MonHoc.SoTiet,
                    ltl.MonHoc.SoHocPhan
                },
            }).SingleOrDefault();
            return Ok(thongTinThiLai);
        }

        [HttpGet]
        [Route("api/MonHoc/LayDsLichThiLaiTatCaCacMon")]
        public IHttpActionResult LayDsLichThiLaiTatCaCacMon()
        {
            var danhSachLichThiLai = _context.LichThiLai.Select(ltl => new
            {
                ltl.Id,
                ltl.ThoiGianThi,
                ltl.DiaDiemThi,
                ltl.MonHocId,
                ltl.MonHoc.TenMonHoc,
                ltl.DaThiXong
            }).ToList();
            return Ok(danhSachLichThiLai);
        }
        [HttpGet]
        [Route("api/MonHoc/LayDsLichThiLai/{monHocId}")]
        public IHttpActionResult LayDsLichThiLai(int monHocId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var danhSachLichThiLai = _context.LichThiLai.Where(ltl => ltl.MonHocId == monHocId).Select(ltl => new
            {
                ltl.Id,
                ltl.ThoiGianThi,
                ltl.DiaDiemThi,
                ltl.DaThiXong
            }).ToList();
            return Ok(danhSachLichThiLai);
        }
        [HttpPost]
        [Route("api/MonHoc/SaveLichThiLai")]
        public IHttpActionResult SaveLichThiLai(LichThiLai4SaveDto lichThiLaiDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            LichThiLai lichThiLai;
            if (lichThiLaiDto.Id == 0)
            {
                lichThiLai = Mapper.Map<LichThiLai4SaveDto, LichThiLai>(lichThiLaiDto);
                _context.LichThiLai.Add(lichThiLai);
                _context.SaveChanges();
                return Ok();
            }

            lichThiLai = _context.LichThiLai.SingleOrDefault(ltl => ltl.Id == lichThiLaiDto.Id);
            if (lichThiLai == null) return NotFound();
            Mapper.Map(lichThiLaiDto, lichThiLai);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/MonHoc/DangKiLichThiLai")]
        public IHttpActionResult DangKiLichThiLaiChoSv(DangKiThiLaiDto dangKiThiLaiDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var diemSinhVien = _context.Diem.Include(d => d.DanhSachDiemBoSung).SingleOrDefault(d =>
                d.MonHocId == dangKiThiLaiDto.MonHocId && d.SinhVienId == dangKiThiLaiDto.SinhVienId);
            if (diemSinhVien == null) return NotFound();
            var lichThiLai = _context.LichThiLai.SingleOrDefault(ltl => ltl.Id == dangKiThiLaiDto.LichThiLaiId);
            if (lichThiLai == null) return NotFound();
            if (lichThiLai.DaThiXong) return BadRequest(); //Nếu đã thi xong thì không cho đăng kí thêm sinh viên
            //Đăng kí lịch thi lại cho sinh viên sẽ reset Điểm thi cho sinh viên đó
            diemSinhVien.DangKiLichThiLai(dangKiThiLaiDto.LichThiLaiId);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/MonHoc/KetThucThiLai/{lichThiLaiId}")]
        public IHttpActionResult KetThucThiLai(int lichThiLaiId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var lichThiLai = _context.LichThiLai.SingleOrDefault(ltl => ltl.Id == lichThiLaiId);
            if (lichThiLai == null) return NotFound();
            lichThiLai.KetThucThiLai();
            _context.SaveChanges();
            return Ok();
        }



        private void ThemDiem(int monHocId, LoaiMon loaiMon, int lopId, int sinhVienId, HocKi hocKi)
        {
            ThemDiemTieuChuan(monHocId, lopId, sinhVienId, hocKi);

            if (loaiMon == LoaiMon.MonCPDT_TT) //Nếu là môn CPĐT-Thông tin
            {
                ThemDiemBs_CPDT(monHocId, sinhVienId);
            }
            else if (loaiMon == LoaiMon.MonTiengAnh)  //Nếu là môn Tiếng Anh
            {
                ThemDiemBs_TA(monHocId, sinhVienId);
            }
        }

        private void ThemDiemBs_TA(int monHocId, int sinhVienId)
        {
            _context.DiemBoSung.Add(new DiemBoSung()
            {
                MonHocId = monHocId,
                SinhVienId = sinhVienId,
                LoaiDiem = LoaiDiem.ThiViet_Ta
            }); //Thêm điểm thi bổ sung: TA Viết
            _context.DiemBoSung.Add(new DiemBoSung()
            {
                MonHocId = monHocId,
                SinhVienId = sinhVienId,
                LoaiDiem = LoaiDiem.ThiNghe_Ta
            }); //Thêm điểm thi bổ sung: TA Nghe
            _context.DiemBoSung.Add(new DiemBoSung()
            {
                MonHocId = monHocId,
                SinhVienId = sinhVienId,
                LoaiDiem = LoaiDiem.ThiNoi_Ta
            }); //Thêm điểm thi bổ sung: TA Nói
        }

        private void ThemDiemBs_CPDT(int monHocId, int sinhVienId)
        {
            _context.DiemBoSung.Add(new DiemBoSung()
            {
                MonHocId = monHocId,
                SinhVienId = sinhVienId,
                LoaiDiem = LoaiDiem.LyThuyet
            }); //Thêm điểm thi bổ sung: lý thuyết
            _context.DiemBoSung.Add(new DiemBoSung()
            {
                MonHocId = monHocId,
                SinhVienId = sinhVienId,
                LoaiDiem = LoaiDiem.ThucHanh
            }); //Thêm điểm thi bổ sung: Thực hành
        }

        private void ThemDiemTieuChuan(int monHocId, int lopId,int sinhVienId, HocKi hocKi)
        {
            _context.Diem.Add(new Diem(monHocId, lopId, sinhVienId, hocKi)); //Thêm điểm tiêu chuẩn
        }

    }
}
