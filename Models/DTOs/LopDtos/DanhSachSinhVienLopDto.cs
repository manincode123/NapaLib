using System.Collections.Generic;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Models.DTOs.LopDtos
{
    public class DanhSachSinhVienLopDto
    {
        public int Id { get; set; }
        public string TenLop { get; set; }

        public virtual IList<SinhVienDtoForTable> DanhSachSinhVien { get; set; }

        public string KyHieuTenLop { get; set; }

        public int soLuongSV { get; set; }
        public int soNam { get; set; }
        public int soNu { get; set; }
        public int khac { get; set; }
    }

}