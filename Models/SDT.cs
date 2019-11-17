namespace NAPASTUDENT.Models
{
    public class SDT
    {
        public int Id { get; set; }

        public string MoTa { get; set; }

        public string SoDienThoai { get; set; }

        public int SinhVienId { get; set; }

        public SinhVien SinhVien { get; set; }


    }
}