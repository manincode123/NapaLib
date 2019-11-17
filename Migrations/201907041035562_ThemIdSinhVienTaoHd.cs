namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemIdSinhVienTaoHd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachHoatDong", "IdSinhVienTaoHd", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachHoatDong", "IdSinhVienTaoHd");
        }
    }
}
