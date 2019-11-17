// dùng để bỏ dấu tiếng việt của searchTerm
function change_alias(alias) {
    var str = alias;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/ + /g, " ");
    str = str.replace(/[^0-9a-zàáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ\s]/gi, '');
    str = str.replace(/\s+/g, ' ');
    str = str.trim();
    return str;
}
function encode_utf8(s) {
    return unescape(encodeURIComponent(s));
}
function decode_utf8(s) {
    return decodeURIComponent(escape(s));
}

function reloadTable(table, data) {
    table.clear().draw();
    table.rows.add(data); // Add new data
    table.columns.adjust().draw(); // Redraw the DataTable
}

function dataTableSetting() {
    $.extend($.fn.dataTable.defaults, {
        language: {
            search: "Tìm kiếm:",
            lengthMenu: "Hiển thị _MENU_ bản ghi",
            info: "Hiển thị _START_ tới _END_ của _TOTAL_ bản ghi",
            infoFiltered: "(được lọc từ _MAX_ bản ghi)",
            zeroRecords: "Không tìm thấy kết quả nào",
            infoEmpty: "Đang xem 0 đến 0 trong tổng số 0 mục",
            loadingRecords: "Đang tải...",
            processing: "Đang xử lý...",
            emptyTable: "Không có dữ liệu nào",
            paginate: {
                first: "Đầu",
                last: "Cuối",
                next: "Trang sau",
                previous: "Trang trước"
            }
        }
    });
}
function returnSvLink(idSinhVien) {
    return "/SinhVien/" + idSinhVien;
}
function returnTenSinhVien(sinhVien) {
    return sinhVien.hoVaTenLot + " " + sinhVien.ten;
}
function returnTenSinhVienWithLink(sinhVien) {
    return '<a href="' + returnSvLink(sinhVien.id) + '" class="tenSvDs">' + returnTenSinhVien(sinhVien) + '</a>';
}
function returnAnhSv(imgSrc) {
    return '<img src="' + imgSrc + '" class="img-fluid anhSvDanhSach">';
}
function taoLinkHoatDong(tenHoatDong, hoatDongId) {
    var compiled = _.template($("#tenHoatDongTemplate").html());
    var html = compiled({
        hoatDongId: hoatDongId,
        tenHoatDong: tenHoatDong
    });
    return html;
}
function taoTinhTrang(hoatDong) {
    if (hoatDong.biHuy) return '<b style="color:red">Đã bị hủy</span>';
    if (!hoatDong.duocPheDuyet && hoatDong.duocPheDuyet != null) return '<b style="color:purple">Chờ phê duyệt</span>';
    if (hoatDong.daKetThuc) return '<b style="color:goldenrod">Đã kết thúc</span>';
    if (new Date(hoatDong.ngayBatDau) > new Date()) return '<b style="color:blue">Sắp diễn ra</span>';
    return '<b style="color:green">Đang diễn ra</span>';
}
var taoDonViToChuc = function (row) {
    var html = '';
    row.danhSachDonViToChuc.forEach(function (donVi) {
        html += "<li>" + donVi + "</li>";
    });
    row.danhSachLopToChuc.forEach(function (lop) {
        html += "<li>" + lop + "</li>";
    });
    return '<ul>' + html + '</ul>';
}
var reloadPage = function () {
    location.reload(true);
}
var returnIntArray = function (array) {
    for (var i = 0; i < array.length; i++) {
        array[i] = parseInt(array[i]);
    }
    return array;
}
var returnDateFromInput = function (gio, ngay) {
    //Func dùng để trả về định dạng giờ để save Hoạt động
    if (gio == null || gio == "") return ngay + "T00:00";
    return ngay + "T" + gio;
}
var returnDateForInput = function (date) {
    return moment(date).format('YYYY-MM-DD');
}
var returnHourForInput = function(time) {
    return moment(time).format("HH:mm");
}
var returnNgayGio = function(dateTime) {
    return moment(dateTime).format("HH [giờ] mm ngày DD/MM/YYYY");
}
var returnNgayGioNgan = function (dateTime) {
    return moment(dateTime).format("HH:mm [ngày] DD/MM/YYYY ");
}
var returnNgay = function(dateTime) {
    return moment(dateTime).format('DD/MM/YYYY');
}
var returnIntFromBoolean = function(boolean) {
    return boolean ? 1 : 0;
}
var returnEmptyStringForNull = function(data) {
    if (data == null) return '';
    return data;
}
var returnKyHieuTenLop = function (kyHieuTenLop) {
    if (kyHieuTenLop == null) return "Chưa có lớp";
    return kyHieuTenLop;
}
var returnCapHoatDong = function (capHoatDong) {
    switch (capHoatDong) {
    case 1:
        return "Cấp Chi hội";
    case 2:
        return "Cấp Liên chi hội";
    case 3:
        return "Cấp Khóa";
    case 4:
        return "Cấp Phân viện";
    }
}
function findObjectIndexInArrayByKey(array, key, value) {
    for (var i = 0; i < array.length; i++) {
        if (array[i][key] == value) {
            return i;
        }
    }
    return null;
}
var sort_by = function (path, reverse, primer, then) {
    var get = function (obj, path) {
            if (path) {
                path = path.split('.');
                for (var i = 0, len = path.length - 1; i < len; i++) {
                    obj = obj[path[i]];
                };
                return obj[path[len]];
            }
            return obj;
        },
        prime = function (obj) {
            return primer ? primer(get(obj, path)) : get(obj, path);
        };

    return function (a, b) {
        var A = prime(a),
            B = prime(b);

        return (
            (A < B) ? -1 :
                (A > B) ? 1 :
                (typeof then === 'function') ? then(a, b) : 0
        ) * [1, -1][+!!reverse];
    };
};
var showTabQuanLy = function (tenTabHienThi,thuTuItem) {
    $("#nav-profile-tab").removeClass("active show");
    $("#nav-profile").removeClass("active show");
    $("#nav-quanLy-tab").addClass("active show");
    $("#nav-quanLy").addClass("active show");

    $("#" + tenTabHienThi).siblings("a").attr("aria-expanded", "true");
    $("#" + tenTabHienThi).addClass("show");
    $("#" + tenTabHienThi).find("a:eq(" + thuTuItem + ")").addClass("active");

}
var showTabProfile = function (tenTabHienThi, thuTuItem) {
    $("#nav-profile-tab").addClass("active show");
    $("#nav-profile").addClass("active show");
    $("#nav-quanLy-tab").removeClass("active show");
    $("#nav-quanLy").removeClass("active show");

    $("#" + tenTabHienThi).siblings("a").attr("aria-expanded", "true");
    $("#" + tenTabHienThi).addClass("show");
    $("#" + tenTabHienThi).find("a:eq(" + thuTuItem + ")").addClass("active");
}
var showLoader = function() {
    $(".loader").show();
}
var hideLoader = function() {
    $(".loader").hide();
}
/*Giới thiệu lớp, đơn vị callback*/
var gioiThieuListener = function() {
    $(".nut-xemThem").on("click", function () {
        $(".gioiThieu").toggleClass("-daMoRong");

        if ($(".gioiThieu").hasClass("-daMoRong")) {
            $(".text-xemThem").html("Thu gọn");
        }
        else {
            $(".text-xemThem").html("Xem thêm");

        }         
        $(this).find("svg").toggleClass("fa-chevron-circle-down fa-chevron-circle-up");
    });
}
/*Jquery Validation*/
var setValidationDefault = function() {
    jQuery.validator.setDefaults({
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("is-invalid").removeClass("is-valid");
            $(element).parents(".form-group").addClass("has-error").removeClass("has-success");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).addClass("is-valid").removeClass("is-invalid");
            $(element).parents(".form-group").addClass("has-success").removeClass("has-error");
        }
    });
}
/*Character Limiter*/
var limitCharacterForInput = function (inputName) {
    var characterLimit = $("#" + inputName).attr("maxlength");
    $("#" + inputName + "Limit").html("Còn " + characterLimit + " kí tự."); //Init số kí tự ban đầu
    $("#" + inputName).keyup(function () {
        var soKiTuHienTai = $("#" + inputName).val().length;
        var soKiTuCon = characterLimit - soKiTuHienTai;
        $("#" + inputName + "Limit").html("Còn " + soKiTuCon + " kí tự.");
    });
}
var setDataForInputWithLimit = function (inputName, value) {
    $("#" + inputName).val(value);
    var characterLimit = $("#" + inputName).attr("maxlength");
    var soKiTuHienTai = $("#" + inputName).val().length;
    var soKiTuCon = characterLimit - soKiTuHienTai;
    $("#" + inputName + "Limit").html("Còn " + soKiTuCon + " kí tự.");

}
/*Tìm kiếm*/
var timKiem = function  () {
    var searchTermValue = $("#searchTermValue").val().trim();
    var searchType = $("#searchType").val();
    if (searchTermValue !== "") {
        window.open("/TimKiem?searchTerm=" + searchTermValue + "&searchType=" + searchType, "_self");
    } else {
        alert("Vui lòng nhập từ khóa trước khi tìm kiếm.");
    }
}
var initTimKiemTypeahead = function () {
    var danhSachKetQua = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace("ketQua"),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: "/api/TimKiem/TimKiemChung?searchTerm=%QUERY",
            wildcard: "%QUERY"
        }
    });
    $("#searchTermValue").typeahead(
            {
                highlight: true
            },
            {
                name: 'ketQua',
                display: function (data) {
                    return data.ketQua;
                },
                source: danhSachKetQua,
                templates: {
                    suggestion: function (data) {
                        return "<div>"+data.ketQua+"</div>";
                    }
                }

            })
        .on("typeahead:autocomplete", timKiem)
        .on("typeahead:select", timKiem)
        .on("typeahead:change",function () {});
}

/*Listener toàn project*/
$(document).ready(function () {

    $("#sidebar-toggle").on("click",
        function() {
            $("#sidebar").toggleClass("active");
            $(".pageBody").toggleClass("active");
            $(".navbar").toggleClass("active");
        });
    gioiThieuListener();
    setValidationDefault();
    $("#searchBtn").on("click", timKiem);
    initTimKiemTypeahead();

});





