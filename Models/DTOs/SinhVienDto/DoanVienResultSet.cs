using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.SinhVienDto
{
    public class DoanVienResultSet
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public int Id { get; set; }
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public string MSSV { get; set; }
        public string TenGioiTinh { get; set; }
        public string KyHieuTenLop { get; set; }
        public string NgaySinh { get; set; }
        public string NgayVaoDoan { get; set; }
        public string NoiVaoDoan { get; set; }
    }
}