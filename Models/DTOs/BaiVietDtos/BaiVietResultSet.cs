using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.BaiVietDtos
{
    public class BaiVietResultSet
    {

        public int Id { get; set; }
        public string TenBaiViet { get; set; }

        public string SoLuoc { get; set; }

        public int SoLuotXem { get; set; }

        public string AnhBia { get; set; }

        public DateTime NgayTao { get; set; }

        public int ChuyenMucGocId { get; set; }

        public string TenChuyenMuc { get; set; }

    }
}