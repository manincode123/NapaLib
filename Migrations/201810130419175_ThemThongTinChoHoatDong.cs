namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemThongTinChoHoatDong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachHoatDong", "SoLuotThamGia", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachHoatDong", "CapHoatDong", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachHoatDong", "CapHoatDong");
            DropColumn("dbo.DanhSachHoatDong", "SoLuotThamGia");
        }
    }
}
