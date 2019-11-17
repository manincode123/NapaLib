using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using AutoMapper;
using NAPASTUDENT.Controllers.Api;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Models
{
    public class SinhVien
    {
        public int Id { get; private set; }

        public string HoVaTenLot { get;private set; }

        public string Ten { get; private set; }
        public string MSSV { get; set; }

        public string GioiThieu { get;private set; }

        public DateTime NgaySinh { get; private set; }

        public string CMND { get; set; }

        public int? LopDangHocId { get;private set; }

        public Lop LopDangHoc { get; set; }

        public DanToc DanToc { get; set; }

        public int DanTocId { get; private set; }

        public TonGiao TonGiao { get; set; }

        public int TonGiaoId { get; private set; }

        public GioiTinh GioiTinh { get; set; }

        public int GioiTinhId { get; private set; }
        public string AnhDaiDien { get;private set; }
        public bool DaRaTruong { get; set; }

        public KhoaHoc KhoaHoc { get; set; }

        public int KhoaHocId { get;private set; }

        public IList<SinhVienLop> DanhSachLop { get; set; }
        public IList<SDT> SDT { get; set; }

        public IList<DiaChi> DiaChi { get; set; }

        public IList<Diem> Diem { get; set; }

        public IList<ThanhVienDonVi> DanhSachDonViThamGia { get; set; }

        public IList<ThamGiaHoatDong> DanhSachHoatDongThamGia { get; set; }
        public IList<TheoDoiHoatDong> DanhSachHoatDongTheoDoi { get; set; }
        public IList<ThongBaoHoatDongSinhVien> DanhSachThongBaoHoatDong { get; set; }

        public IList<BaiViet> DanhSachBaiViet { get; set; }

        public IList<ChucVuLop> ChucVuLop { get; set; }

        public HoiVienHoiSinhVien HoiVien { get; set; }

        public DoanVien DoanVien { get; set; }

        public DangVien DangVien { get; set; }


        [ForeignKey("ApplicationUser")]
        [Required]
        public string ApplicationUserId { get;private set; }
        public ApplicationUser ApplicationUser { get; private set; }

        public IList<DiemTrungBinhHocKi> DiemTrungBinhHocKi { get; set; }
        public IList<ChucVuDonVi> ChucVuDonVi { get; set; }


        public SinhVien()
        {
            SDT = new List<SDT>();
            DiaChi = new List<DiaChi>();
            Diem = new List<Diem>();
            DanhSachDonViThamGia = new List<ThanhVienDonVi>();
            DanhSachHoatDongThamGia = new List<ThamGiaHoatDong>();
            DanhSachHoatDongTheoDoi = new List<TheoDoiHoatDong>();
            DanhSachThongBaoHoatDong = new List<ThongBaoHoatDongSinhVien>();
            DanhSachBaiViet = new List<BaiViet>();
            ChucVuLop = new List<ChucVuLop>();
            ChucVuDonVi = new List<ChucVuDonVi>();
            DanhSachLop = new List<SinhVienLop>();
            DiemTrungBinhHocKi = new List<DiemTrungBinhHocKi>();
        }

        public SinhVien(string hoVaTenLot, string ten, string mssv)
        {
            //Constructor để init SinhVien dùng trong func tạo Batch Sinh Vien 
            HoVaTenLot = hoVaTenLot;
            Ten = ten;
            MSSV = mssv;
            NgaySinh = new DateTime();
            DiemTrungBinhHocKi = new List<DiemTrungBinhHocKi>();
        }


        public void SetLopDangHoc(int lopId)
        {
            LopDangHocId = lopId;
        }

        public void ResetLopDangHoc()
        {
            if (DanhSachLop.Any())
            {
                LopDangHocId = DanhSachLop.LastOrDefault().LopId;
            }

            LopDangHocId = null;
        }

        public void XoaDangKiHoiVien()
        {
            HoiVien = null;
        }

        public void DangKiHoiVien(DangKiHoiVienDoanVienDto hoiVienDto)
        {
            HoiVien = new HoiVienHoiSinhVien
            {
                NgayVaoHoi = hoiVienDto.NgayVao
            };
        }

        public void XoaDangKiDoanVien()
        {
            DoanVien = null;
        }

        public void DangKiDoanVien(DangKiHoiVienDoanVienDto doanVienDto)
        {
            
            DoanVien = new DoanVien()
            {
                NgayVaoDoan = doanVienDto.NgayVao,
                NoiVaoDoan = doanVienDto.NoiVao
            };
        }

        public void ThongBaoHoatDong(ThongBaoHoatDong thongBaoMoi)
        {
            DanhSachThongBaoHoatDong.Add(new ThongBaoHoatDongSinhVien(this,thongBaoMoi));
        }

        public void SetApplicationUser(ApplicationUser user)
        {
            ApplicationUser = user;
        }

        public void ChinhSuaThongTin_QuanLy(SinhVienSaveDto sinhVienDto)
        {
            Mapper.Map(sinhVienDto, this);
        }

        public void TaoDanhSachDiem()
        {
            //Tạo điểm trung bình học kì
            for (var i = 1; i <= 8; i++)
            {
                DiemTrungBinhHocKi.Add(new DiemTrungBinhHocKi { HocKi = (HocKi)i });
            }
        }
        public void TaoSinhVien(SinhVienSaveDto sinhVienDto)
        {
            Mapper.Map(sinhVienDto,this);
            TaoDanhSachDiem();
        }

        public void ChinhSuaThongTin_SinhVien(SinhVienSaveDto sinhVienDto)
        {
            HoVaTenLot = sinhVienDto.HoVaTenLot;
            Ten = sinhVienDto.Ten;
            DanTocId = sinhVienDto.DanTocId;
            TonGiaoId = sinhVienDto.TonGiaoId;
            NgaySinh = sinhVienDto.NgaySinh;
            GioiTinhId = sinhVienDto.GioiTinhId;
            AnhDaiDien = sinhVienDto.AnhDaiDien;
        }

        public void setNgaySinh(DateTime ngaySinh)
        {
            NgaySinh = ngaySinh;
        }

        public void SetBienMaHoa(int gioiTinhId, int khoaHocId, int danTocId, int tonGiaoId)
        {
            GioiTinhId = gioiTinhId;
            KhoaHocId = khoaHocId;
            DanTocId = danTocId;
            TonGiaoId = tonGiaoId;
        }

        public void SetAnhDaiDien(string anhDaiDienSrc)
        {
            AnhDaiDien = anhDaiDienSrc;
        }
    }
}