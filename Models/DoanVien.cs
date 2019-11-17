using System;

namespace NAPASTUDENT.Models
{
    public class DoanVien
    {
        public SinhVien SinhVien { get; set; }

        public int SinhVienId { get; set; }

        public DateTime NgayVaoDoan { get; set; }

        public string NoiVaoDoan { get; set; }

        public void DangKiDoanVien()
        {
            
        }
    }
}