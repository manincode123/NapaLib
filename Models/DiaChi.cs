namespace NAPASTUDENT.Models
{
    public class DiaChi
    {
        public int Id { get; set; }

        public string SoNhaTenDuong { get; set; }

        public LoaiDiaChi LoaiDiaChi { get; set; }
        public CapXa CapXa { get; set; }
        public int CapXaId { get; set; }

        public CapHuyen CapHuyen { get; set; }

        public int CapHuyenId { get; set; }

        public CapTinh CapTinh { get; set; }

        public int CapTinhId { get; set; }

        public int SinhVienId { get; set; }

        public SinhVien SinhVien { get; set; }


    }
}