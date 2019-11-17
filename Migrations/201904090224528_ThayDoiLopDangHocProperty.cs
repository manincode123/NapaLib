namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiLopDangHocProperty : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DanhSachSinhVien", new[] { "LopDangHocId" });
            AlterColumn("dbo.DanhSachSinhVien", "LopDangHocId", c => c.Int());
            CreateIndex("dbo.DanhSachSinhVien", "LopDangHocId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DanhSachSinhVien", new[] { "LopDangHocId" });
            AlterColumn("dbo.DanhSachSinhVien", "LopDangHocId", c => c.Int(nullable: false));
            CreateIndex("dbo.DanhSachSinhVien", "LopDangHocId");
        }
    }
}
