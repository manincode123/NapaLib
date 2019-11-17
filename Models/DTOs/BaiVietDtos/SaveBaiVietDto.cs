using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs
{
    public class SaveBaiVietDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NoiDungBaiViet { get; set; }
        [Required]
        [MaxLength(100)]
        public string TenBaiViet { get; set; }
        [Required]
        [MaxLength(150)]
        public string SoLuoc { get; set; }
        [Required]
        public string AnhBia { get; set; }
        [Required]
        public int ChuyenMucBaiVietId { get; set; }
        public IList<int> DanhSachHoatDongTag { get; set; }
        public IList<int> DanhSachLopTag { get; set; }
        public IList<int> DanhSachDonViTag { get; set; }
        
    }
}