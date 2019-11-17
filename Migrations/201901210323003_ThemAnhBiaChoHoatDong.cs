namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemAnhBiaChoHoatDong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachHoatDong", "AnhBia", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachHoatDong", "AnhBia");
        }
    }
}
