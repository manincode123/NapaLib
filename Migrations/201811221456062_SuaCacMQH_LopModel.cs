namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuaCacMQH_LopModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachSinhVien", "LopId", "dbo.DanhSachLop");
            DropIndex("dbo.DanhSachSinhVien", new[] { "LopId" });
            CreateTable(
                "dbo.DanhSachSinhVienLop",
                c => new
                    {
                        SinhVienId = c.Int(nullable: false),
                        LopChuyenNganh = c.Boolean(nullable: false),
                        LopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SinhVienId, t.LopChuyenNganh })
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.SinhVienId)
                .Index(t => t.LopId);
            
            AddColumn("dbo.DanhSachThamGiaHoatDong", "LopId", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "LopDangHocId", c => c.Int(nullable: false));
            CreateIndex("dbo.DanhSachThamGiaHoatDong", "LopId");
            CreateIndex("dbo.DanhSachSinhVien", "LopDangHocId");
            AddForeignKey("dbo.DanhSachThamGiaHoatDong", "LopId", "dbo.DanhSachLop", "Id");
            AddForeignKey("dbo.DanhSachSinhVien", "LopDangHocId", "dbo.DanhSachLop", "Id");
            AddForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien", "Id", cascadeDelete: true);
            DropColumn("dbo.DanhSachSinhVien", "LopId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachSinhVien", "LopId", c => c.Int(nullable: false));
            DropForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachSinhVien", "LopDangHocId", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachSinhVienLop", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachThamGiaHoatDong", "LopId", "dbo.DanhSachLop");
            DropIndex("dbo.DanhSachSinhVienLop", new[] { "LopId" });
            DropIndex("dbo.DanhSachSinhVienLop", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachSinhVien", new[] { "LopDangHocId" });
            DropIndex("dbo.DanhSachThamGiaHoatDong", new[] { "LopId" });
            DropColumn("dbo.DanhSachSinhVien", "LopDangHocId");
            DropColumn("dbo.DanhSachThamGiaHoatDong", "LopId");
            DropTable("dbo.DanhSachSinhVienLop");
            CreateIndex("dbo.DanhSachSinhVien", "LopId");
            AddForeignKey("dbo.DanhSachSDT", "SinhVienId", "dbo.DanhSachSinhVien", "Id");
            AddForeignKey("dbo.DanhSachDiaChi", "SinhVienId", "dbo.DanhSachSinhVien", "Id");
        }
    }
}
