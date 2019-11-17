using System.Collections.Generic;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Models.DTOs.LopDtos
{
    public class LopDto
    {
        public int Id { get; set; }
        public string TenLop { get; set; }

        public IList<SinhVienDtoForTable> DanhSachSinhVien { get; set; }

        public string KyHieuTenLop { get; set; }

        public int KhoaHocId { get; set; }
        public string KhoaHoc { get; set; }

        public IList<ChucVuLopDto> ChucVuLop { get; set; }
        public int soLuongSV { get; set; }
        public int soNam { get; set; }
        public int soNu { get; set; }
        public int khac { get; set; }
        public int SoHdThangNay { get; set; }
        public int SoHdDangDienRa { get; set; }
        public int TongSoHoatDong { get; set; }
        public int soHdSvThamGiaNamNay { get; set; }
        public int soHdSvThamGiaThangNay { get; set; }
        public string AnhBia { get; set; }
    }
}