namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemBaiViet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachBaiViet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NgayTao = c.DateTime(nullable: false),
                        NoiDungBaiViet = c.String(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.SinhVienId);
            
            CreateTable(
                "dbo.DanhSachBaiVietDonVi",
                c => new
                    {
                        BaiVietId = c.Int(nullable: false),
                        DonViId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BaiVietId)
                .ForeignKey("dbo.DanhSachBaiViet", t => t.BaiVietId)
                .ForeignKey("dbo.DanhSachDonVi", t => t.DonViId, cascadeDelete: true)
                .Index(t => t.BaiVietId)
                .Index(t => t.DonViId);
            
            CreateTable(
                "dbo.DanhSachBaiVietHoatDong",
                c => new
                    {
                        BaiVietId = c.Int(nullable: false),
                        HoatDongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BaiVietId)
                .ForeignKey("dbo.DanhSachBaiViet", t => t.BaiVietId)
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .Index(t => t.BaiVietId)
                .Index(t => t.HoatDongId);
            
            CreateTable(
                "dbo.DanhSachBaiVietLop",
                c => new
                    {
                        BaiVietId = c.Int(nullable: false),
                        LopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BaiVietId)
                .ForeignKey("dbo.DanhSachBaiViet", t => t.BaiVietId)
                .ForeignKey("dbo.DanhSachLop", t => t.LopId, cascadeDelete: true)
                .Index(t => t.BaiVietId)
                .Index(t => t.LopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachBaiViet", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachBaiVietDonVi", "DonViId", "dbo.DanhSachDonVi");
            DropForeignKey("dbo.DanhSachBaiVietLop", "LopId", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachBaiVietLop", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiVietHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.DanhSachBaiVietHoatDong", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiVietDonVi", "BaiVietId", "dbo.DanhSachBaiViet");
            DropIndex("dbo.DanhSachBaiVietLop", new[] { "LopId" });
            DropIndex("dbo.DanhSachBaiVietLop", new[] { "BaiVietId" });
            DropIndex("dbo.DanhSachBaiVietHoatDong", new[] { "HoatDongId" });
            DropIndex("dbo.DanhSachBaiVietHoatDong", new[] { "BaiVietId" });
            DropIndex("dbo.DanhSachBaiVietDonVi", new[] { "DonViId" });
            DropIndex("dbo.DanhSachBaiVietDonVi", new[] { "BaiVietId" });
            DropIndex("dbo.DanhSachBaiViet", new[] { "SinhVienId" });
            DropTable("dbo.DanhSachBaiVietLop");
            DropTable("dbo.DanhSachBaiVietHoatDong");
            DropTable("dbo.DanhSachBaiVietDonVi");
            DropTable("dbo.DanhSachBaiViet");
        }
    }
}
