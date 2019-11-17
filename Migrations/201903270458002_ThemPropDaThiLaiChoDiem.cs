namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemPropDaThiLaiChoDiem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachDiem", "DaThiLai", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachDiem", "DaThiLai");
        }
    }
}
