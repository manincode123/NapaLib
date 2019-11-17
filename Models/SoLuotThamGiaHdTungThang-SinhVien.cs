using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class SoLuotThamGiaHdTrongThang
    {
        //Định dạng như vầy để phù hợp với plugin chart.js
        public IList<string> DanhSachThang{ get; set; }
        public IList<int> SoLuotThamGiaLop { get; set; }
        public IList<int> SoLuotThamGiaHocVien { get; set; }

        public SoLuotThamGiaHdTrongThang()
        {
            DanhSachThang = new List<string>();
            SoLuotThamGiaLop = new List<int>();
            SoLuotThamGiaHocVien = new List<int>();
        }

    }
}