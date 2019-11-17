namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaBaiVietV4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachBaiViet", "DaXoa", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachBaiViet", "DaXoa");
        }
    }
}
