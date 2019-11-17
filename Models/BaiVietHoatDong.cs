using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class BaiVietHoatDong
    {

        public virtual BaiViet BaiViet { get; set; }

        public virtual HoatDong HoatDong { get; set; }

        public int BaiVietId { get; set; }

        public int HoatDongId { get; set; }

    }
}