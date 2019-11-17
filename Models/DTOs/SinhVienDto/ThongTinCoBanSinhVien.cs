using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class ThongTinCoBanSinhVien
    {
        public int Id { get; set; }

        public string HoVaTenLot { get; set; }

        public string Ten { get; set; }

        public DateTime NgaySinh { get; set; }

        public string TenLop { get; set; }

        public string DanToc { get; set; }

        public string TonGiao { get; set; }

        public string GioiTinh { get; set; }

        public string AnhDaiDien { get; set; }

        public string MSSV { get; set; }

        public HoiVienDoanVienDto DoanVien { get; set; }

        public HoiVienDoanVienDto HoiVien { get; set; }

        public HoiVienDoanVienDto DangVien { get; set; }
        public KhoaHocDto KhoaHoc { get; set; }
        public string GioiThieu { get; set; }
    }
}