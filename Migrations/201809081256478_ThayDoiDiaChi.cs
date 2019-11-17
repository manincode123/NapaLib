namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiDiaChi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachDiaChi", "SoNhaTenDuong", c => c.String(nullable: false));
            DropColumn("dbo.DanhSachDiaChi", "MoTa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachDiaChi", "MoTa", c => c.String());
            DropColumn("dbo.DanhSachDiaChi", "SoNhaTenDuong");
        }
    }
}
