using AutoMapper;
using NAPASTUDENT.Models.DTOs;

namespace NAPASTUDENT.Models.ViewModels
{
    public class LopViewModel
    {
        public LopViewModel()
        {
            
        }
        public LopViewModel(Lop lop)
        {
            LopId = lop.Id;
            AnhBia = lop.AnhBia;
            TenLop = lop.TenLop;
            KhoaHoc = Mapper.Map<KhoaHoc,KhoaHocDto>(lop.KhoaHoc);
        }
        
        public int LopId { get; set; }
        public string AnhBia { get; set; }
        public string TenLop { get; set; }
        public KhoaHocDto KhoaHoc { get; set; }
        public string GioiThieu { get; set; }
    }
}