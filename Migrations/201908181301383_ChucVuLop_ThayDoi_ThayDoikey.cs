namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChucVuLop_ThayDoi_ThayDoikey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DanhSachChucVuLop");
            AddPrimaryKey("dbo.DanhSachChucVuLop", new[] { "ChucVuId", "LopId", "SinhVienId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DanhSachChucVuLop");
            AddPrimaryKey("dbo.DanhSachChucVuLop", new[] { "ChucVuId", "LopId" });
        }
    }
}
