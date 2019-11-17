using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class ThongTinMonHocSinhVienDto
    {
        public int Id { get; set; }

        public string MSSV { get; set; }

        public string Ten { get; set; }

        public string HoVaTenLot { get; set; }

        public DiemForTableDto Diem { get; set; }

        public int LopId { get; set; }

        public LichThiLai4SinhVienMon LichThiLai { get; set; }

    }
}