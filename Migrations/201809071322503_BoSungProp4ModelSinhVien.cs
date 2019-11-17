namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoSungProp4ModelSinhVien : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachSinhVien", "MSSV", c => c.String());
            AddColumn("dbo.DanhSachSinhVien", "LaDoanVien", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "LaHoiVien", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "LaDangVien", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "DaRaTruong", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachSinhVien", "DaRaTruong");
            DropColumn("dbo.DanhSachSinhVien", "LaDangVien");
            DropColumn("dbo.DanhSachSinhVien", "LaHoiVien");
            DropColumn("dbo.DanhSachSinhVien", "LaDoanVien");
            DropColumn("dbo.DanhSachSinhVien", "MSSV");
        }
    }
}
