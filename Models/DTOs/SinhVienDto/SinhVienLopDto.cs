using System;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class SinhVienDtoForTable
    {
        public SinhVienDtoForTable()
        {
            
        }
        public SinhVienDtoForTable(SinhVien sv)
        {
            Id = sv.Id;
            NgaySinh = sv.NgaySinh;
            AnhDaiDien = sv.AnhDaiDien;
            GioiTinh = sv.GioiTinh.TenGioiTinh;
            HoVaTenLot = sv.HoVaTenLot;
            Ten = sv.Ten;
        }

        public int Id { get; set; }

        public string HoVaTenLot { get; set; }

        public string Ten { get; set; }

        public DateTime NgaySinh { get; set; }

        public string GioiTinh { get; set; }

        public string AnhDaiDien { get; set; }

        public string TenLop { get; set; }

        public string MSSV { get; set; }
    }
}