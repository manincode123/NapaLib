namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemLichThiLai : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DanhSachLichThiLai",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MonHocId = c.Int(nullable: false),
                        ThoiGianThi = c.DateTime(nullable: false),
                        DiaDiemThi = c.String(nullable: false),
                        DaThiXong = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DanhSachMonHoc", t => t.MonHocId, cascadeDelete: true)
                .Index(t => t.MonHocId);
            
            AddColumn("dbo.DanhSachDiem", "LichThiLaiId", c => c.Int());
            CreateIndex("dbo.DanhSachDiem", "LichThiLaiId");
            AddForeignKey("dbo.DanhSachDiem", "LichThiLaiId", "dbo.DanhSachLichThiLai", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DanhSachLichThiLai", "MonHocId", "dbo.DanhSachMonHoc");
            DropForeignKey("dbo.DanhSachDiem", "LichThiLaiId", "dbo.DanhSachLichThiLai");
            DropIndex("dbo.DanhSachLichThiLai", new[] { "MonHocId" });
            DropIndex("dbo.DanhSachDiem", new[] { "LichThiLaiId" });
            DropColumn("dbo.DanhSachDiem", "LichThiLaiId");
            DropTable("dbo.DanhSachLichThiLai");
        }
    }
}
