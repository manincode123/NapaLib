using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs
{
    public class DanhSachLopDto
    {
        public int Id { get; set; }
        public string TenLop { get; set; }

        public string KyHieuTenLop { get; set; }
        public string AnhBia { get; set; }
        public string TenKhoaHoc { get; set; }

    }
}