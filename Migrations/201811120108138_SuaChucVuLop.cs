namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaChucVuLop : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachChucVuLop", "LopId", "dbo.DanhSachLop");
            AddForeignKey("dbo.DanhSachChucVuLop", "LopId", "dbo.DanhSachLop", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachChucVuLop", "LopId", "dbo.DanhSachLop");
            AddForeignKey("dbo.DanhSachChucVuLop", "LopId", "dbo.DanhSachLop", "Id", cascadeDelete: true);
        }
    }
}
