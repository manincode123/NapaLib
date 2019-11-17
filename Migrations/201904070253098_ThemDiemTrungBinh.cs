namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemDiemTrungBinh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachDiemTrungBinhHocKi",
                c => new
                    {
                        SinhVienId = c.Int(nullable: false),
                        HocKi = c.Int(nullable: false),
                        TongDiem = c.Int(nullable: false),
                        TongHocPhan = c.Byte(nullable: false),
                        DiemTb = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.SinhVienId, t.HocKi })
                .ForeignKey("dbo.DanhSachSinhVien", t => t.SinhVienId, cascadeDelete: true)
                .Index(t => t.SinhVienId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachDiemTrungBinhHocKi", "SinhVienId", "dbo.DanhSachSinhVien");
            DropIndex("dbo.DanhSachDiemTrungBinhHocKi", new[] { "SinhVienId" });
            DropTable("dbo.DanhSachDiemTrungBinhHocKi");
        }
    }
}
