using System;
using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class ChuongTrinhHoatDongDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(400)]
        public string NoiDungChuongTrinh { get; set; }
        [Required]
        [MaxLength(50)]
        public string TieuDe { get; set; }
        [Required]
        public DateTime TgDienRa { get; set; }
        [Required]
        public LoaiHienThi LoaiHienThi { get; set; }
        [Required]
        public int HoatDongId { get; set; }
    }
}