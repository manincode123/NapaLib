using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class HoatDongDtoForSave
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenHoatDong { get; set; }
        [Required]
        [MaxLength(300)]
        public string SoLuoc { get; set; }
        [Required]
        [MaxLength(100)]
        public string DiaDiem { get; set; }
        [Required]
        public string AnhBia { get; set; }
        [Required]
        public DateTime NgayBatDau { get; set; }
        [Required]
        public DateTime NgayKetThuc { get; set; }
        [Required]
        public string NoiDung { get; set; }
        [Required]
        public CapHoatDong CapHoatDong { get; set; }
        [Required]
        public bool HoatDongNgoaiHocVien { get; set; }
        public byte SoNgayTinhNguyen { get; set; }
        public bool DaKetThuc { get; set; }
        public bool DuocPheDuyet { get; set; }
        public IList<int> DanhSachDonViToChuc { get; set; }
        public IList<int> DanhSachLopToChuc { get; set; }
    }
}