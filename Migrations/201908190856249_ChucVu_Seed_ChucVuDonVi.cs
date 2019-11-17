namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChucVu_Seed_ChucVuDonVi : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT [dbo].[DanhSachChucVu] ON" +
                "\r\nINSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (10, N\'Trưởng đơn vị\', 4)" +
                "\r\nINSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (11, N\'Phó đơn vị\', 4)" +
                "\r\nINSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (12, N\'Cán bộ đơn vị\', 4)" +
                "\r\nSET IDENTITY_INSERT [dbo].[DanhSachChucVu] OFF\r\n");
        }
        
        public override void Down()
        {
        }
    }
}
