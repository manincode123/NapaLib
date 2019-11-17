using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class LichThiLai4SaveDto
    {
        public int Id { get; set; }

        public int MonHocId { get; set; }

        public DateTime ThoiGianThi { get; set; }

        [Required]
        public string DiaDiemThi { get; set; }

    }
}