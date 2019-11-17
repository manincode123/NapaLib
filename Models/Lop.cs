using System.Collections.Generic;

namespace NAPASTUDENT.Models
{
    public class Lop
    {
        public Lop()
        {
            DanhSachSinhVien = new List<SinhVienLop>();
            DanhSachMonHoc = new List<LopMonHoc>();
            DanhSachHoatDongToChuc = new List<HoatDongLop>();
            DanhSachBaiVietLop = new List<BaiVietLop>();
            ChucVuLop = new List<ChucVuLop>();
            DanhSachThamGiaHoatDong = new List<ThamGiaHoatDong>();
        }
        public int Id{ get; set; }
        public string TenLop { get; set; }


        public string KyHieuTenLop { get; set; }

        public int KhoaHocId { get; set; }
        public KhoaHoc KhoaHoc { get; set; }

        public string AnhBia { get; set; }

        public bool DaTotNghiep { get; set; }

        public bool LopChuyenNganh { get; set; }
        public IList<LopMonHoc> DanhSachMonHoc { get; set; }

        public IList<HoatDongLop> DanhSachHoatDongToChuc { get; set; }
        public IList<SinhVienLop> DanhSachSinhVien { get; set; }

        public IList<ChucVuLop> ChucVuLop { get; set; }

        public IList<BaiVietLop> DanhSachBaiVietLop { get; set; }

        public IList<ThamGiaHoatDong> DanhSachThamGiaHoatDong { get; set; }

    }
}