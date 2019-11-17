using System.Collections.Generic;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Models.DTOs.LopDtos
{
    public class HoatDongLopDto
    {
        public int Id { get; set; }
        public string TenLop { get; set; }

        public string KyHieuTenLop { get; set; }

        public int KhoaHocId { get; set; }
        public KhoaHocDto KhoaHoc { get; set; }

        public virtual IList<HoatDongDtoForTable> DanhSachHoatDongToChuc { get; set; }

        public int SoHdThangNay { get; set; }
        public int SoHdDangDienRa { get; set; }
        public int SoHdNamNay { get; set; }
        public int SoHdSvThamGiaNamNay { get; set; }
        public int SoHdSvThamGiaThangNay { get; set; }
        public int SoHoatDongChoPheDuyet { get; set; }
    }
}