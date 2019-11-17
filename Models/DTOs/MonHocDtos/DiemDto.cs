using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class DiemDto
    {
        public int SinhVienId { get; set; }

        public int MonHocId { get; set; }

        [Range(0, 10)]

        public byte? DiemChuyenCan { get; set; }

        [Range(0, 10)]
        public byte? DiemDieuKien1 { get; set; }

        [Range(0, 10)]
        public byte? DiemDieuKien2 { get; set; }

        [Range(0, 10)]
        public byte? DiemThi { get; set; }

        public IList<DiemBoSungDto> DanhSachDiemBoSung { get; set; }

    }

    public class DiemBoSungDto
    {
        public int Id { get; set; }

        [Range(0, 10)]
        public byte? Diem { get; set; }

        public LoaiDiem LoaiDiem { get; set; }

    }
}