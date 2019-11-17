using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;

namespace NAPASTUDENT.Controllers.Api
{
    public class KhoaController : ApiController
    {
        private ApplicationDbContext _context;

        public KhoaController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/KhoaHoc/LayDanhSachKhoa")]
        public IHttpActionResult LayDanhSachKhoa()
        {
            var danhSachKhoa = _context.KhoaHoc.ToList();
            return Ok(danhSachKhoa.Select(Mapper.Map<KhoaHoc, KhoaHocDto>));
        }

        [Authorize(Roles = "Admin, QuanLyLop")]
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/KhoaHoc/LayKhoaHocDataForSave/{khoaHocId}")]
        public IHttpActionResult LayKhoaHocDataForSave(int khoaHocId)
        {
            var khoaHoc = _context.KhoaHoc.SingleOrDefault(kh => kh.Id == khoaHocId);
            if (khoaHoc == null) return NotFound();
            return Ok(Mapper.Map<KhoaHoc,KhoaHocDto>(khoaHoc));
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/KhoaHoc/LayDanhSachLop/{khoaHocId}")]
        public IHttpActionResult LayLopCuaKhoaHoc(int khoaHocId)
        {
            var danhSachLop = _context.Lop.Where(lop => lop.KhoaHocId == khoaHocId)
                .Select(lop => new
                {
                    lop.Id,
                    lop.TenLop,
                    lop.LopChuyenNganh
                }).ToList();
            return Ok(danhSachLop);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Route("api/KhoaHoc/LayDanhSachSinhVien/{khoaHocId}")]
        public IHttpActionResult LaySinhVienCuaKhoaHoc(int khoaHocId)
        {
            var danhSachSinhVien = _context.SinhVien.Where(sv => sv.KhoaHocId == khoaHocId)
                .Select(sv => new
                {
                    sv.Id,
                    sv.MSSV,
                    sv.HoVaTenLot,
                    sv.Ten,
                    sv.LopDangHoc.KyHieuTenLop
                }).ToList();
            return Ok(danhSachSinhVien);
        }
        
        [Authorize(Roles = "Admin, QuanLyLop")]
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        [Route("api/KhoaHoc/SaveKhoaHoc")]
        public IHttpActionResult SaveKhoaHoc(KhoaHocDto khoaHocDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var khoaHoc = new KhoaHoc();
            if (khoaHocDto.Id == 0)
            {
                Mapper.Map(khoaHocDto, khoaHoc);
                _context.KhoaHoc.Add(khoaHoc);
                _context.SaveChanges();
                return Ok();
            }

            khoaHoc = _context.KhoaHoc.SingleOrDefault(kh => kh.Id == khoaHocDto.Id);
            if (khoaHoc == null) return NotFound();
            Mapper.Map(khoaHocDto, khoaHoc);
            _context.SaveChanges();
            return Ok();
        }
    }
}
