using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs
{
    public class ChuyenMucResultSet
    {
        public int Id { get; set; }
        public int? ChuyenMucChaId { get; set; }

        public string TenChuyenMucCha { get; set; }

        public string TenChuyenMuc { get; set; }

    }
}