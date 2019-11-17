namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachSinhVien", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.DanhSachSinhVien", new[] { "ApplicationUserId" });
            AlterColumn("dbo.DanhSachSinhVien", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DanhSachSinhVien", "ApplicationUserId");
            AddForeignKey("dbo.DanhSachSinhVien", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachSinhVien", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.DanhSachSinhVien", new[] { "ApplicationUserId" });
            AlterColumn("dbo.DanhSachSinhVien", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DanhSachSinhVien", "ApplicationUserId");
            AddForeignKey("dbo.DanhSachSinhVien", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
