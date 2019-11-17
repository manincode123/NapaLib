namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDiemTB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachDiem", "DiemTb", c => c.Single(nullable: false));
            AddColumn("dbo.DanhSachMonHoc", "HaiDiemDk", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachMonHoc", "LoaiMon", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachMonHoc", "LoaiMon");
            DropColumn("dbo.DanhSachMonHoc", "HaiDiemDk");
            DropColumn("dbo.DanhSachDiem", "DiemTb");
        }
    }
}
