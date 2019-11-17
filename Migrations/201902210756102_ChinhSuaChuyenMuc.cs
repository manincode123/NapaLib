namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaChuyenMuc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachChuyenMucBaiViet", "DaXoa", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachChuyenMucBaiViet", "DaXoa");
        }
    }
}
