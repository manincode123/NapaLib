using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class SinhVienLop
    {

        public Lop Lop { get; set; }

        public int LopId { get; set; }

        public SinhVien SinhVien { get; set; }

        public int SinhVienId { get; set; }

        public bool LopChuyenNganh { get; set; }

    }
}