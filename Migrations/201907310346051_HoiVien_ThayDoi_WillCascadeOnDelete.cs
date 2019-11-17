namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoiVien_ThayDoi_WillCascadeOnDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachHoiVienHoiSinhVien", "SinhVienId", "dbo.DanhSachSinhVien");
            AddForeignKey("dbo.DanhSachHoiVienHoiSinhVien", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachHoiVienHoiSinhVien", "SinhVienId", "dbo.DanhSachSinhVien");
            AddForeignKey("dbo.DanhSachHoiVienHoiSinhVien", "SinhVienId", "dbo.DanhSachSinhVien", "Id");
        }
    }
}
