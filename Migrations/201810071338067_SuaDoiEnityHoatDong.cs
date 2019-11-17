namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaDoiEnityHoatDong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachHoatDong", "BiHuy", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachHoatDong", "HoatDongNgoaiHocVien", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachHoatDong", "DuocPheDuyet", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachHoatDong", "DiaDiem", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachHoatDong", "DiaDiem");
            DropColumn("dbo.DanhSachHoatDong", "DuocPheDuyet");
            DropColumn("dbo.DanhSachHoatDong", "HoatDongNgoaiHocVien");
            DropColumn("dbo.DanhSachHoatDong", "BiHuy");
        }
    }
}
