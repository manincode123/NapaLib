namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDanhSachHoiVien : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachHoiVienHoiSinhVien",
                c => new
                    {
                        SinhVienId = c.Int(nullable: false),
                        NgayVaoHoi = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SinhVienId)
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId)
                .Index(t => t.SinhVienId);
            
            DropColumn("dbo.DanhSachSinhVien", "LaHoiVien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DanhSachSinhVien", "LaHoiVien", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.DanhSachHoiVienHoiSinhVien", "SinhVienId", "dbo.DanhSachSinhVien");
            DropIndex("dbo.DanhSachHoiVienHoiSinhVien", new[] { "SinhVienId" });
            DropTable("dbo.DanhSachHoiVienHoiSinhVien");
        }
    }
}
