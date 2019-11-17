using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;

namespace NAPASTUDENT.Controllers.Api
{
    public class ThongBaoController : ApiController
    {
        private ApplicationDbContext _context;

        public ThongBaoController()
        {
            _context= new ApplicationDbContext();
        }

        [HttpGet]
        [Route("api/ThongBao/HoatDong")]
        public IHttpActionResult LayThongBaoHoatDong()
        {
            var userId = User.Identity.GetUserId();
            var sinhVienId = _context.SinhVien.Where(sv => sv.ApplicationUserId == userId)
                                            .Select(sv => sv.Id).SingleOrDefault();
            var thongBaoChuaDoc = _context.DanhSachThongBaoHoatDongSinhVien
                .Where(tbsv => tbsv.SinhVienId == sinhVienId && !tbsv.DaDoc)
                .Select(tbsv => tbsv.ThongBaoHoatDong)
                .Include(tbhd => tbhd.HoatDong)
                .OrderByDescending(tbhd => tbhd.NgayTaoThongBao)
                .ToList();
            var thongBaoTruocDo = _context.DanhSachThongBaoHoatDongSinhVien
                .Where(tbsv => tbsv.SinhVienId == sinhVienId && tbsv.DaDoc)
                .Select(tbsv => tbsv.ThongBaoHoatDong)
                .Include(tbhd => tbhd.HoatDong)
                .OrderByDescending(tbhd => tbhd.NgayTaoThongBao)
                .Take(10)
                .ToList();
            return Ok(new
            {
                thongBaoChuaDoc = thongBaoChuaDoc.Select(Mapper.Map<ThongBaoHoatDong, ThongBaoHoatDongDto>),
                thongBaoTruocDo = thongBaoTruocDo.Select(Mapper.Map<ThongBaoHoatDong, ThongBaoHoatDongDto>)
            });
        } 
        
        [HttpGet]
        [Route("api/ThongBao/ThongBaoHoatDongTiepTheo/{recordStart}")]
        public IHttpActionResult LayThongBaoHoatDongTiepTheo(int recordStart)
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (userSinhVienId == 0) return NotFound();
            var thongBaoTruocDo = _context.DanhSachThongBaoHoatDongSinhVien
                .Where(tbsv => tbsv.SinhVienId == userSinhVienId && tbsv.DaDoc)
                .Select(tbsv => tbsv.ThongBaoHoatDong)
                .Include(tbhd => tbhd.HoatDong)
                .OrderByDescending(tbhd => tbhd.NgayTaoThongBao)
                .Skip(recordStart)
                .Take(10)
                .ToList();
            return Ok(thongBaoTruocDo.Select(Mapper.Map<ThongBaoHoatDong, ThongBaoHoatDongDto>));
        }

        [HttpPost]
        [Route("api/ThongBao/DanhDauThongBaoHoatDongDaDoc")]
        public IHttpActionResult DanhDauThongBaoDaDoc()
        {
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (userSinhVienId == 0) return NotFound();
            var thongBaoChuaDoc = _context.DanhSachThongBaoHoatDongSinhVien
                .Where(tbsv => tbsv.SinhVienId == userSinhVienId && !tbsv.DaDoc)
                .ToList();
            foreach (var thongBaoHoatDongSinhVien in thongBaoChuaDoc)
            {
                thongBaoHoatDongSinhVien.DanhDauDaDoc();
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
