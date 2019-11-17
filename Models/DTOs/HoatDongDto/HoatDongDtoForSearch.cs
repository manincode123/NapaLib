using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class HoatDongDtoForSearch
    {
        public HoatDongDtoForSearch()
        {
            
        }
        public HoatDongDtoForSearch(HoatDong hd)
        {
            Id = hd.Id;
            TenHoatDong = hd.TenHoatDong;
            NgayBatDau = hd.NgayBatDau;
            NgayKetThuc = hd.NgayKetThuc;
            DaKetThuc = hd.DaKetThuc;
            DiaDiem = hd.DiaDiem;
            BiHuy = hd.BiHuy;
            AnhBia = hd.AnhBia;
        }

        public int Id { get; set; }

        public string TenHoatDong { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool DaKetThuc { get; set; }

        public bool BiHuy { get; set; }

        public string DiaDiem { get; set; }

        public string AnhBia { get; set; }

    }
}