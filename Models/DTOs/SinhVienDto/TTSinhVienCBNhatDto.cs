using System;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class TTSinhVienCBNhatDto
    {
        public TTSinhVienCBNhatDto()
        {
            
        }
        public TTSinhVienCBNhatDto(SinhVien sinhVien)
        {
            AnhDaiDien = sinhVien.AnhDaiDien;
            HoVaTenLot = sinhVien.HoVaTenLot;
            Id = sinhVien.Id;
            MSSV = sinhVien.MSSV;
            Ten = sinhVien.Ten;
            NgaySinh = sinhVien.NgaySinh;
        }

        public int Id { get; set; }
        public string HoVaTenLot { get; set; }

        public string Ten { get; set; }

        public string AnhDaiDien { get; set; }

        public string MSSV { get; set; }
        public string KhoaHoc { get; set; }

        public string KyHieuTenLop { get; set; }
        public DateTime NgaySinh { get; set; }




    }
}