namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XoaIdLopMonHoc : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DanhSachLopMonHoc", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachLopMonHoc", "Id", c => c.Int(nullable: false, identity: true));
        }
    }
}
