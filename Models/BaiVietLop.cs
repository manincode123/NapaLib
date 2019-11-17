using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class BaiVietLop
    {
        public virtual BaiViet BaiViet { get; set; }

        public virtual Lop Lop { get; set; }

        public int BaiVietId { get; set; }

        public int LopId { get; set; }

    }
}