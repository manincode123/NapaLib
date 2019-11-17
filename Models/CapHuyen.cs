using System.Collections.Generic;

namespace NAPASTUDENT.Models
{
    public class CapHuyen
    {
        public int Id { get; set; }

        public string TenHuyen { get; set; }

        public CapTinh CapTinh { get; set; }

        public int CapTinhId { get; set; }
        public virtual IList<CapXa> DanhSachXa { get; set; }

    }
}