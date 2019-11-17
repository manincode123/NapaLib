namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDanhSachThamGiaHoatDong : DbMigration
    {
        public override void Up()
        {

            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "SinhVienId", "dbo.DanhSachSinhVien");
            DropTable("dbo.DanhSachThamGiaHoatDong");
            CreateTable(
                "dbo.DanhSachThamGiaHoatDong",
                c => new
                    {
                        HoatDongId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HoatDongId, t.SinhVienId })
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true);
            
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DanhSachThamGiaHoatDong",
                c => new
                    {
                        HoatDongId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HoatDongId, t.SinhVienId });
            
            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "SinhVienId", "dbo.DanhSachSinhVien");
            DropTable("dbo.DanhSachThamGiaHoatDong");
            AddForeignKey("dbo.DanhSachThamGiaHoatDong", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachThamGiaHoatDong", "HoatDongId", "dbo.DanhSachHoatDong", "Id", cascadeDelete: true);
        }
    }
}
