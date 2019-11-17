using System;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class ThamGiaHoatDongDto
    {
        public int Id { get; set; }
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public string MSSV { get; set; }
        public string KyHieuTenLop { get; set; }
        public string Sdt { get; set; }
        public DateTime NgayThamGia { get; set; }
        public bool DuocPheDuyet { get; set; }

    }
}