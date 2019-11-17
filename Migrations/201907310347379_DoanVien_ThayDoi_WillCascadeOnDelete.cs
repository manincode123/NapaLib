namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoanVien_ThayDoi_WillCascadeOnDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachDoanVien", "SinhVienId", "dbo.DanhSachSinhVien");
            AddForeignKey("dbo.DanhSachDoanVien", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachDoanVien", "SinhVienId", "dbo.DanhSachSinhVien");
            AddForeignKey("dbo.DanhSachDoanVien", "SinhVienId", "dbo.DanhSachSinhVien", "Id");
        }
    }
}
