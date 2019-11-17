var QuanLyChungDonViService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var getDonViData = function (donViId, updateInput) {
        $.ajax({
            url: "/api/DonVi/ThongTin/"+donViId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateInput(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }
    var uploadAnhBiaDonVi = function (dataAnhBia, bindAnhBia) {
        $.ajax({
            type: "POST",
            url: "/api/file/UploadAnhBiaDonVi",
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
    var saveDonVi = function (donViDto, updateAfterSaveDonVi) {
            $.ajax({
                url: "/api/DonVi/SaveDonVi",
                type: "POST",
                headers: { __RequestVerificationToken: csrfToken },
                data: donViDto,
                success: function () {
                    if (donViDto.id == 0) {
                        updateAfterSaveDonVi(true);  //Thêm đơn vị
                    } else {
                        updateAfterSaveDonVi(false); //Chỉnh sửa thông tin đơn vị
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    updateAfterSaveDonVi(true,xhr);
                }
            });
    }
    var xoaDonVi = function(donViMuonXoaId,updateAfterXoaDonVi) {
        $.ajax({
            url: "/api/DonVi/XoaDonVi/" + donViMuonXoaId,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaDonVi();
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterXoaDonVi(xhr);
            }
        });

    }
    //Trang thông tin đơn vị
    var layBaiVietDonVi = function(donViId,themBaiViet) {
        $.ajax({
            url: "/api/BaiViet/DonVi/" + donViId+"/"+0,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });


    }
    var layHoatDongDonVi = function (layHoatDongDto, themHoatDong) {
        $.ajax({
            url: "/api/DonVi/HoatDongToChuc/" + layHoatDongDto.donViId,
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
    var dangKiThanhVien = function (dangKiDto, updateAfterDangKi_HuyDangKi) {
        $.ajax({
            url: "/api/DonVi/DangKiThanhVien",
            data: dangKiDto,
            type: "POST",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterDangKi_HuyDangKi(true);
            },
            error: function (xhr, status, error) {
                updateAfterDangKi_HuyDangKi(true, xhr);
            }

        });
    }
    var huyDangKiThanhVien = function (donViId, updateAfterDangKi_HuyDangKi) {
        $.ajax({
            url: "/api/DonVi/HuyDangKiThanhVien/" + donViId,
            type: "DELETE",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateAfterDangKi_HuyDangKi(false);
            }
        });
    }
    //Trang danh sách đơn vị
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
        getDonViData: getDonViData,
        uploadAnhBiaDonVi: uploadAnhBiaDonVi,
        saveDonVi: saveDonVi,
        xoaDonVi: xoaDonVi,
        layBaiVietDonVi: layBaiVietDonVi,
        layHoatDongDonVi : layHoatDongDonVi,
        dangKiThanhVien : dangKiThanhVien,
        huyDangKiThanhVien: huyDangKiThanhVien,
        layDanhSachDonVi: layDanhSachDonVi
    }
}();

var QuanLyThanhVienDonViService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var totNghiepThanhVien = function (dataThanhVien, updateAfterTotNghiep) {
        $.ajax({
            url: "/api/DonVi/TotNghiepThanhVien",
            type: "POST",
            data: dataThanhVien,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                    updateAfterTotNghiep();  //Thêm đơn vị
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterTotNghiep(xhr);
            }
        });

    }
    var thayDoiThanhVien = function (dataThanhVien, updateAfterThayDoi) {
        $.ajax({
            url: "/api/DonVi/ThayDoiThanhVien",
            type: "PUT",
            data: dataThanhVien,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterThayDoi();  //Thêm đơn vị
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterThayDoi(xhr);
            }
        });
    }
    var xoaThanhVien = function (dataThanhVien, updateAfterXoa) {
        $.ajax({
            url: "/api/DonVi/XoaThanhVien",
            type: "DELETE",
            data: dataThanhVien,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoa();  //Thêm đơn vị
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterXoa(xhr);
            }
        });
    }
    var pheDuyetDangKi= function(dataThanhVien, updateAfterPheDuyetDangKi) {
        $.ajax({
            url: "/api/DonVi/PheDuyetDangKiThanhVien",
            type: "POST",
            data: dataThanhVien,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterPheDuyetDangKi();  //Thêm đơn vị
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterPheDuyetDangKi(xhr);
            }
        });

    }
    var themThanhVien= function(dataThanhVien, updateAfterThemThanhVien) {
        $.ajax({
            url: "/api/DonVi/ThemThanhVien",
            type: "POST",
            data: dataThanhVien,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterThemThanhVien();  //Thêm đơn vị
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterThemThanhVien(xhr);
            }
        });
    }
return {
    totNghiepThanhVien: totNghiepThanhVien,
    thayDoiThanhVien: thayDoiThanhVien,
    xoaThanhVien: xoaThanhVien,
    pheDuyetDangKi: pheDuyetDangKi,
    themThanhVien: themThanhVien
}
}();

var QuanLyChucVuDonViService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layDanhSachThanhVien = function (donViId, bindDanhSachThanhVien) {
        $.ajax({
            url: "/api/DonVi/ThanhVien/" + donViId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                bindDanhSachThanhVien(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }
    var saveChucVu = function (chucVuDonViDto, updateAfterSaveChucVu) {
        $.ajax({
            url: "/api/DonVi/SaveChucVu",
            data:  chucVuDonViDto,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterSaveChucVu(chucVuDonViDto.themChucVu);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                updateAfterSaveChucVu(chucVuDonViDto.themChucVu,xhr);

            }
        });
    }
    var xoaChucVu = function (chucVuDonViDto, updateAfterXoaChucVu) {
        $.ajax({
            url: "/api/DonVi/XoaChucVu",
            data:  chucVuDonViDto,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaChucVu();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                updateAfterXoaChucVu(xhr);

            }
        });
    }
    return{
        layDanhSachThanhVien: layDanhSachThanhVien,
        saveChucVu: saveChucVu,
        xoaChucVu: xoaChucVu
    }
}();

var QuanLyHoatDongDonViService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layThongTinHoatDong = function (donViId, themHoatDongVaoDom) {
        $.ajax({
            url: "/api/DonVi/HoatDongDangToChuc/" + donViId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (danhSachHoatDong) {
                themHoatDongVaoDom(danhSachHoatDong);
            }
        });

    }
    var layThongKeHoatDong = function (donViId, namHocLay, themDuLieuVaoBieuDo) {
        $.ajax({
            url: "/api/DonVi/ThongKeHoatDong/" + donViId + "/" + namHocLay,
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
            data: huyToChucDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                reloadPage();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                reloadPage(xhr);

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
    return{
        layThongTinHoatDong: layThongTinHoatDong,
        layThongKeHoatDong: layThongKeHoatDong,
        huyToChucHoatDong: huyToChucHoatDong,
        huyHoatDong: huyHoatDong
    }
}();
