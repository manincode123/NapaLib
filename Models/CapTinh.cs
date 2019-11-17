using System.Collections.Generic;

namespace NAPASTUDENT.Models
{
    public class CapTinh
    {
        public int Id { get; set; }

        public string TenTinh { get; set; }

        public virtual IList<CapHuyen> DanhSachHuyen { get; set; }


    }
}