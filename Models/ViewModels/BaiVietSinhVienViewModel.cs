using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.ViewModels
{
    public class BaiVietSinhVienViewModel
    {
        public int Id { get; set; }
        public string HoVaTenLot { get; set; }

        public string Ten { get; set; }

        public string AnhDaiDien { get; set; }

        public string MSSV { get; set; }

        public string GioiThieu { get; set; }
    }
}