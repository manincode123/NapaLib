using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class ChuyenMucBaiViet
    {
        public ChuyenMucBaiViet()
        {
            DanhSachBaiViet = new List<BaiViet>();
            DanhSachChuyenMucCon = new List<ChuyenMucBaiViet>();
        }

        public int Id { get; set; }

        public string TenChuyenMuc { get; set; }

        public string MoTa { get; set; }

        public string AnhBia { get; set; }

        public ChuyenMucBaiViet ChuyenMucCha { get; set; }

        public bool DaXoa { get; private set; }

        public int? ChuyenMucChaId { get; set; }

        public IList<ChuyenMucBaiViet> DanhSachChuyenMucCon { get; set; }

        public IList<BaiViet> DanhSachBaiViet { get; set; }


        public void XoaChuyenMuc()
        {
            DaXoa = true;
            foreach (var baiViet in DanhSachBaiViet)
            {
                baiViet.XoaBaiViet();
            }
            foreach (var chuyenMucBaiViet in DanhSachChuyenMucCon)
            {
                chuyenMucBaiViet.XoaChuyenMuc();
            }
        }
    }
}