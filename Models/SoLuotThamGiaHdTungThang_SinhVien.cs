using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class SoLuotThamGiaHdTungThang_SinhVien
    {
        /*
         Cấu trúc class mình thường làm List<LuotThamGiaThang>
         LuotThamGiaThang: {string Thang; int SoLuotThamGia}
         */
        //Cấu trúc của số lượt tham gia được kết cấu như vậy vì để phù hợp với plugin tạo biểu đồ
        public IList<string> DanhSachThang{ get; set; }
        public IList<int> SoLuotThamGiaLop { get; set; }

        public SoLuotThamGiaHdTungThang_SinhVien()
        {
            DanhSachThang = new List<string>();
            SoLuotThamGiaLop = new List<int>();
        }

    }
}