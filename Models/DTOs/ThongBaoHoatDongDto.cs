using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs
{
    public class ThongBaoHoatDongDto
    {
        public LoaiThongBaoHoatDong LoaiThongBaoHoatDong { get; private set; }
        public DateTime NgayTaoThongBao { get; private set; }
        public DateTime? NgayBatDauGoc { get; set; }
        public DateTime? NgayKetThucGoc { get; set; }
        public string DiaDiemGoc { get; set; }
        public int HoatDongId { get; set; }
        public string AnhBia { get; set; }
        public string TenHoatDong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string DiaDiem { get; set; }
      
    }
}