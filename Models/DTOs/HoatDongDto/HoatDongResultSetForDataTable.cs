using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class HoatDongResultSetForDataTable
    {
        public int Id { get; set; }

        public string AnhBia { get; set; }
        public string TenHoatDong { get; set; }
        public string SoLuoc { get; set; }
        public string DiaDiem { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool DaKetThuc { get; set; }
        public bool BiHuy { get; set; }
        public bool DuocPheDuyet { get; set; }

        public CapHoatDong CapHoatDong { get; set; }

        public int SoLuotThamGia { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
}