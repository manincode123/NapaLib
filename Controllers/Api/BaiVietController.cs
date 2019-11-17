using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.BaiVietDtos;
using NAPASTUDENT.Models.DTOs.ChuyenMucDtos;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Controllers.Api
{
    public class BaiVietController : ApiController
    {
        private ApplicationDbContext _context;
        public BaiVietController()
        {
            _context = new ApplicationDbContext();
        }

        //Các API cho việc quản lý bài viết
            //Lấy danh sách bài viết để quản lý
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/DanhSachQuanLy")]
        public IHttpActionResult DanhSachQuanLy(PagingSearchingSorting pagingSearchingSorting)
        {
            //Tên cột để sort cho sql server
            // @ColumnName = 'SoLuotXem' THEN DanhSachTamCTE.SoLuotXem 
            // @ColumnName = 'NgayTao' THEN DanhSachTamCTE.NgayTao
            if (!ModelState.IsValid) return BadRequest();
            List<BaiVietResultSetForDataTable> baiVietResultSet;
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                baiVietResultSet = _context.Database
                    .SqlQuery<BaiVietResultSetForDataTable>("LayDanhSachBaiVietASC @SearchTerm, " +
                                                "@ColumnName, @RecordStart,@PageSize",
                        searchTerm, columnName, recordStart, pageSize).ToList();
            }
            else
            {
                baiVietResultSet = _context.Database
                    .SqlQuery<BaiVietResultSetForDataTable>("LayDanhSachBaiVietDESC @SearchTerm," +
                                                " @ColumnName, @RecordStart,@PageSize",
                        searchTerm, columnName, recordStart, pageSize).ToList();
            }

            int recordsTotal, recordsFiltered;
            if (baiVietResultSet.Count > 0)
            {
                recordsTotal = baiVietResultSet[0].TotalCount;
                recordsFiltered = baiVietResultSet[0].FilteredCount;
            }
            else
            {
                recordsTotal = _context.DanhSachBaiViet.Count();
                recordsFiltered = 0;
            }
            //Tính số trang dựa trên pageIndex và pageSize
            var pageNumbers = (int)Math.Ceiling((double)recordsTotal / pagingSearchingSorting.PageSize);
            return Ok(new
            {
                pagingSearchingSorting.Draw,
                data = baiVietResultSet,
                pageNumbers,
                pagingSearchingSorting.PageIndex,
                recordsFiltered,
                recordsTotal
            });
        }

            //Lấy danh sách bài viết chờ phê duyệt
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/DanhSachPheDuyet")]
        public IHttpActionResult LayDanhSachPheDuyet()
        {
            var danhSachPheDuyet = _context.DanhSachBaiViet.Where(bv => !bv.DuocPheDuyet && !bv.DaXoa)
                .Select(bv => new
                {
                    bv.Id,
                    bv.TenBaiViet,
                    bv.SoLuoc,
                    bv.AnhBia,
                    bv.ChuyenMucBaiViet.TenChuyenMuc,
                    bv.NgayTao,
                    NguoiTao = new
                    {
                        bv.NguoiTao.Id,
                        TenNguoiTao = bv.NguoiTao.HoVaTenLot + " " + bv.NguoiTao.Ten
                    }
                }).ToList();
            return Ok(danhSachPheDuyet);
        }

            //Phê duyệt bài viết
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/PheDuyet/{baiVietId}")]
        public IHttpActionResult PheDuyetBaiViet(int baiVietId)
        {
            var baiViet = _context.DanhSachBaiViet.SingleOrDefault(bv => bv.Id == baiVietId);
            if (baiViet == null) return NotFound();
            baiViet.PheDuyetBaiViet();
            _context.SaveChanges();
            return Ok();
        }

            //Save Bài viết
        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/SaveBaiViet")]
        public IHttpActionResult SaveBaiViet(SaveBaiVietDto baiVietDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var baiViet = new BaiViet();
            var userSinhVienId = User.Identity.GetSinhVienId();
            //Nếu là tạo bài viết mới
            if (baiVietDto.Id == 0)
            {
                if (userSinhVienId == 0) return BadRequest("Hãy liên kết tài khoản này với một sinh viên để tạo bài viết");
                baiViet.TaoBaiViet(baiVietDto, userSinhVienId);
                _context.DanhSachBaiViet.Add(baiViet);
                _context.SaveChanges();
                return Ok();
            }
            //Nếu là chỉnh sửa bài viết có sẵn
            baiViet = _context.DanhSachBaiViet
                .Include(bv => bv.BaiVietHoatDong)
                .Include(bv => bv.BaiVietDonVi)
                .Include(bv => bv.BaiVietLop)
                .SingleOrDefault(cthd => cthd.Id == baiVietDto.Id);
            if (baiViet == null) return NotFound();
            //Check quyền sửa bài viết
            if (baiViet.NguoiTaoId != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyBaiViet"))
                return BadRequest();
            baiViet.ChinhSuaBaiViet(baiVietDto);
            _context.SaveChanges();
            return Ok();
        }

            //Xóa bài viết
        [HttpDelete]
        [Authorize]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/Xoa/{baiVietId}")]
        public IHttpActionResult XoaBaiViet(int baiVietId)
        {
            var baiViet = _context.DanhSachBaiViet.SingleOrDefault(bv => bv.Id == baiVietId);
            if (baiViet == null) return NotFound();
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (baiViet.NguoiTaoId != userSinhVienId && !User.IsInRole("Admin") && !User.IsInRole("QuanLyBaiViet"))
                return BadRequest();
            baiViet.XoaBaiViet();
            _context.SaveChanges();
            return Ok();
        }

        //Các API lấy bài viết
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/{baiVietId}")]
        public IHttpActionResult LayThongTinBaiViet(int baiVietId)
        {
            var baiViet = _context.DanhSachBaiViet.Where(bv => bv.Id == baiVietId)
                .Select(bv => new
                {
                    bv.Id,
                    bv.TenBaiViet,
                    bv.AnhBia,
                    bv.SoLuoc,
                    bv.NoiDungBaiViet,
                    bv.ChuyenMucBaiVietId,
                    bv.ChuyenMucBaiViet.TenChuyenMuc,
                    bv.NgayTao,
                    nguoiTao = new {
                        bv.NguoiTao.Ten,
                        bv.NguoiTao.HoVaTenLot,
                        bv.NguoiTao.Id
                    },
                    donViTag = bv.BaiVietDonVi.Select(bvdv => new
                    {
                        bvdv.DonVi.Id,
                        bvdv.DonVi.TenDonVi
                    }), 
                    lopTag = bv.BaiVietLop.Select(bvl => new
                    {
                        bvl.Lop.Id,
                        bvl.Lop.TenLop
                    }) ,
                    hoatDongTag = bv.BaiVietHoatDong.Select(bvhd => new
                    {
                        bvhd.HoatDong.Id,
                        bvhd.HoatDong.TenHoatDong,
                        bvhd.HoatDong.AnhBia,
                        bvhd.HoatDong.NgayBatDau,
                        bvhd.HoatDong.NgayKetThuc,
                        bvhd.HoatDong.DiaDiem
                    })

                }).SingleOrDefault();
            if (baiViet == null) return NotFound();
            _context.DanhSachBaiViet.SingleOrDefault(bv => bv.Id == baiVietId).TangLuotXem();
            _context.SaveChanges();
            return Ok(baiViet);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/TrangChu")]
        public IHttpActionResult BaiVietTrangChu()
        {
            
            var command = _context.Database.Connection.CreateCommand();
            command.CommandText = "[dbo].[QueryBaiVietTrangChu]";
            command.CommandType = CommandType.StoredProcedure;
           
            IList<ChuyenMucDto> chuyenMucDto = new List<ChuyenMucDto>();
            try
            {

                _context.Database.Connection.Open();
                var reader = command.ExecuteReader();

                //Lấy bài viết theo chuyên mục (result set đầu tiên)
                var baiVietResultSet = ((IObjectContextAdapter)_context)
                    .ObjectContext
                    .Translate<BaiVietResultSet>(reader).ToList();

                //Lấy bài viết nổi bật  (result set thứ hai)
                reader.NextResult();
                baiVietResultSet.AddRange(((IObjectContextAdapter)_context)
                    .ObjectContext
                    .Translate<BaiVietResultSet>(reader).ToList());

                //Lấy chuyên mục (result set thứ ba)
                reader.NextResult();
                var chuyenMucResultSet = ((IObjectContextAdapter)_context)
                    .ObjectContext
                    .Translate<ChuyenMucResultSet>(reader).ToList();
                
                chuyenMucDto.Add(new ChuyenMucDto
                {
                    Id = 0,
                    TenChuyenMuc = "Nổi bật"
                });

                foreach (var item in chuyenMucResultSet)
                {
                    if (item.ChuyenMucChaId == null) //Nếu là chuyên mục gốc
                         chuyenMucDto.Add(new ChuyenMucDto
                         {
                             Id = item.Id,
                             TenChuyenMuc = item.TenChuyenMuc
                         });
                     else //Nếu là chuyên mục con
                     {
                         var chuyenMucCha = chuyenMucDto.SingleOrDefault(cm => cm.Id == item.ChuyenMucChaId);
                         if (chuyenMucCha == null) continue;
                         //Thêm chuyên mục con này vào chuyên mục cha, nếu không có chyên mục cha tương ứng, bỏ qua
                         chuyenMucDto.SingleOrDefault(cm => cm.Id == item.ChuyenMucChaId)
                         .ChuyenMucCon.Add(new ChuyenMucConDto
                         {
                             Id = item.Id,
                             TenChuyenMuc = item.TenChuyenMuc
                         });
                     }   
                }

                foreach (var baiViet in baiVietResultSet)
                {
                    chuyenMucDto.SingleOrDefault(cm => cm.Id == baiViet.ChuyenMucGocId)
                        .BaiViet.Add(new BaiVietSoLuocDto
                        {
                            Id = baiViet.Id,
                            TenBaiViet = baiViet.TenBaiViet,
                            AnhBia = baiViet.AnhBia,
                            SoLuoc = baiViet.SoLuoc
                        });
                }
                reader.Close();
            }
            finally { _context.Database.Connection.Close(); }
            return Ok(chuyenMucDto);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/ChuyenMuc/{chuyenMucId}")]
        public IHttpActionResult BaiVietChuyenMuc(int chuyenMucId)
        {

            IList<ChuyenMucDto> chuyenMucDto = new List<ChuyenMucDto>();
            var tenChuyenMucMuonLay = "";
            var tenChuyenMucCha = "";
            var chuyenMucChaId = 0;
            var command = _context.Database.Connection.CreateCommand();
            command.CommandText = "[dbo].[QueryBaiVietChuyenMuc]";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@ChuyenMucId", chuyenMucId));
            try
            {
                _context.Database.Connection.Open();
                var reader = command.ExecuteReader();

                var baiVietResultSet = ((IObjectContextAdapter)_context)
                    .ObjectContext
                    .Translate<BaiVietResultSet>(reader).ToList();

                reader.NextResult();

                var chuyenMucResultSet = ((IObjectContextAdapter)_context)
                    .ObjectContext
                    .Translate<ChuyenMucResultSet>(reader).ToList();

                chuyenMucDto.Add(new ChuyenMucDto
                {
                    Id = 0,
                    TenChuyenMuc = "Tin mới nhất"
                });

                foreach (var item in chuyenMucResultSet)
                {
                    //Nếu chuyên mục này là chuyên mục muốn lấy
                    if (item.Id == chuyenMucId)
                    {
                        //Gán tên chuyên mục muốn lấy
                        tenChuyenMucMuonLay = item.TenChuyenMuc;
                        //Nếu mà chuyên mục muốn lấy có chuyên mục cha
                        if (item.ChuyenMucChaId != null)
                        {
                            tenChuyenMucCha = item.TenChuyenMucCha;
                            chuyenMucChaId = (int) item.ChuyenMucChaId;
                        }
                    }
                    //Nếu là chuyên mục con của chuyên mục muốn lấy
                    if (item.ChuyenMucChaId == chuyenMucId) 
                        chuyenMucDto.Add(new ChuyenMucDto
                        {
                            Id = item.Id,
                            TenChuyenMuc = item.TenChuyenMuc
                        }); 
                    else
                    {
                        var chuyenMucCon = chuyenMucDto.SingleOrDefault(cm => cm.Id == item.ChuyenMucChaId);
                        if (chuyenMucCon == null) continue;
                        chuyenMucDto.SingleOrDefault(cm => cm.Id == item.ChuyenMucChaId)
                        .ChuyenMucCon.Add(new ChuyenMucConDto
                        {
                            Id = item.Id,
                            TenChuyenMuc = item.TenChuyenMuc
                        });
                    }   //Nếu là chuyên mục con
                }

                foreach (var baiViet in baiVietResultSet)
                {
                    chuyenMucDto.SingleOrDefault(cm => cm.Id == baiViet.ChuyenMucGocId)
                        .BaiViet.Add(new BaiVietSoLuocDto
                        {
                            Id = baiViet.Id,
                            TenBaiViet = baiViet.TenBaiViet,
                            AnhBia = baiViet.AnhBia,
                            SoLuoc = baiViet.SoLuoc,
                            NgayTao = baiViet.NgayTao,
                            SoLuotXem = baiViet.SoLuotXem
                        });
                }
            }
            finally { _context.Database.Connection.Close();   }

            return Ok( new {
                tenChuyenMucGoc = tenChuyenMucMuonLay,
                tenChuyenMucCha,
                chuyenMucChaId,
                chuyenMucDto
            });

        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/LayBaiVietChuyenMucTiepTheo/{chuyenMucId}/{recordStart}")]
        public IHttpActionResult LayBaiVietChuyenMucTiepTheo(int chuyenMucId, int recordStart = 10)
        {
            //Action này dùng để lấy các bài viết mới tiếp theo của chuyên mục, mỗi lần lấy 10 bài
            //10 bài viết mới nhất đầu tiên được lấy chung trong BaiVietChuyenMuc (ở trên) nên recordStart = 10 
            List<BaiVietResultSet> baiVietResultSet;

            var chuyenMucIdParameter = new SqlParameter("@ChuyenMucId", chuyenMucId);
            var recordStartParameter = new SqlParameter("@RecordStart", recordStart);

            baiVietResultSet = _context.Database
                                       .SqlQuery<BaiVietResultSet>("LayBaiVietChuyenMucTiepTheo " +
                                                                   "@ChuyenMucId, @RecordStart",
                                                                   chuyenMucIdParameter,recordStartParameter)
                                       .ToList();
            return Ok(baiVietResultSet.Select(Mapper.Map<BaiVietResultSet, BaiVietSoLuocDto>));
        }
        
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/LayBaiVietSinhVien/{sinhVienId}/{recordStart}")]
        public IHttpActionResult LayBaiVietSinhVien(int sinhVienId, int recordStart = 0)
        {
            List<BaiVietResultSet> baiVietResultSet;
            //Thông tin sinh viên sẽ được lấy ở MVC controller, để check 1 lần luôn
            var sinhVienIdParameter = new SqlParameter("@SinhVienId", sinhVienId);
            var recordStartParameter = new SqlParameter("@RecordStart", recordStart);

            baiVietResultSet = _context.Database
                                       .SqlQuery<BaiVietResultSet>("LayBaiVietSinhVien " +
                                                                   "@SinhVienId, @RecordStart",
                                                                   sinhVienIdParameter, recordStartParameter)
                                       .ToList();
            return Ok(baiVietResultSet.Select(Mapper.Map<BaiVietResultSet, BaiVietSoLuocDto>));
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/LayBaiVietLop/{lopId}/{recordStart}")]
        public IHttpActionResult LayBaiVietLop(int lopId, int recordStart = 0)
        {
            List<BaiVietResultSet> baiVietResultSet;
            //Thông tin sinh viên sẽ được lấy ở MVC controller, để check 1 lần luôn
            var lopIdParameter = new SqlParameter("@LopId", lopId);
            var recordStartParameter = new SqlParameter("@RecordStart", recordStart);

            baiVietResultSet = _context.Database
                                       .SqlQuery<BaiVietResultSet>("LayBaiVietLop " +
                                                                   "@LopId, @RecordStart",
                                                                    lopIdParameter, recordStartParameter)
                                       .ToList();
            return Ok(baiVietResultSet.Select(Mapper.Map<BaiVietResultSet, BaiVietSoLuocDto>));
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/DonVi/{donViId}/{recordStart}")]
        public IHttpActionResult LayBaiVietDonVi(int donViId, int recordStart = 0)
        {
            List<BaiVietResultSet> baiVietResultSet;
            var donViIdParameter = new SqlParameter("@DonViId", donViId);
            var recordStartParameter = new SqlParameter("@RecordStart", recordStart);

            baiVietResultSet = _context.Database
                                       .SqlQuery<BaiVietResultSet>("LayBaiVietDonVi " +
                                                                   "@DonViId, @RecordStart",
                                                                    donViIdParameter, recordStartParameter)
                                       .ToList();
            return Ok(baiVietResultSet.Select(Mapper.Map<BaiVietResultSet, BaiVietSoLuocDto>));
        }

        [Authorize]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/BaiViet/BaiVietCaNhan")]
        public IHttpActionResult LayBaiVietCaNhan(PagingSearchingSorting pagingSearchingSorting)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Lấy Sinh viên Id của User
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (userSinhVienId == 0) return BadRequest();
            //Lấy danh sách bài viết của sinh viên
            if (!ModelState.IsValid) return BadRequest();
            List<BaiVietResultSetForDataTable> baiVietResultSet;
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            var sinhVienId = new SqlParameter("@SinhVienId", userSinhVienId);
            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                baiVietResultSet = _context.Database
                    .SqlQuery<BaiVietResultSetForDataTable>("LayBaiVietCaNhanASC " +
                                                            "@SearchTerm,@ColumnName, " +
                                                            "@RecordStart,@PageSize, @SinhVienId",
                                                            searchTerm, columnName,
                                                            recordStart, pageSize, sinhVienId)
                    .ToList();
            }
            else
            {
                baiVietResultSet = _context.Database
                    .SqlQuery<BaiVietResultSetForDataTable>("LayBaiVietCaNhanDESC " +
                                                            "@SearchTerm,@ColumnName, " +
                                                            "@RecordStart,@PageSize, @SinhVienId",
                                                             searchTerm, columnName, 
                                                             recordStart, pageSize, sinhVienId)
                    .ToList();
            }

            int recordsTotal, recordsFiltered;
            if (baiVietResultSet.Count > 0)
            {
                recordsTotal = baiVietResultSet[0].TotalCount;
                recordsFiltered = baiVietResultSet[0].FilteredCount;
            }
            else
            {
                recordsTotal = _context.DanhSachHoiVienHoiSinhVien.Count();
                recordsFiltered = 0;
            }
            //Tính số trang dựa trên pageIndex và pageSize
            var pageNumbers = (int)Math.Ceiling((double)recordsTotal / pagingSearchingSorting.PageSize);
            return Ok(new
            {
                pagingSearchingSorting.Draw,
                data = baiVietResultSet,
                pageNumbers,
                pagingSearchingSorting.PageIndex,
                recordsFiltered,
                recordsTotal
            });
        }
        
    }
}
