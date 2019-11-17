using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs.LopDtos
{
    public class SaveLopDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TenLop { get; set; }
        [Required]
        [MaxLength(15)]
        public string KyHieuTenLop { get; set; }
        [Required]
        public bool LopChuyenNganh { get; set; }
        [Required]
        public int KhoaHocId { get; set; }
        [Required]
        public string AnhBia { get; set; }

    }
}