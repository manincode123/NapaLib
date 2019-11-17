using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class LichThiLai
    {

        public int Id { get; private set; }

        public int MonHocId { get;private set; }

        public MonHoc MonHoc { get; set; }

        public DateTime ThoiGianThi { get;private set; }

        public string DiaDiemThi { get;private set; }

        public bool DaThiXong { get; private set; }

        public IList<Diem> DanhSachThiLai { get; set; }

        public LichThiLai()
        {
            DanhSachThiLai = new List<Diem>();
        }

        public void KetThucThiLai()
        {
            DaThiXong = true;
        }
    }
}