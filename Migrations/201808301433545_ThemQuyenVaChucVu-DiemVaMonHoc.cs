namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemQuyenVaChucVuDiemVaMonHoc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.GroupId })
                .ForeignKey("dbo.DanhSachGroup", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.DanhSachGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.DanhSachGroup", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.DanhSachDiem",
                c => new
                    {
                        SinhVienId = c.Int(nullable: false),
                        MonHocId = c.Int(nullable: false),
                        DiemChuyenCan = c.Byte(nullable: false),
                        DiemDieuKien = c.Byte(nullable: false),
                        DiemThi = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => new { t.SinhVienId, t.MonHocId })
                .ForeignKey("dbo.DanhSachMonHoc", t => t.MonHocId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.SinhVienId)
                .Index(t => t.MonHocId);
            
            CreateTable(
                "dbo.DanhSachMonHoc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenMonHoc = c.String(),
                        HocPhan = c.Byte(nullable: false),
                        SoTiet = c.Byte(nullable: false),
                        Lop_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachLop", t => t.Lop_Id)
                .Index(t => t.Lop_Id);
            
            CreateTable(
                "dbo.DanhSachGroupCVLop",
                c => new
                    {
                        ChucVuId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChucVuId)
                .ForeignKey("dbo.DanhSachChucVu", t => t.ChucVuId)
                .ForeignKey("dbo.DanhSachGroup", t => t.ChucVuId)
                .Index(t => t.ChucVuId);
            
            CreateTable(
                "dbo.DanhSachChucVu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenChucVu = c.String(),
                        LoaiChucVu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DanhSachLopMonHoc",
                c => new
                    {
                        LopId = c.Int(nullable: false),
                        MonHocId = c.Int(nullable: false),
                        HocKi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LopId, t.MonHocId })
                .ForeignKey("dbo.DanhSachLop", t => t.LopId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachMonHoc", t => t.MonHocId, cascadeDelete: true)
                .Index(t => t.LopId)
                .Index(t => t.MonHocId);
            
            CreateTable(
                "dbo.DanhSachChucVuLop",
                c => new
                    {
                        ChucVuId = c.Int(nullable: false),
                        LopId = c.Int(nullable: false),
                        SinhVienId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChucVuId, t.LopId })
                .ForeignKey("dbo.DanhSachChucVu", t => t.ChucVuId)
                .ForeignKey("dbo.DanhSachLop", t => t.LopId, cascadeDelete: true)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId)
                .Index(t => t.ChucVuId)
                .Index(t => t.LopId)
                .Index(t => t.SinhVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachChucVuLop", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachChucVuLop", "LopId", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachChucVuLop", "ChucVuId", "dbo.DanhSachChucVu");
            DropForeignKey("dbo.DanhSachLopMonHoc", "MonHocId", "dbo.DanhSachMonHoc");
            DropForeignKey("dbo.DanhSachLopMonHoc", "LopId", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachGroupCVLop", "ChucVuId", "dbo.DanhSachGroup");
            DropForeignKey("dbo.DanhSachGroupCVLop", "ChucVuId", "dbo.DanhSachChucVu");
            DropForeignKey("dbo.DanhSachMonHoc", "Lop_Id", "dbo.DanhSachLop");
            DropForeignKey("dbo.DanhSachDiem", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DanhSachDiem", "MonHocId", "dbo.DanhSachMonHoc");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.DanhSachGroup");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.DanhSachGroup");
            DropIndex("dbo.DanhSachChucVuLop", new[] { "SinhVienId" });
            DropIndex("dbo.DanhSachChucVuLop", new[] { "LopId" });
            DropIndex("dbo.DanhSachChucVuLop", new[] { "ChucVuId" });
            DropIndex("dbo.DanhSachLopMonHoc", new[] { "MonHocId" });
            DropIndex("dbo.DanhSachLopMonHoc", new[] { "LopId" });
            DropIndex("dbo.DanhSachGroupCVLop", new[] { "ChucVuId" });
            DropIndex("dbo.DanhSachMonHoc", new[] { "Lop_Id" });
            DropIndex("dbo.DanhSachDiem", new[] { "MonHocId" });
            DropIndex("dbo.DanhSachDiem", new[] { "SinhVienId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropTable("dbo.DanhSachChucVuLop");
            DropTable("dbo.DanhSachLopMonHoc");
            DropTable("dbo.DanhSachChucVu");
            DropTable("dbo.DanhSachGroupCVLop");
            DropTable("dbo.DanhSachMonHoc");
            DropTable("dbo.DanhSachDiem");
            DropTable("dbo.ApplicationUserGroups");
            DropTable("dbo.DanhSachGroup");
            DropTable("dbo.ApplicationRoleGroups");
        }
    }
}
