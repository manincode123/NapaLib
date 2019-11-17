namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaBaiVietV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachBaiViet", "TenBaiViet", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.DanhSachBaiViet", "SoLuoc", c => c.String(nullable: false, maxLength: 150));
            AddColumn("dbo.DanhSachBaiViet", "AnhBia", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachBaiViet", "AnhBia");
            DropColumn("dbo.DanhSachBaiViet", "SoLuoc");
            DropColumn("dbo.DanhSachBaiViet", "TenBaiViet");
        }
    }
}
