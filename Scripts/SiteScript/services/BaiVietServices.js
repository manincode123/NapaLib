var SaveBaiVietService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var getDonViData = function (initDonViSelectList) {
        $.ajax({
            type: "GET",
            url: '/api/HoatDong/LayDanhSachDonViToChuc',
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                initDonViSelectList(data.danhSachLop, data.danhSachDonVi);
            }
        });
    }
    var getChuyenMucData = function (initChuyenMucSelectList) {
        $.ajax({
            type: "GET",
            url: '/api/ChuyenMuc/LayDanhSachChuyenMuc',
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                initChuyenMucSelectList(data);
            }
        });
    }
    var getBaiViet = function (baiVietId,updatePage) {
        $.ajax({
            type: "GET",
            url: '/api/BaiViet/'+baiVietId,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updatePage(data);
            }
        });
    }
    var themBaiViet = function (baiVietDto, updateAfterThem) {
        $.ajax({
            type: "POST",
            url: '/api/BaiViet/SaveBaiViet',
            data : baiVietDto,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateAfterThem();
            }
        });
    }
    var chinhSuaBaiViet = function (baiVietDto, updateAfterChinhSua) {
        $.ajax({
            type: "POST",
            url: '/api/BaiViet/SaveBaiViet',
            data : baiVietDto,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateAfterChinhSua(baiVietDto.id);

            }
        });
    }

    return {
        getDonViData: getDonViData,
        getChuyenMucData: getChuyenMucData,
        themBaiViet: themBaiViet,
        getBaiViet: getBaiViet,
        chinhSuaBaiViet: chinhSuaBaiViet
}
}();

var XemBaiVietService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var getBaiViet = function (baiVietId, updateDom) {
        $.ajax({
            type: "GET",
            url: '/api/BaiViet/' + baiVietId,
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (baiVietDto) {
                updateDom(baiVietDto);
            }
        });
    }
    return {
        getBaiViet : getBaiViet
    }
}();

var PheDuyetBaiVietService= function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layBaiViet = function (appendBaiVietToModal) {
       $.ajax({
           url: "/BaiViet/PartialView",
           type: "GET",
           headers: { __RequestVerificationToken: csrfToken },
           success: function (data) {
               appendBaiVietToModal(data);
           }
       });
    }
    var pheDuyetBaiViet = function (baiVietId, updatePage) {
        $.ajax({
            url: "/api/BaiViet/PheDuyet/"+baiVietId,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updatePage();
            }
        });
    }
    var xoaBaiViet = function (baiVietId, updatePage) {
        $.ajax({
            url: "/api/BaiViet/Xoa/" + baiVietId,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updatePage();
            }
        });
    }
    return{
        layBaiViet: layBaiViet,
        pheDuyetBaiViet: pheDuyetBaiViet,
        xoaBaiViet :xoaBaiViet
    }
}();

var QuanLyBaiVietService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layBaiViet = function (appendBaiVietToModal) {
        $.ajax({
            url: "/BaiViet/PartialView",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                appendBaiVietToModal(data);
            }
        });
    }
    var xoaBaiViet = function (baiVietId, updatePage) {
        $.ajax({
            url: "/api/BaiViet/Xoa/" + baiVietId,
            type: "DELETE",
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updatePage();
            }
        });
    }
    return {
        layBaiViet: layBaiViet,
        xoaBaiViet: xoaBaiViet
    }
}();

var QuanLyChuyenMucService = function() {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layChuyenMuc = function (chuyenMucId, updateInput) {
        $.ajax({
            url: "/api/ChuyenMuc/" + chuyenMucId,
            type: "GET",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateInput(data);
            }
        });
    }
    var layChuyenMucSelectList = function (chuyenMucId, initSelectList) {
        $.ajax({
            url: "/api/ChuyenMuc/CmChaSelectList?chuyenMucId=" + chuyenMucId,
            type: "GET",
            async: false,
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                initSelectList(data);
            }
        });
    }
    var saveChuyeMuc = function (chuyenMucDto, updateDom) {
        $.ajax({
            url: "/api/ChuyenMuc/SaveChuyenMuc",
            type: "POST",
            data: chuyenMucDto,
            headers: { __RequestVerificationToken: csrfToken },
            success: function () {
                updateDom();
            }
        });
    }
return {
    layChuyenMuc: layChuyenMuc,
    layChuyenMucSelectList: layChuyenMucSelectList,
    saveChuyeMuc: saveChuyeMuc
}
}();

var DanhSachBaiVietService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var layThongTinTrangChu = function (updateDom) {
        $.ajax({
            url: "/api/BaiViet/TrangChu",
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateDom(data);
            }
        });
    }
    var layBaiVietSinhVien = function (sinhVienId, recordStart, themBaiViet) {
        $.ajax({
            url: "/api/BaiViet/LayBaiVietSinhVien/" + sinhVienId + "/" + recordStart,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            }
        });
    }
    var layBaiVietLop = function (lopId, recordStart, themBaiViet) {
        $.ajax({
            url: "/api/BaiViet/LayBaiVietLop/" + lopId + "/" + recordStart,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            }
        });
    }
    var layBaiVietDonVi = function (donViId, recordStart, themBaiViet) {
        $.ajax({
            url: "/api/BaiViet/DonVi/" + donViId + "/" + recordStart,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            }
        });
    }
    var layChuyenMucData = function (chuyenMucId, updateDom) {
        $.ajax({
            url: "/api/BaiViet/ChuyenMuc/" + chuyenMucId,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                updateDom(data);
            }
        });
    }
    var layBaiVietChuyenMuc = function (chuyenMucId, recordStart, themBaiViet) {
        $.ajax({
            url: "/api/BaiViet/LayBaiVietChuyenMucTiepTheo/" + chuyenMucId + "/" + recordStart,
            type: "GET",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                themBaiViet(data);
            }
        });
    }


    return {
        layThongTinTrangChu: layThongTinTrangChu,
        layBaiVietSinhVien: layBaiVietSinhVien,
        layBaiVietLop: layBaiVietLop,
        layBaiVietDonVi : layBaiVietDonVi,
        layChuyenMucData: layChuyenMucData,
        layBaiVietChuyenMuc: layBaiVietChuyenMuc
    }
}();
