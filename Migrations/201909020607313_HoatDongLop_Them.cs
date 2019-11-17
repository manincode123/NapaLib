namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoatDongLop_Them : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HoatDongLop", newName: "DanhSachHoatDongLop");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DanhSachHoatDongLop", newName: "HoatDongLop");
        }
    }
}
