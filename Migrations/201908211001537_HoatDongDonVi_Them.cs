namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoatDongDonVi_Them : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DanhSachHoatDongDonVi", newName: "DanhSachHoatDongDonVi");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DanhSachHoatDongDonVi", newName: "DanhSachHoatDongDonVi");
        }
    }
}
