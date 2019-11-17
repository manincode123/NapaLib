namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThayDoiDiemTrungBinh : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DanhSachDiemTrungBinhHocKi", "TongDiem", c => c.Int());
            AlterColumn("dbo.DanhSachDiemTrungBinhHocKi", "TongHocPhan", c => c.Byte());
            AlterColumn("dbo.DanhSachDiemTrungBinhHocKi", "DiemTb", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DanhSachDiemTrungBinhHocKi", "DiemTb", c => c.Single(nullable: false));
            AlterColumn("dbo.DanhSachDiemTrungBinhHocKi", "TongHocPhan", c => c.Byte(nullable: false));
            AlterColumn("dbo.DanhSachDiemTrungBinhHocKi", "TongDiem", c => c.Int(nullable: false));
        }
    }
}
