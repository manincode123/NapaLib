using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class DiaChiDtoForSave
    {
        [Required]
        public int SinhVienId { get; set; }

        [Required]
        public string SoNhaTenDuong { get; set; }

        [Required]
        public LoaiDiaChi LoaiDiaChi { get; set; }
        [Required]
        public int CapXaId { get; set; }

        [Required]
        public int CapHuyenId { get; set; }

        [Required]
        public int CapTinhId { get; set; }
    }
}