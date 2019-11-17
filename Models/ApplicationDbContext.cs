using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NAPASTUDENT.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<SinhVien> SinhVien { get; set; }
        public virtual DbSet<Lop> Lop { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHoc { get; set; }
        public virtual DbSet<SDT> SDT { get; set; }

        public virtual DbSet<HoiVienHoiSinhVien> DanhSachHoiVienHoiSinhVien{ get; set; }
        public virtual DbSet<DoanVien> DanhSachDoanVien{ get; set; }
        public virtual DbSet<DangVien> DanhSachDangVien{ get; set; }
        public virtual DbSet<DiaChi> DiaChi { get; set; }
        public virtual DbSet<CapTinh> CapTinh { get; set; }
        public virtual DbSet<CapHuyen> CapHuyen { get; set; }
        public virtual DbSet<CapXa> CapXa { get; set; }
        public virtual DbSet<LopMonHoc> LopMonHoc { get; set; }
        public virtual DbSet<MonHoc> MonHoc{ get; set; }
        public virtual DbSet<LichHoc> LichHoc{ get; set; }
        public virtual DbSet<Diem> Diem { get; set; }
        public virtual DbSet<DiemBoSung> DiemBoSung { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<ApplicationRoleGroup> ApplicationRoleGroups { get; set; }
        public virtual DbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }
        public virtual DbSet<IdentityUserRole> UserRoles { get; set; }

        public virtual DbSet<ChucVuLop> ChucVuLop { get; set; }
        public virtual DbSet<GroupCVLop> GroupCvLop { get; set; }
        public virtual DbSet<DonVi> DanhSachDonVi { get; set; }
        public virtual DbSet<ThanhVienDonVi> DanhSachThanhVienDonVi { get; set; }
        public virtual DbSet<ChucVuDonVi> DanhSachChucVuDonVi { get; set; }
        public virtual DbSet<HoatDong> DanhSachHoatDong { get; set; }
        public virtual DbSet<HoatDongDonVi> DanhSachHoatDongDonVi { get; set; }
        public virtual DbSet<HoatDongLop> DanhSachHoatDongLop { get; set; }
        public virtual DbSet<TonGiao> TonGiao { get; set; }
        public virtual DbSet<DanToc> DanToc { get; set; }
        public virtual DbSet<GioiTinh> GioiTinh { get; set; }
        public virtual DbSet<ThamGiaHoatDong> DanhSachThamGiaHoatDong { get; set; }      
        public virtual DbSet<TheoDoiHoatDong> DanhSachTheoDoiHoatDong { get; set; }      
        public virtual DbSet<BaiViet> DanhSachBaiViet { get; set; }  
        public virtual DbSet<BaiVietHoatDong> DanhSachBaiVietHoatDong { get; set; }  
        public virtual DbSet<BaiVietLop> DanhSachBaiVietLop { get; set; }  
        public virtual DbSet<BaiVietDonVi> DanhSachBaiVietDonVi { get; set; }
        public virtual DbSet<ChuyenMucBaiViet> DanhSachChuyenMucBaiViet { get; set; }
        public virtual DbSet<ChuongTrinhHoatDong> DanhSachChuongTrinhHoatDong { get; set; }
        public virtual DbSet<SinhVienLop> DanhSachSinhVienLop { get; set; }
        public virtual DbSet<LichThiLai> LichThiLai { get; set; }
        public virtual DbSet<DiemTrungBinhHocKi> DiemTrungBinhHocKi { get; set; }
        public virtual DbSet<ThongBaoHoatDong> DanhSachThongBaoHoatDong { get; set; }
        public virtual DbSet<ThongBaoHoatDongSinhVien> DanhSachThongBaoHoatDongSinhVien { get; set; }

 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SinhVien>().ToTable("DanhSachSinhVien");
            modelBuilder.Entity<SinhVien>().Property(sv => sv.HoVaTenLot).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<SinhVien>().Property(sv => sv.Ten).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<SinhVien>().Property(sv => sv.MSSV).IsRequired().HasMaxLength(8);
            modelBuilder.Entity<SinhVien>().HasOptional(sv => sv.LopDangHoc).WithMany()
                .HasForeignKey(sv => sv.LopDangHocId).WillCascadeOnDelete(false);
            modelBuilder.Entity<SinhVien>().HasMany(sv => sv.SDT).WithRequired(sdt => sdt.SinhVien)
                .HasForeignKey(sdt => sdt.SinhVienId).WillCascadeOnDelete(true);
            modelBuilder.Entity<SinhVien>().HasMany(sv => sv.DiaChi).WithRequired(dc =>  dc.SinhVien)
                .HasForeignKey(dc => dc.SinhVienId).WillCascadeOnDelete(true);
            modelBuilder.Entity<SinhVien>().HasMany(sv => sv.Diem).WithRequired(diem =>  diem.SinhVien);
            modelBuilder.Entity<SinhVien>().HasRequired(sv => sv.KhoaHoc).WithMany(kh => kh.DanhSachSinhVien)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<SinhVien>().HasMany(sv => sv.DanhSachHoatDongThamGia)
                .WithRequired(hd => hd.SinhVien);
            modelBuilder.Entity<SinhVien>().HasMany(sv => sv.DanhSachHoatDongTheoDoi)
                .WithRequired(tdhd => tdhd.SinhVien);


            //Danh sách lớp và các mối quan hệ của nó
            modelBuilder.Entity<Lop>().ToTable("DanhSachLop");
            modelBuilder.Entity<Lop>().Property(lop => lop.KyHieuTenLop).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Lop>().Property(lop => lop.TenLop).IsRequired();
            modelBuilder.Entity<Lop>().HasRequired(lop => lop.KhoaHoc).WithMany(kh => kh.DanhSachLop)
                .HasForeignKey(lop => lop.KhoaHocId);
            modelBuilder.Entity<Lop>().HasMany(lop => lop.DanhSachMonHoc).WithRequired(lmh => lmh.Lop)
                .HasForeignKey(lmh => lmh.LopId);

            modelBuilder.Entity<SinhVienLop>().ToTable("DanhSachSinhVienLop");
            modelBuilder.Entity<SinhVienLop>().HasKey(svl => new {svl.SinhVienId, svl.LopId});
            modelBuilder.Entity<SinhVienLop>().HasRequired(svl => svl.SinhVien).WithMany(sv => sv.DanhSachLop)
                .HasForeignKey(svl => svl.SinhVienId);
            modelBuilder.Entity<SinhVienLop>().HasRequired(svl => svl.Lop).WithMany(lop => lop.DanhSachSinhVien)
                .HasForeignKey(svl => svl.LopId);


            //Danh sách sinh viên có chức vụ lớp
            modelBuilder.Entity<ChucVuLop>().ToTable("DanhSachChucVuLop");
            modelBuilder.Entity<ChucVuLop>().HasKey(lopsv => new { lopsv.ChucVuId, lopsv.LopId,lopsv.SinhVienId });
            modelBuilder.Entity<ChucVuLop>().HasRequired(cvl => cvl.ChucVu).WithMany().HasForeignKey(cvl => cvl.ChucVuId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ChucVuLop>().HasRequired(cvl => cvl.SinhVien).WithMany(sv => sv.ChucVuLop).HasForeignKey(cvl => cvl.SinhVienId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ChucVuLop>().HasRequired(cvl => cvl.Lop).WithMany(lop => lop.ChucVuLop).HasForeignKey(cvl => cvl.LopId).WillCascadeOnDelete(false);

            //Model Group (Quyền nhóm)
            modelBuilder.Entity<Group>().ToTable("DanhSachGroup");
            modelBuilder.Entity<Group>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Group>().HasMany(g => g.Roles);

            // Danh sách Chức vụ lớp và quyền (group) của chức vụ đó đó
            modelBuilder.Entity<GroupCVLop>().ToTable("DanhSachGroupCVLop");
            modelBuilder.Entity<GroupCVLop>().HasRequired(grCvLop => grCvLop.ChucVu).WithOptional().WillCascadeOnDelete(false);
            modelBuilder.Entity<GroupCVLop>().HasRequired(grCvLop => grCvLop.Group).WithOptional().WillCascadeOnDelete(false);
            modelBuilder.Entity<GroupCVLop>().HasKey(groupCv => groupCv.ChucVuId);

            //Danh sách các chức vụ trong Học viện
            modelBuilder.Entity<ChucVu>().ToTable("DanhSachChucVu");
            modelBuilder.Entity<ChucVu>().Property(cv => cv.TenChucVu).IsRequired();


            modelBuilder.Entity<DonVi>().ToTable("DanhSachDonVi");
            modelBuilder.Entity<DonVi>().Property(dv =>dv.TenDonVi).IsRequired();

            modelBuilder.Entity<ThanhVienDonVi>().ToTable("DanhSachThanhVienDonVi");
            modelBuilder.Entity<ThanhVienDonVi>().HasKey(tvdv => new {tvdv.DonViId,tvdv.SinhVienId});
            modelBuilder.Entity<ThanhVienDonVi>().HasRequired(tvdv => tvdv.SinhVien)
                .WithMany(sv => sv.DanhSachDonViThamGia).HasForeignKey(tvdv => tvdv.SinhVienId);
            modelBuilder.Entity<ThanhVienDonVi>().HasRequired(tvdv => tvdv.DonVi)
                .WithMany(tvdv => tvdv.DanhSachThanhVienDonVi).HasForeignKey(tvdv => tvdv.DonViId);

            //Danh sách sinh viên có chức vụ đơn vị
            modelBuilder.Entity<ChucVuDonVi>().ToTable("DanhSachChucVuDonVi");
            modelBuilder.Entity<ChucVuDonVi>().HasKey(cvdv => new { cvdv.ChucVuId, cvdv.DonViId, cvdv.SinhVienId });
            modelBuilder.Entity<ChucVuDonVi>().HasRequired(cvdv => cvdv.ChucVu)
                                              .WithMany()
                                              .HasForeignKey(cvdv => cvdv.ChucVuId)
                                              .WillCascadeOnDelete(false);
            modelBuilder.Entity<ChucVuDonVi>().HasRequired(cvl => cvl.SinhVien)
                                              .WithMany(cvdv => cvdv.ChucVuDonVi)
                                              .HasForeignKey(cvdv => cvdv.SinhVienId)
                                              .WillCascadeOnDelete(false);
            modelBuilder.Entity<ChucVuDonVi>().HasRequired(cvdv => cvdv.DonVi)
                                              .WithMany(dv => dv.ChucVuDonVi)
                                              .HasForeignKey(cvdv => cvdv.DonViId)
                                              .WillCascadeOnDelete(false);
            modelBuilder.Entity<ChucVuDonVi>().HasRequired(cv => cv.ThanhVienDonVi)
                                              .WithMany(tv => tv.DanhSachChucVuDonVi)
                                              .HasForeignKey(cv => new {cv.DonViId, cv.SinhVienId})
                                              .WillCascadeOnDelete();



            modelBuilder.Entity<ApplicationRoleGroup>().HasKey(gr =>
                new {gr.RoleId, gr.GroupId }).ToTable("ApplicationRoleGroups");

            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Groups);   

            modelBuilder.Entity<ApplicationUserGroup>().HasKey(r =>
                new {r.UserId, r.GroupId }).ToTable("ApplicationUserGroups");

            //Danh Sach Diem
            modelBuilder.Entity<Diem>().ToTable("DanhSachDiem");
            modelBuilder.Entity<Diem>().HasKey(diem => new {diem.SinhVienId, diem.MonHocId});
            modelBuilder.Entity<Diem>().HasRequired(d => d.LopMonHoc).WithMany(lmh => lmh.DanhSachDiem)
                                       .WillCascadeOnDelete(false);
            //Danh Sach Diem
            modelBuilder.Entity<DiemTrungBinhHocKi>().ToTable("DanhSachDiemTrungBinhHocKi");
            modelBuilder.Entity<DiemTrungBinhHocKi>().HasKey(dtb => new { dtb.SinhVienId, dtb.HocKi });
            modelBuilder.Entity<DiemTrungBinhHocKi>().HasRequired(dtb => dtb.SinhVien)
                .WithMany(sv => sv.DiemTrungBinhHocKi);
                                    


            modelBuilder.Entity<DiemBoSung>().ToTable("DanhSachDiemBoSung");
            modelBuilder.Entity<Diem>().HasMany(diem => diem.DanhSachDiemBoSung).WithRequired(dbs => dbs.DiemGoc)
                .HasForeignKey(dbs => new {dbs.SinhVienId,dbs.MonHocId });

            modelBuilder.Entity<LopMonHoc>().ToTable("DanhSachLopMonHoc");
            modelBuilder.Entity<LopMonHoc>().Property(lmh => lmh.DiaDiemThi).IsRequired();
            modelBuilder.Entity<LopMonHoc>().HasKey(lmh => new {lmh.LopId,lmh.MonHocId});


            modelBuilder.Entity<LichThiLai>().ToTable("DanhSachLichThiLai");
            modelBuilder.Entity<LichThiLai>().Property(ltl => ltl.DiaDiemThi).IsRequired();


                //Danh sách môn học
            modelBuilder.Entity<MonHoc>().ToTable("DanhSachMonHoc");
            modelBuilder.Entity<MonHoc>().Property(mh => mh.TenMonHoc).IsRequired();
            modelBuilder.Entity<MonHoc>().Property(mh => mh.KyHieuMonHoc).HasMaxLength(10);
            modelBuilder.Entity<MonHoc>().HasMany(mh => mh.DanhSachLopHocMonHoc).WithRequired(lmh => lmh.MonHoc)
                .HasForeignKey(lmh => lmh.MonHocId);
                //Danh sách lịch môn học
            modelBuilder.Entity<LichHoc>().ToTable("DanhSachLichHoc");
            modelBuilder.Entity<LichHoc>().Property(lh => lh.PhongHoc).IsRequired();
            modelBuilder.Entity<LichHoc>().Property(lh => lh.GiaoVienDay).IsRequired();
            modelBuilder.Entity<LichHoc>().HasRequired(lh => lh.LopMonHoc).WithMany(lmh => lmh.DanhSachLichHoc)
                .HasForeignKey(lh => new {lh.LopId, lh.MonHocId});

            /*Hoạt động và enity liên quan*/
                //Enity hoạt động
                    modelBuilder.Entity<HoatDong>().ToTable("DanhSachHoatDong");
                    modelBuilder.Entity<HoatDong>().Property(hd => hd.TenHoatDong).IsRequired();
                    modelBuilder.Entity<HoatDong>().Property(hd => hd.DiaDiem).IsRequired();
                    modelBuilder.Entity<HoatDong>().HasRequired(hd => hd.SinhVienTaoHd)
                                                   .WithMany().HasForeignKey(hd => hd.IdSinhVienTaoHd)
                                                   .WillCascadeOnDelete(false);
                //Hoạt động đơn vị tổ chức
                    modelBuilder.Entity<HoatDongDonVi>().ToTable("DanhSachHoatDongDonVi");                     
                    modelBuilder.Entity<HoatDongDonVi>().HasKey(hddv => new {hddv.HoatDongId,hddv.DonViId});                     
                    modelBuilder.Entity<HoatDong>().HasMany(hd => hd.DanhSachDonViToChuc)
                        .WithRequired(hddv => hddv.HoatDong)
                        .HasForeignKey(hddv => hddv.HoatDongId);                    
                    modelBuilder.Entity<DonVi>().HasMany(hd => hd.DanhSachHoatDongToChuc)
                        .WithRequired(hddv => hddv.DonVi)
                        .HasForeignKey(hddv => hddv.DonViId);
                //Hoạt động lớp tổ chức
                    modelBuilder.Entity<HoatDongLop>().ToTable("DanhSachHoatDongLop");                     
                    modelBuilder.Entity<HoatDongLop>().HasKey(hdl => new {hdl.HoatDongId,hdl.LopId});                     
                    modelBuilder.Entity<HoatDong>().HasMany(hd => hd.DanhSachLopToChuc)
                        .WithRequired(hdl => hdl.HoatDong)
                        .HasForeignKey(hddv => hddv.HoatDongId);
                     modelBuilder.Entity<Lop>().HasMany(hd => hd.DanhSachHoatDongToChuc)
                        .WithRequired(hdl => hdl.Lop)
                        .HasForeignKey(hdl => hdl.LopId);
                //Danh sách tham gia hoạt động
                    modelBuilder.Entity<HoatDong>().HasMany(hd => hd.DanhSachSinhVienThamGia)
                        .WithRequired(dstghd => dstghd.HoatDong );
                    modelBuilder.Entity<ThamGiaHoatDong>().ToTable("DanhSachThamGiaHoatDong");
                    modelBuilder.Entity<ThamGiaHoatDong>()
                                .HasKey(dstghd => new {dstghd.HoatDongId, dstghd.SinhVienId});
                    modelBuilder.Entity<ThamGiaHoatDong>().HasIndex(dstghd => dstghd.SinhVienId);
                    modelBuilder.Entity<ThamGiaHoatDong>().HasIndex(dstghd => dstghd.HoatDongId);
                    modelBuilder.Entity<ThamGiaHoatDong>().HasRequired(dstghd => dstghd.Lop).WithMany(lop => lop.DanhSachThamGiaHoatDong)
                        .HasForeignKey(dstghd => dstghd.LopId).WillCascadeOnDelete(false);
                //Danh sách theo dõi hoạt động
                    modelBuilder.Entity<HoatDong>().HasMany(hd => hd.DanhSachSinhVienTheoDoi)
                        .WithRequired(dstd => dstd.HoatDong);
                    modelBuilder.Entity<TheoDoiHoatDong>().ToTable("DanhSachTheoDoiHoatDong");
                    modelBuilder.Entity<TheoDoiHoatDong>()
                                .HasKey(dstd => new { dstd.HoatDongId, dstd.SinhVienId });
                    modelBuilder.Entity<TheoDoiHoatDong>().HasIndex(dstd => dstd.SinhVienId);
                    modelBuilder.Entity<TheoDoiHoatDong>().HasIndex(dstd => dstd.HoatDongId);
                //Chương trình hoạt động
                    modelBuilder.Entity<ChuongTrinhHoatDong>().ToTable("DanhSachChuongTrinhHoatDong");
                    modelBuilder.Entity<ChuongTrinhHoatDong>().Property(cthd => cthd.TieuDe).IsRequired().HasMaxLength(150);
                    modelBuilder.Entity<ChuongTrinhHoatDong>().Property(cthd => cthd.NoiDungChuongTrinh).IsRequired();
                    modelBuilder.Entity<ChuongTrinhHoatDong>().HasRequired(cthd => cthd.HoatDong)
                        .WithMany(hd => hd.DanhSachChuongTrinhHoatDong)
                        .HasForeignKey(cthd => cthd.HoatDongId);
                //Thông báo hoạt động
                    modelBuilder.Entity<ThongBaoHoatDong>().ToTable("DanhSachThongBaoHoatDong");
                    modelBuilder.Entity<ThongBaoHoatDong>().HasRequired(tb => tb.HoatDong)
                        .WithMany(hd => hd.DanhSachThongBaoHoatDong).HasForeignKey(tb => tb.HoatDongId);
                //Thông báo hoạt động cho sinh viên
                    modelBuilder.Entity<ThongBaoHoatDongSinhVien>().ToTable("DanhSachThongBaoHoatDongSinhVien");
                    modelBuilder.Entity<ThongBaoHoatDongSinhVien>()
                        .HasKey(tbsv => new {tbsv.ThongBaoHoatDongId, tbsv.SinhVienId});
                    modelBuilder.Entity<ThongBaoHoatDongSinhVien>().HasRequired(tbsv => tbsv.SinhVien)
                        .WithMany(sv => sv.DanhSachThongBaoHoatDong).HasForeignKey(tbsv => tbsv.SinhVienId).WillCascadeOnDelete(true);
                    modelBuilder.Entity<ThongBaoHoatDongSinhVien>().HasRequired(tbsv => tbsv.ThongBaoHoatDong)
                        .WithMany()
                        .HasForeignKey(tbsv => tbsv.ThongBaoHoatDongId);
            
            //Entity Bài viết và các entity liên quan
            modelBuilder.Entity<BaiViet>().ToTable("DanhSachBaiViet");
            modelBuilder.Entity<BaiViet>().Property(bv => bv.NgayTao).IsRequired();
            modelBuilder.Entity<BaiViet>().Property(bv => bv.NoiDungBaiViet).IsRequired();
            modelBuilder.Entity<BaiViet>().Property(bv => bv.NguoiTaoId).IsRequired();
            modelBuilder.Entity<BaiViet>().HasRequired(bv => bv.NguoiTao).WithMany(sv => sv.DanhSachBaiViet);
            modelBuilder.Entity<BaiViet>().Property(bv => bv.TenBaiViet).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<BaiViet>().Property(bv => bv.SoLuoc).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<BaiViet>().Property(bv => bv.AnhBia).IsRequired();
                //Bài viết hoạt động
            modelBuilder.Entity<BaiVietHoatDong>().ToTable("DanhSachBaiVietHoatDong");
            modelBuilder.Entity<BaiVietHoatDong>().HasKey(bvhd => new {bvhd.BaiVietId,bvhd.HoatDongId});
            modelBuilder.Entity<BaiVietHoatDong>().HasRequired(bvhd => bvhd.BaiViet)
                                                  .WithMany(bv => bv.BaiVietHoatDong);
            modelBuilder.Entity<BaiVietHoatDong>().HasRequired(hd => hd.HoatDong).WithMany(bvhd => bvhd.DanhSachBaiViet)
                                           .HasForeignKey(bvhd => bvhd.HoatDongId);
                //Bài viết lớp
            modelBuilder.Entity<BaiVietLop>().ToTable("DanhSachBaiVietLop");
            modelBuilder.Entity<BaiVietLop>().HasKey(bvl => new {bvl.BaiVietId,bvl.LopId});
            modelBuilder.Entity<BaiVietLop>().HasRequired(bvl =>bvl.BaiViet).WithMany(bv => bv.BaiVietLop);
            modelBuilder.Entity<BaiVietLop>().HasRequired(bvl =>bvl.Lop).WithMany(lop => lop.DanhSachBaiVietLop)
                                             .HasForeignKey(bvl => bvl.LopId);
                //Bài viết đơn vị
            modelBuilder.Entity<BaiVietDonVi>().ToTable("DanhSachBaiVietDonVi");
            modelBuilder.Entity<BaiVietDonVi>().HasKey(bvdv => new {bvdv.BaiVietId,bvdv.DonViId});
            modelBuilder.Entity<BaiVietDonVi>().HasRequired(bvdv => bvdv.BaiViet).WithMany(bv => bv.BaiVietDonVi);
            modelBuilder.Entity<BaiVietDonVi>().HasRequired(bvdv => bvdv.DonVi).WithMany(dv => dv.DanhSachBaiVietDonVi)
                                               .HasForeignKey(bvdv => bvdv.DonViId);
                //Chuyên mục bài viết
            modelBuilder.Entity<ChuyenMucBaiViet>().ToTable("DanhSachChuyenMucBaiViet");
            modelBuilder.Entity<ChuyenMucBaiViet>().HasKey(cm => cm.Id);
            modelBuilder.Entity<ChuyenMucBaiViet>().Property(cm => cm.TenChuyenMuc).IsRequired();
            modelBuilder.Entity<ChuyenMucBaiViet>().Property(cm => cm.MoTa).IsRequired();
            modelBuilder.Entity<ChuyenMucBaiViet>().HasOptional(cm => cm.ChuyenMucCha)
                .WithMany(cmc => cmc.DanhSachChuyenMucCon);
            modelBuilder.Entity<ChuyenMucBaiViet>().HasMany(cm => cm.DanhSachBaiViet)
                .WithRequired(bv => bv.ChuyenMucBaiViet).WillCascadeOnDelete(true);

            // Hội viên, đoàn viên, đảng viên
            modelBuilder.Entity<HoiVienHoiSinhVien>().ToTable("DanhSachHoiVienHoiSinhVien");
            modelBuilder.Entity<HoiVienHoiSinhVien>().HasKey(hv => hv.SinhVienId);
            modelBuilder.Entity<HoiVienHoiSinhVien>().HasRequired(hv => hv.SinhVien).WithOptional(sv => sv.HoiVien).WillCascadeOnDelete(true);

            modelBuilder.Entity<DoanVien>().ToTable("DanhSachDoanVien");
            modelBuilder.Entity<DoanVien>().HasKey(dv => dv.SinhVienId);
            modelBuilder.Entity<DoanVien>().Property(dv => dv.NoiVaoDoan).IsRequired();
            modelBuilder.Entity<DoanVien>().HasRequired(dv => dv.SinhVien).WithOptional(sv => sv.DoanVien).WillCascadeOnDelete(true);
            
            modelBuilder.Entity<DangVien>().ToTable("DangVien");
            modelBuilder.Entity<DangVien>().HasKey(dv => dv.SinhVienId);
            modelBuilder.Entity<DangVien>().Property(dv => dv.NoiVaoDang).IsRequired();
            modelBuilder.Entity<DangVien>().HasRequired(dv => dv.SinhVien).WithOptional(sv => sv.DangVien);
            
            //Enity chứa thông tin cá nhân
                //Danh sách Dân tộc
            modelBuilder.Entity<DanToc>().ToTable("DanhSachDanToc");
            modelBuilder.Entity<DanToc>().Property(dt => dt.TenDanToc).IsRequired().HasMaxLength(100);
                //Danh sách Tôn giáo    
            modelBuilder.Entity<TonGiao>().ToTable("DanhSachTonGiao");
            modelBuilder.Entity<TonGiao>().Property(tg => tg.TenTonGiao).IsRequired().HasMaxLength(100);
                //Danh sách Giới tính
            modelBuilder.Entity<GioiTinh>().ToTable("DanhSachGioiTinh");
            modelBuilder.Entity<GioiTinh>().Property(gt => gt.TenGioiTinh).IsRequired().HasMaxLength(100);
                //Danh sách Khóa học
            modelBuilder.Entity<KhoaHoc>().ToTable("DanhSachKhoaHoc");
            modelBuilder.Entity<KhoaHoc>().Property(kh => kh.TenKhoa).IsRequired();
                //Số điện thoại
            modelBuilder.Entity<SDT>().ToTable("DanhSachSDT");
            modelBuilder.Entity<SDT>().Property(sdt => sdt.SoDienThoai).HasMaxLength(11);
                //Địa chỉ
            modelBuilder.Entity<DiaChi>().ToTable("DanhSachDiaChi");
            modelBuilder.Entity<DiaChi>().Property(dc => dc.SoNhaTenDuong).IsRequired();
                //Danh sách các tỉnh
            modelBuilder.Entity<CapTinh>().ToTable("DanhSachTinh");
            modelBuilder.Entity<CapTinh>().Property(h => h.TenTinh).IsRequired().HasMaxLength(100);
                //Danh sách các huyện
            modelBuilder.Entity<CapHuyen>().ToTable("DanhSachHuyen");
            modelBuilder.Entity<CapHuyen>().Property(h => h.TenHuyen).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CapHuyen>().HasRequired(huyen => huyen.CapTinh)
                                           .WithMany(tinh => tinh.DanhSachHuyen)
                                           .HasForeignKey(huyen => huyen.CapTinhId).WillCascadeOnDelete(false);
                //Danh sách các xã
            modelBuilder.Entity<CapXa>().ToTable("DanhSachXa");
            modelBuilder.Entity<CapXa>().Property(h => h.TenXa).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CapXa>().HasRequired(xa => xa.CapHuyen)
                                        .WithMany(huyen => huyen.DanhSachXa)
                                        .HasForeignKey(xa => xa.CapHuyenId).WillCascadeOnDelete(false);

            
            base.OnModelCreating(modelBuilder);
        }

        public ApplicationDbContext()
            : base("NapaStudentConnection", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = false;
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}