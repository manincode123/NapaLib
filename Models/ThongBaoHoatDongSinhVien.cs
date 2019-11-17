using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class ThongBaoHoatDongSinhVien
    {
        protected ThongBaoHoatDongSinhVien()
        {
            
        }
        public ThongBaoHoatDongSinhVien(SinhVien sinhVien, ThongBaoHoatDong thongBaoMoi)
        {
            if (sinhVien == null) throw new ArgumentNullException("sinhVien");
            if (thongBaoMoi == null) throw new ArgumentNullException("thongBaoMoi");
            SinhVien = sinhVien;
            ThongBaoHoatDong = thongBaoMoi;
        }
                                                                                        
        public ThongBaoHoatDong ThongBaoHoatDong { get;private set; }
        public int ThongBaoHoatDongId { get; set; }
        public SinhVien SinhVien { get;private set; }
        public int SinhVienId { get; set; }
        public bool DaDoc { get; private set; }

        public void DanhDauDaDoc()
        {
            DaDoc = true;
        }
    }
}