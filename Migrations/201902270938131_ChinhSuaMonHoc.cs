namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaMonHoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DanhSachMonHoc", "SoHocPhan", c => c.Byte(nullable: false));
            DropColumn("dbo.DanhSachMonHoc", "HocPhan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachMonHoc", "HocPhan", c => c.Byte(nullable: false));
            DropColumn("dbo.DanhSachMonHoc", "SoHocPhan");
        }
    }
}
