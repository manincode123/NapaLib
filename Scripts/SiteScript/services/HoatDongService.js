var HoatDongService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layChiTietHoatDong = function (hoatDongId, mapObject, themThongTinHdVaoDom) {
        $.ajax({
            url: "/api/HoatDong/ChiTiet/" + hoatDongId,
            type: "GET",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                mapObject(data);
                themThongTinHdVaoDom();
            }
        });
    }
    var layDiemDanhHoatDong = function (hoatDongId, themThongTinHdVaoDom) {
        $.ajax({
            url: "/api/HoatDong/LayThongTinTrangDiemDanh/" + hoatDongId,
            type: "GET",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themThongTinHdVaoDom(data);
            }
        });
    }

    var diemDanhHoatDong = function (thamGiaDto, updateAfterDiemDanhXoaDiemDanh) {
        $.ajax({
            url: "/api/HoatDong/DiemDanh",
            type: "POST",
            data: thamGiaDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterDiemDanhXoaDiemDanh();
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }
    var pheDuyetDangKi = function (thamGiaDto, updateAfterPheDuyetDangKi) {
        $.ajax({
            url: "/api/HoatDong/PheDuyetLuotDangKi",
            type: "POST",
            data: thamGiaDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterPheDuyetDangKi(thamGiaDto.sinhVienId);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }
    var xoaDangKi = function (thamGiaDto, updateAfterXoaDangKi) {
        $.ajax({
            url: "/api/HoatDong/XoaDangKi/" + thamGiaDto.hoatDongId + "/" + thamGiaDto.sinhVienId,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaDangKi(thamGiaDto.sinhVienId);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }

    var xoaLuotThamGia = function (thamGiaDto, updateAfterDiemDanhXoaDiemDanh) {
        $.ajax({
            url: "/api/HoatDong/XoaDiemDanh",
            data: thamGiaDto,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterDiemDanhXoaDiemDanh();
            }
        });
    }
    var saveChuongTrinh = function (chuongTrinhHoatDong, updateChuongTrinhHoatDongSauSave) {

        $.ajax({
            url: "/api/HoatDong/SaveChuongTrinhHoatDong",
            data: chuongTrinhHoatDong,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {

                updateChuongTrinhHoatDongSauSave(data);


            }
        });
    }

    var deleteChuongTrinh = function (id, updateChuongTrinhHoatDongSauDelete) {
        $.ajax({
            url: "/api/HoatDong/DeleteChuongTrinhHoatDong/" + id,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateChuongTrinhHoatDongSauDelete(id);
            }
        });
    }
    
    var dangKiHoatDong = function(hoatDongId,updateAfterDangKi_HuyDangKi) {
        $.ajax({
            url: "/api/HoatDong/DangKi/"+hoatDongId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateAfterDangKi_HuyDangKi(true);
            },
            error: function (xhr, status, error) {
                updateAfterDangKi_HuyDangKi(true, xhr.responseJSON.message);
            }
                                                       
        });
    }
    var huyDangKi= function(hoatDongId,updateAfterDangKi_HuyDangKi) {
        $.ajax({
            url: "/api/HoatDong/XoaDangKi/" + hoatDongId,
            type: "Delete",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateAfterDangKi_HuyDangKi(false);
            }
        });
    }
    var theoDoiHoatDong = function (hoatDongId, updateAfterTheoDoi_BoTheoDoi) {
        $.ajax({
            url: "/api/HoatDong/TheoDoi/" + hoatDongId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterTheoDoi_BoTheoDoi(true);
            }
        });
    }
    var huyTheoDoi = function (hoatDongId, updateAfterTheoDoi_BoTheoDoi) {
        $.ajax({
            url: "/api/HoatDong/XoaTheoDoi/" + hoatDongId,
            type: "Delete",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateAfterTheoDoi_BoTheoDoi(false);
            }
        });
    }

    return {
        layChiTietHoatDong: layChiTietHoatDong,
        saveChuongTrinh: saveChuongTrinh,
        deleteChuongTrinh: deleteChuongTrinh,
        layDiemDanhHoatDong: layDiemDanhHoatDong,
        diemDanhHoatDong: diemDanhHoatDong,
        pheDuyetDangKi: pheDuyetDangKi,
        xoaDangKi: xoaDangKi,
        xoaLuotThamGia: xoaLuotThamGia,
        dangKiHoatDong: dangKiHoatDong,
        huyDangKi: huyDangKi,
        theoDoiHoatDong: theoDoiHoatDong,
        huyTheoDoi: huyTheoDoi
    }

}();

var QuanLyHoatDongService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var quanLyHoatDong = function (trangQuanLyHdMap, updateTrang) {
        $.ajax({
            url: "/api/HoatDong/QuanLyChung",
            Type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                trangQuanLyHdMap(data);
                updateTrang();
            }
        });
    }

    var loadSaveHoatDongHtml = function (insertHtmlToSaveHoatDongModal) {
        $.ajax({
            url: "/HoatDong/SaveHoatDong",
            async: false,
            success: function (data) {
                insertHtmlToSaveHoatDongModal(data);
            }
        });
    }
    
    var saveHoatDong = function (hoatDong,reloadPage) {
        $.ajax({
            url: "/api/HoatDong/SaveHoatDong",
            type: "POST",
            data: hoatDong,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                if (hoatDong.id == 0) {
                    alert("Đã thêm hoạt động.");
                } else {
                    alert("Đã thay đổi thông tin hoạt động.");
                }
                
                if (reloadPage != null) reloadPage();
            }
        });
    }

    var ketThucHoatDong = function (hoatDongId, reloadPage) {
        $.ajax({
            url: "/api/HoatDong/KetThucHoatDong/" + hoatDongId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                alert("Đã kết thúc hoạt động.");
                if (reloadPage != null) reloadPage();
            }
        });
    }
    var moLaiHoatDong = function (hoatDongId, reloadPage) {
        $.ajax({
            url: "/api/HoatDong/MoLaiHoatDong/" + hoatDongId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                alert("Hoạt động đã được mở lại.");
                if (reloadPage != null) reloadPage();
            }
        });
    }
    var huyHoatDong = function (hoatDongId, reloadPage) {
        $.ajax({
            url: "/api/HoatDong/HuyHoatDong/" + hoatDongId,
            type: "Delete",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                alert("Đã hủy hoạt động.");
                if (reloadPage != null) reloadPage();
            }
        });
    }
    var pheDuyetHoatDong = function (hoatDongId, updatePage) {
        $.ajax({
            url: "/api/HoatDong/PheDuyetHoatDong/" + hoatDongId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updatePage(false);
            }
        });
    }
    var huyHoatDongTrangPheDuyet = function (hoatDongId, updatePage) {
        $.ajax({
            url: "/api/HoatDong/HuyHoatDong/" + hoatDongId,
            type: "Delete",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updatePage(true);
            },
            error: function(xhr) {
                updatePage(true,xhr);
            }
        });
    }
    var khoiPhucHoatDong = function (hoatDongId, reloadPage) {
        $.ajax({
            url: "/api/HoatDong/KhoiPhucHoatDong/" + hoatDongId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                alert("Đã khôi phục lại hoạt động.");
                if (reloadPage != null) reloadPage();
            }
        });
    }

    var getSelectListData = function (initSelectList) {
        $.ajax({
            type: "GET",
            url: '/api/HoatDong/LayDanhSachDonViToChuc',
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                initSelectList(data.danhSachLop, data.danhSachDonVi);
            }
        });
    }
    return {
        quanLyHoatDong: quanLyHoatDong,
        loadSaveHoatDongHtml: loadSaveHoatDongHtml,
        getSelectListData: getSelectListData,
        saveHoatDong: saveHoatDong,
        ketThucHoatDong: ketThucHoatDong,
        huyHoatDong: huyHoatDong,
        moLaiHoatDong: moLaiHoatDong,
        khoiPhucHoatDong: khoiPhucHoatDong,
        huyHoatDongTrangPheDuyet: huyHoatDongTrangPheDuyet,
        pheDuyetHoatDong: pheDuyetHoatDong
    }
}();

var TrangChuHoatDongService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    //Trang chủ hoạt động
    var layHoatDongTrangHoatDongChung = function (themHoatDongVaoDom) {
        $.ajax({
            url: "/api/HoatDong/TrangChu",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themHoatDongVaoDom(data);
            }
        });
    }
    //Trang hoạt động cá nhân sinh viên
    var layHoatDongTrangCaNhan = function (themHoatDongVaoDom) {
        $.ajax({
            url: "/api/HoatDong/TrangCaNhan",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themHoatDongVaoDom(data);
            }
        });
    }
    var layThongBaoHoatDong = function (taoThongBao) {
        $.ajax({
            url: "/api/ThongBao/HoatDong",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                taoThongBao(data);
            }
        });
    }
    var layThemThongBao = function(recordStart,themThongBao) {
        $.ajax({
            url: "/api/ThongBao/ThongBaoHoatDongTiepTheo/" + recordStart,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themThongBao(data);
            }
        });
    }
    var danhDauThongBaoDaDoc = function () {
        $.ajax({
            url: "/api/ThongBao/DanhDauThongBaoHoatDongDaDoc",
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
            }
        });
    }
    //Trang thống kê hoạt động cá nhân sinh viên
    var layHoatDongSinhVienThamGia = function(sinhVienId,reloadPage) {
        $.ajax({
            url: "/api/HoatDong/HoatDongSinhVienThamGia/" + sinhVienId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                reloadPage(data);
            }
        });
    }
    var layThongKeHoatDongSinhVien = function (sinhVienId, namHocLay, themDuLieuVaoBieuDo) {
        $.ajax({
            url: "/api/HoatDong/ThongKeHoatDongSinhVien/" + sinhVienId + "/" + namHocLay,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themDuLieuVaoBieuDo(data);
            }
        });
    }
    //Trang thống kê hoạt động học viện
    var layThongKeHoatDongChung = function (namHocLay, updatePage) {
        $.ajax({
            url: "/api/HoatDong/ThongKeChung/"+ namHocLay,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updatePage(data);                      
            }
        });
    }
    //Trang hoạt động theo đơn vị
    var layDanhSachDonVi = function (themDonViVaoDom) {
        $.ajax({
            url: "/api/DonVi/DanhSachDonVi",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themDonViVaoDom(data);
            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });


    }

    return {
        layHoatDongTrangHoatDongChung: layHoatDongTrangHoatDongChung,
        layHoatDongTrangCaNhan: layHoatDongTrangCaNhan,
        layThongBaoHoatDong: layThongBaoHoatDong,
        layThemThongBao : layThemThongBao,
        danhDauThongBaoDaDoc: danhDauThongBaoDaDoc,
        layHoatDongSinhVienThamGia: layHoatDongSinhVienThamGia,
        layThongKeHoatDongSinhVien: layThongKeHoatDongSinhVien,
        layThongKeHoatDongChung: layThongKeHoatDongChung,
        layDanhSachDonVi : layDanhSachDonVi
    }
}();
