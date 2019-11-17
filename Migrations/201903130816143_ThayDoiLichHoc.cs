namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiLichHoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachLichHoc", "BaTietDau", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachLichHoc", "BaTietDau");
        }
    }
}
