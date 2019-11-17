namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDangKyThamGia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachThamGiaHoatDong", "DuocPheDuyet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachThamGiaHoatDong", "DuocPheDuyet");
        }
    }
}
