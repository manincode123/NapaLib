namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaModelLop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachLop", "SoLuongSinhVien", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachLop", "SoLuongDoanVien", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachLop", "SoLuongDangVien", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachLop", "DaTotNghiep", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachLop", "LopChuyenNganh", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachLop", "LopChuyenNganh");
            DropColumn("dbo.DanhSachLop", "DaTotNghiep");
            DropColumn("dbo.DanhSachLop", "SoLuongDangVien");
            DropColumn("dbo.DanhSachLop", "SoLuongDoanVien");
            DropColumn("dbo.DanhSachLop", "SoLuongSinhVien");
        }
    }
}
