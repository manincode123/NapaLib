namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemIndexChoDanhSachThamGiaHoatDong : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "HoatDongId" });
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "SinhVienId" });
            CreateIndex("dbo.DanhSachThamGiaHoatDong", "HoatDongId");
            CreateIndex("dbo.DanhSachThamGiaHoatDong", "SinhVienId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "HoatDongId" });
            CreateIndex("dbo.DanhSachThamGiaHoatDong", "SinhVienId");
            CreateIndex("dbo.DanhSachThamGiaHoatDong", "HoatDongId");
        }
    }
}
