using System;

namespace NAPASTUDENT.Models.ViewModels
{
    public class QuanLyDonViViewModel
    {
        public QuanLyDonViViewModel()
        {
            DaDangKi = false;
            ThanhVienChinhThuc = false;
        }

        public QuanLyDonViViewModel(DonVi donVi)
        {
            DaDangKi = false;
            ThanhVienChinhThuc = false;
            DonViId = donVi.Id;
            TenDonVi = donVi.TenDonVi;
            NgayThanhLap = donVi.NgayThanhLap;
            AnhBia = donVi.AnhBia;
            GioiThieu = donVi.GioiThieu;
            LoaiDonVi = donVi.LoaiDonVi;
        }

        public string TenDonVi { get; set; }
        public DateTime NgayThanhLap { get; set; }
        public string AnhBia { get; set; }
        public string GioiThieu { get; set; }
        public LoaiDonVi LoaiDonVi { get; set; }
        public bool DaDangKi { get;private set; }
        public bool ThanhVienChinhThuc { get;private set; }
        public bool QuanLyThongTin { get;private set; }
        public bool QuanLyThanhVien { get; private set; }
        public bool QuanLyChucVu { get; private set; }
        public bool QuanLyHoatDong { get; private set; }
        public int DonViId { get;private set; }
        public void SetQuyenQuanLyChucVu()
        {
            QuanLyChucVu = true;
        }
        public void SetQuanLyHoatDong()
        {
            QuanLyHoatDong = true;
        }
        public void SetQuanLyThanhVien()
        {
            QuanLyThanhVien = true;
        }
        public void SetQuanLyThongTin()
        {
            QuanLyThongTin = true;
        }

        public void SetQuyenCaoNhat()
        {
            SetQuyenQuanLyChucVu();
            SetQuanLyThanhVien();
            SetQuanLyThongTin();
            SetQuanLyHoatDong();
        }


        public void SetLaThanhVienChinhThuc()
        {
            DaDangKi = true;
            ThanhVienChinhThuc = true;
        }

        public void SetMoiDangKi()
        {
            DaDangKi = true;
            ThanhVienChinhThuc = false;
        }
    }
}