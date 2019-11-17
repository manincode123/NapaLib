using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs.MonHocDtos;

namespace NAPASTUDENT.Controllers
{
    public class MonHocController : Controller
    {

        public ActionResult QuanLyChungMonHoc()
        {
            return View();
        }

        [HttpGet]
        [Route("MonHoc/QuanLyMonHoc/{monHocId}")]
        public ActionResult QuanLyMonHoc(int monHocId)
        {
            return View(monHocId);
        }
        
        [HttpGet]
        [Route("MonHoc/QuanLyThiLaiMon/{monHocId}")]
        public ActionResult QuanLyThiLaiMon(int monHocId)
        {
            return View(monHocId);
        }
        
        [HttpGet]
        [Route("MonHoc/QuanLyThiLai/{lichThiLaiId}")]
        public ActionResult QuanLyThiLai(int lichThiLaiId)
        {
            return View(lichThiLaiId);
        }

        [HttpGet]
        [Route("MonHoc/QuanLyLopMonHoc")]
        public ActionResult QuanLyLopMonHoc(int monHoc, int lop)
        {
            var lopMonHoc = new LopMonHocDto() {LopId = lop, MonHocId = monHoc};
            return View(lopMonHoc);

        }

        [HttpGet]
        [Route("MonHoc/ThongTinMon/{monHocId}")]
        public ActionResult XemDiemSinhVienMonHoc(int monHocId)
        {
            //View này để sinh viên xem thông tin môn học và điểm của mình môn đó
            return View(monHocId);
        }

        [HttpGet]
        [Route("MonHoc/XemDiemHocKi/{hocKi}")]
        public ActionResult XemDiemSinhVienHocKi(int hocKi)
        {
            //View này để sinh viên xem điểm học kì của mình
            return View(hocKi);
        }
        
       





        public ActionResult LayDiemLopHocKi(int lopId,HocKi hocKi)
        {
            var diemLopHocKi = new DiemLopHocKiDto() {LopId = lopId, HocKi = hocKi};
            return View(diemLopHocKi);
        }
    }
}