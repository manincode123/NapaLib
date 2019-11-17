using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class DangKiThiLaiDto
    {
        public int LichThiLaiId { get; set; }

        public int MonHocId { get; set; }

        public int SinhVienId { get; set; }
    }
}