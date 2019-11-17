namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaoChucVuTrongLop : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT [dbo].[DanhSachChucVu] ON
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (1, N'Lớp trưởng', 1)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (2, N'Lớp phó', 1)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (3, N'Thủ quỹ', 1)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (4, N'Bí thư', 2)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (5, N'Phó bí thư', 2)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (7, N'Ủy viên Ban chấp hành Chi Đoàn', 2)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (8, N'Chi hội trưởng', 3)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (9, N'Chi hội phó', 3)
                INSERT INTO [dbo].[DanhSachChucVu] ([Id], [TenChucVu], [LoaiChucVu]) VALUES (10, N'Ủy viên Ban chấp hành Chi Hội', 3)
                SET IDENTITY_INSERT [dbo].[DanhSachChucVu] OFF
                ");
        }
        
        public override void Down()
        {
        }
    }
}
