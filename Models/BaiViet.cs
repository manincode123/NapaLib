using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NAPASTUDENT.Models.DTOs;

namespace NAPASTUDENT.Models
{
    public class BaiViet
    {
        public BaiViet()
        {
            BaiVietDonVi = new List<BaiVietDonVi>();
            BaiVietLop = new List<BaiVietLop>();
            BaiVietHoatDong =  new List<BaiVietHoatDong>();
        }
        public int Id { get; set; }

        public SinhVien NguoiTao { get; set; }

        public DateTime NgayTao { get; set; }

        public string NoiDungBaiViet { get; private set; }

        public string TenBaiViet { get; private set; }

        public string SoLuoc { get; private set; }

        public string AnhBia { get; private set; }

        public int NguoiTaoId { get; private set; }
        public int SoLuotThich { get; set; }
        public int SoLuotXem { get; private set; }
        public bool DuocPheDuyet { get;private set; }

        public bool DaXoa { get; private set; }

        public IList<BaiVietHoatDong> BaiVietHoatDong { get; set; }
        public IList<BaiVietLop> BaiVietLop { get; set; }
        public IList<BaiVietDonVi> BaiVietDonVi { get; set; }

        public int ChuyenMucBaiVietId { get; set; }

        public ChuyenMucBaiViet ChuyenMucBaiViet { get; set; }

        public void XoaBaiViet()
        {
            DaXoa = true;
        }

        public void PheDuyetBaiViet()
        {
            DuocPheDuyet = true;
        }

        public void TangLuotXem()
        {
            SoLuotXem++;
        }

        public void TaoBaiViet(SaveBaiVietDto baiVietDto, int userSinhVienId)
        {
            Mapper.Map(baiVietDto,this);
            Tag(baiVietDto);
            NgayTao = DateTime.Now.Date;
            NguoiTaoId = userSinhVienId;
        }

        public void ChinhSuaBaiViet(SaveBaiVietDto baiVietDto)
        {
            Mapper.Map(baiVietDto, this);
            BaiVietDonVi.Clear();
            BaiVietLop.Clear();
            BaiVietHoatDong.Clear();
            Tag(baiVietDto);
        }

        private void Tag(SaveBaiVietDto baiVietDto)
        {
            //Tag đơn vị
            if (baiVietDto.DanhSachDonViTag != null)
            {
                foreach (var donVi in baiVietDto.DanhSachDonViTag)
                {
                    BaiVietDonVi.Add(new BaiVietDonVi { DonViId = donVi });
                }
            }
            //tag lớp
            if (baiVietDto.DanhSachLopTag != null)
            {
                foreach (var lop in baiVietDto.DanhSachLopTag)
                {
                    BaiVietLop.Add(new BaiVietLop { LopId = lop });
                }
            }
            //Tag hoạt động
            if (baiVietDto.DanhSachHoatDongTag != null)
            {
                foreach (var hoatDong in baiVietDto.DanhSachHoatDongTag)
                {
                    BaiVietHoatDong.Add(new BaiVietHoatDong { HoatDongId = hoatDong });
                }
            }
        }
    }
}