using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class BaiVietDonVi
    {

        public BaiViet BaiViet { get; set; }

        public DonVi DonVi { get; set; }

        public int BaiVietId { get; set; }

        public int DonViId { get; set; }

    }
}