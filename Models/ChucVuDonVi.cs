using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NAPASTUDENT.Controllers.Api;

namespace NAPASTUDENT.Models
{
    public class ChucVuDonVi
    {
        public ThanhVienDonVi ThanhVienDonVi { get; set; }
        public SinhVien SinhVien { get; set; }
        public int SinhVienId { get; private set; }
        public DonVi DonVi { get; set; }
        public int DonViId { get; private set; }
        public ChucVu ChucVu { get; set; }
        public int ChucVuId { get; private set; }
        public string TenChucVu { get; private set; }
        public bool QuanLyThongTin { get; private set; }
        public bool QuanLyThanhVien { get; private set; }
        public bool QuanLyChucVu { get; private set; }
        public bool QuanLyHoatDong { get;private set; }


        public void TaoMoiChucVu(ChucVuDonViDto chucVuDonViDto)
        {
            Mapper.Map(chucVuDonViDto, this);
        }

        public void ThayDoiChucVu(ChucVuDonViDto chucVuDonViDto)
        {
            Mapper.Map(chucVuDonViDto, this);
        }
    }
}