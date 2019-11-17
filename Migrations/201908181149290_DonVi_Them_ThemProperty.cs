namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DonVi_Them_ThemProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachDonVi", "GioiThieu", c => c.String());
            AddColumn("dbo.DanhSachDonVi", "LoaiDonVi", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachDonVi", "AnhBia", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachDonVi", "AnhBia");
            DropColumn("dbo.DanhSachDonVi", "LoaiDonVi");
            DropColumn("dbo.DanhSachDonVi", "GioiThieu");
        }
    }
}
