namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_Them_SinhVienId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SinhVienId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SinhVienId");
        }
    }
}
