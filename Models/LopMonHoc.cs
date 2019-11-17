using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAPASTUDENT.Models.DTOs.MonHocDtos;

namespace NAPASTUDENT.Models
{
    public class LopMonHoc
    {
        public LopMonHoc()
        {
            DanhSachLichHoc = new List<LichHoc>();
            DanhSachDiem = new List<Diem>();
        }

        public Lop Lop { get; set; }

        public int LopId { get; set; }

        public MonHoc MonHoc { get; set; }

        public int MonHocId { get; set; }

        public HocKi HocKi { get; set; }

        public DateTime NgayThi { get; set; }

        public string DiaDiemThi { get; set; }

        public IList<LichHoc> DanhSachLichHoc { get; set; }

        public IList<Diem> DanhSachDiem { get; set; }

        public void MapForEdit(DangKiMonHocDto dangKiMonHocDto)
        {
            NgayThi = dangKiMonHocDto.NgayThi;
            DiaDiemThi = dangKiMonHocDto.DiaDiemThi;
        }

        public void XoaDiem()
        {
            DanhSachDiem.Clear();
        }
    }
}