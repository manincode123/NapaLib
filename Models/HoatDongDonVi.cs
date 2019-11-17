using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class HoatDongDonVi
    {
        public int HoatDongId { get; set; }
        public HoatDong HoatDong { get; set; }
        public int DonViId { get; set; }
        public DonVi DonVi { get; set; }
    }
}