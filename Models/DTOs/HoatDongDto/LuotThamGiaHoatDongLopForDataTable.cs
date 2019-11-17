using System;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class LuotThamGiaHoatDongLopForDataTable
    {
        public int HoatDongId { get; set; }
        public string TenHoatDong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SoLuotThamGiaLop { get; set; }
        public int SoLuotThamGiaToanHoatDong { get; set; }

    }
}