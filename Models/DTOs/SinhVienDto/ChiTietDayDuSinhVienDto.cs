using System;
using System.Collections.Generic;
using NAPASTUDENT.Controllers.Api;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class ChiTietDayDuSinhVienDto
    {
        public int Id { get; set; }

        public string HoVaTenLot { get; set; }

        public string Ten { get; set; }

        public DateTime NgaySinh { get; set; }

        public string CMND { get; set; }

        public string TenLop { get; set; }
        public int LopId { get; set; }

        public string DanToc { get; set; }

        public string TonGiao { get; set; }

        public string GioiTinh { get; set; }

        public int DanTocId { get; set; }

        public int TonGiaoId { get; set; }

        public int GioiTinhId { get; set; }

        public IList<SDTDto> SDT { get; set; }

        public IList<DiaChiDto> DiaChi { get; set; }

        public string AnhDaiDien { get; set; }

        public string MSSV { get; set; }

        public HoiVienDoanVienDto DoanVien { get; set; }

        public HoiVienDoanVienDto HoiVien { get; set; }

        public HoiVienDoanVienDto DangVien { get; set; }

        public bool DaRaTruong { get; set; }

        public KhoaHocDto KhoaHoc { get; set; }


    }

    public class HoiVienDoanVienDto
    {
        public DateTime NgayVao { get; set; }
        public string NoiVao { get; set; }
    }
}