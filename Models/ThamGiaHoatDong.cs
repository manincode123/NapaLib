using System;
using NAPASTUDENT.Models.DTOs;

namespace NAPASTUDENT.Models
{
    public class ThamGiaHoatDong
    {
        public int HoatDongId { get; set; }

        public int SinhVienId { get; set; }

        public HoatDong HoatDong { get; set; }

        public SinhVien SinhVien { get; set; }

        public DateTime NgayThamGia { get; set; }

        public Lop Lop { get; set; }

        public int LopId { get; set; }

        public bool DuocPheDuyet { get; set; }

        public ThamGiaHoatDong()
        {
            
        }

        public ThamGiaHoatDong(int hoatDongId, SinhVien sinhVien)
        {
            HoatDongId = hoatDongId;
            SinhVienId = sinhVien.Id;
            LopId = sinhVien.LopDangHocId.GetValueOrDefault();
            NgayThamGia = DateTime.Now;
            DuocPheDuyet = true;
        }
        public ThamGiaHoatDong(int hoatDongId, SinhVien sinhVien, bool dangKiThamGia)
        {
            HoatDongId = hoatDongId;
            SinhVienId = sinhVien.Id;
            LopId = sinhVien.LopDangHocId.GetValueOrDefault();
            NgayThamGia = DateTime.Now;
            DuocPheDuyet = !dangKiThamGia;
        }

        public void PheDuyetLuotDangKi()
        {
            SinhVien.ThongBaoHoatDong(ThongBaoHoatDong.TaoThongBaoPheDuyetDangKi(HoatDong));
            DuocPheDuyet = true;
            NgayThamGia = DateTime.Now;
            HoatDong.TangSoLuotThamGia();
        }

        public void Xoa()
        {
            SinhVien.ThongBaoHoatDong(ThongBaoHoatDong.TaoThongBaoHuyDiemDanh(HoatDong));
            HoatDong.XoaLuotThamGia();
        }

        public void HuyLuotDangKi()
        {
            SinhVien.ThongBaoHoatDong(ThongBaoHoatDong.TaoThongBaoHuyDangKi(HoatDong));
        }
    }
}