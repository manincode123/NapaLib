using NAPASTUDENT.Models;

namespace NAPASTUDENT.Controllers.Api
{
    public class MonHocDto
    {
        public int Id { get; set; }
        public string TenMonHoc { get; set; }

        public byte SoHocPhan { get; set; }

        public byte SoTiet { get; set; }
        public string KyHieuMonHoc { get; set; }
        public bool HaiDiemDk { get; set; }
        public LoaiMon LoaiMon { get; set; }
    }
}