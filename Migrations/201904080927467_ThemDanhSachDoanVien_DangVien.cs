namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDanhSachDoanVien_DangVien : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DangVien",
                c => new
                    {
                        SinhVienId = c.Int(nullable: false),
                        NgayVaoDangChinhThuc = c.DateTime(nullable: false),
                        NoiVaoDang = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SinhVienId)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId)
                .Index(t => t.SinhVienId);
            
            CreateTable(
                "dbo.DanhSachDoanVien",
                c => new
                    {
                        SinhVienId = c.Int(nullable: false),
                        NgayVaoDoan = c.DateTime(nullable: false),
                        NoiVaoDoan = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SinhVienId)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId)
                .Index(t => t.SinhVienId);
            
            DropColumn("dbo.DanhSachSinhVien", "LaDoanVien");
            DropColumn("dbo.DanhSachSinhVien", "LaDangVien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachSinhVien", "LaDangVien", c => c.Boolean(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "LaDoanVien", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.DanhSachDoanVien", "SinhVienId", "dbo.DanhSachSinhVien");
            DropForeignKey("dbo.DangVien", "SinhVienId", "dbo.DanhSachSinhVien");
            DropIndex("dbo.DanhSachDoanVien", new[] { "SinhVienId" });
            DropIndex("dbo.DangVien", new[] { "SinhVienId" });
            DropTable("dbo.DanhSachDoanVien");
            DropTable("dbo.DangVien");
        }
    }
}
