namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDonVi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachThanhVienDonVi",
                c => new
                    {
                        DonViId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                        ChucVuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DonViId, t.SinhVienId })
                .ForeignKey("dbo.DanhSachChucVu", t => t.ChucVuId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachDonVi", t => t.DonViId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.DonViId)
                .Index(t => t.SinhVienId)
                .Index(t => t.ChucVuId);
            
            CreateTable(
                "dbo.DanhSachDonVi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenDonVi = c.String(),
                        NgayThanhLap = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachThanhVienDonVi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachThanhVienDonVi", "DonViId", "dbo.DanhSachDonVi");
            DropForeignKey("dbo.DanhSachThanhVienDonVi", "ChucVuId", "dbo.DanhSachChucVu");
            DropIndex("dbo.DanhSachThanhVienDonVi", new[] { "ChucVuId" });
            DropIndex("dbo.DanhSachThanhVienDonVi", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachThanhVienDonVi", new[] { "DonViId" });
            DropTable("dbo.DanhSachDonVi");
            DropTable("dbo.DanhSachThanhVienDonVi");
        }
    }
}
