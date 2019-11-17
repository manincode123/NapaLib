namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemIdLopMonHoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachLopMonHoc", "Id", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DanhSachLopMonHoc", "Id");
        }
    }
}
