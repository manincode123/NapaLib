namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoiTenTieuDeChuongTrinhHoatDong : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.DanhSachChuongTrinhHoatDong", "ChuDe", "TieuDe");
            
        }
        
        public override void Down()
        {
            RenameColumn("dbo.DanhSachChuongTrinhHoatDong", "TieuDe", "ChuDe");

        }
    }
}
