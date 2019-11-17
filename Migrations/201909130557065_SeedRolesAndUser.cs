namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedRolesAndUser : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'SuperAdmin')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Admin')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'QuanLyLop')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4', N'QuanLySinhVien')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5', N'QuanLyHoiVien')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'6', N'QuanLyDoanVien')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'7', N'QuanLyBaiViet')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8', N'QuanLyHoatDong')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'9', N'DiemDanhHoatDong')
            INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'10', N'QuanLyDonVi')
            ");
            Sql(@"
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [SinhVienId], [TenNguoiDung]) VALUES (N'42163794-5c20-4a1a-a5c7-4c71d724bd04', N'saadmin@hsvnapa.com', 0, N'AL6wg1m52i08+qrk5BoY428vuzFvRHHEnEJ+9RngafJnODFPYEQQ1KwXganz7gHFqQ==', N'1977b3db-485b-4519-a815-890bbbda368b', NULL, 0, 0, NULL, 1, 0, N'saadmin@hsvnapa.com', 0, N'SuperAdmin')
            INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [SinhVienId], [TenNguoiDung]) VALUES (N'1a4a290f-5d80-45b9-a388-7628842b5f94', N'admin@hsvnapa.com', 0, N'AHc5zaCV4CWbTDQAiCv+rDfD6qvT2oVq8XbNr+Ad+exvZ6GCr7ooLU0BullUP7lfqQ==', N'9a7f8d53-d5af-4372-9fd6-2685639482e8', NULL, 0, 0, NULL, 1, 0, N'admin@hsvnapa.com', 0, N'Admin')
            ");
            Sql(@"
            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'42163794-5c20-4a1a-a5c7-4c71d724bd04', N'1')
            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'42163794-5c20-4a1a-a5c7-4c71d724bd04', N'2')
            INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1a4a290f-5d80-45b9-a388-7628842b5f94', N'2')
            ");

        }
        
        public override void Down()
        {
        }
    }
}
