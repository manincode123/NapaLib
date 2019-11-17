namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaBaiVietV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachBaiViet", "SoLuotThich", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachBaiViet", "SoLuotXem", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachBaiViet", "DuocPheDuyet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachBaiViet", "DuocPheDuyet");
            DropColumn("dbo.DanhSachBaiViet", "SoLuotXem");
            DropColumn("dbo.DanhSachBaiViet", "SoLuotThich");
        }
    }
}
