using System.Collections.Generic;

namespace NAPASTUDENT.Models.DTOs.ChuyenMucDtos
{
    public class ChuyenMucDto
    {
        public ChuyenMucDto()
        {
            BaiViet = new List<BaiVietSoLuocDto>();
            ChuyenMucCon = new List<ChuyenMucConDto>();
        }
        public string TenChuyenMuc { get; set; }

        public int Id { get; set; }

        public IList<BaiVietSoLuocDto> BaiViet { get; set; }
        public IList<ChuyenMucConDto> ChuyenMucCon { get; set; }
    }

    public class ChuyenMucConDto
    {
        public string TenChuyenMuc { get; set; }

        public int Id { get; set; }
    }
}