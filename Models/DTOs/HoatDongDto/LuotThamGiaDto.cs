using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs.HoatDongDto
{
    public class LuotThamGiaDto
    {
        [Required]
        public int HoatDongId { get; set; }
        [Required]
        public int SinhVienId { get; set; }
    }
}