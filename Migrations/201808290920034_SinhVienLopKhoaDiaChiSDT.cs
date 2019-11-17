namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SinhVienLopKhoaDiaChiSDT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachHuyen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenHuyen = c.String(),
                        CapTinhId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachTinh", t => t.CapTinhId)
                .Index(t => t.CapTinhId);
            
            CreateTable(
                "dbo.DanhSachTinh",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenTinh = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DanhSachXa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenXa = c.String(),
                        CapHuyenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachHuyen", t => t.CapHuyenId)
                .Index(t => t.CapHuyenId);
            
            CreateTable(
                "dbo.DanhSachDiaChi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MoTa = c.String(),
                        LoaiDiaChi = c.Int(nullable: false),
                        CapXaId = c.Int(nullable: false),
                        CapHuyenId = c.Int(nullable: false),
                        CapTinhId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachHuyen", t => t.CapHuyenId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachTinh", t => t.CapTinhId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachXa", t => t.CapXaId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.CapXaId)
                .Index(t => t.CapHuyenId)
                .Index(t => t.CapTinhId)
                .Index(t => t.SinhVienId);
            
            CreateTable(
                "dbo.DanhSachSinhVien",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoVaTenLot = c.String(nullable: false, maxLength: 200),
                        Ten = c.String(nullable: false, maxLength: 50),
                        NgaySinh = c.DateTime(nullable: false),
                        CMND = c.String(),
                        LopId = c.Int(nullable: false),
                        AnhDaiDien = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachLop", t => t.LopId, cascadeDelete: true)
                .Index(t => t.LopId);
            
            CreateTable(
                "dbo.DanhSachLop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenLop = c.String(),
                        KyHieuTenLop = c.String(),
                        KhoaHocId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachKhoaHoc", t => t.KhoaHocId, cascadeDelete: true)
                .Index(t => t.KhoaHocId);
            
            CreateTable(
                "dbo.DanhSachKhoaHoc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenKhoa = c.String(),
                        NamBatDau = c.DateTime(nullable: false),
                        NamKetThuc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DanhSachSDT",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MoTa = c.String(),
                        SoDienThoai = c.String(),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.SinhVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachSinhVien", "LopId", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachLop", "KhoaHocId", "dbo.DanhSachKhoaHoc");
            DropForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachDiaChi", "CapXaId", "dbo.DanhSachXa");
            DropForeignKey("dbo.DanhSachDiaChi", "CapTinhId", "dbo.DanhSachTinh");
            DropForeignKey("dbo.DanhSachDiaChi", "CapHuyenId", "dbo.DanhSachHuyen");
            DropForeignKey("dbo.DanhSachXa", "CapHuyenId", "dbo.DanhSachHuyen");
            DropForeignKey("dbo.DanhSachHuyen", "CapTinhId", "dbo.DanhSachTinh");
            DropIndex("dbo.DanhSachSDT", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachLop", new[] { "KhoaHocId" });
            DropIndex("dbo.DanhSachSinhVien", new[] { "LopId" });
            DropIndex("dbo.DanhSachDiaChi", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachDiaChi", new[] { "CapTinhId" });
            DropIndex("dbo.DanhSachDiaChi", new[] { "CapHuyenId" });
            DropIndex("dbo.DanhSachDiaChi", new[] { "CapXaId" });
            DropIndex("dbo.DanhSachXa", new[] { "CapHuyenId" });
            DropIndex("dbo.DanhSachHuyen", new[] { "CapTinhId" });
            DropTable("dbo.DanhSachSDT");
            DropTable("dbo.DanhSachKhoaHoc");
            DropTable("dbo.DanhSachLop");
            DropTable("dbo.DanhSachSinhVien");
            DropTable("dbo.DanhSachDiaChi");
            DropTable("dbo.DanhSachXa");
            DropTable("dbo.DanhSachTinh");
            DropTable("dbo.DanhSachHuyen");
        }
    }
}
