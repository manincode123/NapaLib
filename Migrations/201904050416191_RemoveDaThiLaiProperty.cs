namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDaThiLaiProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DanhSachDiem", "DaThiLai");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachDiem", "DaThiLai", c => c.Boolean(nullable: false));
        }
    }
}
