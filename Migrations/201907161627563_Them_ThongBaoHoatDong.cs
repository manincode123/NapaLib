namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Them_ThongBaoHoatDong : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachThongBaoHoatDongSinhVien",
                c => new
                    {
                        ThongBaoHoatDongId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                        DaDoc = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ThongBaoHoatDongId, t.SinhVienId })
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachThongBaoHoatDong", t => t.ThongBaoHoatDongId, cascadeDelete: true)
                .Index(t => t.ThongBaoHoatDongId)
                .Index(t => t.SinhVienId);
            
            CreateTable(
                "dbo.DanhSachThongBaoHoatDong",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoatDongId = c.Int(nullable: false),
                        LoaiThongBaoHoatDong = c.Int(nullable: false),
                        NgayTaoThongBao = c.DateTime(nullable: false),
                        NgayBatDauGoc = c.DateTime(),
                        NgayKetThucGoc = c.DateTime(),
                        DiaDiemGoc = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .Index(t => t.HoatDongId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachThongBaoHoatDongSinhVien", "ThongBaoHoatDongId", "dbo.DanhSachThongBaoHoatDong");
            DropForeignKey("dbo.DanhSachThongBaoHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropForeignKey("dbo.DanhSachThongBaoHoatDongSinhVien", "SinhVienId", "dbo.DanhSachSinhVien");
            DropIndex("dbo.DanhSachThongBaoHoatDong", new[] { "HoatDongId" });
            DropIndex("dbo.DanhSachThongBaoHoatDongSinhVien", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachThongBaoHoatDongSinhVien", new[] { "ThongBaoHoatDongId" });
            DropTable("dbo.DanhSachThongBaoHoatDong");
            DropTable("dbo.DanhSachThongBaoHoatDongSinhVien");
        }
    }
}
