namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemTonGiao_DanToc_GioiTinh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachDanToc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenDanToc = c.String(maxLength: 100),
                        TenGoiKhac = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DanhSachGioiTinh",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenGioiTinh = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DanhSachTonGiao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenTonGiao = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DanhSachSinhVien", "DanTocId", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "TonGiaoId", c => c.Int(nullable: false));
            AddColumn("dbo.DanhSachSinhVien", "GioiTinhId", c => c.Int(nullable: false));
            CreateIndex("dbo.DanhSachSinhVien", "DanTocId");
            CreateIndex("dbo.DanhSachSinhVien", "TonGiaoId");
            CreateIndex("dbo.DanhSachSinhVien", "GioiTinhId");
            AddForeignKey("dbo.DanhSachSinhVien", "DanTocId", "dbo.DanhSachDanToc", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachSinhVien", "GioiTinhId", "dbo.DanhSachGioiTinh", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DanhSachSinhVien", "TonGiaoId", "dbo.DanhSachTonGiao", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachSinhVien", "TonGiaoId", "dbo.DanhSachTonGiao");
            DropForeignKey("dbo.DanhSachSinhVien", "GioiTinhId", "dbo.DanhSachGioiTinh");
            DropForeignKey("dbo.DanhSachSinhVien", "DanTocId", "dbo.DanhSachDanToc");
            DropIndex("dbo.DanhSachSinhVien", new[] { "GioiTinhId" });
            DropIndex("dbo.DanhSachSinhVien", new[] { "TonGiaoId" });
            DropIndex("dbo.DanhSachSinhVien", new[] { "DanTocId" });
            DropColumn("dbo.DanhSachSinhVien", "GioiTinhId");
            DropColumn("dbo.DanhSachSinhVien", "TonGiaoId");
            DropColumn("dbo.DanhSachSinhVien", "DanTocId");
            DropTable("dbo.DanhSachTonGiao");
            DropTable("dbo.DanhSachGioiTinh");
            DropTable("dbo.DanhSachDanToc");
        }
    }
}
