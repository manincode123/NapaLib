namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaBaiViet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachBaiVietLop", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiVietDonVi", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiVietHoatDong", "BaiVietId", "dbo.DanhSachBaiViet");
            RenameColumn(table: "dbo.DanhSachBaiViet", name: "SinhVienId", newName: "NguoiTaoId");
            RenameIndex(table: "dbo.DanhSachBaiViet", name: "IX_SinhVienId", newName: "IX_NguoiTaoId");
            DropPrimaryKey("dbo.DanhSachBaiVietLop");
            DropPrimaryKey("dbo.DanhSachBaiVietDonVi");
            DropPrimaryKey("dbo.DanhSachBaiVietHoatDong");
            CreateTable(
                "dbo.DanhSachChuyenMucBaiViet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenChuyenMuc = c.String(nullable: false),
                        MoTa = c.String(nullable: false),
                        AnhBia = c.String(),
                        ChuyenMucChaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachChuyenMucBaiViet", t => t.ChuyenMucChaId)
                .Index(t => t.ChuyenMucChaId);
            
            AddColumn("dbo.DanhSachBaiViet", "ChuyenMucBaiVietId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DanhSachBaiVietLop", new[] { "BaiVietId", "LopId" });
            AddPrimaryKey("dbo.DanhSachBaiVietDonVi", new[] { "BaiVietId", "DonViId" });
            AddPrimaryKey("dbo.DanhSachBaiVietHoatDong", new[] { "BaiVietId", "HoatDongId" });
            CreateIndex("dbo.DanhSachBaiViet", "ChuyenMucBaiVietId");
            AddForeignKey("dbo.DanhSachBaiViet", "ChuyenMucBaiVietId", "dbo.DanhSachChuyenMucBaiViet", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachBaiVietLop", "BaiVietId", "dbo.DanhSachBaiViet", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachBaiVietDonVi", "BaiVietId", "dbo.DanhSachBaiViet", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachBaiVietHoatDong", "BaiVietId", "dbo.DanhSachBaiViet", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachBaiVietHoatDong", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiVietDonVi", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiVietLop", "BaiVietId", "dbo.DanhSachBaiViet");
            DropForeignKey("dbo.DanhSachBaiViet", "ChuyenMucBaiVietId", "dbo.DanhSachChuyenMucBaiViet");
            DropForeignKey("dbo.DanhSachChuyenMucBaiViet", "ChuyenMucChaId", "dbo.DanhSachChuyenMucBaiViet");
            DropIndex("dbo.DanhSachChuyenMucBaiViet", new[] { "ChuyenMucChaId" });
            DropIndex("dbo.DanhSachBaiViet", new[] { "ChuyenMucBaiVietId" });
            DropPrimaryKey("dbo.DanhSachBaiVietHoatDong");
            DropPrimaryKey("dbo.DanhSachBaiVietDonVi");
            DropPrimaryKey("dbo.DanhSachBaiVietLop");
            DropColumn("dbo.DanhSachBaiViet", "ChuyenMucBaiVietId");
            DropTable("dbo.DanhSachChuyenMucBaiViet");
            AddPrimaryKey("dbo.DanhSachBaiVietHoatDong", "BaiVietId");
            AddPrimaryKey("dbo.DanhSachBaiVietDonVi", "BaiVietId");
            AddPrimaryKey("dbo.DanhSachBaiVietLop", "BaiVietId");
            RenameIndex(table: "dbo.DanhSachBaiViet", name: "IX_NguoiTaoId", newName: "IX_SinhVienId");
            RenameColumn(table: "dbo.DanhSachBaiViet", name: "NguoiTaoId", newName: "SinhVienId");
            AddForeignKey("dbo.DanhSachBaiVietHoatDong", "BaiVietId", "dbo.DanhSachBaiViet", "Id");
            AddForeignKey("dbo.DanhSachBaiVietDonVi", "BaiVietId", "dbo.DanhSachBaiViet", "Id");
            AddForeignKey("dbo.DanhSachBaiVietLop", "BaiVietId", "dbo.DanhSachBaiViet", "Id");
        }
    }
}
