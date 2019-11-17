namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemNoiDungSoLuoc_ModelHoatDong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachHoatDong", "NoiDung", c => c.String());
            RenameColumn("dbo.DanhSachHoatDong", "MoTa","SoLuoc");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.DanhSachHoatDong", "SoLuoc", "MoTa");
            DropColumn("dbo.DanhSachHoatDong", "NoiDung");
        }
    }
}
