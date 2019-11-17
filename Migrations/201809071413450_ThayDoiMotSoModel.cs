namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiMotSoModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachMonHoc", "Lop_Id", "dbo.DanhSachLop");
            DropIndex("dbo.DanhSachMonHoc", new[] { "Lop_Id" });
            AddColumn("dbo.DanhSachSinhVien", "KhoaHocId", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachMonHoc", "KyHieuMonHoc", c => c.String(maxLength: 10));
            AlterColumn("dbo.DanhSachHuyen", "TenHuyen", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.DanhSachTinh", "TenTinh", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.DanhSachXa", "TenXa", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.DanhSachDanToc", "TenDanToc", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.DanhSachSinhVien", "MSSV", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.DanhSachChucVu", "TenChucVu", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachDonVi", "TenDonVi", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachHoatDong", "TenHoatDong", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachLop", "TenLop", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachLop", "KyHieuTenLop", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.DanhSachMonHoc", "TenMonHoc", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachKhoaHoc", "TenKhoa", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachGioiTinh", "TenGioiTinh", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.DanhSachSDT", "SoDienThoai", c => c.String(maxLength: 11));
            AlterColumn("dbo.DanhSachTonGiao", "TenTonGiao", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.DanhSachSinhVien", "KhoaHocId");
            AddForeignKey("dbo.DanhSachSinhVien", "KhoaHocId", "dbo.DanhSachKhoaHoc", "Id");
            AddForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien", "Id");
            AddForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien", "Id");
            DropColumn("dbo.DanhSachMonHoc", "Lop_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachMonHoc", "Lop_Id", c => c.Int());
            DropForeignKey("dbo.DanhSachLopMonHoc", "LopId", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachSinhVien", "KhoaHocId", "dbo.DanhSachKhoaHoc");
            DropIndex("dbo.DanhSachSinhVien", new[] { "KhoaHocId" });
            AlterColumn("dbo.DanhSachTonGiao", "TenTonGiao", c => c.String(maxLength: 100));
            AlterColumn("dbo.DanhSachSDT", "SoDienThoai", c => c.String());
            AlterColumn("dbo.DanhSachGioiTinh", "TenGioiTinh", c => c.String(maxLength: 100));
            AlterColumn("dbo.DanhSachKhoaHoc", "TenKhoa", c => c.String());
            AlterColumn("dbo.DanhSachMonHoc", "TenMonHoc", c => c.String());
            AlterColumn("dbo.DanhSachLop", "KyHieuTenLop", c => c.String());
            AlterColumn("dbo.DanhSachLop", "TenLop", c => c.String());
            AlterColumn("dbo.DanhSachHoatDong", "TenHoatDong", c => c.String());
            AlterColumn("dbo.DanhSachDonVi", "TenDonVi", c => c.String());
            AlterColumn("dbo.DanhSachChucVu", "TenChucVu", c => c.String());
            AlterColumn("dbo.DanhSachSinhVien", "MSSV", c => c.String());
            AlterColumn("dbo.DanhSachDanToc", "TenDanToc", c => c.String(maxLength: 100));
            AlterColumn("dbo.DanhSachXa", "TenXa", c => c.String());
            AlterColumn("dbo.DanhSachTinh", "TenTinh", c => c.String());
            AlterColumn("dbo.DanhSachHuyen", "TenHuyen", c => c.String());
            DropColumn("dbo.DanhSachMonHoc", "KyHieuMonHoc");
            DropColumn("dbo.DanhSachSinhVien", "KhoaHocId");
            RenameColumn(table: "dbo.DanhSachLopMonHoc", name: "LopId", newName: "Lop_Id");
            AddColumn("dbo.DanhSachLopMonHoc", "LopId", c => c.Int(nullable: false));
            CreateIndex("dbo.DanhSachMonHoc", "Lop_Id");
            AddForeignKey("dbo.DanhSachMonHoc", "Lop_Id", "dbo.DanhSachLop", "Id");
            AddForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
        }
    }
}
