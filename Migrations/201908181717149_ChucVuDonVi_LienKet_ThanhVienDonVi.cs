namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChucVuDonVi_LienKet_ThanhVienDonVi : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DanhSachChucVuDonVi", new[] { "DonViId", "SinhVienId" });
            AddForeignKey("dbo.DanhSachChucVuDonVi", new[] { "DonViId", "SinhVienId" }, "dbo.DanhSachThanhVienDonVi", new[] { "DonViId", "SinhVienId" }, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachChucVuDonVi", new[] { "DonViId", "SinhVienId" }, "dbo.DanhSachThanhVienDonVi");
            DropIndex("dbo.DanhSachChucVuDonVi", new[] { "DonViId", "SinhVienId" });
        }
    }
}
