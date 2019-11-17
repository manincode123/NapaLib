namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaDiemV3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DanhSachDiem", "DiemTb", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DanhSachDiem", "DiemTb", c => c.Byte(nullable: false));
        }
    }
}
