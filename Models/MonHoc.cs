using System.Collections.Generic;
using NAPASTUDENT.Controllers.Api;

namespace NAPASTUDENT.Models
{
    public class MonHoc
    {
        public int Id { get; set; }
        public string TenMonHoc { get; set; }

        public byte SoHocPhan { get; set; }

        public byte SoTiet { get; set; }
        public string KyHieuMonHoc { get; set; }
        public bool HaiDiemDk { get; set; }

        public LoaiMon LoaiMon { get; set; }

        public virtual IList<LopMonHoc> DanhSachLopHocMonHoc { get; set; }

        public MonHoc()
        {
            DanhSachLopHocMonHoc = new List<LopMonHoc>();
        }

        public void MapForEdit(MonHocDto monHocDto)
        {
            KyHieuMonHoc = monHocDto.KyHieuMonHoc;
            TenMonHoc = monHocDto.TenMonHoc;
            SoTiet = monHocDto.SoTiet;
            SoHocPhan = monHocDto.SoHocPhan;
        }
    }

    public enum LoaiMon
    {
        MonThuong = 1,
        MonTiengAnh = 2,
        MonCPDT_TT = 3
    }
}