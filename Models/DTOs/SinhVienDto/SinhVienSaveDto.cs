using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class SinhVienSaveDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string HoVaTenLot { get; set; }
        [Required]
        [MaxLength(50)]
        public string Ten { get; set; }
        [Required]
        public DateTime NgaySinh { get; set; }
        [Required]
        public string MSSV { get; set; }
        [Required]
        public int DanTocId { get; set; }
        [Required]
        public int TonGiaoId { get; set; }
        [Required]
        public int GioiTinhId { get; set; }
        [Required]
        public int KhoaHocId { get; set; }
        [Required]
        public string AnhDaiDien { get; set; }
        public string GioiThieu { get; set; }
        public string CMND { get; set; }
    }
}