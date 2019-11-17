using System;

namespace NAPASTUDENT.Models.DTOs
{
    public class BaiVietDtoForDataTable
    {
        //Dto này để gửi tới Datable
        public int Id { get; set; }
        public string TenBaiViet { get; set; }

        public string SoLuoc { get; set; }

        public string AnhBia { get; set; }

        public string NgayTao { get; set; }
        public string HoVaTenLot { get; set; }
        public string TenSinhVien { get; set; }
        public string SoLuotXem { get; set; }
        public string TenChuyenMuc { get; set; }
    }
}