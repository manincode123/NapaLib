var QuanLySinhVienService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();

    var getDuLieuChoFormSaveSinhVien = function (populateForm) {
        $.ajax({
            url: "/api/SinhVien/LayDuLieuChoFormSaveSinhVien",
            headers: { __RequestVerificationToken: csrfToken },
            type: "GET",
            success: function (data) {
                populateForm(data);
            }
        });
    }
    var getSinhVienData = function (sinhVienId, updateInput) {
        $.ajax({
            url: "/api/SinhVien/GetDataForSave/" + sinhVienId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateInput(data);
            }
        });
    }
    var saveSinhVien = function (sinhVienDto, updateAfterSaveSinhVien) {
        if (sinhVienDto.id == 0) {  //Nếu là thêm sinh viên (id = 0)
            $.ajax({
                url: "/api/SinhVien/ThemSinhVien",
                type: "POST",
                headers: { __RequestVerificationToken: csrfToken },
                data: sinhVienDto,
                success: function(data) {
                    updateAfterSaveSinhVien();
                },
                error: function (xhr, textStatus, errorThrown) {
                    updateAfterSaveSinhVien(xhr);
                }
            });
        } else {   //Nếu là chỉnh sửa sinh viên (id != 0)
            $.ajax({
                url: "/api/SinhVien/ChinhSuaSinhVien",
                type: "PUT",
                headers: { __RequestVerificationToken: csrfToken },
                data: sinhVienDto,
                success: function(data) {
                    updateAfterSaveSinhVien();
                }
            });
        }
        
    }
    var uploadAnhDaiDien = function (dataAnhDaiDien, bindAnhDaiDien) {
            $.ajax({
                type: "POST",
                url: "/api/file/UploadAnhBiaSv",
                headers: { __RequestVerificationToken: csrfToken },
                data: dataAnhDaiDien,
                async: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    bindAnhDaiDien(data);
                }
            });
    }
    //Trang thêm lô sinh viên
    var taoBatchSinhVien = function (formData,traKetQuaTaoSinhVien) {
        $.ajax({
            url: "/api/SinhVien/TaoBatchSinhVien",
            headers: { __RequestVerificationToken: csrfToken },
            cache: false,
            processData: false,
            contentType: false,
            data: formData,
            type: "POST",
            success: function (data) {
                traKetQuaTaoSinhVien(data);
            }
        });
    }
    var getDuLieuMaHoa = function (taoBangDuLieuMaHoa) {
        $.ajax({
            url: "/api/SinhVien/LayDuLieuChoFormSaveSinhVien",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                taoBangDuLieuMaHoa(data);
            }
        });
    }

    return {
        getDuLieuChoFormSaveSinhVien: getDuLieuChoFormSaveSinhVien,
        saveSinhVien: saveSinhVien,
        uploadAnhDaiDien: uploadAnhDaiDien,
        taoBatchSinhVien: taoBatchSinhVien,
        getDuLieuMaHoa: getDuLieuMaHoa,
        getSinhVienData: getSinhVienData
}
}();

var QuanLyHoiVienService = function() {
    var dangKiHoiVien = function (dangKiHoiVienDto, updateAfterDangKi) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DangKiHoiVien",
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            data: dangKiHoiVienDto,
            success: function () {
                updateAfterDangKi();
            }
        });
    }
    var dangKiHoiVienHangLoat = function (formData, traKetQuaDangKiHoiVien) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: '/api/SinhVien/DangKiHoiVienHangLoat',
            cache: false,
            processData: false,
            contentType: false,
            data: formData,
            headers: { __RequestVerificationToken: csrfToken },
            type: 'POST',
            success: function (data) {
                traKetQuaDangKiHoiVien(data);
            }
        });
    }
    var xoaDangKiHoiVien = function (mssv, updateAfterXoaDangKi) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/XoaDangKiHoiVien/" + mssv,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaDangKi();
            }
        });
    }

    return{
        dangKiHoiVien: dangKiHoiVien,
        dangKiHoiVienHangLoat: dangKiHoiVienHangLoat,
        xoaDangKiHoiVien: xoaDangKiHoiVien
}
}();

var QuanLyDoanVienService = function() {
    var dangKiDoanVien = function (dangKiDoanVienDto, updateAfterDangKi) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DangKiDoanVien",
            type: "POST",
            data: dangKiDoanVienDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterDangKi();
            }
        });
    }
    var dangKiDoanVienHangLoat = function (formData, traKetQuaDangKiDoanVien) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DangKiDoanVienHangLoat",
            cache: false,
            processData: false,
            contentType: false,
            data: formData,
            headers: { __RequestVerificationToken: csrfToken },
            type: "POST",
            success: function (data) {
                traKetQuaDangKiDoanVien(data);
            }
        });
    }
    var xoaDangKiDoanVien = function (mssv, updateAfterXoaDangKi) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/XoaDangKiDoanVien/" + mssv,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaDangKi();
            }
        });
    }

return {
    dangKiDoanVien: dangKiDoanVien,
    dangKiDoanVienHangLoat: dangKiDoanVienHangLoat,
    xoaDangKiDoanVien: xoaDangKiDoanVien
}
}();

var QuanLyTrangCaNhanService = function () {
    var getSinhVienData = function (sinhVienId, updatePage) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/ChiTiet/" + sinhVienId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updatePage(data);
            }
        });
    }
    var saveSinhVien = function (sinhVienDto, updateAfterSaveSinhVien) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
                url: "/api/SinhVien/ChinhSuaSinhVien",
                type: "PUT",
                data: sinhVienDto,
                headers: { __RequestVerificationToken: csrfToken },
                success: function () {
                    updateAfterSaveSinhVien();
                }
            });
    }
    var layDanhSachTinh = function(initDanhSachTinh) {
        $.ajax({
            url: "/api/SinhVien/LayDanhSachTinh",
            type: "GET",
            success: function (data) {
                initDanhSachTinh(data);
            }
        });
    }
    var layDanhSachHuyen = function (idTinh, reloadDanhSachHuyenSelectList) {
        $.ajax({
            url: "/api/SinhVien/LayHuyenCuaTinh/"+idTinh,
            type: "GET",
            success: function (data) {
                reloadDanhSachHuyenSelectList(data);
            }
        });
    }
    var layDanhSachXa = function (idHuyen, reloadDanhSachXaSelectList) {
        $.ajax({
            url: "/api/SinhVien/LayXaCuaHuyen/" + idHuyen,
            type: "GET",
            success: function (data) {
                reloadDanhSachXaSelectList(data);
            }
        });
    }
    var saveDiaChi = function(diaChiDto,updateAfterSaveDiaChi) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/SaveDiaChi",
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            data: diaChiDto,
            success: function (data) {
                updateAfterSaveDiaChi(data);
            }
        });
    }
    var xoaDiaChi = function (idDiaChiMuonXoa, updateAfterXoaDiaChi) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DeleteDiaChi/" + idDiaChiMuonXoa,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaDiaChi(idDiaChiMuonXoa);
            }
        });
    }
    var saveSdt = function (sdtDto, updateAfterSaveSdt) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/SaveSoDienThoai",
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            data: sdtDto,
            success: function (data) {
                updateAfterSaveSdt(data);
            }
        });
    }
    var xoaSdt = function (idSdtMuonXoa, updateAfterXoaSdt) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DeleteSoDienThoai/" + idSdtMuonXoa,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateAfterXoaSdt(idSdtMuonXoa);
            }
        });
    }
    //Trang chi tiết sinh viên
    var layThongTinCoBanSinhVien = function (sinhVienId, updateInput) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/ThongTinCoBan/" + sinhVienId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateInput(data);
            }
        });
    }
    var layBaiVietSinhVien = function (sinhVienId, themBaiViet) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/BaiViet/LayBaiVietSinhVien/" + sinhVienId + "/" + 0,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            }
        });
    }
    var layDanhSachDonViSinhVien = function (sinhVienId, themDonViVaoDom) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DonViThamGia/" + sinhVienId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (danhSachDonVi) {
                themDonViVaoDom(danhSachDonVi);
            }
        });
    }


return{
    getSinhVienData: getSinhVienData,
    saveSinhVien: saveSinhVien,
    layDanhSachTinh: layDanhSachTinh,
    layDanhSachHuyen: layDanhSachHuyen,
    layDanhSachXa: layDanhSachXa,
    saveDiaChi: saveDiaChi,
    xoaDiaChi: xoaDiaChi,
    saveSdt: saveSdt,
    xoaSdt: xoaSdt,
    layThongTinCoBanSinhVien: layThongTinCoBanSinhVien,
    layBaiVietSinhVien: layBaiVietSinhVien,
    layDanhSachDonViSinhVien: layDanhSachDonViSinhVien
}
}();

var CacTrangCaNhanService = function () {
    var layDanhSachLopCuaToi = function (themLopCuaToiVaoDom) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/LopCuaToi",
            headers: { __RequestVerificationToken: csrfToken },
            success: function(danhSachLop) {
                themLopCuaToiVaoDom(danhSachLop);
            }
        });
    }
    var layDanhSachDonViCuaToi = function (themDonViCuaToiVaoDom) {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        $.ajax({
            url: "/api/SinhVien/DonViCuaToi",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (danhSachDonVi) {
                themDonViCuaToiVaoDom(danhSachDonVi);
            }
        });
    }
    return {
        layDanhSachLopCuaToi: layDanhSachLopCuaToi,
        layDanhSachDonViCuaToi: layDanhSachDonViCuaToi
}
}();