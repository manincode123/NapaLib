namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChinhSuaDiem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachDiemBoSung",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SinhVienId = c.Int(nullable: false),
                        MonHocId = c.Int(nullable: false),
                        Diem = c.Byte(nullable: false),
                        LoaiDiem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachDiem", t => new { t.SinhVienId, t.MonHocId }, cascadeDelete: true)
                .Index(t => new { t.SinhVienId, t.MonHocId });
            
            CreateTable(
                "dbo.DanhSachLichHoc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LopId = c.Int(nullable: false),
                        MonHocId = c.Int(nullable: false),
                        BuoiSang = c.Boolean(nullable: false),
                        Thu246 = c.Boolean(nullable: false),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayKetThuc = c.DateTime(nullable: false),
                        GiaoVienDay = c.String(nullable: false),
                        PhongHoc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachLopMonHoc", t => new { t.LopId, t.MonHocId }, cascadeDelete: true)
                .Index(t => new { t.LopId, t.MonHocId });
            
            AddColumn("dbo.DanhSachDiem", "DiemDieuKien1", c => c.Byte());
            AddColumn("dbo.DanhSachDiem", "DiemDieuKien2", c => c.Byte());
            AddColumn("dbo.DanhSachDiem", "HocKi", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachLopMonHoc", "NgayThi", c => c.DateTime(nullable: false));
            AddColumn("dbo.DanhSachLopMonHoc", "DiaDiemThi", c => c.String(nullable: false));
            AlterColumn("dbo.DanhSachDiem", "DiemChuyenCan", c => c.Byte());
            AlterColumn("dbo.DanhSachDiem", "DiemThi", c => c.Byte());
            DropColumn("dbo.DanhSachDiem", "DiemDieuKien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachDiem", "DiemDieuKien", c => c.Byte(nullable: false));
            DropForeignKey("dbo.DanhSachLichHoc", new[] { "LopId", "MonHocId" }, "dbo.DanhSachLopMonHoc");
            DropForeignKey("dbo.DanhSachDiemBoSung", new[] { "SinhVienId", "MonHocId" }, "dbo.DanhSachDiem");
            DropIndex("dbo.DanhSachLichHoc", new[] { "LopId", "MonHocId" });
            DropIndex("dbo.DanhSachDiemBoSung", new[] { "SinhVienId", "MonHocId" });
            AlterColumn("dbo.DanhSachDiem", "DiemThi", c => c.Byte(nullable: false));
            AlterColumn("dbo.DanhSachDiem", "DiemChuyenCan", c => c.Byte(nullable: false));
            DropColumn("dbo.DanhSachLopMonHoc", "DiaDiemThi");
            DropColumn("dbo.DanhSachLopMonHoc", "NgayThi");
            DropColumn("dbo.DanhSachDiem", "HocKi");
            DropColumn("dbo.DanhSachDiem", "DiemDieuKien2");
            DropColumn("dbo.DanhSachDiem", "DiemDieuKien1");
            DropTable("dbo.DanhSachLichHoc");
            DropTable("dbo.DanhSachDiemBoSung");
        }
    }
}
