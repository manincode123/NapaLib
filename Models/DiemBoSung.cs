using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class DiemBoSung
    {
        public int Id { get; set; }

        public int SinhVienId { get; set; }

        public int MonHocId { get; set; }

        [Range(0, 10)]
        public byte? Diem { get; private set; }

        public LoaiDiem LoaiDiem { get; set; }

        public Diem DiemGoc { get; set; }

        public void SetDiem(byte? diem)
        {
            Diem = diem;
        }

        public void ResetDiem()
        {
            Diem = null;
        }
    }

    public enum LoaiDiem
    {
        ThiNoi_Ta = 1,
        ThiNghe_Ta = 2,
        ThiViet_Ta = 3,
        LyThuyet = 4,
        ThucHanh = 5,
        
    }
}