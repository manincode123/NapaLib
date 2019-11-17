using System;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class ThongTinCoBanHoatDongDto
    {
        public int Id { get; set; }

        public string TenHoatDong { get; set; }

        public bool DaKetThuc { get; set; }

        //public DateTime NgayBatDau { get; set; }

        //public DateTime NgayKetThuc { get; set; }
    }
}