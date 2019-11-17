namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThanhVienDonVi_ThayDoi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachThanhVienDonVi", "ChucVuId", "dbo.DanhSachChucVu");
            DropIndex("dbo.DanhSachThanhVienDonVi", new[] { "ChucVuId" });
            AddColumn("dbo.DanhSachThanhVienDonVi", "NgayGiaNhap", c => c.DateTime(nullable: false));
            AddColumn("dbo.DanhSachThanhVienDonVi", "NgayRoi", c => c.DateTime());
            AddColumn("dbo.DanhSachThanhVienDonVi", "NgungThamGia", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachThanhVienDonVi", "DuocPheDuyet", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachThanhVienDonVi", "GhiChu", c => c.String());
            DropColumn("dbo.DanhSachThanhVienDonVi", "ChucVuId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachThanhVienDonVi", "ChucVuId", c => c.Int(nullable: false));
            DropColumn("dbo.DanhSachThanhVienDonVi", "GhiChu");
            DropColumn("dbo.DanhSachThanhVienDonVi", "DuocPheDuyet");
            DropColumn("dbo.DanhSachThanhVienDonVi", "NgungThamGia");
            DropColumn("dbo.DanhSachThanhVienDonVi", "NgayRoi");
            DropColumn("dbo.DanhSachThanhVienDonVi", "NgayGiaNhap");
            CreateIndex("dbo.DanhSachThanhVienDonVi", "ChucVuId");
            AddForeignKey("dbo.DanhSachThanhVienDonVi", "ChucVuId", "dbo.DanhSachChucVu", "Id", cascadeDelete: true);
        }
    }
}
