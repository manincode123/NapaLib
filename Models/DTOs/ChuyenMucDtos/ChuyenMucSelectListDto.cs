using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models.DTOs.ChuyenMucDtos
{
    public class ChuyenMucSelectListDto
    {
        public ChuyenMucSelectListDto()
        {
            DanhSachChuyenMucCon = new List<ChuyenMucSelectListDto>();
        }

        public string TenChuyenMuc { get; set; }       //Là tên chuyên mục

        public int Id { get; set; }

        public int ChuyenMucChaId { get; set; }

        public IList<ChuyenMucSelectListDto> DanhSachChuyenMucCon { get; set; }
    }
}