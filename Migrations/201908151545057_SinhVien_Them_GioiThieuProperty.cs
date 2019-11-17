namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SinhVien_Them_GioiThieuProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachSinhVien", "GioiThieu", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachSinhVien", "GioiThieu");
        }
    }
}
