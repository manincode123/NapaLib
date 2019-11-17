using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using AutoMapper;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs.ChuyenMucDtos;

namespace NAPASTUDENT.Controllers.Api
{
    public class ChuyenMucController : ApiController
    {
        private ApplicationDbContext _context;
        public ChuyenMucController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("api/ChuyenMuc/LayDanhSachChuyenMuc")]
        public IHttpActionResult LayDanhSachChuyenMucChoBaiViet( )
        {
            var danhSachChuyenMuc = _context.DanhSachChuyenMucBaiViet.ToList();
            danhSachChuyenMuc = danhSachChuyenMuc.Where(cm => cm.ChuyenMucChaId == null).ToList();
            var result = new List<object>();
            foreach (var chuyenMuc in danhSachChuyenMuc)
            {
                var level = 0;
                result.Add(new
                {
                    id = chuyenMuc.Id,
                    text = chuyenMuc.TenChuyenMuc
                });
                if (chuyenMuc.DanhSachChuyenMucCon != null) LayChuyenMucCon(chuyenMuc, level, ref result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [HttpGet]
        [Route("api/ChuyenMuc/DanhSachQuanLyChuyenMuc")]
        public IHttpActionResult LayDanhSachQuanLyChuyenMuc()
        {
            var danhSachChuyenMuc = _context.DanhSachChuyenMucBaiViet.Select(cm => new
            {
                cm.Id,
                cm.AnhBia,
                cm.TenChuyenMuc,
                cm.MoTa,
                ChuyenMucCha = new { cm.ChuyenMucChaId, cm.ChuyenMucCha.TenChuyenMuc} ,
                SoBaiViet = cm.DanhSachBaiViet.Count

            });
            return Ok(danhSachChuyenMuc);
        }

        [HttpGet]
        [Route("api/ChuyenMuc/CmChaSelectList")]
        public IHttpActionResult LayDanhSachChuyenMucChaSelectList(int chuyenMucId=0)
        {
            var danhSachChuyenMuc = _context.DanhSachChuyenMucBaiViet.ToList();
            danhSachChuyenMuc = danhSachChuyenMuc.Where(cm => cm.ChuyenMucChaId == null).ToList();
            var result = new List<object>();
            result.Add(new
            {
                Id = 0,
                text = "Không có chuyên mục cha"
                
            });
            foreach (var chuyenMuc in danhSachChuyenMuc)
            {
                if (chuyenMuc.Id != chuyenMucId && chuyenMuc.ChuyenMucChaId != chuyenMucId)
                {
                    var level = 0;
                    result.Add(new
                    {
                        id = chuyenMuc.Id,
                        text = chuyenMuc.TenChuyenMuc
                    });
                    if (chuyenMuc.DanhSachChuyenMucCon != null)
                    {
                        AddChuyenMucToSelectList(chuyenMuc.DanhSachChuyenMucCon, level, ref result, chuyenMucId);
                    }
                }
            }
            
            return Ok(result);
        }

        [HttpGet]
        [Route("api/ChuyenMuc/{chuyenMucId}")]
        public IHttpActionResult LayChuyenMuc(int chuyenMucId)
        {
            var chuyenMuc = _context.DanhSachChuyenMucBaiViet.Where(cm => cm.Id == chuyenMucId)
                .Select(cm => new
                {
                   cm.Id,
                   cm.ChuyenMucChaId,
                   cm.TenChuyenMuc,
                   cm.AnhBia,
                   cm.MoTa
                }).SingleOrDefault();
            return Ok(chuyenMuc);
        }
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        [Route("api/ChuyenMuc/SaveChuyenMuc")]
        public IHttpActionResult SaveChuyenMuc(SaveChuyenMucDto chuyenMucDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            ChuyenMucBaiViet chuyenMuc;
            if (chuyenMucDto.Id == 0)
            {
                chuyenMuc = Mapper.Map<SaveChuyenMucDto, ChuyenMucBaiViet>(chuyenMucDto);
                if (chuyenMuc.ChuyenMucChaId == 0) chuyenMuc.ChuyenMucChaId = null;
                _context.DanhSachChuyenMucBaiViet.Add(chuyenMuc);
                _context.SaveChanges();
                return Ok();
            }

            chuyenMuc = _context.DanhSachChuyenMucBaiViet.SingleOrDefault(cm => cm.Id == chuyenMucDto.Id);
            if (chuyenMuc == null) return NotFound();
            if (chuyenMucDto.AnhBia != null) chuyenMuc.AnhBia = chuyenMucDto.AnhBia;
            if (chuyenMucDto.MoTa != null) chuyenMuc.MoTa = chuyenMucDto.MoTa;
            if (chuyenMucDto.TenChuyenMuc != null) chuyenMuc.TenChuyenMuc = chuyenMucDto.TenChuyenMuc;
            if (chuyenMucDto.ChuyenMucChaId != null)
            {
                chuyenMuc.ChuyenMucChaId = chuyenMucDto.ChuyenMucChaId == 0 ? null : chuyenMucDto.ChuyenMucChaId;
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,QuanLyBaiViet")]
        [ApiValidateAntiForgeryToken]
        [Route("api/ChuyenMuc/XoaChuyenMuc/{chuyenMucId}")]
        public IHttpActionResult XoaChuyenMuc(int chuyenMucId)
        {
            var chuyenMuc = _context.DanhSachChuyenMucBaiViet.Include(cm => cm.DanhSachBaiViet)
                .SingleOrDefault(cm => cm.Id == chuyenMucId);
            if (chuyenMuc == null) return NotFound();
            chuyenMuc.XoaChuyenMuc();
            _context.SaveChanges();
            return Ok();
        }

        private void LayChuyenMucCon(ChuyenMucBaiViet chuyenMuc, int level,ref List<object> result)
        {
            level++;
            var whiteSpace = String.Concat(Enumerable.Repeat("&emsp;&emsp;", level));
            foreach (var chuyenMucCon in chuyenMuc.DanhSachChuyenMucCon)
            {
                result.Add(new
                {
                    id = chuyenMucCon.Id,
                    text = String.Concat(whiteSpace,chuyenMucCon.TenChuyenMuc)
                });
                if (chuyenMucCon.DanhSachChuyenMucCon != null) LayChuyenMucCon(chuyenMucCon, level, ref result);
                
            }
        }
        private void AddChuyenMucToSelectList(IList<ChuyenMucBaiViet> danhSachChuyenMuc, int level, ref List<object> result, int chuyenMucId)
        {
            level++;
            var whiteSpace = String.Concat(Enumerable.Repeat("&emsp;&emsp;", level));
            foreach (var chuyenMuc in danhSachChuyenMuc)
            {
                if (chuyenMuc.Id != chuyenMucId && chuyenMuc.ChuyenMucChaId != chuyenMucId)
                {
                    result.Add(new
                    {
                        id = chuyenMuc.Id,
                        text = String.Concat(whiteSpace, chuyenMuc.TenChuyenMuc)
                    });
                    if (chuyenMuc.DanhSachChuyenMucCon != null) AddChuyenMucToSelectList(chuyenMuc.DanhSachChuyenMucCon, level, ref result, chuyenMucId);
                }
            }
        }
        
    }
}
