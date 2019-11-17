using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NAPASTUDENT.Controllers.Api;

namespace NAPASTUDENT.Models
{
    public class DonVi
    {
        public int Id { get; set; }

        public string TenDonVi { get; set; }

        public DateTime NgayThanhLap { get; set; }
        public string GioiThieu { get; set; }
        public LoaiDonVi LoaiDonVi { get; set; }
        public string AnhBia { get; set; }

        public IList<ThanhVienDonVi> DanhSachThanhVienDonVi { get; set; }

        public IList<HoatDongDonVi> DanhSachHoatDongToChuc { get; set; }
        public IList<BaiVietDonVi> DanhSachBaiVietDonVi { get; set; }
        public IList<ChucVuDonVi> ChucVuDonVi { get; set; }


        public DonVi()
        {
            DanhSachThanhVienDonVi = new List<ThanhVienDonVi>();
            DanhSachHoatDongToChuc = new List<HoatDongDonVi>();
            DanhSachBaiVietDonVi = new List<BaiVietDonVi>();
        }

        public void TaoDonViMoi(DonViDto donViDto)
        {
            Mapper.Map(donViDto, this);
        }

        public void ChinhSuaThongTin(DonViDto donViDto)
        {
            Mapper.Map(donViDto, this);
        }

        public void XoaDonVi()
        {
            DanhSachThanhVienDonVi.Clear();
            DanhSachHoatDongToChuc.Clear();
            DanhSachBaiVietDonVi.Clear();
            ChucVuDonVi.Clear();
        }
    }

    public enum LoaiDonVi
    {
        DonViDoan = 1,
        DonViHoi =2
    }
}