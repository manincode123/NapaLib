namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_Them_TenNguoiDung : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TenNguoiDung", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TenNguoiDung");
        }
    }
}
