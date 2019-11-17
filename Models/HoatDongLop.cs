using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class HoatDongLop
    {
        public Lop Lop { get; set; }

        public HoatDong HoatDong { get; set; }

        public int LopId { get; set; }

        public int HoatDongId { get; set; }
    }
}