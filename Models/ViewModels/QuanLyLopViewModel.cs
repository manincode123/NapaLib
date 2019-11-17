using AutoMapper;
using NAPASTUDENT.Models.DTOs;

namespace NAPASTUDENT.Models.ViewModels
{
    public class QuanLyLopViewModel
    {
        public QuanLyLopViewModel()
        {
            
        }
        public QuanLyLopViewModel(Lop lop)
        {
            LopId = lop.Id;
            AnhBia = lop.AnhBia;
            TenLop = lop.TenLop;
            KhoaHoc = Mapper.Map<KhoaHoc, KhoaHocDto>(lop.KhoaHoc);
        }

        public int ChucVu { get;private set; }
        /*Đối với trang quản lý lớp*/
        //ChucVu = 1: quyền truy cập cao nhất dành cho lớp trưởng và admin và quản lý lớp
        //ChucVu = 2: quyền truy cập cho Bí thư, chi hội trưởng (Chỉ thấy nút đổi chức vụ và nút quản lý hoạt động)
        public int LopId { get; set; }
        public string AnhBia { get; set; }
        public string TenLop { get; set; }
        public KhoaHocDto KhoaHoc { get; set; }
        public string GioiThieu { get; set; }

        public void SetChucVuLopTruong()
        {
            ChucVu = 1;
        }

        public void SetChucVuBiThuChiHoiTruong()
        {
            ChucVu = 2;
        }
    }
}