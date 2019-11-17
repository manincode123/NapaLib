using System;

namespace NAPASTUDENT.Models.DTOs.MonHocDtos
{
    public class LichHocDto
    {
        public int Id { get; set; }

        public int LopId { get; set; }

        public int MonHocId { get; set; }
        public bool BuoiSang { get; set; }  //True là học buổi sáng, false là học buổi chiều

        public bool Thu246 { get; set; }   // True là học thứ 2,4,6; false là học 3,5

        public bool BaTietDau { get; set; }   // True là học 3 tiết đầu; false là học 2 tiết sau

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public string GiaoVienDay { get; set; }

        public string PhongHoc { get; set; }
    }
}