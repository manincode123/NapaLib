using System;
using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs
{
    public class KhoaHocDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenKhoa { get; set; }
        [Required]
        public DateTime NamBatDau { get; set; }
        [Required]
        public DateTime NamKetThuc { get; set; }
    }
}