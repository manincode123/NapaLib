using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NAPASTUDENT.Controllers.AntiHack;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.DTOs;
using NAPASTUDENT.Models.DTOs.SinhVienDto;
using OfficeOpenXml;
namespace NAPASTUDENT.Controllers.Api
{
    public class SinhVienController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SinhVienController()
        {
            _context = new ApplicationDbContext();
            var roleStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(roleStore);
        }


        /*Quản lý chung sinh viên*/
        [System.Web.Http.Authorize(Roles = "Admin,QuanLySinhVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DanhSachTatCaSinhVien")]
        public IHttpActionResult LayTatCaSinhVien(PagingSearchingSorting pagingSearchingSorting)
        {
            var sinhVienResultSet = new List<SinhVienResultSet>();
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);
            
            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                sinhVienResultSet = _context.Database
                           .SqlQuery<SinhVienResultSet>("LayDanhSachSinhVienASC @SearchTerm, " +
                                                         "@ColumnName, @RecordStart,@PageSize", 
                                                          searchTerm, columnName, recordStart, pageSize).ToList();
            }
            else
            {
                sinhVienResultSet = _context.Database
                    .SqlQuery<SinhVienResultSet>("LayDanhSachSinhVienDESC @SearchTerm," +
                                                   " @ColumnName, @RecordStart,@PageSize", 
                                                searchTerm, columnName, recordStart, pageSize).ToList();
            }
            int recordsTotal, recordsFiltered;
            if (sinhVienResultSet.Count > 0)
            {
                recordsTotal = sinhVienResultSet[0].TotalCount;
                recordsFiltered = sinhVienResultSet[0].FilteredCount;
            }
            else
            {
                recordsTotal = _context.SinhVien.Count();
                recordsFiltered = 0;
            }
            //Tính số trang dựa trên pageIndex và pageSize
            var pageNumbers = (int)Math.Ceiling((double)recordsTotal / pagingSearchingSorting.PageSize);

            return Ok(new
            {
                pagingSearchingSorting.Draw,
                data = sinhVienResultSet,
                pageNumbers,
                pagingSearchingSorting.PageIndex,
                recordsFiltered,
                recordsTotal
            });
        }


        [System.Web.Http.Authorize(Roles = "Admin,QuanLySinhVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/ThemSinhVien")]
        public IHttpActionResult ThemSinhVien(SinhVienSaveDto sinhVienDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Kiểm tra có quyền tạo không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            if (!userRole) return StatusCode(HttpStatusCode.Forbidden);
            //Kiểm tra xem đã có sinh viên nào với MSSV này chưa
            var sinhVien = _context.SinhVien.SingleOrDefault(sv => sv.MSSV == sinhVienDto.MSSV);
            if (sinhVien != null) return BadRequest("Đã có sinh viên với MSSV này");
            //Bind SinhVien Data
            sinhVien = new SinhVien();
            sinhVien.TaoSinhVien(sinhVienDto);
            //Tạo user với sinh viên
            var user = new ApplicationUser()
            {
                UserName = sinhVienDto.MSSV,
                TenNguoiDung = sinhVienDto.Ten
            };
            //Password sẽ là mã số sinh viên + ngày sinh + tháng sinh
            //Ví dụ: MSSV: AS155006, ngày sinh 10/01/1997 thì password sẽ là AS1550061001
            var password = sinhVienDto.MSSV + sinhVienDto.NgaySinh.ToString("ddMM");
            _userManager.Create(user, password);
            sinhVien.SetApplicationUser(user);
            _context.SinhVien.Add(sinhVien);
            var sinhVienResult = Mapper.Map<SinhVien, ChiTietDayDuSinhVienDto>(sinhVien);
            _context.SaveChanges();
            user.SetSinhVienId(sinhVien.Id);
            _context.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + sinhVien.Id), sinhVienResult);
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLySinhVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/TaoBatchSinhVien")]
        public IHttpActionResult TaoBatchSinhVien()
        {
            SinhVien sinhVien;
            var defaultAvatar = "/Content/AnhBia/AnhSV/avatar.png";
            //Danh sách dữ liệu mã hóa để kiểm tra
            var danhSachTonGiao = _context.TonGiao.Select(tg => tg.Id).ToList();
            var danhSachDanToc = _context.DanToc.Select(dt => dt.Id).ToList();
            var danhSachGioiTinh = _context.GioiTinh.Select(gt => gt.Id).ToList();
            var danhSachKhoa = _context.KhoaHoc.Select(khoa => khoa.Id).ToList();
            // Biến trả kết quả
            IList<SinhVienDtoForTable> danhSachSinhVienDaTao = new List<SinhVienDtoForTable>();
            IList<SinhVienTaoBiLoi> danhSachSinhVienKhongTaoDuoc = new List<SinhVienTaoBiLoi>();
            //Hai biến để lưu sinh viên Id vào User
            IList<SinhVien> danhSachSinhVien = new List<SinhVien>();
            IList<ApplicationUser> danhSachUser = new List<ApplicationUser>();
            //Đọc file excel
            var uploadFile = HttpContext.Current.Request.Files["uploadFile"];
            if (uploadFile == null || uploadFile.ContentLength <= 0) return BadRequest("Hãy chọn một file excel có nội dung.");
            var filePath = uploadFile.FileName;
            var fileExtension = Path.GetExtension(filePath);
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
                return BadRequest("Chỉ được sử dụng file excel: Tệp .xlsx hoặc .xls");

            using (var package = new ExcelPackage(uploadFile.InputStream))
            {
                //Lấy sheet và số dòng
                var workSheet = package.Workbook.Worksheets["Sheet1"];
                var noOfRow = workSheet.Dimension.End.Row;


                IList<string> danhSachMSSV = new List<string>();
                for (var rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    try
                    {
                        danhSachMSSV.Add(workSheet.Cells[rowIterator, 3].Value.ToString());
                    }
                    catch (NullReferenceException)
                    {
                        //Sẽ để ở dưới xử lý
                    }
                }
                //Lấy danh sách sinh viên sẵn có để kiểm tra xem có sinh viên với mssv trùng với mssv muốn tạo chưa
                var danhSachSinhVienTrungMSSV = _context.SinhVien.Where(sv => danhSachMSSV.Contains(sv.MSSV)).ToList();

                //Lặp qua từng dòng trong file excel (bỏ dòng đầu tiên là header) để tạo sinh viên
                for (var rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    //Gán họ tên, mssv
                    try
                    {
                        sinhVien = new SinhVien(workSheet.Cells[rowIterator, 1].Value.ToString(), //HoVaTenLot
                                                workSheet.Cells[rowIterator, 2].Value.ToString(), //Tên
                                                workSheet.Cells[rowIterator, 3].Value.ToString()); //MSSV

                    }
                    catch (NullReferenceException)
                    {
                        danhSachSinhVienKhongTaoDuoc.Add(new SinhVienTaoBiLoi()
                        {
                            Loi = "Không được để trống bất kỳ ô nào. Kiểm tra lại dữ liệu trong tệp gửi.",
                            SinhVien = new SinhVienDtoForTable()
                            {
                                HoVaTenLot = "",
                                Ten = ""
                            },
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }
                    //Gán ngày sinh
                    try
                    {
                        sinhVien.setNgaySinh((DateTime)workSheet.Cells[rowIterator, 4].Value);
                    }
                    catch (Exception e)
                    {
                        if (e is InvalidCastException)
                        {
                            danhSachSinhVienKhongTaoDuoc.Add(new SinhVienTaoBiLoi()
                            {
                                Loi = "Định dạng ngày sinh bị sai.",
                                SinhVien = Mapper.Map<SinhVien, SinhVienDtoForTable>(sinhVien),
                                SoDongBiLoi = rowIterator
                            });
                        }
                        if (e is NullReferenceException)
                        {
                            danhSachSinhVienKhongTaoDuoc.Add(new SinhVienTaoBiLoi()
                            {
                                Loi = "Không được để trống ngày sinh.",
                                SinhVien = Mapper.Map<SinhVien, SinhVienDtoForTable>(sinhVien),
                                SoDongBiLoi = rowIterator
                            });
                        }
                        continue;
                    }
                    //Nếu có MSSV này đã có đăng kí trong cơ sở dữ liệu thì tiếp tục, không tạo được
                    //Cái này để ở cuối chứ không phải đầu tiên (sau khi check tên và ngày sinh) để có tên sinh viên để hiển thị cho người quản lý dễ biết
                    if (danhSachSinhVienTrungMSSV.Any(sv => sv.MSSV == sinhVien.MSSV))
                    {
                        danhSachSinhVienKhongTaoDuoc.Add(new SinhVienTaoBiLoi()
                        {
                            Loi = "MSSV đã được đăng kí cho sinh viên khác.",
                            SinhVien = Mapper.Map<SinhVien, SinhVienDtoForTable>(sinhVien),
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }
                    //Gán các biến mã hóa
                    try
                    {
                        sinhVien.SetBienMaHoa(Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value),
                                              Convert.ToInt32(workSheet.Cells[rowIterator, 6].Value),
                                              Convert.ToInt32(workSheet.Cells[rowIterator, 7].Value),
                                              Convert.ToInt32(workSheet.Cells[rowIterator, 8].Value));
                    }
                    catch (FormatException)
                    {
                        danhSachSinhVienKhongTaoDuoc.Add(new SinhVienTaoBiLoi()
                        {
                            Loi = "Một trong các cột mã hóa bị định dạng sai. Phải là số.",
                            SinhVien = Mapper.Map<SinhVien, SinhVienDtoForTable>(sinhVien),
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }
                    //Nếu mã số không tồn tại
                    if (!danhSachDanToc.Contains(sinhVien.DanTocId) || !danhSachGioiTinh.Contains(sinhVien.GioiTinhId)
                        || !danhSachKhoa.Contains(sinhVien.KhoaHocId) || !danhSachTonGiao.Contains(sinhVien.TonGiaoId))
                    {
                        danhSachSinhVienKhongTaoDuoc.Add(new SinhVienTaoBiLoi()
                        {
                            Loi = "Dữ liệu của một trong các cột mã hóa không đúng với dữ liệu mã hóa có sẵn. Xem lại các bảng phía trên.",
                            SinhVien = Mapper.Map<SinhVien, SinhVienDtoForTable>(sinhVien),
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }

                    //Tạo tài khoản
                    var user = new ApplicationUser
                    {
                        UserName = sinhVien.MSSV,
                        TenNguoiDung = sinhVien.Ten
                    };
                    _userManager.Create(user, sinhVien.MSSV + sinhVien.NgaySinh.ToString("ddMM"));
                    sinhVien.SetApplicationUser(user); //Gán user với ApplicationUser trong SinhVien
                    danhSachUser.Add(user); //Thêm vào danh sách để tí nữa gán property SinhVienId
                    //Tạo sinh viên
                    sinhVien.SetAnhDaiDien(defaultAvatar); //Thêm ảnh đại diện mặc định
                    sinhVien.TaoDanhSachDiem(); //Tạo điểm trung bình từng học kì
                    _context.SinhVien.Add(sinhVien);  //Thêm vào danh sách để tí nữa lấy property SinhVien.Id (sau khi đã save mới có)
                    danhSachSinhVienDaTao.Add(Mapper.Map<SinhVien, SinhVienDtoForTable>(sinhVien));
                    danhSachSinhVien.Add(sinhVien);
                }
                _context.SaveChanges();
                //Lặp qua để gán property SinhVienId của User
                for (var index = 0; index < danhSachUser.Count; index++)
                {
                    danhSachUser[index].SetSinhVienId(danhSachSinhVien[index].Id);
                }
                _context.SaveChanges();
            }
            return Ok(new
            {
                danhSachSinhVienDaTao,
                danhSachSinhVienKhongTaoDuoc,
                soSinhVienDaTao = danhSachSinhVienDaTao.Count,
                soSinhVienKhongTaoDuoc = danhSachSinhVienKhongTaoDuoc.Count
            });
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPut]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/ChinhSuaSinhVien")]
        public IHttpActionResult ChinhSuaSinhVien(SinhVienSaveDto sinhVienDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != sinhVienDto.Id) return Unauthorized();
            //Phải lấy sinh viên bằng ID chứ ko phải vì MSSV vì MSSV cũng có thể thay đổi
            var sinhVien = _context.SinhVien.SingleOrDefault(sv => sv.Id == sinhVienDto.Id);
            if (sinhVien == null) return NotFound();
            if (userRole)
            {
                //Kiểm tra để tránh trường hợp thay đổi mssv nhưng trùng với mssv của người khác
                var sinhVienKhac = _context.SinhVien.Any(sv => sv.MSSV == sinhVienDto.MSSV
                                                                           && sv.Id != sinhVienDto.Id);
                if (sinhVienKhac) return BadRequest("Mã số sinh viên " + sinhVienDto.MSSV + " đã thuộc về sinh viên khác. Vui lòng chọn lại.");
                sinhVien.ChinhSuaThongTin_QuanLy(sinhVienDto);
            }
            else
            {
                sinhVien.ChinhSuaThongTin_SinhVien(sinhVienDto);
            }
            _context.SaveChanges();
            return Ok();
        }

        /*Quản lý hội viên*/
        [System.Web.Http.Authorize(Roles = "Admin,QuanLyHoiVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/LayDanhSachHoiVien")]
        public IHttpActionResult LayDanhSachHoiVien(PagingSearchingSorting pagingSearchingSorting)
        {
            List<HoiVienResultSet> hoiVienResultSet;
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);

            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                hoiVienResultSet = _context.Database
                     .SqlQuery<HoiVienResultSet>("LayDanhSachHoiVienASC @SearchTerm, " +
                                                         "@ColumnName, @RecordStart,@PageSize",
                                                          searchTerm, columnName, recordStart, pageSize).ToList();
            }
            else
            {
                hoiVienResultSet = _context.Database
                    .SqlQuery<HoiVienResultSet>("LayDanhSachHoiVienDESC @SearchTerm," +
                                                   " @ColumnName, @RecordStart,@PageSize",
                                                searchTerm, columnName, recordStart, pageSize).ToList();
            }
            int recordsTotal, recordsFiltered;
            if (hoiVienResultSet.Count > 0)
            {
                recordsTotal = hoiVienResultSet[0].TotalCount;
                recordsFiltered = hoiVienResultSet[0].FilteredCount;
            }
            else
            {
                recordsTotal = _context.DanhSachHoiVienHoiSinhVien.Count();
                recordsFiltered = 0;
            }
            //Tính số trang dựa trên pageIndex và pageSize
            var pageNumbers = (int)Math.Ceiling((double)recordsTotal / pagingSearchingSorting.PageSize);
            return Ok(new
            {
                pagingSearchingSorting.Draw,
                data = hoiVienResultSet,
                pageNumbers,
                pagingSearchingSorting.PageIndex,
                recordsFiltered,
                recordsTotal
            });
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLyHoiVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DangKiHoiVien")]
        public IHttpActionResult DangKiHoiVien(DangKiHoiVienDoanVienDto hoiVienDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var sinhVien = _context.SinhVien.Include(sv => sv.HoiVien).SingleOrDefault(sv => sv.MSSV == hoiVienDto.MSSV);
            if (sinhVien == null) return NotFound();
            sinhVien.DangKiHoiVien(hoiVienDto);
            _context.SaveChanges();
            return Ok();
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLyHoiVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DangKiHoiVienHangLoat")]
        public IHttpActionResult DangKiHoiVienHangLoat()
        {
            string mssv;
            DateTime ngayVaoHoi;
            IList<LoiDangKiHoiVienDoanVien> danhSachLoiDangKiHoiVien = new List<LoiDangKiHoiVienDoanVien>();
            IList<SinhVienDaDangKiHoiVienDoanVien> danhSachSinhVienDaDangKi = new List<SinhVienDaDangKiHoiVienDoanVien>();
            //Đọc file excel
            var uploadFile = HttpContext.Current.Request.Files["uploadFile"];
            if (uploadFile == null || uploadFile.ContentLength <= 0) return BadRequest("Hãy chọn một file excel có nội dung.");
            var filePath = uploadFile.FileName;
            var fileExtension = Path.GetExtension(filePath);
            if (fileExtension != ".xlsx" && fileExtension != ".xls") return BadRequest("Chỉ được sử dụng file excel: Tệp .xlsx hoặc .xls");

            using (var package = new ExcelPackage(uploadFile.InputStream))
            {
                //Lấy sheet và số dòng
                var workSheet = package.Workbook.Worksheets["Sheet1"];
                var noOfRow = workSheet.Dimension.End.Row;

                //Lấy danh sách sinh viên sẵn có để kiểm tra xem có sinh viên với mssv trùng với mssv muốn tạo chưa
                IList<DangKiHoiVienDoanVienDto> danhSachDangKiHoiVien = new List<DangKiHoiVienDoanVienDto>();
                IList<string> danhSachMSSV = new List<string>();
                for (var rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    //Gán biến mssv
                    try
                    {
                        mssv = workSheet.Cells[rowIterator, 1].Value.ToString();
                    }
                    catch (NullReferenceException)
                    {
                        danhSachLoiDangKiHoiVien.Add(new LoiDangKiHoiVienDoanVien()
                        {
                            Loi = "Không được để trống mã số sinh viên",
                            MSSV = "",
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }
                    //Gán biến ngày vào hội
                    try
                    {
                        ngayVaoHoi = (DateTime)workSheet.Cells[rowIterator, 2].Value;
                    }
                    catch (Exception e)
                    {
                        if (e is InvalidCastException)
                        {
                            danhSachLoiDangKiHoiVien.Add(new LoiDangKiHoiVienDoanVien()
                            {
                                Loi = "Định dạng ngày vào hội bị sai.",
                                MSSV = mssv,
                                SoDongBiLoi = rowIterator
                            });
                        }
                        if (e is NullReferenceException)
                        {
                            danhSachLoiDangKiHoiVien.Add(new LoiDangKiHoiVienDoanVien()
                            {
                                Loi = "Không được để trống ngày vào hội.",
                                MSSV = mssv,
                                SoDongBiLoi = rowIterator
                            });
                        }
                        continue;
                    }
                    //Nếu qua 2 vòng kiểm tra trên được thì thêm vào danh sách
                    danhSachDangKiHoiVien.Add(new DangKiHoiVienDoanVienDto()
                    {
                        MSSV = mssv,
                        NgayVao = ngayVaoHoi,
                        SoDong = rowIterator
                    });
                    danhSachMSSV.Add(mssv);
                }
                //Lấy danh sách sinh viên dựa trên danh sách mssv ở trên
                var danhSachSinhVien = _context.SinhVien.Include(sv => sv.HoiVien).Where(sv => danhSachMSSV.Contains(sv.MSSV)).ToList();

                //Lặp qua danh sách dangKiHoiVienDto
                foreach (var dangKiHoiVienDto in danhSachDangKiHoiVien)
                {
                    var sinhVien = danhSachSinhVien.SingleOrDefault(sv => sv.MSSV == dangKiHoiVienDto.MSSV);
                    //Kiểm tra sinh viên có tồn tại không
                    if (sinhVien == null)
                    {
                        danhSachLoiDangKiHoiVien.Add(new LoiDangKiHoiVienDoanVien()
                        {
                            Loi = "Không tồn tại sinh viên với MSSV này.",
                            MSSV = dangKiHoiVienDto.MSSV,
                            NgayVao = dangKiHoiVienDto.NgayVao,
                            SoDongBiLoi = dangKiHoiVienDto.SoDong
                        });
                        continue;
                    }
                    //Thêm Hội viên
                    sinhVien.DangKiHoiVien(dangKiHoiVienDto);
                    danhSachSinhVienDaDangKi.Add(new SinhVienDaDangKiHoiVienDoanVien()
                    {
                        HoVaTenLot = sinhVien.HoVaTenLot,
                        Ten = sinhVien.Ten,
                        MSSV = sinhVien.MSSV,
                        NgayVao = dangKiHoiVienDto.NgayVao
                    });
                }
                _context.SaveChanges();
            }
            return Ok(new
            {
                danhSachLoiDangKiHoiVien,
                soLoiDangKi = danhSachLoiDangKiHoiVien.Count,
                danhSachSinhVienDaDangKi,
                soSinhVienDaDangKi = danhSachSinhVienDaDangKi.Count
            });
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLyHoiVien")]
        [System.Web.Http.HttpDelete]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/XoaDangKiHoiVien/{mssv}")]
        public IHttpActionResult XoaDangKiHoiVien(string mssv)
        {
            var sinhVien = _context.SinhVien.Include(sv => sv.HoiVien).SingleOrDefault(sv => sv.MSSV == mssv);
            if (sinhVien == null) return NotFound();
            sinhVien.XoaDangKiHoiVien();
            _context.SaveChanges();
            return Ok();
        }

        //Quản lý đoàn viên
        [System.Web.Http.Authorize(Roles = "Admin,QuanLyDoanVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/LayDanhSachDoanVien")]
        public IHttpActionResult LayDanhSachDoanVien(PagingSearchingSorting pagingSearchingSorting)
        {
            List<DoanVienResultSet> doanVienResultSet;
            if (pagingSearchingSorting.SearchTerm == null) pagingSearchingSorting.SearchTerm = "";
            var searchTerm = new SqlParameter("@SearchTerm", pagingSearchingSorting.SearchTerm);
            var columnName = new SqlParameter("@ColumnName", pagingSearchingSorting.OrderColumn);
            var recordStart = new SqlParameter("@RecordStart", pagingSearchingSorting.StartRecord);
            var pageSize = new SqlParameter("@PageSize", pagingSearchingSorting.PageSize);

            if (pagingSearchingSorting.OrderType.ToUpper() == "ASC")
            {
                doanVienResultSet = _context.Database
                    .SqlQuery<DoanVienResultSet>("LayDanhSachDoanVienASC @SearchTerm, " +
                                                "@ColumnName, @RecordStart,@PageSize",
                        searchTerm, columnName, recordStart, pageSize).ToList();
            }
            else
            {
                doanVienResultSet = _context.Database
                    .SqlQuery<DoanVienResultSet>("LayDanhSachDoanVienDESC @SearchTerm," +
                                                " @ColumnName, @RecordStart,@PageSize",
                        searchTerm, columnName, recordStart, pageSize).ToList();
            }

            int recordsTotal, recordsFiltered;
            if (doanVienResultSet.Count > 0)
            {
                recordsTotal = doanVienResultSet[0].TotalCount;
                recordsFiltered = doanVienResultSet[0].FilteredCount;
            }
            else
            {
                recordsTotal = _context.DanhSachDoanVien.Count();
                recordsFiltered = 0;
            }

            //Tính số trang dựa trên pageIndex và pageSize
            var pageNumbers = (int)Math.Ceiling((double)recordsTotal / pagingSearchingSorting.PageSize);
            return Ok(new
            {
                pagingSearchingSorting.Draw,
                data = doanVienResultSet,
                pageNumbers,
                pagingSearchingSorting.PageIndex,
                recordsFiltered,
                recordsTotal
            });
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLyDoanVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DangKiDoanVien")]
        public IHttpActionResult DangKiDoanVien(DangKiHoiVienDoanVienDto doanVienDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (doanVienDto.NoiVao == null) return BadRequest(); //Vì dùng chung Dto với hội viên nên Prop NoiVao ko có required nên check ở đây
            var sinhVien = _context.SinhVien.Include(sv => sv.DoanVien).SingleOrDefault(sv => sv.MSSV == doanVienDto.MSSV);
            if (sinhVien == null) return NotFound();
            sinhVien.DangKiDoanVien(doanVienDto);
            _context.SaveChanges();
            return Ok();
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLyDoanVien")]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DangKiDoanVienHangLoat")]
        public IHttpActionResult DangKiDoanVienHangLoat()
        {
            string mssv, noiVaoDoan;
            DateTime ngayVaoDoan;
            IList<DangKiHoiVienDoanVienDto> danhSachDangKiDoanVien = new List<DangKiHoiVienDoanVienDto>();
            IList<string> danhSachMSSV = new List<string>();
            IList<LoiDangKiHoiVienDoanVien> danhSachLoiDangKiDoanVien = new List<LoiDangKiHoiVienDoanVien>();
            IList<SinhVienDaDangKiHoiVienDoanVien> danhSachSinhVienDaDangKi = new List<SinhVienDaDangKiHoiVienDoanVien>();
            //Đọc file excel
            var uploadFile = HttpContext.Current.Request.Files["uploadFile"];
            if (uploadFile == null || uploadFile.ContentLength <= 0) return BadRequest("Hãy chọn một file excel có nội dung.");
            var filePath = uploadFile.FileName;
            var fileExtension = Path.GetExtension(filePath);
            if (fileExtension != ".xlsx" && fileExtension != ".xls") return BadRequest("Chỉ được sử dụng file excel: Tệp .xlsx hoặc .xls");

            using (var package = new ExcelPackage(uploadFile.InputStream))
            {
                //Lấy sheet và số dòng
                var workSheet = package.Workbook.Worksheets["Sheet1"];
                var noOfRow = workSheet.Dimension.End.Row;

                for (var rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    //Gán biến mssv
                    try
                    {
                        mssv = workSheet.Cells[rowIterator, 1].Value.ToString();
                    }
                    catch (NullReferenceException)
                    {
                        danhSachLoiDangKiDoanVien.Add(new LoiDangKiHoiVienDoanVien()
                        {
                            Loi = "Không được để trống mã số sinh viên",
                            MSSV = "Còn thiếu",
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }
                    //Gán biến ngày vào đoàn
                    try
                    {
                        ngayVaoDoan = (DateTime)workSheet.Cells[rowIterator, 2].Value;
                    }
                    catch (Exception e)
                    {
                        if (e is InvalidCastException)
                        {
                            danhSachLoiDangKiDoanVien.Add(new LoiDangKiHoiVienDoanVien()
                            {
                                Loi = "Định dạng ngày vào đoàn bị sai.",
                                MSSV = mssv,
                                SoDongBiLoi = rowIterator
                            });
                        }
                        if (e is NullReferenceException)
                        {
                            danhSachLoiDangKiDoanVien.Add(new LoiDangKiHoiVienDoanVien()
                            {
                                Loi = "Không được để trống ngày vào đoàn.",
                                MSSV = mssv,
                                SoDongBiLoi = rowIterator
                            });
                        }
                        continue;
                    }
                    //Gán biến nơi vào đoàn
                    try
                    {
                        noiVaoDoan = workSheet.Cells[rowIterator, 3].Value.ToString();
                    }
                    catch (NullReferenceException)
                    {
                        danhSachLoiDangKiDoanVien.Add(new LoiDangKiHoiVienDoanVien()
                        {
                            Loi = "Không được để trống nơi vào đoàn",
                            MSSV = mssv,
                            NgayVao = ngayVaoDoan,
                            NoiVao = "Còn thiếu",
                            SoDongBiLoi = rowIterator
                        });
                        continue;
                    }
                    //Nếu qua 3 vòng kiểm tra trên được thì thêm vào danh sách
                    danhSachDangKiDoanVien.Add(new DangKiHoiVienDoanVienDto()
                    {
                        MSSV = mssv,
                        NgayVao = ngayVaoDoan,
                        NoiVao = noiVaoDoan,
                        SoDong = rowIterator
                    });
                    danhSachMSSV.Add(mssv);
                }
                //Lấy danh sách sinh viên dựa trên danh sách mssv ở trên
                var danhSachSinhVien = _context.SinhVien.Include(sv => sv.DoanVien).Where(sv => danhSachMSSV.Contains(sv.MSSV)).ToList();

                //Lặp qua danh sách dangKiHoiVienDto
                foreach (var dangKiDoanVien in danhSachDangKiDoanVien)
                {
                    var sinhVien = danhSachSinhVien.SingleOrDefault(sv => sv.MSSV == dangKiDoanVien.MSSV);
                    //Kiểm tra sinh viên có tồn tại không
                    if (sinhVien == null)
                    {
                        danhSachLoiDangKiDoanVien.Add(new LoiDangKiHoiVienDoanVien()
                        {
                            Loi = "Không tồn tại sinh viên với MSSV này.",
                            MSSV = dangKiDoanVien.MSSV,
                            NgayVao = dangKiDoanVien.NgayVao,
                            SoDongBiLoi = dangKiDoanVien.SoDong
                        });
                        continue;
                    }
                    //Thêm Đoàn viên
                    sinhVien.DangKiDoanVien(dangKiDoanVien);
                    danhSachSinhVienDaDangKi.Add(new SinhVienDaDangKiHoiVienDoanVien()
                    {
                        HoVaTenLot = sinhVien.HoVaTenLot,
                        Ten = sinhVien.Ten,
                        MSSV = sinhVien.MSSV,
                        NgayVao = dangKiDoanVien.NgayVao,
                        NoiVao = dangKiDoanVien.NoiVao
                    });
                }
                _context.SaveChanges();
            }
            return Ok(new
            {
                danhSachLoiDangKiDoanVien,
                soLoiDangKi = danhSachLoiDangKiDoanVien.Count,
                danhSachSinhVienDaDangKi,
                soSinhVienDaDangKi = danhSachSinhVienDaDangKi.Count
            });
        }

        [System.Web.Http.Authorize(Roles = "Admin,QuanLyDoanVien")]
        [System.Web.Http.HttpDelete]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/XoaDangKiDoanVien/{mssv}")]
        public IHttpActionResult XoaDangKiDoanVien(string mssv)
        {
            var sinhVien = _context.SinhVien.Include(sv => sv.DoanVien).SingleOrDefault(sv => sv.MSSV == mssv);
            if (sinhVien == null) return NotFound();
            sinhVien.XoaDangKiDoanVien();
            _context.SaveChanges();
            return Ok();
        }

        /*Các API cho sinh viên dùng (không nhất thiết là quản lý)*/
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SinhVien/TimKiem")]
        public IHttpActionResult TimKiemSinhVien(string searchTerm, int pageIndex = 1)
        {
            searchTerm = searchTerm.Trim();
            if (searchTerm.Length <= 2) return Ok();
            var param1 = new SqlParameter("@SearchTerm", searchTerm);
            var param2 = new SqlParameter("@CurrentPage", pageIndex);
            var result = _context.Database.SqlQuery<TTSinhVienCBNhatDto>("SearchSinhVien @SearchTerm, @CurrentPage", param1, param2).ToList();

            return Ok(result);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/ChiTiet/{sinhVienId}")]
        public IHttpActionResult XemChiTietSinhVien(int sinhVienId)
        {
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != sinhVienId) return Unauthorized();
            //Lấy thông tin sinh viên
            var sinhVien = ChiTietSinhVien(sinhVienId);
            if (sinhVien == null) return NotFound();
            return Ok(Mapper.Map<SinhVien, ChiTietDayDuSinhVienDto>(sinhVien));
        }   
        
        [System.Web.Http.HttpGet]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/ThongTinCoBan/{sinhVienId}")]
        public IHttpActionResult XemChiTietCoBanSinhVien(int sinhVienId)
        {
            var sinhVien = _context.SinhVien.Where(sv => sv.Id == sinhVienId)
                .Select(sv => new ThongTinCoBanSinhVien()
                {
                    Id = sv.Id,
                    AnhDaiDien = sv.AnhDaiDien,
                    DanToc = sv.DanToc.TenDanToc,
                    GioiTinh = sv.GioiTinh.TenGioiTinh,
                    TonGiao = sv.TonGiao.TenTonGiao,
                    TenLop = sv.LopDangHoc.KyHieuTenLop,
                    HoVaTenLot = sv.HoVaTenLot,
                    GioiThieu = sv.GioiThieu,
                    Ten = sv.Ten,
                    NgaySinh = sv.NgaySinh,
                    MSSV = sv.MSSV,
                    KhoaHoc = new KhoaHocDto{TenKhoa = sv.KhoaHoc.TenKhoa},
                    DoanVien = sv.DoanVien != null ? new HoiVienDoanVienDto():null,
                    HoiVien = sv.HoiVien != null ? new HoiVienDoanVienDto() : null,
                    DangVien = sv.DangVien != null ? new HoiVienDoanVienDto() : null
                })
                .SingleOrDefault();
            if (sinhVien == null) return NotFound();
            return Ok(sinhVien);
        }

        [System.Web.Http.HttpGet]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/GetDataForSave/{sinhVienId}")]
        public IHttpActionResult GetSinhDataForSave(int sinhVienId)
        {
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != sinhVienId) return Unauthorized();
            //Chức năng dùng để lấy thông tin cơ bản của sinh viên để chỉnh sửa (trang quản lý sinh viên)
            var sinhVien = _context.SinhVien
                .Select(sv => new
                {
                    sv.Id,
                    sv.HoVaTenLot,
                    sv.Ten,
                    sv.MSSV,
                    sv.DanTocId,
                    sv.TonGiaoId,
                    sv.GioiTinhId,
                    sv.KhoaHocId,
                    sv.AnhDaiDien,
                    sv.NgaySinh,
                    sv.GioiThieu
                })                   
                .SingleOrDefault(sv => sv.Id == sinhVienId);
            if (sinhVien == null) return NotFound();
            return Ok(sinhVien);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SinhVien/LayDuLieuChoFormSaveSinhVien")]
        public IHttpActionResult LayDuLieuChoFormSaveSinhVien()
        {
            var danhSachTonGiao = _context.TonGiao.Select(tg => new
            {
                tg.Id,
                text = tg.TenTonGiao
            }).ToList();
            var danhSachDanToc = _context.DanToc.Select(dt => new
            {
                dt.Id,
                text = dt.TenDanToc
            }).ToList();
            var danhSachGioiTinh = _context.GioiTinh.Select(gt => new
            {
                gt.Id,
                text = gt.TenGioiTinh
            }).ToList();
            var danhSachKhoa = _context.KhoaHoc.Select(khoa => new
            {
                khoa.Id,
                text = khoa.TenKhoa + "("+khoa.NamBatDau.Year + "-" +khoa.NamKetThuc.Year+ ")"
            }).ToList();
            return Ok(new
            {
                danhSachKhoa,
                danhSachDanToc,
                danhSachGioiTinh,
                danhSachTonGiao
            });
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/LopCuaToi")]
        public IHttpActionResult LayLopCuaSinhVien()
        {
            //Lấy Sinh viên Id của User
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (userSinhVienId == 0) return BadRequest();
            //Lấy danh sách lớp của sinh viên
            var danhSachLop = _context.DanhSachSinhVienLop
                                      .Where(svl => svl.SinhVienId == userSinhVienId)
                                      .Select(svl => svl.Lop)
                                      .Include(l => l.KhoaHoc)
                                      .ToList();
            return Ok(danhSachLop.Select(Mapper.Map<Lop,DanhSachLopDto>));
        }        
        
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DonViCuaToi")]
        public IHttpActionResult LayDonViCuaSinhVien()
        {
            //Lấy Sinh viên Id của User
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (userSinhVienId == 0) return BadRequest();
            //Lấy danh sách lớp của sinh viên
            var danhSachDonVi = _context.DanhSachThanhVienDonVi
                                      .Where(svl => svl.SinhVienId == userSinhVienId)
                                      .Select(dv => new
                                             {
                                               dv.DonViId,
                                               dv.DuocPheDuyet,
                                               dv.NgungThamGia,
                                               dv.DonVi.AnhBia,
                                               dv.DonVi.TenDonVi
                                             })
                                      .ToList();
            return Ok(danhSachDonVi);
        }                
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SinhVien/DonViThamGia/{sinhVienId}")]
        public IHttpActionResult LayDonViCuaSinhVien(int sinhVienId)
        {
            //Lấy danh sách lớp của sinh viên
            var danhSachDonVi = _context.DanhSachThanhVienDonVi
                                      .Where(tvdv => tvdv.SinhVienId == sinhVienId && tvdv.DuocPheDuyet)
                                      .Select(tvdv => new
                                             {
                                               tvdv.DonViId,
                                               tvdv.DuocPheDuyet,
                                               tvdv.NgungThamGia,
                                               tvdv.DonVi.AnhBia,
                                               tvdv.DonVi.TenDonVi
                                             })
                                      .ToList();
            return Ok(danhSachDonVi);
        }

        //Các api cho địa chỉ
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SinhVien/LayDanhSachTinh")]
        public IHttpActionResult LayDanhSachTinh()
        {
            var danhSachTinh = _context.CapTinh.Select(tinh => new
            {
                tinh.Id,
                Text = tinh.TenTinh
            }).OrderBy(a => a.Id).ToList();
            return Ok(danhSachTinh);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SinhVien/LayHuyenCuaTinh/{idTinh}")]
        public IHttpActionResult LayHuyenCuaTinh(int idTinh)
        {
            var danhSachHuyen = _context.CapHuyen.Where(h => h.CapTinhId == idTinh).Select(h => new
            {
                h.Id,
                Text = h.TenHuyen
            }).OrderBy(a => a.Id).ToList();
            return Ok(danhSachHuyen);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SinhVien/LayXaCuaHuyen/{idHuyen}")]
        public IHttpActionResult LayXaCuaHuyen(int idHuyen)
        {
            var danhSachHuyen = _context.CapXa.Where(h => h.CapHuyenId == idHuyen).Select(h => new
            {
                h.Id,
                Text = h.TenXa
            }).OrderBy(a => a.Id).ToList();
            return Ok(danhSachHuyen);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/SaveDiaChi")]
        public IHttpActionResult SaveDiaChi(DiaChiDtoForSave diaChiDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != diaChiDto.SinhVienId) return Unauthorized();
            //Add địa chỉ
            _context.DiaChi.Add(Mapper.Map<DiaChiDtoForSave,DiaChi>(diaChiDto));
            _context.SaveChanges();
            //Lấy địa chỉ mới vừa tạo để gửi về datatable
            var diaChiSinhVien = _context.DiaChi.Where(dc => dc.SinhVienId == diaChiDto.SinhVienId).Select(dc => new DiaChiDto
            {
                CapTinh = dc.CapTinh.TenTinh,
                CapHuyen = dc.CapHuyen.TenHuyen,
                CapXa = dc.CapXa.TenXa,
                SoNhaTenDuong = dc.SoNhaTenDuong,
                LoaiDiaChi = dc.LoaiDiaChi.ToString()
            });
            return Ok(diaChiSinhVien);
        }
        
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpDelete]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DeleteDiaChi/{diaChiId}")]
        public IHttpActionResult DeleteDiaChi(int diaChiId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var diaChi = _context.DiaChi.SingleOrDefault(dc => dc.Id == diaChiId);
            if (diaChi == null) return NotFound();
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != diaChi.SinhVienId) return Unauthorized();
            //Xóa địa chỉ
            _context.DiaChi.Remove(diaChi);
            _context.SaveChanges();
            return Ok();
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/SaveSoDienThoai")]                                          
        public IHttpActionResult SaveSoDienThoai(SDTDto sdtDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != sdtDto.SinhVienId) return Unauthorized();
            //Thêm số điện thoại
            _context.SDT.Add(Mapper.Map<SDTDto, SDT>(sdtDto));
            _context.SaveChanges();
            var sdtSinhVien = _context.SDT.Where(sdt => sdt.SinhVienId == sdtDto.SinhVienId).ToList();
            return Ok(sdtSinhVien.Select(Mapper.Map<SDT, SDTDto>));
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpDelete]
        [ApiValidateAntiForgeryToken]
        [System.Web.Http.Route("api/SinhVien/DeleteSoDienThoai/{sdtId}")]
        public IHttpActionResult DeleteSoDienThoai(int sdtId)
        {
            if (!ModelState.IsValid) return BadRequest();
            var sdt = _context.SDT.SingleOrDefault(s => s.Id == sdtId);
            if (sdt == null) return NotFound();
            //Kiểm tra xem người dùng có quyền chỉnh sửa không
            var userRole = User.IsInRole("QuanLySinhVien") || User.IsInRole("Admin");
            var userSinhVienId = User.Identity.GetSinhVienId();
            if (!userRole && userSinhVienId != sdt.SinhVienId) return Unauthorized();
            //Xóa sđt
            _context.SDT.Remove(sdt);
            _context.SaveChanges();
            return Ok();
        }

        private SinhVien ChiTietCoBanSinhVien(int sinhVienID)
        {
            return _context.SinhVien
                .Include(sv => sv.LopDangHoc)
                .Include(sv => sv.GioiTinh)
                .Include(sv => sv.TonGiao)
                .Include(sv => sv.DanToc)
                .Include(sv => sv.KhoaHoc)
                .SingleOrDefault(sv => sv.Id == sinhVienID);
        }            
        private SinhVien ChiTietSinhVien(int sinhVienID)
        {
            return _context.SinhVien
                .Include(sv => sv.LopDangHoc)
                .Include(sv => sv.GioiTinh)
                .Include(sv => sv.TonGiao)
                .Include(sv => sv.DanToc)
                .Include(sv => sv.DiaChi.Select(dc => dc.CapTinh))
                .Include(sv => sv.DiaChi.Select(dc => dc.CapHuyen))
                .Include(sv => sv.DiaChi.Select(dc => dc.CapXa))
                .Include(sv => sv.SDT)
                .Include(sv => sv.KhoaHoc)
                .Include(sv => sv.DoanVien)
                .Include(sv => sv.HoiVien)
                .SingleOrDefault(sv => sv.Id == sinhVienID);
        }
        private List<SinhVien> LayDanhSachSinhVien()
        {
            return _context.SinhVien
                .Include(sv => sv.LopDangHoc.DanhSachSinhVien)
                .Include(sv => sv.GioiTinh)
                .Include(sv => sv.DanToc)
                .Include(sv => sv.TonGiao)
                .Include(sv => sv.KhoaHoc)
                .ToList();
        }
    }

    public class SinhVienDaDangKiHoiVienDoanVien  
    {
        public string HoVaTenLot { get; set; }
        public string Ten { get; set; }
        public string MSSV { get; set; }
        public DateTime NgayVao { get; set; }
        public string NoiVao { get; set; }
    }

    public class DangKiHoiVienDoanVienDto
    {
        public int SoDong { get; set; } //Lưu lại số dòng trong file excel, để tí nữa có lỗi thì biết để báo
        [Required]
        public string MSSV { get; set; }
        [Required]
        public DateTime NgayVao { get; set; }
        public string NoiVao { get; set; }
    }

    public class LoiDangKiHoiVienDoanVien  
    {
        public string Loi { get; set; }
        public int SoDongBiLoi { get; set; }
        public string MSSV { get; set; }
        public DateTime NgayVao { get; set; }
        public string NoiVao { get; set; }
    }

    public class SinhVienTaoBiLoi
    {
        public string Loi { get; set; }

        public int SoDongBiLoi { get; set; }
        public SinhVienDtoForTable SinhVien { get; set; }
    }
}
