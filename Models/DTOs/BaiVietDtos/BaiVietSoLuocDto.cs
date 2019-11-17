using System;

namespace NAPASTUDENT.Models.DTOs
{
    public class BaiVietSoLuocDto
    {
        public int Id { get; set; }
        public string TenBaiViet { get; set; }

        public string SoLuoc { get; set; }

        public int SoLuotXem { get; set; }

        public string AnhBia { get; set; }

        public DateTime NgayTao { get; set; }
    }
}