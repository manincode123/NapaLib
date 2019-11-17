using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAPASTUDENT.Models.DTOs.SinhVienDto;

namespace NAPASTUDENT.Models.DTOs
{
    public class ChucVuLopDto
    {
        public ChucVuLopDto()
        {
            
        }
        public ChucVuLopDto(ChucVuLop cvl)
        {

            SinhVien = new TTSinhVienCBNhatDto(cvl.SinhVien);
            ChucVu = cvl.ChucVu.TenChucVu;
            ChucVuId = cvl.ChucVuId;
        }

        public TTSinhVienCBNhatDto SinhVien { get; set; }

        public string ChucVu { get; set; }
        public int ChucVuId { get; set; }

    }
}