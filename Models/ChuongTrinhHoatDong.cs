using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NAPASTUDENT.Models.DTOs.HoatDongDto;

namespace NAPASTUDENT.Models
{
    public class ChuongTrinhHoatDong
    {
        public int Id { get; set; }

        public string NoiDungChuongTrinh { get; set; }

        public string TieuDe { get; set; }

        public DateTime TgDienRa { get; set; }

        public LoaiHienThi LoaiHienThi { get; set; }

        public HoatDong HoatDong { get; set; }
        public int HoatDongId { get; set; }

        public ChuongTrinhHoatDong()
        {
            
        }

        public ChuongTrinhHoatDong(ChuongTrinhHoatDongDto chuongTrinhDto)
        {
            Mapper.Map(chuongTrinhDto,this);
        }
    }

    public enum LoaiHienThi
    {
        HienThiNgay = 1,
        HienThiGio = 2
    }
}