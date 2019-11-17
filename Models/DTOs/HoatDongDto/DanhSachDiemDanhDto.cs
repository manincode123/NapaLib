using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class DanhSachDiemDanhDto
    {
        public int Id { get; set; }
        public string TenHoatDong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public bool DaKetThuc { get; set; }
        public bool DuocPheDuyet { get; set; }
        public bool BiHuy { get; set; }
        //public IList<ThamGiaHoatDongDto> DanhSachSinhVienThamGia { get; set; }    Không cần cái này vì  DanhSachSinhVienThamGia sẽ sử dụng ajax
        public IList<ThamGiaHoatDongDto> DanhSachSinhVienDangKi { get; set; }
        public int IdSinhVienTao { get; set; }
    }
}