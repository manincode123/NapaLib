namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemTheoDoiHoatDong : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachTheoDoiHoatDong",
                c => new
                    {
                        HoatDongId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HoatDongId, t.SinhVienId })
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .Index(t => t.HoatDongId)
                .Index(t => t.SinhVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachTheoDoiHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.DanhSachTheoDoiHoatDong", "SinhVienId", "dbo.DanhSachSinhVien");
            DropIndex("dbo.DanhSachTheoDoiHoatDong", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachTheoDoiHoatDong", new[] { "HoatDongId" });
            DropTable("dbo.DanhSachTheoDoiHoatDong");
        }
    }
}
