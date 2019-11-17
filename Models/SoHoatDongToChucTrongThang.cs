using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class SoHoatDongToChucTrongThang
    {

        public IList<string> DanhSachThang{ get; set; }

        public IList<int> SoHoatDongLopToChuc { get; set; }
        public IList<int> SoHoatDongHocVienToChuc { get; set; }

        public SoHoatDongToChucTrongThang()
        {
            DanhSachThang = new List<string>();
            SoHoatDongLopToChuc = new List<int>();
            SoHoatDongHocVienToChuc = new List<int>();
        }

    }
}