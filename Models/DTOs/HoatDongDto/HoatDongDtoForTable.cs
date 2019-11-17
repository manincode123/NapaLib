using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class HoatDongDtoForTable
    {
        public HoatDongDtoForTable()
        {
            
        }
        public HoatDongDtoForTable(HoatDong hd)
        {
            Id = hd.Id;
            TenHoatDong = hd.TenHoatDong;
            NgayBatDau = hd.NgayBatDau;
            NgayKetThuc = hd.NgayKetThuc;
            DaKetThuc = hd.DaKetThuc;
            SoLuotThamGia = hd.SoLuotThamGia;
        }

        public int Id { get; set; }

        public string TenHoatDong { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool DaKetThuc { get; set; }
        public bool BiHuy { get; set; }
        public bool DuocPheDuyet { get; set; }

        public CapHoatDong CapHoatDong { get; set; }

        public virtual IList<string> DanhSachDonViToChuc { get; set; }

        public virtual IList<string> DanhSachLopToChuc { get; set; }

        public int SoLuotThamGia { get; set; }

    }
}