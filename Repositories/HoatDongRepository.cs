using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using NAPASTUDENT.Controllers.Api;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs.HoatDongDto;

namespace NAPASTUDENT.Repositories
{
    public class HoatDongRepository
    {
        private readonly ApplicationDbContext _context;

        public HoatDongRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ThamGiaHoatDong> HoatDongSvThamGia()
        {
            return _context.DanhSachThamGiaHoatDong.Include(dstghd => dstghd.SinhVien)
                .Include(dstghd => dstghd.HoatDong);
        }

        public SoHoatDongToChucTrongThang LaySoHdTcTungThangChoLop_DonVi(List<HoatDongToChucDto> danhSachHoatDongToChucResultSet, 
                                                                         int namHocLay = 0)
     {
         //Ngoài ra còn dùng method này để lấy cho đơn vị, có lẽ nên đổi tên method...sau đi :p
          //Lấy số hoạt động tổ chức từng tháng của lớp và của học viện (để so sánh) 
          var soHoatDongToChucTrongThang = new SoHoatDongToChucTrongThang();
          var batDauNamHoc = new DateTime();
          var tgDungTinh = new DateTime();
          //^ Mốc thời gian để dừng lấy hoạt động, nếu là lấy hđ năm nay thì dừng tới tháng hiện tại
          //Nếu lấy hoạt động của một năm học cụ thể thì đó là 31/7/năm học
          if (namHocLay == 0) //Nếu năm học lấy = 0, lấy năm học hiện tại
              {
               tgDungTinh = DateTime.Today;
               var thisYear = tgDungTinh.Year;
               batDauNamHoc = DateTime.Today.Month < 8 ? new DateTime(thisYear - 1, 8, 1) : new DateTime(thisYear, 8, 1);
              }
           else  //Nếu ta lấy hoạt động trong 1 năm cụ thể
              {
                    tgDungTinh = new DateTime(namHocLay + 1, 7, 31);
                    batDauNamHoc = new DateTime(namHocLay, 8, 1);
              }

            for (var time = batDauNamHoc; time <= tgDungTinh; )
               {
                   soHoatDongToChucTrongThang.DanhSachThang.Add(time.Month + "-" + time.Year);
                   var soHoatDongToChuc = danhSachHoatDongToChucResultSet
                       .Where(LayHoatDongThang_HoatDongToChucDtoFunc(time))
                       .Count(hd => hd.CoThamGiaToChuc);
                   var soHoatDongHocVienToChuc = danhSachHoatDongToChucResultSet
                       .Count(LayHoatDongThang_HoatDongToChucDtoFunc(time));
                   soHoatDongToChucTrongThang.SoHoatDongLopToChuc.Add(soHoatDongToChuc);
                   soHoatDongToChucTrongThang.SoHoatDongHocVienToChuc.Add(soHoatDongHocVienToChuc);
                    time = time.AddMonths(1);
               }
            return soHoatDongToChucTrongThang;
        }        
        public SoHoatDongToChucTrongThang LaySoHdTcTungThangChoHocVien(IList<HoatDongDtoForTable> danhSachHoatDong, 
                                                                       int namHocLay = 0)
        {
            //Lấy số hoạt động tổ chức từng tháng của học viện 
            //Hoặc có thể dùng để tính số hoạt động 1 sinh viên tham gia trong từng tháng
            var soHoatDongToChucTrongThang = new SoHoatDongToChucTrongThang();
            DateTime batDauNamHoc = new DateTime();

            DateTime tgDungTinh = new DateTime();
            //^ Mốc thời gian để dừng lấy hoạt động, nếu là lấy hđ năm nay thì dừng tới tháng hiện tại
            //Nếu lấy hoạt động của một năm học cụ thể thì đó là 31/7/năm học

            if (namHocLay == 0) //Nếu ta lấy hoạt động của năm học hiện tại
            {
                tgDungTinh = DateTime.Today;
                var thisYear = tgDungTinh.Year;

                if (DateTime.Today.Month < 8) //Nghĩa là đang là tháng 1-7 của năm học này, sẽ phải lấy từ tháng 8 năm trước
                {
                    batDauNamHoc = new DateTime(thisYear - 1, 8, 1);
                }
                else      //Nếu là đang trong tháng 8 -12 thì lấy luôn năm nay
                {
                    batDauNamHoc = new DateTime(thisYear, 8, 1);
                }
            }

            else  //Nếu ta lấy hoạt động trong 1 năm cụ thể
            {
                tgDungTinh = new DateTime(namHocLay + 1, 7, 31);
                batDauNamHoc = new DateTime(namHocLay, 8, 1);
            }

            for (var time = batDauNamHoc; time <= tgDungTinh; )
            {
                soHoatDongToChucTrongThang.DanhSachThang.Add(time.Month + "-" + time.Year);
                soHoatDongToChucTrongThang.SoHoatDongHocVienToChuc.Add(danhSachHoatDong.Count(LayHoatDongThang_HoatDongDtoFunc(time)));
                time = time.AddMonths(1);
            }

            return soHoatDongToChucTrongThang;
        }
        public SoLuotThamGiaHdTrongThang LaySoLuotTgTungThangChoLop(List<LuotThamGiaHoatDongLopForDataTable> danhSachLuotThamGia,
                                                                          int namHocLay = 0)
        {
            //Lấy số lượt tham gia hoạt động từng tháng của sinh viên lớp và của sinh viên học viện (để so sánh) 
            var soLuotThamGiaHdTrongThang = new SoLuotThamGiaHdTrongThang();
            DateTime batDauNamHoc = new DateTime();
            DateTime tgDungTinh = new DateTime();
            //^ Mốc thời gian để dừng lấy  số lượt tham gia hoạt động, nếu là lấy số lượt năm nay thì dừng tới tháng hiện tại
            //Nếu lấy số lượt tham gia hoạt động của một năm học cụ thể thì đó là 31/7/năm học

            if (namHocLay == 0) //Nếu ta lấy số lượt tham gia của năm học hiện tại
            {
                tgDungTinh = DateTime.Today;
                var thisYear = tgDungTinh.Year;

                if (DateTime.Today.Month < 8) //Nghĩa là đang là tháng 1-7 của năm học này, sẽ phải lấy từ tháng 8 năm trước
                {
                    batDauNamHoc = new DateTime(thisYear - 1, 8, 1);
                }
                else      //Nếu là đang trong tháng 8 -12 thì lấy luôn năm nay
                {
                    batDauNamHoc = new DateTime(thisYear, 8, 1);
                }
            }

            else  //Nếu ta lấy hoạt động trong 1 năm cụ thể
            {
                tgDungTinh = new DateTime(namHocLay + 1, 7, 31);
                batDauNamHoc = new DateTime(namHocLay, 8, 1);
            }

            for (var time = batDauNamHoc; time <= tgDungTinh; )
            {
                soLuotThamGiaHdTrongThang.DanhSachThang.Add(time.Month + "-" + time.Year);
                var soLuotThamGiaLop = danhSachLuotThamGia
                                       .Where(LayHdSvThamGiaTrongThangFunc(time))  //Trong tháng đó
                                       .Sum(tg => tg.SoLuotThamGiaLop); //Tổng lượt tham gia từng hoạt động của lớp
                var soLuotThamGiaToanHocVien = danhSachLuotThamGia
                                              .Where(LayHdSvThamGiaTrongThangFunc(time))  //Trong tháng đó
                                              .Sum(tg => tg.SoLuotThamGiaToanHoatDong); //Lấy tổng lượt tham gia từng hoạt động
                soLuotThamGiaHdTrongThang.SoLuotThamGiaLop.Add(soLuotThamGiaLop);
                soLuotThamGiaHdTrongThang.SoLuotThamGiaHocVien.Add(soLuotThamGiaToanHocVien);
                time = time.AddMonths(1);
            }

            return soLuotThamGiaHdTrongThang;
        }
        public SoHoatDongTheoCapHoatDong LaySoHoatDongTungCap(List<HoatDongDtoForTable> danhSachHoatDong)
        {
            var soHoatDongTheoCapHoatDong = new SoHoatDongTheoCapHoatDong();
            soHoatDongTheoCapHoatDong.DanhSachCapHoatDong = new List<string>{"Cấp Phân viện","Cấp Khóa","Cấp Liên Chi hội","Cấp Chi hội"};
            //Thêm số hoạt động cấp Phân viện (index = 0)
            soHoatDongTheoCapHoatDong.SoHoatDongTungCap.Add(danhSachHoatDong.Count(hd => hd.CapHoatDong == CapHoatDong.CapPhanVien));
            //Thêm số hoạt động cấp Khóa (index = 1)
            soHoatDongTheoCapHoatDong.SoHoatDongTungCap.Add(danhSachHoatDong.Count(hd => hd.CapHoatDong == CapHoatDong.CapKhoa));
            //Thêm số hoạt động cấp Liên Chi hội (index = 2)
            soHoatDongTheoCapHoatDong.SoHoatDongTungCap.Add(danhSachHoatDong.Count(hd => hd.CapHoatDong == CapHoatDong.CapLienChiHoi));
            //Thêm số hoạt động cấp Chi hội (index = 3)
            soHoatDongTheoCapHoatDong.SoHoatDongTungCap.Add(danhSachHoatDong.Count(hd => hd.CapHoatDong == CapHoatDong.CapChiHoi));
            return soHoatDongTheoCapHoatDong;
        }
        public SoLuotThamGiaHdTrongThang LaySoLuotTgTungThangChoHocVien(IList<HoatDongDtoForTable> hoatDong,
                                                                        int namHocLay = 0)
        {
            //Lấy số lượt tham gia hoạt động trong từng tháng của sinh viên học viện
            var soLuotThamGiaHdTrongThang = new SoLuotThamGiaHdTrongThang();
            DateTime batDauNamHoc = new DateTime();

            DateTime tgDungTinh = new DateTime();
            //^ Mốc thời gian để dừng lấy  số lượt tham gia hoạt động, nếu là lấy số lượt năm nay thì dừng tới tháng hiện tại
            //Nếu lấy số lượt tham gia hoạt động của một năm học cụ thể thì đó là 31/7/năm học

            if (namHocLay == 0) //Nếu ta lấy số lượt tham gia của năm học hiện tại
            {
                tgDungTinh = DateTime.Today;
                var thisYear = tgDungTinh.Year;


                if (DateTime.Today.Month < 8) //Nghĩa là đang là tháng 1-7 của năm học này, sẽ phải lấy từ tháng 8 năm trước
                {
                    batDauNamHoc = new DateTime(thisYear - 1, 8, 1);
                }
                else      //Nếu là đang trong tháng 8 -12 thì lấy luôn năm nay
                {
                    batDauNamHoc = new DateTime(thisYear, 8, 1);
                }
            }

            else  //Nếu ta lấy hoạt động trong 1 năm cụ thể
            {
                tgDungTinh = new DateTime(namHocLay + 1, 7, 31);                    
                batDauNamHoc = new DateTime(namHocLay, 8, 1);
            }
            //Lặp qua từng tháng để đếm số lượng tham gia hoạt động tháng đó
            for (var time = batDauNamHoc; time <= tgDungTinh; )
            {
                soLuotThamGiaHdTrongThang.DanhSachThang.Add(time.Month + "-" + time.Year);
                soLuotThamGiaHdTrongThang.SoLuotThamGiaHocVien.Add(hoatDong.Where(LayHoatDongThang_HoatDongDtoFunc(time)).Sum(hd => hd.SoLuotThamGia));
                time = time.AddMonths(1);
            }
            return soLuotThamGiaHdTrongThang;
        }
        public Expression<Func<ThamGiaHoatDong, bool>> LayHoatDongNam_ThamGiaHoatDongFunc(int namHocLay = 0)
        {
            DateTime batDauNamHoc;
            DateTime tgDungTinh;

            if (namHocLay == 0) //Nếu không có năm học lấy nghĩa là lấy năm nay
            {
                var thisMonth = DateTime.Today.Month;
                var thisYear = DateTime.Today.Year;

                if (thisMonth >= 8)
                {
                    batDauNamHoc = new DateTime(thisYear, 8, 1);
                    tgDungTinh = new DateTime(thisYear + 1, 7, 31);
                }
                else
                {
                    batDauNamHoc = new DateTime(thisYear - 1, 8, 1);
                    tgDungTinh = new DateTime(thisYear, 7, 31);
                }
            }
            else  //Nếu có thì tạo datetime dựa trên năm lấy
            {
                batDauNamHoc = new DateTime(namHocLay, 8, 1);
                tgDungTinh = new DateTime(namHocLay + 1, 7, 31);
            }

            return dstghd => dstghd.DuocPheDuyet //Lượt tham gia phải được phê duyệt
                          && dstghd.HoatDong.DuocPheDuyet //Hoạt động phải được phê duyệt
                          && (   
                                 dstghd.HoatDong.NgayBatDau >= batDauNamHoc && dstghd.HoatDong.NgayBatDau <= tgDungTinh
                                 || 
                                 dstghd.HoatDong.NgayKetThuc >= batDauNamHoc && dstghd.HoatDong.NgayKetThuc <= tgDungTinh
                              ); 
        }

        public Expression<Func<HoatDong, bool>> LayHoatDongNam_HoatDongFunc(int namHocLay=0)
        {
            DateTime batDauNamHoc;
            DateTime tgDungTinh;

            if (namHocLay == 0)  //Nếu không có năm học lấy nghĩa là lấy năm nay
            {
                var thisMonth = DateTime.Today.Month;
                var thisYear = DateTime.Today.Year;

                if (thisMonth >= 8)  //Nếu tháng này là tháng 8 trở đi thì lấy từ tháng 8/năm nay - tháng 7/năm sau
                {
                    batDauNamHoc = new DateTime(thisYear, 8, 1);
                    tgDungTinh = new DateTime(thisYear + 1, 7, 31);
                }
                else   //Nếu tháng này là trước tháng 8 (tháng 7 trở xuống) thì lấy từ tháng 8/năm trước - tháng 7/năm nay
                {
                    batDauNamHoc = new DateTime(thisYear - 1, 8, 1);
                    tgDungTinh = new DateTime(thisYear + 1, 7, 31);
                }
            }
            else  //Nếu có thì tạo datetime dựa trên năm lấy
            {
                batDauNamHoc = new DateTime(namHocLay, 8, 1);
                tgDungTinh = new DateTime(namHocLay + 1, 7, 31);
            }

            return hd => (hd.NgayBatDau >= batDauNamHoc && hd.NgayBatDau <= tgDungTinh)
                         || (hd.NgayKetThuc >= batDauNamHoc && hd.NgayKetThuc <= tgDungTinh);
        }
        public Expression<Func<HoatDongDtoForTable, bool>> LayHoatDongNam_HoatDongDtoFunc(int namHocLay=0)
        {
            DateTime batDauNamHoc;
            DateTime tgDungTinh;

            if (namHocLay == 0)  //Nếu không có năm học lấy nghĩa là lấy năm nay
            {
                var thisMonth = DateTime.Today.Month;
                var thisYear = DateTime.Today.Year;

                if (thisMonth >= 8)  //Nếu tháng này là tháng 8 trở đi thì lấy từ tháng 8/năm nay - tháng 7/năm sau
                {
                    batDauNamHoc = new DateTime(thisYear, 8, 1);
                    tgDungTinh = new DateTime(thisYear + 1, 7, 31);
                }
                else   //Nếu tháng này là trước tháng 8 (tháng 7 trở xuống) thì lấy từ tháng 8/năm trước - tháng 7/năm nay
                {
                    batDauNamHoc = new DateTime(thisYear - 1, 8, 1);
                    tgDungTinh = new DateTime(thisYear + 1, 7, 31);
                }
            }
            else  //Nếu có thì tạo datetime dựa trên năm lấy
            {
                batDauNamHoc = new DateTime(namHocLay, 8, 1);
                tgDungTinh = new DateTime(namHocLay + 1, 7, 31);
            }
            
            return hd => (hd.NgayBatDau >= batDauNamHoc && hd.NgayBatDau <= tgDungTinh)
                         || (hd.NgayKetThuc >= batDauNamHoc && hd.NgayKetThuc <= tgDungTinh)
                         && hd.DuocPheDuyet;
        }
        public Func<HoatDongDtoForTable, bool> LayHoatDongThang_HoatDongDtoFunc(DateTime time)
        {
            //biến time sẽ là ngày đầu tiên của một tháng. Ví dụ 1/1/2018
            var thang = time.Month;
            var nam = time.Year;

            return hd => ((hd.NgayBatDau.Month <= thang && hd.NgayBatDau.Year == nam)
                         || hd.NgayBatDau <= time)
                         &&
                         ((hd.NgayKetThuc.Month >= thang && hd.NgayKetThuc.Year == nam)
                         || hd.NgayKetThuc >= time);
            //Func này được dùng để lấy hoạt động mà đơn vị, lớp tổ chức tháng này
        }
        public Func<HoatDongToChucDto, bool> LayHoatDongThang_HoatDongToChucDtoFunc(DateTime time)
        {
            //biến time sẽ là ngày đầu tiên của một tháng. Ví dụ 1/1/2018
            var thang = time.Month;
            var nam = time.Year;

            return hd => ((hd.NgayBatDau.Month <= thang && hd.NgayBatDau.Year == nam)
                         || hd.NgayBatDau <= time)
                         &&
                         ((hd.NgayKetThuc.Month >= thang && hd.NgayKetThuc.Year == nam)
                         || hd.NgayKetThuc >= time);
            //Func này được dùng để lấy hoạt động mà đơn vị, lớp tổ chức tháng này
        }
        public Func<LuotThamGiaHoatDongLopForDataTable, bool> LayHdSvThamGiaTrongThangFunc(DateTime time)
        {
            //biến time sẽ là ngày đầu tiên của một tháng. Ví dụ 1/1/2018

            var thang = time.Month;
            var nam = time.Year;

            return dstghd => ((dstghd.NgayBatDau.Month <= thang && dstghd.NgayBatDau.Year == nam)
                              || dstghd.NgayBatDau <= time)
                             &&
                             ((dstghd.NgayKetThuc.Month >= thang && dstghd.NgayKetThuc.Year == nam)
                              || dstghd.NgayKetThuc >= time);
            //Func này được dùng để lấy hoạt động mà đơn vị, lớp tổ chức trong 1 tháng
        }

        
    }
}