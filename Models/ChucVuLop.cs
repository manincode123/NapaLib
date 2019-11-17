using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class ChucVuLop
    {

        public ChucVuLop()
        {
            
        }
        public ChucVuLop(SinhVien sinhVien, Lop lop, int chucVuId)
        {
            SinhVien = sinhVien;
            Lop = lop;
            ChucVuId = chucVuId;
        }

        public SinhVien SinhVien { get; set; }

        public int SinhVienId { get; set; }

        public Lop Lop { get; set; }

        public int LopId { get; set; }

        public ChucVu ChucVu { get; set; }

        public int ChucVuId { get; set; }

    }
}