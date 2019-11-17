namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lop_Them_AnhBiaLop_Xoa_SoLuong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachLop", "AnhBia", c => c.String());
            DropColumn("dbo.DanhSachLop", "SoLuongSinhVien");
            DropColumn("dbo.DanhSachLop", "SoLuongDoanVien");
            DropColumn("dbo.DanhSachLop", "SoLuongDangVien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachLop", "SoLuongDangVien", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachLop", "SoLuongDoanVien", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachLop", "SoLuongSinhVien", c => c.Int(nullable: false));
            DropColumn("dbo.DanhSachLop", "AnhBia");
        }
    }
}
