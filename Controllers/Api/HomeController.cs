using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Controllers.Api
{
    public class HomeController : ApiController
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("api/TimKiem/TimKiemChung")]
        public IHttpActionResult TimKiemChung(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            if (searchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", searchTerm);
            var result = _context.Database.SqlQuery<TimKiemChungDto>("SearchTenChung @SearchTerm", param1).ToList();
            return Ok(result);
        }
        /*Các API cho sinh viên dùng (không nhất thiết là quản lý)*/
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/TimKiem/SinhVien")]
        public IHttpActionResult TimKiemSinhVien(PagingSearchingSorting pagingSearchingSorting)
        {
            pagingSearchingSorting.SearchTerm = pagingSearchingSorting.SearchTerm.Trim();
            if (pagingSearchingSorting.SearchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var param2 = new SqlParameter("@CurrentPage", pagingSearchingSorting.PageIndex);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            var danhSachSinhVien = _context.Database.SqlQuery<KetQuaTimKiemSinhVien>("SearchSinhVien @SearchTerm, @CurrentPage, @PageSize", param1, param2, pageSize).ToList();
            var totalRecords = 0;
            var pageNumbers = 0;

            if (danhSachSinhVien.Count > 0)
            {
                totalRecords = danhSachSinhVien[0].TotalRecords;
                //Tính số trang dựa trên pageIndex và pageSize
                pageNumbers = (int)Math.Ceiling((double)totalRecords / pagingSearchingSorting.PageSize);
            }
            return Ok(new
            {
                danhSachSinhVien,
                totalRecords,
                pageNumbers
            });
        }        
        
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/TimKiem/HoatDong")]
        public IHttpActionResult TimKiemHoatDong(PagingSearchingSorting pagingSearchingSorting)
        {
            pagingSearchingSorting.SearchTerm = pagingSearchingSorting.SearchTerm.Trim();
            if (pagingSearchingSorting.SearchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var param2 = new SqlParameter("@CurrentPage", pagingSearchingSorting.PageIndex);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            var danhSachHoatDong = _context.Database.SqlQuery<KetQuaTimKiemHoatDong>("SearchHoatDong @SearchTerm, @CurrentPage, @PageSize", param1, param2, pageSize).ToList();
            var totalRecords = 0;
            var pageNumbers = 0;

            if (danhSachHoatDong.Count > 0)
            {
                totalRecords = danhSachHoatDong[0].TotalRecords;
                //Tính số trang dựa trên pageIndex và pageSize
                pageNumbers = (int)Math.Ceiling((double)totalRecords / pagingSearchingSorting.PageSize);
            }
            return Ok(new
            {
                danhSachHoatDong,
                totalRecords,
                pageNumbers
            });
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/TimKiem/BaiViet")]
        public IHttpActionResult TimKiemBaiViet(PagingSearchingSorting pagingSearchingSorting)
        {
            pagingSearchingSorting.SearchTerm = pagingSearchingSorting.SearchTerm.Trim();
            if (pagingSearchingSorting.SearchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var param2 = new SqlParameter("@CurrentPage", pagingSearchingSorting.PageIndex);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            var danhSachBaiViet = _context.Database.SqlQuery<KetQuaTimKiemBaiViet>("SearchBaiViet @SearchTerm, @CurrentPage, @PageSize", param1, param2, pageSize).ToList();
            var totalRecords = 0;
            var pageNumbers = 0;

            if (danhSachBaiViet.Count > 0)
            {
                totalRecords = danhSachBaiViet[0].TotalRecords;
                //Tính số trang dựa trên pageIndex và pageSize
                pageNumbers = (int)Math.Ceiling((double)totalRecords / pagingSearchingSorting.PageSize);
            }
            return Ok(new
            {
                danhSachBaiViet,
                totalRecords,
                pageNumbers
            });
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/TimKiem/DonVi")]
        public IHttpActionResult TimKiemDonVi(PagingSearchingSorting pagingSearchingSorting)
        {
            pagingSearchingSorting.SearchTerm = pagingSearchingSorting.SearchTerm.Trim();
            if (pagingSearchingSorting.SearchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var param2 = new SqlParameter("@CurrentPage", pagingSearchingSorting.PageIndex);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            var danhSachDonVi = _context.Database.SqlQuery<KetQuaTimKiemDonVi>("SearchDonVi @SearchTerm, @CurrentPage, @PageSize", param1, param2, pageSize).ToList();
            var totalRecords = 0;
            var pageNumbers = 0;

            if (danhSachDonVi.Count > 0)
            {
                totalRecords = danhSachDonVi[0].TotalRecords;
                //Tính số trang dựa trên pageIndex và pageSize
                pageNumbers = (int)Math.Ceiling((double)totalRecords / pagingSearchingSorting.PageSize);
            }
            return Ok(new
            {
                danhSachDonVi,
                totalRecords,
                pageNumbers
            });
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/TimKiem/Lop")]
        public IHttpActionResult TimKiemLop(PagingSearchingSorting pagingSearchingSorting)
        {
            pagingSearchingSorting.SearchTerm = pagingSearchingSorting.SearchTerm.Trim();
            if (pagingSearchingSorting.SearchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var param2 = new SqlParameter("@CurrentPage", pagingSearchingSorting.PageIndex);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            var danhSachLop = _context.Database.SqlQuery<KetQuaTimKiemLop>("SearchLop @SearchTerm, @CurrentPage, @PageSize", param1, param2,pageSize).ToList();
            var totalRecords = 0;
            var pageNumbers = 0;

            if (danhSachLop.Count > 0)
            {
                totalRecords = danhSachLop[0].TotalRecords;
                //Tính số trang dựa trên pageIndex và pageSize
                pageNumbers = (int)Math.Ceiling((double)totalRecords / pagingSearchingSorting.PageSize);
            }
            return Ok(new
            {
                danhSachLop,
                totalRecords,
                pageNumbers
            });
        }

    }

    public class KetQuaTimKiemLop
    {
        public int LopId { get; set; }
        public string TenLop { get; set; }
        public string KyHieuTenLop { get; set; }
        public bool LopChuyenNganh { get; set; }
        public string AnhBia { get; set; }
        public string TenKhoa { get; set; }
        public int TotalRecords { get; set; }

    }

    public class KetQuaTimKiemDonVi
    {
        public int DonViId { get; set; }
        public string TenDonVi { get; set; }
        public LoaiDonVi LoaiDonVi { get; set; }
        public DateTime NgayThanhLap { get; set; }
        public string AnhBia { get; set; }
        public int TotalRecords { get; set; }

    }

    public class KetQuaTimKiemBaiViet
    {
        public int Id { get; set; }
        public string TenBaiViet { get; set; }
        public string SoLuoc { get; set; }
        public int SoLuotXem { get; set; }
        public string AnhBia { get; set; }
        public DateTime NgayTao { get; set; }
        public int ChuyenMucId { get; set; }
        public string TenChuyenMuc { get; set; }
        public int TotalRecords { get; set; }
    }

    public class KetQuaTimKiemHoatDong
    {
        public int Id { get; set; }

        public string TenHoatDong { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool DaKetThuc { get; set; }

        public bool BiHuy { get; set; }

        public string DiaDiem { get; set; }
        public string SoLuoc { get; set; }
        public int SoLuotThamGia { get; set; }

        public string AnhBia { get; set; }
        public int TotalRecords { get; set; }

    }

    public class KetQuaTimKiemSinhVien
    {
        public int Id { get; set; }
        public string HoVaTenLot { get; set; }

        public string Ten { get; set; }

        public string AnhDaiDien { get; set; }

        public string MSSV { get; set; }
        public string KhoaHoc { get; set; }

        public string KyHieuTenLop { get; set; }
        public DateTime NgaySinh { get; set; }

        public int TotalRecords { get; set; }
    }

    public class TimKiemChungDto
    {
        public string KetQua { get; set; }
    }
}
