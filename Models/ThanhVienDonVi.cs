using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAPASTUDENT.Controllers.Api;

namespace NAPASTUDENT.Models
{
    public class ThanhVienDonVi
    {
        public SinhVien SinhVien { get; set; }

        public int SinhVienId   { get; set; }

        public DonVi DonVi { get; set; }

        public int DonViId { get; set; }

        public DateTime NgayGiaNhap { get; set; }

        public DateTime? NgayRoi { get; private set; }

        public bool NgungThamGia { get;private set; }

        public bool DuocPheDuyet { get; private set; }

        public string GhiChu { get; private set; }

        public IList<ChucVuDonVi> DanhSachChucVuDonVi { get; set; }

        public ThanhVienDonVi()
        {
            DanhSachChucVuDonVi = new List<ChucVuDonVi>();
        }

        public void ThemThanhVien(ThemXoaThanhVienDonViDto themThanhVienDto)
        {
            SinhVienId = themThanhVienDto.SinhVienId;
            DonViId = themThanhVienDto.DonViId;
            DuocPheDuyet = true;
            NgayGiaNhap = DateTime.Now;
        }

        public void SetTotNghiep(TotNghiepThanhVienDto totNghiepThanhVienDto)
        {
            NgungThamGia = true;
            NgayGiaNhap = totNghiepThanhVienDto.NgayGiaNhap;
            NgayRoi = totNghiepThanhVienDto.NgayRoi;
            GhiChu = totNghiepThanhVienDto.GhiChu;
        }

        public void DangKiThanhVien(DangKiThanhVienDto dangKiDto, int userSinhVienId)
        {
            SinhVienId = userSinhVienId;
            DonViId = dangKiDto.DonViId;
            GhiChu = dangKiDto.GioiThieu;
            DuocPheDuyet = false;
            NgayGiaNhap = DateTime.Now;
        }

        public void PheDuyetDangKi()
        {
            DuocPheDuyet = true;
            NgayGiaNhap = DateTime.Now;
        }

        public void ThayDoi(ThayDoiThanhVienDto thayDoiDto)
        {
            NgayGiaNhap = thayDoiDto.NgayGiaNhap;
        }
    }
}