using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NAPASTUDENT.Models.DTOs.HoatDongDto;

namespace NAPASTUDENT.Models
{
    public class ThongBaoHoatDong
    {
        public int Id { get; set; }
        [Required]
        public HoatDong HoatDong { get;private set; }
        public int HoatDongId { get; set; }
        public LoaiThongBaoHoatDong LoaiThongBaoHoatDong { get; private set; }
        public DateTime NgayTaoThongBao { get; private set; }
        public DateTime? NgayBatDauGoc { get;private set; }
        public DateTime? NgayKetThucGoc { get; private set; }
        public string DiaDiemGoc { get; private set; }

        protected ThongBaoHoatDong()
        {
            
        }

        protected ThongBaoHoatDong(HoatDong hoatDong,LoaiThongBaoHoatDong loaiThongBao)
        {
            if (hoatDong == null) throw new ArgumentNullException("hoatDong");
            if (loaiThongBao == 0) throw new ArgumentNullException("loaiThongBao");
            HoatDong = hoatDong;
            LoaiThongBaoHoatDong = loaiThongBao;
            NgayTaoThongBao = DateTime.Now;
        }

        public static ThongBaoHoatDong TaoThongBaoKetThuc(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.DaKetThuc);
            return thongBao;
        }
        public static ThongBaoHoatDong TaoThongBaoHuy (HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.BiHuy);            
            return thongBao;
        }

        public static ThongBaoHoatDong TaoThongBaoPheDuyet(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.DuocPheDuyet);
            return thongBao;
        }
        
        public static ThongBaoHoatDong TaoThongBaoPheDuyetDangKi(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.PheDuyetDangKi);
            return thongBao;
        }

        public static ThongBaoHoatDong TaoThongBaoHuyDangKi(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.HuyDangKi);
            return thongBao;
        }

        public static ThongBaoHoatDong TaoThongBaoDiemDanh(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.DiemDanh);
            return thongBao;
        }

        public static ThongBaoHoatDong TaoThongBaoHuyDiemDanh(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.HuyDiemDanh);
            return thongBao; 
        }

        public static ThongBaoHoatDong TaoThongBaoThemChuongTrinh(HoatDong hoatDong)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.ThemChuongTrinh);
            return thongBao; 
        }

        public static ThongBaoHoatDong TaoThongBaoThayDoi(HoatDong hoatDong,HoatDongDtoForSave hoatDongGoc)
        {
            var thongBao = new ThongBaoHoatDong(hoatDong, LoaiThongBaoHoatDong.ThayDoi);
            if (hoatDong.NgayBatDau != hoatDongGoc.NgayBatDau || hoatDong.NgayKetThuc != hoatDongGoc.NgayKetThuc)
            {
                thongBao.NgayBatDauGoc = hoatDongGoc.NgayBatDau;
                thongBao.NgayKetThucGoc = hoatDongGoc.NgayKetThuc;
            }
            if (hoatDong.DiaDiem != hoatDongGoc.DiaDiem) thongBao.DiaDiemGoc = hoatDongGoc.DiaDiem;
            return thongBao;
        }
    }
}