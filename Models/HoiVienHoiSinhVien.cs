using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class HoiVienHoiSinhVien
    {
        public SinhVien SinhVien { get; set; }
        public int SinhVienId { get; set; }
        public DateTime NgayVaoHoi { get; set; }

    }
}