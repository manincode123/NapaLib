using System;
using System.Linq;
using System.Web;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using NAPASTUDENT.Controllers.Api;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.BaiVietDtos;
using NAPASTUDENT.Models.DTOs.ChuyenMucDtos;
using NAPASTUDENT.Models.DTOs.HoatDongDto;
using NAPASTUDENT.Models.DTOs.LopDtos;
using NAPASTUDENT.Models.DTOs.MonHocDtos;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SinhVien, ChiTietDayDuSinhVienDto>()
                .ForMember(svdto => svdto.TonGiao, opt => opt.MapFrom(sv => sv.TonGiao.TenTonGiao))
                .ForMember(svdto => svdto.DanToc, opt => opt.MapFrom(sv => sv.DanToc.TenDanToc))
                .ForMember(svdto => svdto.GioiTinh, opt => opt.MapFrom(sv => sv.GioiTinh.TenGioiTinh))
                .ForMember(svdto => svdto.TenLop, opt => opt.MapFrom(sv => sv.LopDangHoc.KyHieuTenLop))
                .ForMember(svdto => svdto.LopId, opt => opt.MapFrom(sv => sv.LopDangHoc.Id));
                

            CreateMap<DoanVien, HoiVienDoanVienDto>()
                .ForMember(dvdto => dvdto.NoiVao, opt => opt.MapFrom(sv => sv.NoiVaoDoan))
                .ForMember(dvdto => dvdto.NgayVao, opt => opt.MapFrom(sv => sv.NgayVaoDoan));

            CreateMap<HoiVienHoiSinhVien, HoiVienDoanVienDto>()
                .ForMember(hvdto => hvdto.NgayVao, opt => opt.MapFrom(sv => sv.NgayVaoHoi));

            CreateMap<KhoaHoc, KhoaHocDto>();
            CreateMap<KhoaHocDto, KhoaHoc>()
                .ForMember(kh => kh.Id,opt => opt.Ignore());

            CreateMap<SinhVien, SinhVienDtoForTable>()
                .ForMember(svdto => svdto.GioiTinh, opt => opt.MapFrom(sv => sv.GioiTinh.TenGioiTinh));

            CreateMap<SinhVien, TTSinhVienCBNhatDto>();

            CreateMap<SinhVienSaveDto, SinhVien>()
                .ForMember(sv => sv.Id, opt => opt.Ignore()) //Bỏ qua Id map Dto -> Entity
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                
                          
            CreateMap<ChucVuLop, ChucVuLopDto>()
                .ForMember(cvldto => cvldto.ChucVu, opt => opt.MapFrom(cvl => cvl.ChucVu.TenChucVu));

            CreateMap<HdVaSoLuotTg, HdVaSoLuotTgDto>();

            CreateMap<HoatDong, ThongTinCoBanHoatDongDto>();
            CreateMap<HoatDong, HoatDongDtoForTable>();

            CreateMap<DiaChi, DiaChiDto>()
                .ForMember(diachiDto => diachiDto.CapTinh, opt => opt.MapFrom(diachi => diachi.CapTinh.TenTinh))
                .ForMember(diachiDto => diachiDto.CapHuyen, opt => opt.MapFrom(diachi => diachi.CapHuyen.TenHuyen))
                .ForMember(diachiDto => diachiDto.CapXa, opt => opt.MapFrom(diachi => diachi.CapXa.TenXa));
            CreateMap<DiaChiDtoForSave, DiaChi>();

            CreateMap<SDT, SDTDto>();
            CreateMap<SDTDto, SDT>();

            CreateMap<Lop, DanhSachLopDto>()
                .ForMember(ldto => ldto.TenKhoaHoc,opt => opt.MapFrom(l => l.KhoaHoc.TenKhoa));

            CreateMap<Lop, LopDto>()
                .ForMember(ldto => ldto.KhoaHoc, opt => opt.MapFrom(l => l.KhoaHoc.TenKhoa));
            //.ForMember(ldto => ldto.soLuongSV, opt => opt.MapFrom(l => l.KhoaHoc.DanhSachSinhVien.Count))
            //.ForMember(ldto => ldto.soNu,
            //    opt => opt.MapFrom(l => l.KhoaHoc.DanhSachSinhVien.Count(sv => sv.GioiTinhId == 1)))
            //.ForMember(ldto => ldto.soNam,
            //    opt => opt.MapFrom(l => l.KhoaHoc.DanhSachSinhVien.Count(sv => sv.GioiTinhId == 2)));

            CreateMap<ChuongTrinhHoatDongDto, ChuongTrinhHoatDong>()
                .ForMember(cthd => cthd.Id, opt => opt.Ignore()); 
            
            CreateMap<HoatDongDtoForSave, HoatDong>()
                .ForMember(hd => hd.Id, opt => opt.Ignore())
                .ForMember(hd => hd.DanhSachDonViToChuc, opt => opt.Ignore())
                .ForMember(hd => hd.DanhSachLopToChuc, opt => opt.Ignore())
                .ForMember(hd => hd.DaKetThuc, opt => opt.Ignore())
                .ForMember(hd => hd.BiHuy, opt => opt.Ignore())
                .ForMember(hd => hd.DuocPheDuyet, opt => opt.Ignore());

            CreateMap<SaveLopDto, Lop>();
            CreateMap<SaveBaiVietDto, BaiViet>();

            CreateMap<ChuyenMucBaiViet, ChuyenMucSelectListDto>();
            CreateMap<SaveChuyenMucDto, ChuyenMucBaiViet>();

            CreateMap<MonHoc, MonHocDto>();
            CreateMap<MonHocDto, MonHoc>();

            CreateMap<DangKiMonHocDto,LopMonHoc>()
                .ForMember(lmh => lmh.DanhSachLichHoc, opt => opt.Ignore());

            CreateMap<LichHocDto, LichHoc>();

            CreateMap<LichThiLai4SaveDto, LichThiLai>()
                .ForMember(ltl => ltl.Id, opt => opt.Ignore());

            CreateMap<ThongBaoHoatDong, ThongBaoHoatDongDto>()
                .ForMember(tbDto => tbDto.TenHoatDong, opt => opt.MapFrom(tb => tb.HoatDong.TenHoatDong))
                .ForMember(tbDto => tbDto.NgayBatDau, opt => opt.MapFrom(tb => tb.HoatDong.NgayBatDau))
                .ForMember(tbDto => tbDto.NgayKetThuc, opt => opt.MapFrom(tb => tb.HoatDong.NgayKetThuc))
                .ForMember(tbDto => tbDto.AnhBia, opt => opt.MapFrom(tb => tb.HoatDong.AnhBia))
                .ForMember(tbDto => tbDto.DiaDiem, opt => opt.MapFrom(tb => tb.HoatDong.DiaDiem));

            CreateMap<BaiVietResultSet, BaiVietSoLuocDto>();

            CreateMap<ChucVuDonViDto, ChucVuDonVi>();
            CreateMap<DonViDto, DonVi>()
                .ForMember(dv => dv.Id, opt => opt.Ignore());

        }
    }
}