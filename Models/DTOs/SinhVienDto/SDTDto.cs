using System.ComponentModel.DataAnnotations;

namespace NAPASTUDENT.Models.DTOs

{
    public class SDTDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string MoTa { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        public string SoDienThoai { get; set; }
        [Required]
        public int SinhVienId { get; set; }

    }
}