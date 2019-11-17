namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaDiemV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DanhSachDiem", "DiemTb", c => c.Byte(nullable: false));
            AlterColumn("dbo.DanhSachDiemBoSung", "Diem", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DanhSachDiemBoSung", "Diem", c => c.Byte(nullable: false));
            AlterColumn("dbo.DanhSachDiem", "DiemTb", c => c.Single(nullable: false));
        }
    }
}
