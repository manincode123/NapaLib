using System;
using System.Collections.Generic;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class DangKiMonHocDto
    {
        public int LopId { get; set; }
        public int MonHocId { get; set; }

        public HocKi HocKi { get; set; }

        public DateTime NgayThi { get; set; }

        public string DiaDiemThi { get; set; }

        public IList<LichHoc> LichHoc { get; set; }
    }
}