namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemChuongTrinhHoatDong : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachChuongTrinhHoatDong",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NoiDungChuongTrinh = c.String(nullable: false),
                        ChuDe = c.String(nullable: false, maxLength: 150),
                        TgDienRa = c.DateTime(nullable: false),
                        LoaiHienThi = c.Int(nullable: false),
                        HoatDongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachHoatDong", t => t.HoatDongId, cascadeDelete: true)
                .Index(t => t.HoatDongId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachChuongTrinhHoatDong", "HoatDongId", "dbo.DanhSachHoatDong");
            DropIndex("dbo.DanhSachChuongTrinhHoatDong", new[] { "HoatDongId" });
            DropTable("dbo.DanhSachChuongTrinhHoatDong");
        }
    }
}
