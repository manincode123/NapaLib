using System;
using System.Collections.Generic;

namespace NAPASTUDENT.Models
{
    public class KhoaHoc
    {
        public int Id { get; set; }

        public string TenKhoa { get; set; }

        public DateTime NamBatDau { get; set; }

        public DateTime NamKetThuc { get; set; }

        public IList<Lop> DanhSachLop { get; set; }

        public IList<SinhVien> DanhSachSinhVien { get; set; }

        public KhoaHoc()
        {
            DanhSachLop = new List<Lop>();
            DanhSachSinhVien = new List<SinhVien>();
        }


    }
}