using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class SetLichThiLaiDto
    {
        [Required]
        public int MonHocId { get; set; }
        [Required]

        public int SinhVienId { get; set; }
        [Required]
        public int LichThiLaiId { get; set; }
    }
}