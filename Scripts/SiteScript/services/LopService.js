var QuanLyKhoaHocService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layDanhSachLopService = function (khoaHocId, reloadDanhSachLop) {
        $.ajax({
            url: "/api/KhoaHoc/LayDanhSachLop/" + khoaHocId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                reloadDanhSachLop(data);
            }
        });
    }
    var layKhoaHocData = function(khoaHocId,updateInput) {
        $.ajax({
            url: "/api/KhoaHoc/LayKhoaHocDataForSave/" + khoaHocId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateInput(data);
            }
        });
    }
    var saveKhoaHoc = function (khoaHocDto, updateAfterSaveKhoaHoc) {
        $.ajax({
            url: "/api/KhoaHoc/SaveKhoaHoc/",
            type: "POST",
            data: khoaHocDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterSaveKhoaHoc();
            },
            error: function(xhr) {
                updateAfterSaveKhoaHoc(xhr);
            }
        });
    } 
    return{
        layDanhSachLopService: layDanhSachLopService,
        layKhoaHocData: layKhoaHocData,
        saveKhoaHoc: saveKhoaHoc
    }
}();

var QuanLyChungLopService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var saveLop = function (lopDto, updateTrangSauKhiSave) {
        $.ajax({
            url: "/api/Lop/SaveLop",
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            data: lopDto,
            success: function () {
                updateTrangSauKhiSave();
            },
            error : function(xhr) {
                updateTrangSauKhiSave(xhr);
            }
        });
    }
    var uploadAnhBiaLop = function (dataAnhBia, bindAnhBia) {
        $.ajax({
            type: "POST",
            url: "/api/file/UploadAnhBiaLop",
            headers: { __RequestVerificationToken: csrfToken },
            data: dataAnhBia,
            async: false,
            contentType: false,
            processData: false,
            success: function (data) {
                bindAnhBia(data);
            }
        });
    }

    var getSaveLopModal = function (insertSaveLopModalHtml) {
        $.ajax({
            url: "/Lop/SaveLopModal",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            async: false,
            success: function (data) {
                insertSaveLopModalHtml(data);
            }
        });
    }
    var layDanhSachKhoa = function () {
        var selectList = [];
        $.ajax({
            type: "GET",
            url: '/api/KhoaHoc/layDanhSachKhoa',
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {

                data.forEach(function (khoa) {
                    //Khởi tạo object mới
                    var object = {};
                    //Gán value
                    object.id = khoa.id;
                    //Gán text
                    object.text = khoa.tenKhoa + " (" + moment(khoa.namBatDau).format("YYYY")
                        + "-" + moment(khoa.namKetThuc).format("YYYY") + ")";
                    //Add object vào list
                    selectList.push(object);
                });
            }
        });
        return selectList;
    }
    var getLopDataForSave = function (lopId, updateInput) {
        $.ajax({
            url: "/api/Lop/GetLopDataForSave/" + lopId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            async: false,
            success: function (data) {
                updateInput(data);
            }
        });

    }
    var layThongTinLop = function (lopId, bindData) {
        $.ajax({
            url: "/api/lop/chitiet/" + lopId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            async: false,
            success: function (result) {
                bindData(result);
            }
        });
    }
    //Trang chi tiết, quản lý lớp cụ thể
    var layBaiVietLop = function (lopId, themBaiViet) {
        $.ajax({
            url: "/api/BaiViet/LayBaiVietLop/" + lopId + "/" + 0,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });


    }
    var layHoatDongLop = function (layHoatDongDto, themHoatDong) {
        $.ajax({
            url: "/api/Lop/HoatDongToChuc/" + layHoatDongDto.lopId,
            type: "POST",
            data: layHoatDongDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themHoatDong(data.data);
            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });


    }
    var layDanhSachChucVu = function (lopId, appendChucVuLop) {
        $.ajax({
            url: "/api/Lop/LayDanhSachChucVu/" + lopId,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            type: "GET",
            success: function (data) {
                appendChucVuLop(data);
            }
        });
    }

    return {
        saveLop: saveLop,
        getSaveLopModal: getSaveLopModal,
        layDanhSachKhoa: layDanhSachKhoa,
        getLopData: getLopDataForSave,
        uploadAnhBiaLop: uploadAnhBiaLop,
        layThongTinLop: layThongTinLop,
        layBaiVietLop: layBaiVietLop,
        layHoatDongLop: layHoatDongLop,
        layDanhSachChucVu: layDanhSachChucVu
}
}();

var QuanLyHoatDongLopService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layThongTinHoatDongLop = function (lopId, themHoatDongVaoDom) {
        $.ajax({
            url: "/api/lop/HoatDongDangToChuc/" + lopId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            async: false,
            success: function (result) {
                themHoatDongVaoDom(result);
            }
        });
    }
    var layHoatDongLopToChuc = function (lopId, updateHoatDongLopToChuc) {
        $.ajax({
            url: "/api/Lop/HoatDongLopToChuc/" + lopId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (result) {
                updateHoatDongLopToChuc(result);
            }
        });
    }
    var layLuotThamGiaHoatDongCuaLop = function (lopId,namHocLay, updateHoatDongVaLuotThamGia) {
        $.ajax({
            url: "/api/Lop/LuotThamGiaHd/" + lopId+"/"+namHocLay,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (result) {
                updateHoatDongVaLuotThamGia(result);
            }
        });
    }
    var layThongKeHoatDong = function (lopId, namHocLay, themDuLieuVaoBieuDo) {
        $.ajax({
            url: "/api/Lop/ThongKeHoatDong/" + lopId + "/" + namHocLay,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (result) {
                themDuLieuVaoBieuDo(result);
            }
        });
    }
    var huyToChucHoatDong = function (huyToChucDto, reloadPage) {
        $.ajax({
            url: "/api/HoatDong/HuyThamGiaToChucHoatDong",
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            data: huyToChucDto,
            success: function (result) {
                reloadPage();
            }
        });
    }
    var huyHoatDong = function (hoatDongId, updatePage) {
        $.ajax({
            url: "/api/HoatDong/HuyHoatDong/" + hoatDongId,
            type: "Delete",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updatePage(true);
            },
            error: function (xhr) {
                updatePage(true, xhr);
            }
        });
    }

    return {
        layThongTinHoatDongLop: layThongTinHoatDongLop,
        layHoatDongLopToChuc  : layHoatDongLopToChuc ,
        layLuotThamGiaHoatDongCuaLop: layLuotThamGiaHoatDongCuaLop,
        layThongKeHoatDong: layThongKeHoatDong ,
        huyToChucHoatDong : huyToChucHoatDong,
        huyHoatDong: huyHoatDong

    }

}();

var QuanLySinhVienLopService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layDanhSachSinhVienLop = function (lopId, updateDom) {
        $.ajax({
            url: "/api/Lop/LayDanhSachSinhVien/" + lopId,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateDom(data);
            }
        });
    }
    var themSinhVien = function (sinhVienDto, updatePageAfterThemSinhVien) {
        $.ajax({
            url: "/api/Lop/ThemSinhVien",
            data: sinhVienDto,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            async: false,
            success: function (data) {
                updatePageAfterThemSinhVien();
            }
        });
    }
    var xoaSinhVien = function (sinhVienDto, updatePageAfterXoaSinhVien) {
        $.ajax({
            url: "/api/Lop/xoaSinhVien",
            data: sinhVienDto,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            async: false,
            success: function (data) {
                updatePageAfterXoaSinhVien();
            }
        });

    }

    return{
        themSinhVien: themSinhVien,
        xoaSinhVien : xoaSinhVien,
        layDanhSachSinhVienLop: layDanhSachSinhVienLop
    }
}();

var QuanLyChucVuLopService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layDanhSachSinhVien = function (lopId, bindDanhSachSinhVien) {
        $.ajax({
            url: "/api/Lop/LayDanhSachSinhVien/" + lopId,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            type: "GET",
            success: function (data) {
                bindDanhSachSinhVien(data);
            }
        });
    }
    var layDanhSachChucVu = function (lopId, appendChucVuLop) {
        $.ajax({
            url: "/api/Lop/LayDanhSachChucVu/" + lopId,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            type: "GET",
            success: function (data) {
                appendChucVuLop(data);
            }
        });
    }
    var chinhSuaChucVu = function (chinhSuaChucVuDto, updatePageSauKhiChinhSua) {
        $.ajax({
            url: "/api/Lop/ChinhSuaChucVu",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            type: "POST",
            data: chinhSuaChucVuDto,
            success: function () {
                layDanhSachChucVu(chinhSuaChucVuDto.lopId, updatePageSauKhiChinhSua);
                alert("Đã thay đổi chức vụ.");
            }
        });
    }
    var xoaChucVu = function (xoaChucVuDto, updatePageAfterXoaChucVu) {
        $.ajax({
            url: "/api/Lop/XoaChucVu",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            type: "DELETE",
            data: xoaChucVuDto,
            success: function () {
                updatePageAfterXoaChucVu();
            }
        });
    }

    return {
        layDanhSachSinhVien: layDanhSachSinhVien,
        layDanhSachChucVu : layDanhSachChucVu,
        chinhSuaChucVu: chinhSuaChucVu,
        xoaChucVu: xoaChucVu
    }
}();
