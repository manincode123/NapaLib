using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs
{
    public class ChinhSuaChucVuLopDto
    {
        [Required]
        public int LopId { get; set; }
        [Required]
        public int IdSinhVienGiuChucVuCu { get; set; }       
        [Required]
        public int IdSinhVienGiuChucVuMoi { get; set; }
        [Required]
        public int ChucVuId { get; set; }
    }
}