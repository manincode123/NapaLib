namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemHoatDong : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachHoatDong",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenHoatDong = c.String(),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayKetThuc = c.DateTime(nullable: false),
                        MoTa = c.String(),
                        SoNgayTinhNguyen = c.Byte(nullable: false),
                        DaKetThuc = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HoatDongDonVi",
                c => new
                    {
                        HoatDongId = c.Int(nullable: false),
                        DonViId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HoatDongId, t.DonViId })
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachDonVi", t => t.DonViId, cascadeDelete: true)
                .Index(t => t.HoatDongId)
                .Index(t => t.DonViId);
            
            CreateTable(
                "dbo.HoatDongLop",
                c => new
                    {
                        HoatDongId = c.Int(nullable: false),
                        LopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HoatDongId, t.LopId })
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachLop", t => t.LopId, cascadeDelete: true)
                .Index(t => t.HoatDongId)
                .Index(t => t.LopId);
            
            CreateTable(
                "dbo.DanhSachThamGiaHoatDong",
                c => new
                    {
                        HoatDongId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HoatDongId, t.SinhVienId })
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.HoatDongId)
                .Index(t => t.SinhVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.HoatDongLop", "LopId", "dbo.DanhSachLop");
            DropForeignKey("dbo.HoatDongLop", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.HoatDongDonVi", "DonViId", "dbo.DanhSachDonVi");
            DropForeignKey("dbo.HoatDongDonVi", "HoatDongId", "dbo.DanhSachHoatDong");
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "HoatDongId" });
            DropIndex("dbo.HoatDongLop", new[] { "LopId" });
            DropIndex("dbo.HoatDongLop", new[] { "HoatDongId" });
            DropIndex("dbo.HoatDongDonVi", new[] { "DonViId" });
            DropIndex("dbo.HoatDongDonVi", new[] { "HoatDongId" });
            DropTable("dbo.DanhSachThamGiaHoatDong");
            DropTable("dbo.HoatDongLop");
            DropTable("dbo.HoatDongDonVi");
            DropTable("dbo.DanhSachHoatDong");
        }
    }
}
