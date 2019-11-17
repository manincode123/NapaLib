namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSua_HoatDong_ThemLienKetSinhVienTaoHoatDong : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DanhSachHoatDong", "IdSinhVienTaoHd");
            AddForeignKey("dbo.DanhSachHoatDong", "IdSinhVienTaoHd", "dbo.DanhSachSinhVien", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachHoatDong", "IdSinhVienTaoHd", "dbo.DanhSachSinhVien");
            DropIndex("dbo.DanhSachHoatDong", new[] { "IdSinhVienTaoHd" });
        }
    }
}
