namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemNgayThamGiaVaoDanhSachThamGiaHD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachThamGiaHoatDong", "NgayThamGia", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachThamGiaHoatDong", "NgayThamGia");
        }
    }
}
