using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class DiemTrungBinhHocKi
    {
        public DiemTrungBinhHocKi()
        {
            TongDiem = 0;
            TongHocPhan = 0;
            DiemTb = 0;
        }
        public SinhVien SinhVien { get; set; }

        public int SinhVienId { get; set; }

        public HocKi HocKi { get; set; }

        public int? TongDiem { get; set; }

        public byte? TongHocPhan { get; set; }

        public float? DiemTb { get; set; }
    }
}