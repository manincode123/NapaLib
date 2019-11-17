namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LienKetSinhVien_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachSinhVien", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DanhSachSinhVien", "ApplicationUserId");
            AddForeignKey("dbo.DanhSachSinhVien", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachSinhVien", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.DanhSachSinhVien", new[] { "ApplicationUserId" });
            DropColumn("dbo.DanhSachSinhVien", "ApplicationUserId");
        }
    }
}
