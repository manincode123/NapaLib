using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class ThemXoaSinhVienDto
    {
        public int SinhVienId { get; set; }
        public int LopId { get; set; }
        public bool LopChuyenNganh { get; set; }
    }
}