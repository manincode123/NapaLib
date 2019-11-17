using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs.ChuyenMucDtos
{
    public class SaveChuyenMucDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenChuyenMuc { get; set; }
        [Required]
        [MaxLength(70)]
        public string MoTa { get; set; }
        [Required]
        public string AnhBia { get; set; }

        public int? ChuyenMucChaId { get; set; }
    }
}