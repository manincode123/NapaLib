using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class DiemForTableDto
    {
        public byte? DiemChuyenCan { get; set; }

        public byte? DiemDieuKien1 { get; set; }

        public byte? DiemDieuKien2 { get; set; }

        public byte? DiemThi { get; set; }
        public byte? DiemTb { get; set; }

        public IList<DiemBoSungForTableDto> DanhSachDiemBoSung { get; set; }
        public string TenMonHoc { get; set; }
        public byte SoHocPhan { get; set; }
        public byte SoTiet { get; set; }
        public LoaiMon LoaiMon { get; set; }
        public bool HaiDiemDk { get; set; }
    }

    public class DiemBoSungForTableDto
    {
        public byte? Diem { get; set; }

        public LoaiDiem LoaiDiem { get; set; }
    }
}