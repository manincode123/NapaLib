namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChucVuDonVi_Them : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachChucVuDonVi",
                c => new
                    {
                        ChucVuId = c.Int(nullable: false),
                        DonViId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                        TenChucVu = c.String(),
                        QuanLyThongTin = c.Boolean(nullable: false),
                        QuanLyThanhVien = c.Boolean(nullable: false),
                        QuanLyChucVu = c.Boolean(nullable: false),
                        QuanLyHoatDong = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChucVuId, t.DonViId, t.SinhVienId })
                .ForeignKey("dbo.DanhSachChucVu", t => t.ChucVuId)
                .ForeignKey("dbo.DanhSachDonVi", t => t.DonViId)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId)
                .Index(t => t.ChucVuId)
                .Index(t => t.DonViId)
                .Index(t => t.SinhVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachChucVuDonVi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachChucVuDonVi", "DonViId", "dbo.DanhSachDonVi");
            DropForeignKey("dbo.DanhSachChucVuDonVi", "ChucVuId", "dbo.DanhSachChucVu");
            DropIndex("dbo.DanhSachChucVuDonVi", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachChucVuDonVi", new[] { "DonViId" });
            DropIndex("dbo.DanhSachChucVuDonVi", new[] { "ChucVuId" });
            DropTable("dbo.DanhSachChucVuDonVi");
        }
    }
}
