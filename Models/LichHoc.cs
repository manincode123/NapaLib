using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAPASTUDENT.Models.DTOs.MonHocDtos;

namespace NAPASTUDENT.Models
{
    public class LichHoc
    {
        public int Id { get; set; }

        public int LopId { get; set; }

        public int MonHocId { get; set; }

        public bool BuoiSang { get; private set; }  //True là học buổi sáng, false là học buổi chiều

        public bool Thu246 { get; private set; }   // True là học thứ 2,4,6; false là học 3,5

        public bool BaTietDau { get; private set; }   // True là học 3 tiết đầu; false là học 2 tiết sau

        public DateTime NgayBatDau { get; private set; }

        public DateTime NgayKetThuc { get; private set; }

        public string GiaoVienDay { get; private set; }

        public string PhongHoc { get; private set; }

        public LopMonHoc LopMonHoc { get; set; }

        public void MapForEdit(LichHocDto lichHocDto)
        {
            BuoiSang = lichHocDto.BuoiSang;
            BaTietDau = lichHocDto.BaTietDau;
            Thu246 = lichHocDto.Thu246;
            NgayBatDau = lichHocDto.NgayBatDau;
            NgayKetThuc = lichHocDto.NgayKetThuc;
            GiaoVienDay = lichHocDto.GiaoVienDay;
            PhongHoc = lichHocDto.PhongHoc;
        }
    }
}