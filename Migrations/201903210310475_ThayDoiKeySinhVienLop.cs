namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiKeySinhVienLop : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DanhSachSinhVienLop");
            AddPrimaryKey("dbo.DanhSachSinhVienLop", new[] { "SinhVienId", "LopId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DanhSachSinhVienLop");
            AddPrimaryKey("dbo.DanhSachSinhVienLop", new[] { "SinhVienId", "LopChuyenNganh" });
        }
    }
}
