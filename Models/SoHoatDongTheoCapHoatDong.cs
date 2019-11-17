using System.Collections.Generic;

namespace NAPASTUDENT.Models
{
    public class SoHoatDongTheoCapHoatDong
    {
        //Định dạng như vầy để phù hợp với plugin chart.js
        public IList<string> DanhSachCapHoatDong{ get; set; }
        public IList<int> SoHoatDongTungCap { get; set; }

        public SoHoatDongTheoCapHoatDong()
        {
            DanhSachCapHoatDong = new List<string>();
            SoHoatDongTungCap = new List<int>();
        }
    }
}