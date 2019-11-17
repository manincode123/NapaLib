namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LienKetDiem_LopMonHoc : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DanhSachDiem", new[] { "MonHocId" });
            AddColumn("dbo.DanhSachDiem", "LopId", c => c.Int(nullable: false));
            CreateIndex("dbo.DanhSachDiem", new[] { "LopId", "MonHocId" });
            AddForeignKey("dbo.DanhSachDiem", new[] { "LopId", "MonHocId" }, "dbo.DanhSachLopMonHoc", new[] { "LopId", "MonHocId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachDiem", new[] { "LopId", "MonHocId" }, "dbo.DanhSachLopMonHoc");
            DropIndex("dbo.DanhSachDiem", new[] { "LopId", "MonHocId" });
            DropColumn("dbo.DanhSachDiem", "LopId");
            CreateIndex("dbo.DanhSachDiem", "MonHocId");
        }
    }
}
