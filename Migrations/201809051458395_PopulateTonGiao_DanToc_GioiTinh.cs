namespace NAPASTUDENT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateTonGiao_DanToc_GioiTinh : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT [dbo].[DanhSachDanToc] ON
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (1, N'Kinh', N'Việt')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (2, N'Tày', N'Thổ, Ngạn, Phén, Thù Lao, Pa Dí, Tày Khao')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (3, N'Thái', N'Tày Đăm, Tày Mười, Tày Thanh, Mán Thanh, Hàng Bông, Tày Mường, Pa Thay, Thổ Đà Bắc')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (4, N'Hoa', N'Hán, Triều Châu, Phúc Kiến, Quảng Đông, Hải Nam, Hạ, Xạ Phạng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (5, N'Khơ-me', N'Cur, Cul, Cu, Thổ, Việt gốc Miên, Krôm')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (6, N'Mường', N'Mol, Mual, Mọi, Mọi Bi, Ao Tá, Ậu Tá')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (7, N'Nùng', N'Xuồng, Giang, Nùng An, Phàn Sinh, Nùng Cháo, Nùng Lòi, Quý Rim, Khèn Lài')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (8, N'HMông', N'Mèo, Hoa, Mèo Xanh, Mèo Đỏ, Mèo Đen, Ná Mẻo, Mán Trắng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (9, N'Dao', N'Mán, Động, Trại, Xá, Dìu, Miên, Kiềm, Miền, Quần Trắng, Dao Đỏ, Quần Chẹt, Lô Giang, Dao Tiền, Thanh Y, Lan Tẻn, Đại Bản, Tiểu Bản, Cóc Ngáng, Cóc Mùn, Sơn Đầu')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (10, N'Gia-rai', N'Giơ-rai, Tơ-buăn, Chơ-rai, Hơ-bau, Hđrung, Chor')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (11, N'Ngái', N'Xín, Lê, Đản, Khách Gia')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (12, N'Ê-đê', N'Ra-đê, Đê, Kpạ, A-đham, Krung, Ktul, Đliê Ruê, Blô, Epan, Mđhur, Bih')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (13, N'Ba na', N'Giơ-lar. Tơ-lô, Giơ-lâng, Y-lăng, Rơ-ngao, Krem, Roh, ConKđe, A-la Công, Kpăng Công, Bơ-nâm')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (14, N'Xơ-Đăng', N'Xơ-teng, Hđang, Tơ-đra, Mơ-nâm, Ha-lăng, Ca-dong, Kmrâng, ConLan, Bri-la, Tang')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (15, N'Sán Chay', N'Cao Lan, Sán Chỉ, Mán Cao Lan, Hờn Bạn, Sơn Tử')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (16, N'Cơ-ho', N'Xrê, Nốp, Tu-lốp, Cơ-don, Chil, Lat, Lach, Trinh')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (17, N'Chăm', N'Chàm, Chiêm Thành, Hroi')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (18, N'Sán Dìu', N'Sán Dẻo, Trại, Trại Đất, Mán, Quần Cộc')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (19, N'Hrê', N'Chăm Rê, Chom, Krẹ Luỹ')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (20, N'Mnông', N'Pnông, Nông, Pré, Bu-đâng, ĐiPri, Biat, Gar, Rơ-lam, Chil')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (21, N'Ra-glai', N'Ra-clây, Rai, Noang, La-oang')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (22, N'Xtiêng', N'Xa-điêng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (23, N'Bru-Vân Kiều', N'Bru, Vân Kiều, Măng Coong, Tri Khùa')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (24, N'Thổ', N'Kẹo, Mọn, Cuối, Họ, Đan Lai, Ly Hà, Tày Pọng, Con Kha, Xá Lá Vàng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (25, N'Giáy', N'Nhắng, Dẩng, Pầu Thìn Nu Nà, Cùi Chu, Xa')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (26, N'Cơ-tu', N'Ca-tu, Cao, Hạ, Phương, Ca-tang')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (27, N'Gié Triêng', N'Đgiéh, Tareb, Giang Rẫy Pin, Triêng, Treng, Ta-riêng, Ve, Veh, La-ve, Ca-tang')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (28, N'Mạ', N'Châu Mạ, Mạ Ngăn, Mạ Xóp, Mạ Tô, Mạ Krung')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (29, N'Khơ-mú', N'Xá Cẩu, Mứn Xen, Pu Thênh, Tềnh, Tày Hay')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (30, N'Co', N'Cor, Col, Cùa, Trầu')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (31, N'Tà-ôi', N'Tôi-ôi, Pa-co, Pa-hi, Ba-hi')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (32, N'Chơ-ro', N'Dơ-ro, Châu-ro')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (33, N'Kháng', N'Xá Khao, Xá Súa, Xá Dón, Xá Dẩng, Xá Hốc, Xá Ái, Xá Bung, Quảng Lâm')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (34, N'Xinh-mun', N'Puộc, Pụa')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (35, N'Hà Nhì', N'U Ni, Xá U Ni')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (36, N'Chu ru', N'Chơ-ru, Chu')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (37, N'Lào', N'Là Bốc, Lào Nọi')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (38, N'La Chí', N'Cù Tê, La Quả')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (39, N'La Ha', N'Xá Khao, Khlá Phlạo')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (40, N'Phù Lá', N'Bồ Khô Pạ, Mu Di Pạ Xá, Phó, Phổ, Va Xơ')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (41, N'La Hủ', N'Lao, Pu Đang, Khù Xung, Cò Xung, Khả Quy')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (42, N'Lự', N'Lừ, Nhuồn, Duôn')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (43, N'Lô Lô', N'Mun Di')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (44, N'Chứt', N'Sách, Máy, Rục, Mã-liêng, A-rem, Tu vang, Pa-leng, Xơ-Lang, Tơ-hung, Chà-củi, Tắc-củi, U-mo, Xá Lá Vàng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (45, N'Mảng', N'Mảng Ư, Xá Lá Vàng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (46, N'Pà Thẻn', N'Pà Hưng, Tống')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (47, N'Co Lao', N'')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (48, N'Cống', N'Xắm Khống, Mấng Nhé, Xá Xeng')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (49, N'Bố Y', N'Chủng Chá, Trọng Gia, Tu Di, Tu Din')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (50, N'Si La', N'Cù Dề Xừ, Khả pẻ')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (51, N'Pu Péo', N'Ka Pèo, Pen Ti Lô Lô')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (52, N'Brâu', N'Brao')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (53, N'Ơ Đu', N'Tày Hạt')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (54, N'Rơ măm', N'')
                    INSERT INTO [dbo].[DanhSachDanToc] ([Id], [TenDanToc], [TenGoiKhac]) VALUES (55, N'Người nước ngoài', N'')
                  SET IDENTITY_INSERT [dbo].[DanhSachDanToc] OFF

                  SET IDENTITY_INSERT [dbo].[DanhSachGioiTinh] ON
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (1, N'Nữ')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (2, N'Nam')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (3, N'Lưỡng tính')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (4, N'Không có giới tính')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (5, N'Không xác định')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (6, N'Nam chuyển giới nữ')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (7, N'Nữ chuyển giới nam')
                    INSERT INTO [dbo].[DanhSachGioiTinh] ([Id], [TenGioiTinh]) VALUES (8, N'Khác')
                 SET IDENTITY_INSE  RT [dbo].[DanhSachGioiTinh] OFF

                 SET IDENTITY_INSERT [dbo].[DanhSachTonGiao] ON
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (1, N'Không tôn giáo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (2, N'Phật giáo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (3, N'Công giáo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (4, N'Phật giáo Hoà Hảo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (5, N'Hồi giáo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (6, N'Cao Đài')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (7, N'Minh sư đạo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (8, N'Minh Lý đạo')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (9, N'Tin Lành')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (10, N'Tịnh độ cư sĩ Phật hội Việt Nam')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (11, N'Đạo Tứ ấn hiếu nghĩa')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (12, N'Bửu sơn Kỳ hương')
                    INSERT INTO [dbo].[DanhSachTonGiao] ([Id], [TenTonGiao]) VALUES (13, N'Ba Ha''i')
                SET IDENTITY_INSERT [dbo].[DanhSachTonGiao] OFF


");
        }
        
        public override void Down()
        {
        }
    }
}
