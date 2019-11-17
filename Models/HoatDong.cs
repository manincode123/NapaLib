using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using AutoMapper;
using NAPASTUDENT.Models.DTOs.HoatDongDto;

namespace NAPASTUDENT.Models
{
    public class HoatDong
    {
        public int Id { get; set; }
        public string TenHoatDong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string NoiDung { get; set; }
        public string SoLuoc { get; set; }
        public string AnhBia { get; set; }
        public byte SoNgayTinhNguyen { get; set; }
        public int SoLuotThamGia { get;private set; }
        public CapHoatDong CapHoatDong { get; set; }
        public bool DaKetThuc { get;private set; }
        public bool BiHuy { get; private set; }
        public bool HoatDongNgoaiHocVien { get; set; }
        public bool DuocPheDuyet { get; private set; }
        public string DiaDiem { get; set; }
        public int IdSinhVienTaoHd { get; set; }
        public SinhVien SinhVienTaoHd { get; set; }
        public IList<HoatDongDonVi> DanhSachDonViToChuc { get;private set; }

        public IList<HoatDongLop> DanhSachLopToChuc { get;private set; }

        public IList<ThamGiaHoatDong> DanhSachSinhVienThamGia { get; set; }
        public IList<TheoDoiHoatDong> DanhSachSinhVienTheoDoi { get; set; }
        public IList<BaiVietHoatDong> DanhSachBaiViet{ get; set; }
        public IList<ChuongTrinhHoatDong> DanhSachChuongTrinhHoatDong { get; set; }

        public IList<ThongBaoHoatDong> DanhSachThongBaoHoatDong { get; set; }


        public HoatDong()
        {
            DanhSachSinhVienThamGia = new List<ThamGiaHoatDong>();
            DanhSachSinhVienTheoDoi = new List<TheoDoiHoatDong>();
            DanhSachDonViToChuc = new List<HoatDongDonVi>();
            DanhSachLopToChuc = new List<HoatDongLop>();
            DanhSachBaiViet = new List<BaiVietHoatDong>();
            DanhSachChuongTrinhHoatDong = new List<ChuongTrinhHoatDong>();
            DanhSachThongBaoHoatDong = new List<ThongBaoHoatDong>();
            SoLuotThamGia = 0;
        }

        public void TaoHoatDong(HoatDongDtoForSave hoatDongDto, int sinhVienId, List<Lop> danhSachLop, List<DonVi> danhSachDonVi)
        {
            Mapper.Map(hoatDongDto,this);

            ThemLopToChuc(danhSachLop,hoatDongDto.DanhSachLopToChuc);
            ThemDonViToChuc(danhSachDonVi, hoatDongDto.DanhSachDonViToChuc);

            IdSinhVienTaoHd = sinhVienId;
            //Tạo các thông báo thông dụng mà hầu như hoạt động nào cũng sử dụng
            DanhSachThongBaoHoatDong.Add(ThongBaoHoatDong.TaoThongBaoPheDuyetDangKi(this));
            DanhSachThongBaoHoatDong.Add(ThongBaoHoatDong.TaoThongBaoHuyDangKi(this));
            DanhSachThongBaoHoatDong.Add(ThongBaoHoatDong.TaoThongBaoDiemDanh(this));
            DanhSachThongBaoHoatDong.Add(ThongBaoHoatDong.TaoThongBaoHuyDiemDanh(this));
        }

        public void ThayDoi(HoatDongDtoForSave hoatDongDto, List<Lop> danhSachLop, List<DonVi> danhSachDonVi)
        {
            var hoatDongGoc = Mapper.Map<HoatDong,HoatDongDtoForSave>(this);  //Lưu biến hoạt động lại trước khi thay đổi

            Mapper.Map(hoatDongDto, this);
            ThemLopToChuc(danhSachLop, hoatDongDto.DanhSachLopToChuc);
            ThemDonViToChuc(danhSachDonVi, hoatDongDto.DanhSachDonViToChuc);

            var thongBaoMoi = ThongBaoHoatDong.TaoThongBaoThayDoi(this, hoatDongGoc);
            foreach (var sinhVien in DanhSachSinhVienTheoDoi.Select(td => td.SinhVien))
            {
                sinhVien.ThongBaoHoatDong(thongBaoMoi);
            }
        }
        public void ThemLopToChuc(List<Lop> danhsachLop, IList<int> danhSachLopToChuc)
        {
            if (danhsachLop == null) throw new ArgumentNullException("danhsachLop");
            DanhSachLopToChuc.Clear(); //Clear trước, vì có trường hợp SỬA HOẠT ĐỘNG với  danhSachLopToChuc = null
                                       //có nghĩa là hoạt động không có lớp tổ chức
                                       //Xóa các lớp tổ chức trước đó nếu có
            if (danhSachLopToChuc == null) return; //Nếu truyền vào danhSachLopToChuc null thì không tiếp tục            
            foreach (var lopId in danhSachLopToChuc)
            {
                var lop = danhsachLop.SingleOrDefault(l => l.Id == lopId);
                DanhSachLopToChuc.Add(new HoatDongLop
                {
                    Lop = lop
                });
            }
        }

        public void ThemDonViToChuc(List<DonVi> danhSachDonVi, IList<int> danhSachDonViToChuc)
        {
            if (danhSachDonVi == null) throw new ArgumentNullException("danhSachDonVi");
            DanhSachDonViToChuc.Clear(); //Clear trước, vì có trường hợp SỬA HOẠT ĐỘNG với  danhSachDonViToChuc = null
                                       //có nghĩa là hoạt động không có đơn vị tổ chức
                                       //Xóa các đơn vị tổ chức trước đó nếu có
            if (danhSachDonViToChuc == null) return;  //Nếu truyền vào danhSachDonViToChuc null thì không tiếp tục
            foreach (var donViId in danhSachDonViToChuc)
            {
                var donVi = danhSachDonVi.SingleOrDefault(l => l.Id == donViId);
                DanhSachDonViToChuc.Add(new HoatDongDonVi
                {
                    DonVi = donVi
                });
            }
        }

        public void HuyHoatDong()
        {
            var thongBaoMoi = ThongBaoHoatDong.TaoThongBaoHuy(this);
            foreach (var sinhVien in DanhSachSinhVienTheoDoi.Select(td => td.SinhVien))
            {
                sinhVien.ThongBaoHoatDong(thongBaoMoi);
            }
            SinhVienTaoHd.ThongBaoHoatDong(thongBaoMoi);
            BiHuy = true;
            DuocPheDuyet = true;
            DanhSachSinhVienThamGia.Clear();
            DanhSachSinhVienTheoDoi.Clear();
            DanhSachChuongTrinhHoatDong.Clear();
            SoLuotThamGia = 0;
        }

        public void KetThucHoatDong()
        {
            var thongBaoMoi = ThongBaoHoatDong.TaoThongBaoKetThuc(this);
            foreach (var sinhVien in DanhSachSinhVienTheoDoi.Select(td => td.SinhVien))
            {
                sinhVien.ThongBaoHoatDong(thongBaoMoi);
            }
            SinhVienTaoHd.ThongBaoHoatDong(thongBaoMoi);
            DaKetThuc = true;
        }

        public void PheDuyetHoatDong()
        {
            var thongBaoMoi = ThongBaoHoatDong.TaoThongBaoPheDuyet(this);
            SinhVienTaoHd.ThongBaoHoatDong(thongBaoMoi);
            DuocPheDuyet = true;
        }
                                                                       
        public void MoLaiHoatDong()
        {
            DaKetThuc = false;
        }

        public void KhoiPhucHoatDong()
        {
            BiHuy = false;
            DuocPheDuyet = false;
        }

        public void ThemLuotThamGia(ThamGiaHoatDong luotThamGia, SinhVien sinhVien)
        {
            DanhSachSinhVienThamGia.Add(luotThamGia);
            sinhVien.ThongBaoHoatDong(ThongBaoHoatDong.TaoThongBaoDiemDanh(this));
            SoLuotThamGia++;
        }
        public void ThemLuotDangKi(ThamGiaHoatDong luotDangKi)
        {
            DanhSachSinhVienThamGia.Add(luotDangKi);
        }
        public void ThemLuotTheoDoi(TheoDoiHoatDong luotTheoDoi)
        {
            DanhSachSinhVienTheoDoi.Add(luotTheoDoi);
        }
        public void XoaLuotThamGia()
        {
            SoLuotThamGia--;
        }


        public void ThemChuongTrinh(ChuongTrinhHoatDongDto chuongTrinhDto)
        {
            var chuongTrinh = new ChuongTrinhHoatDong(chuongTrinhDto);
            DanhSachChuongTrinhHoatDong.Add(chuongTrinh);
            var thongBaoMoi = ThongBaoHoatDong.TaoThongBaoThemChuongTrinh(this);
            foreach (var sinhVien in DanhSachSinhVienTheoDoi.Select(td => td.SinhVien))
            {
                sinhVien.ThongBaoHoatDong(thongBaoMoi);
            }
        }

        public void XoaLopToChuc(int lopId)
        {
            var lop = DanhSachLopToChuc.SingleOrDefault(l => l.LopId == lopId);
            DanhSachLopToChuc.Remove(lop);
        }

        public void XoaDonViToChuc(int donViId)
        {
            var donVi = DanhSachDonViToChuc.SingleOrDefault(dv => dv.DonViId == donViId);
            DanhSachDonViToChuc.Remove(donVi);
        }

        public void TangSoLuotThamGia()
        {
            SoLuotThamGia++;
        }
    }
}