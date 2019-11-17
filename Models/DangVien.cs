using System;

namespace NAPASTUDENT.Models
{
    public class DangVien
    {
        public SinhVien SinhVien { get; set; }

        public int SinhVienId { get; set; }

        public DateTime NgayVaoDangChinhThuc { get; set; }

        public string NoiVaoDang { get; set; }
    }
}