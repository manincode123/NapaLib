var setLoaiMon = function (loaiMon) {
    if (loaiMon === 1) { $("#loaiMon").html("Môn thường"); }
    if (loaiMon === 2) { $("#loaiMon").html("Môn Tiếng anh"); }
    if (loaiMon === 3) { $("#loaiMon").html("Môn CPĐT-TTTQLNN"); }

}
var setSoCotDieuKien = function (haiDiemDk) {
    if (haiDiemDk == false) {
        $("#haiDiemDk").html("1 cột");
    }

    else { $("#haiDiemDk").html("2 cột") }
}
var setHocKi = function (hocKi) {
    $('#hocKi').html('Học kì ' + hocKi);
}
var returnThoiGianHoc = function (lichHoc) {
    return returnNgay(lichHoc.ngayBatDau) + ' - ' + returnNgay(lichHoc.ngayKetThuc);
}
var returnNgayHoc = function (thu246) {
    if (thu246) return "Thứ hai, tư, sáu";
    return "Thứ ba, năm";
}
var returnBuoiHoc = function (buoiSang) {
    if (buoiSang) { return 'Buổi sáng' }
    else { return 'Buổi chiều' }
}
var returnTietHoc = function (baTietDau) {
    if (baTietDau) { return 'Tiết 1,2,3' }
    else { return 'Tiết 4,5' }
}
var setLoaiMon = function (loaiMon) {
    if (loaiMon === 1) { $("#loaiMon").html("Môn thường"); }
    if (loaiMon === 2) { $("#loaiMon").html("Môn Tiếng anh"); }
    if (loaiMon === 3) { $("#loaiMon").html("Môn CPĐT-TTTQLNN"); }

}
var setSoCotDieuKien = function (haiDiemDk) {
    if (haiDiemDk == false) {
        $("#haiDiemDk").html("1 cột");
    }

    else { $("#haiDiemDk").html("2 cột") }
}

var QuanLyChungMonHocController = function (quanLyChungMonHocService) {
    var danhSachMonTable, danhSachLichThiLaiTable, danhSachSvPhaiThiLaiTable;
    var monHocId, lichThiLaiId;
    var tenMocHocMax, kyHieuMonHocMax;
    var monHocDto = {};

    //Tạo bảng
    var returnTenLoaiMon = function (data) {
        if (data === 1) return "Môn thường";
        if (data === 2) return "Môn tiếng anh";
        if (data === 3) return "Môn CPĐT-TTTQLNN";
    }
    var returnTenMon = function (data) {
        return '<a href="" class="link">' + data + '</a>';
    }
    var initDanhSachMonTable = function () {
        danhSachMonTable = $("#danhSachMonHoc").DataTable({
            ajax: {
                url: "/api/MonHoc/DanhSachMonHoc",
                dataSrc: ''
            },
            "order": [[0, 'desc']],
            columns: [
                { data: 'tenMonHoc' },
                { data: 'kyHieuMonHoc' },
                { data: 'soTiet' },
                { data: 'soHocPhan' },
                { data: 'loaiMon' },
                { data: 'id' }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {
                        return returnTenMon(data);
                    }
                },
                {
                    targets: 1,
                    width: 100
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return returnTenLoaiMon(data);
                    }
                },
                {
                    targets: 5,
                    width: 300,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary quanLyMon-js">Quản lý</button>'
                            + '<button class="btn btn-success chinhSuaMon-js">Chỉnh sửa</button>'
                            + '<button class="btn btn-danger xoaMon-js">Xóa môn</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var returnDaThiLai = function (lichThiLaiId) {
        if (lichThiLaiId == null) return '<button class="btn btn-danger quanLyThiLaiMon-js">Chưa có lịch thi lại</button>';
        return '<button class="btn btn-success quanLyThiLai-js">Đã có lịch thi lại</button>';
    }
    var returnThiLaiDaKetThuc = function (daThiXong) {
        if (daThiXong) return '<strong style="color:blue">Đã kết thúc</strong>';
        return '<strong style="color:#22ff22">Đang thi</strong>';
    }
    var initDanhSachSinhVienPhaiThiLaiTable = function () {
        danhSachSvPhaiThiLaiTable = $("#danhSachSvPhaiThiLai").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsSvPhaiThiLaiTatCaMon",
                dataSrc: ''
            },
            "order": [[0, 'desc']],
            columns: [
                { data: 'mssv' },
                { data: '' },                          
                { data: 'tenLop' },
                { data: 'tenMonHoc' },
                { data: 'diemTb' },
                { data: 'lichThiLaiId' }
            ],
            rowId: 'monHocId',
            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return row.hoVaTenLot + ' ' + row.ten;
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return returnDaThiLai(row);
                    },
                    createdCell: function (td, cellData) {
                        $(td).attr('id', cellData);
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var initLichThiLaiTable = function () {
        danhSachLichThiLaiTable = $("#danhSachLichThiLai").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsLichThiLaiTatCaCacMon",
                dataSrc: ''
            },
            "order": [[3, 'desc']],
            columns: [
                { data: 'thoiGianThi' },
                { data: 'diaDiemThi' },
                { data: 'tenMonHoc' },
                { data: 'daThiXong' },
                { data: 'id' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return returnNgayGio(data);
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return returnThiLaiDaKetThuc(data);
                    }
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary quanLyThiLai-js">Quản lý</button>';
                    },
                    createdCell: function (td, cellData) {
                        $(td).attr('id', cellData);
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }

    //Phần chỉnh sửa, thêm môn học
    var limitCharacter = function () {
        //Hạn chế số kí tự tên môn học
        tenMocHocMax = 50;
        $('#tenMonHocLimit').html('Còn ' + tenMocHocMax + ' kí tự.'); //Init số kí tự ban đầu
        $('#tenMonHoc-input').keyup(function () {
            var soKiTuHienTai = $('#tenMonHoc-input').val().length;
            var soKiTuCon = tenMocHocMax - soKiTuHienTai;
            $('#tenMonHocLimit').html('Còn ' + soKiTuCon + ' kí tự.');
        });

        //Hạn chế số kí tự ký hiệu môn học
        kyHieuMonHocMax = 10;
        $('#kyHieuMonHocLimit').html('Còn ' + kyHieuMonHocMax + ' kí tự.');  //Init số kí tự ban đầu
        $('#kyHieuMonHoc-input').keyup(function () {
            var soKiTuHienTai = $('#kyHieuMonHoc-input').val().length;
            var soKiTuCon = kyHieuMonHocMax - soKiTuHienTai;
            $('#kyHieuMonHocLimit').html('Còn ' + soKiTuCon + ' kí tự.');
        });
    }
    var setTenMonHoc = function (tenMonHoc) {
        $("#tenMonHoc-input").val(tenMonHoc);
        var soKiTuHienTai = tenMonHoc.length;
        var soKiTuCon = tenMocHocMax - soKiTuHienTai;
        $('#tenMonHocLimit').html('Còn ' + soKiTuCon + ' kí tự.');

    }
    var setKyHieuMonHoc = function (kyHieuMonHoc) {
        $("#kyHieuMonHoc-input").val(kyHieuMonHoc);
        var soKiTuHienTai = kyHieuMonHoc.length;
        var soKiTuCon = kyHieuMonHocMax - soKiTuHienTai;
        $('#kyHieuMonHocLimit').html('Còn ' + soKiTuCon + ' kí tự.');
    }
    var setCheckBoxValue = function (haiDiemDk) {
        if (haiDiemDk == 1) {
            $("#haiDiemDk-input").prop("checked", true);
        }

        else { $("#haiDiemDk-input").prop("checked", false); }
    }
    var updateInput = function (monHoc) {
        setTenMonHoc(monHoc.tenMonHoc);
        setKyHieuMonHoc(monHoc.kyHieuMonHoc);
        setCheckBoxValue(monHoc.haiDiemDk);
        $("#monHocId").val(monHoc.id);
        $("#soHocPhan-input").val(monHoc.soHocPhan);
        $("#soTiet-input").val(monHoc.soTiet);
        $("#loaiMon-input").val(monHoc.loaiMon);
    }
    var resetInput = function () {
        setTenMonHoc("");
        setKyHieuMonHoc("");
        $("#monHocId").val(0);
        $("#soHocPhan-input").val("");
        $("#soTiet-input").val("");
        $("#haiDiemDk-input").val(0);
        $("#loaiMon-input").val(1);
    }
    var hienThiModalChinhSuaMon = function (e) {
        var button = $(e.target);
        //Lấy id môn học muốn chỉnh sửa
        monHocId = button.closest("tr").attr("id");
        $(".modal-title").html("Chỉnh sửa môn học");
        $("#saveMonHoc").html("Chỉnh sửa");
        quanLyChungMonHocService.layMonHoc(monHocId, updateInput);
        $("#haiDiemDk-input").attr("disabled", true);
        $("#loaiMon-input").attr("disabled", true);
        $("#saveMonHoc").html("Chỉnh sửa");
        $("#saveMonHocModal").modal("show");
    }
    var hienThiModalThemMon = function () {
        monHocId = 0;
        $(".modal-title").html("Thêm môn học");
        $("#saveMonHoc").html("Thêm");
        resetInput();
        $("#haiDiemDk-input").attr("disabled", false);
        $("#loaiMon-input").attr("disabled", false);
        $("#saveMonHocModal").modal("show");
    }
    var getCheckBoxValue = function() {
        var haiDiemDk;
        if ($("#haiDiemDk-input").prop("checked") === true) haiDiemDk = true;
        else { haiDiemDk = false }
        return haiDiemDk;
    }
    var onSelectLoaiMon = function () {
        var loaiMon = $("#loaiMon").val();
        if (loaiMon == 1) {
            $("#haiDiemDk-input").attr("disabled", false);
        } else {
            $("#haiDiemDk-input").prop("checked", true);
            $("#haiDiemDk-input").attr("disabled", true);
        }
    }
    var mapMonHocObject = function () {
        monHocDto.id = parseInt($("#monHocId").val());
        monHocDto.tenMonHoc = $("#tenMonHoc-input").val();
        monHocDto.soHocPhan = parseInt($("#soHocPhan-input").val());
        monHocDto.soTiet = parseInt($("#soTiet-input").val());
        monHocDto.kyHieuMonHoc = $("#kyHieuMonHoc-input").val();
        monHocDto.haiDiemDk = getCheckBoxValue();
        monHocDto.loaiMon = parseInt($("#loaiMon-input").val());
    }
    var updateAfterSave = function() {
        if (monHocId === 0) { alert("Đã thêm môn học.") }
        else { alert("Đã chỉnh sửa môn học.") }
        danhSachMonTable.ajax.reload();
        $("#saveMonHocModal").modal("hide");
    }
    var saveMon = function () {
        mapMonHocObject();
        quanLyChungMonHocService.saveMon(monHocDto, updateAfterSave);
    }

    //Chuyển tới trang quản lý
    var chuyenDenTrangQuanLyMon = function(e) {
        var button = $(e.target);
        //Lấy id môn học muốn chuyển tới trang quản lý
        monHocId = button.closest("tr").attr("id");
        window.location = '/MonHoc/QuanLyMonHoc/'+monHocId;
    }
    var chuyenDenTrangQuanLyThiLai = function(e) {
        var button = $(e.target);
        //Lấy id môn học muốn chuyển tới trang quản lý
        lichThiLaiId = button.closest("td").attr("id");
        window.location = '/MonHoc/QuanLyThiLai/' + lichThiLaiId;
    }
    var chuyenDenTrangQuanLyThiLaiMon = function (e) {
        var button = $(e.target);
        //Lấy id môn học muốn chuyển tới trang quản lý
        monHocId = button.closest("tr").attr("id");
        window.location = '/MonHoc/QuanLyThiLaiMon/' + monHocId;
    }
    //Xóa môn
    var hienThiModalXoaMon = function (e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        monHocId = button.closest("tr").attr("id");
        $("#xoaMonHocModal").modal("show");
    }
    var updateAfterXoaMon = function () {
        danhSachMonTable.ajax.reload();
        alert('Đã xóa môn học.');
        $("#xoaMonHocModal").modal("hide");
    }
    var xoaMon = function () {
        quanLyChungMonHocService.xoaMon(monHocId, updateAfterXoaMon);
    }

    var initTrang = function () {
        dataTableSetting();
        initDanhSachMonTable();
        initDanhSachSinhVienPhaiThiLaiTable();
        initLichThiLaiTable();
        limitCharacter();
        $('#loaiMon').on('change', onSelectLoaiMon);
        $("body").on("click", ".xoaMon-js", hienThiModalXoaMon);
        $("#xoaMonHoc").on("click", xoaMon);
        $("body").on("click", ".chinhSuaMon-js", hienThiModalChinhSuaMon);
        $("#themMonHoc").on("click", hienThiModalThemMon);
        $("#saveMonHoc").on("click", saveMon);
        $("body").on("click", ".quanLyMon-js", chuyenDenTrangQuanLyMon);
        $("body").on("click", ".quanLyThiLaiMon-js", chuyenDenTrangQuanLyThiLaiMon);
        $("body").on("click", ".quanLyThiLai-js", chuyenDenTrangQuanLyThiLai);
    }

    return {
        initTrang: initTrang,
        updateInput: updateInput,
        limitCharacter: limitCharacter
}
}(QuanLyChungMonHocService);

var QuanLyMonHocController = function (quanLyMonHocService, quanLyChungMonHocController) {
    var danhSachLopMonHoc, monHoc, monHocId,danhSachSvPhaiThiLaiTable;
    var dangKiMonHocDto = {}, monHocDto ={}, xoaDangKiMonHocDto ={};

    //Phần chỉnh sửa thông tin môn học
    var updateDom = function (monHocData) {
        monHoc = monHocData;
        $("#tenMonHoc").html(monHoc.tenMonHoc);
        $("#kyHieuMonHoc").html(monHoc.kyHieuMonHoc);
        $("#soHocPhan").html(monHoc.soHocPhan);
        $("#soTiet").html(monHoc.soTiet);
        setLoaiMon(monHoc.loaiMon);
        setSoCotDieuKien(monHoc.haiDiemDk);
    }
    var hienThiModalChinhSuaMon = function () {
        $(".modal-title").html("Chỉnh sửa môn học");
        $("#saveMonHoc").html("Chỉnh sửa");
        quanLyChungMonHocController.updateInput(monHoc);
        $("#haiDiemDk-input").attr("disabled", true);
        $("#loaiMon-input").attr("disabled", true);
        $("#saveMonHoc").html("Chỉnh sửa");
        $("#saveMonHocModal").modal("show");
    }
    var updateAfterSaveMonHoc = function() {
        alert('Đã chỉnh sửa thông tin môn học');
        window.location.reload(true);
    }
    var mapMonHocObject = function () {
        monHocDto.id = monHocId;
        monHocDto.tenMonHoc = $("#tenMonHoc-input").val();
        monHocDto.soHocPhan = parseInt($("#soHocPhan-input").val());
        monHocDto.soTiet = parseInt($("#soTiet-input").val());
        monHocDto.kyHieuMonHoc = $("#kyHieuMonHoc-input").val();
    }
    var saveMon = function () {
        mapMonHocObject();
        quanLyMonHocService.saveMon(monHocDto, updateAfterSaveMonHoc);
    }

    //Phần quản lý, đăng kí môn học (thêm lớp môn học)
    var initDanhSachLopMonHocTable = function (monHocId) {
        danhSachLopMonHoc = $("#danhSachLopMonHoc").DataTable({
            ajax: {
                url: "/api/MonHoc/DanhSachLopMonHoc/" + monHocId,
                dataSrc: ''
            },
            "order": [[0, 'desc']],
            columns: [
                { data: 'tenLop' },
                { data: 'hocKi' },
                { data: 'ngayThi' },
                { data: 'diaDiemThi' },
                { data: 'id' }
            ],
            rowId: "lopId",
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {
                        return data;
                    }
                },
                {
                    targets: 1,
                    width: 100
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnNgayGio(data);
                    }
                },
                {
                    targets: 4,
                    width: 300,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary quanLyLopMonHoc-js">Quản lý</button>'
                             + '<button class="btn btn-danger xoaLopMonHoc-js">Xóa lớp</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var resetInput = function () {
        $("#selectLop").val('').trigger('change');
        $("#hocKi-input").val(1);
        $("#diaDiemThi-input").val("");
        $("#ngay").val("");
        $("#gio").val("");
    }
    var initLopSelectList = function (danhSachLop) {
        $('#selectLop').select2({
            language: "vi",
            data: danhSachLop
        });
    }
    var hienThiModalThemLopMonHoc = function () {
        resetInput();
        $("#saveLopMonHocModal").modal("show");
    }
    var updateAfterSaveLopMonHoc = function(xhr) {
        if (xhr == null) {
            alert("Đã đăng kí môn học cho lớp");
            danhSachLopMonHoc.ajax.reload();
            $('#saveLopMonHocModal').modal('hide');
        } else {
            alert(xhr.responseJSON.message);
            $('#saveLopMonHocModal').modal('hide');
        }
    }
    var returnDateForThemLopMonHoc = function () {
        return $("#ngay").val() + "T" + $("#gio").val();;
    }
    var mapLopMonHocObject = function () {
        dangKiMonHocDto.lopId = $("#selectLop").val();
        dangKiMonHocDto.monHocId = monHocId;
        dangKiMonHocDto.hocKi = $("#hocKi-input").val();
        dangKiMonHocDto.diaDiemThi = $("#diaDiemThi-input").val();
        dangKiMonHocDto.ngayThi = returnDateForThemLopMonHoc();
    }
    var saveLopMonHoc = function() {
        mapLopMonHocObject();
        quanLyMonHocService.saveLopMonHoc(dangKiMonHocDto, updateAfterSaveLopMonHoc);
    }
    //Xóa đăng kí môn học
    var hienThiXoaDangKiMonHocModal = function(e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        xoaDangKiMonHocDto.lopId = button.closest("tr").attr("id");
        xoaDangKiMonHocDto.monHocId = monHocId;
        $('#xoaDangKiMonHocModal').modal('show');
    }
    var updateAfterXoaDangKiMonHoc = function () {
        alert("Đã xóa đăng kí môn học cho lớp");
        danhSachLopMonHoc.ajax.reload();
        $('#xoaDangKiMonHocModal').modal('hide');
    }
    var xoaDangKiMonHoc = function () {
        quanLyMonHocService.xoaDangKiMonHoc(xoaDangKiMonHocDto, updateAfterXoaDangKiMonHoc);
    }
    
    var chuyenToiTrangQuanLyLopMonHoc = function (e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        var lopId = button.closest("tr").attr("id");
        window.location = '/MonHoc/QuanLyLopMonHoc?monHoc=' + monHocId + '&lop=' + lopId;
    }

    //Phần sinh viên thi lại
    var initDanhSachSinhVienPhaiThiLaiTable = function (monHocId) {
        danhSachSvPhaiThiLaiTable = $("#danhSachSvPhaiThiLai").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsSvPhaiThiLai/" + monHocId,
                dataSrc: ''
            },
            "order": [[0, 'desc']],
            columns: [
                { data: 'mssv' },
                { data: '' },
                { data: 'tenLop' },
                { data: 'diemTb' }
            ],
            rowId: "sinhVienId",
            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return row.hoVaTenLot + ' ' + row.ten;
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }


    var initTrang = function (idMonHoc) {
        monHocId = idMonHoc;
        dataTableSetting();
        quanLyMonHocService.layMonHoc(monHocId, updateDom);
        quanLyMonHocService.getLopSelectList(initLopSelectList);
        quanLyChungMonHocController.limitCharacter();
        initDanhSachLopMonHocTable(monHocId);
        initDanhSachSinhVienPhaiThiLaiTable(monHocId);
        $("#themLopMonHoc").on("click", hienThiModalThemLopMonHoc);
        $("#saveLopMonHoc").on("click", saveLopMonHoc);
        $("#chinhSuaMonHoc").on("click", hienThiModalChinhSuaMon);
        $("#saveMonHoc").on("click", saveMon);
        $('body').on('click', '.quanLyLopMonHoc-js', chuyenToiTrangQuanLyLopMonHoc);
        $('body').on('click', '.xoaLopMonHoc-js', hienThiXoaDangKiMonHocModal);
        $("#xoaDangKiMonHoc").on("click", xoaDangKiMonHoc);
    }

    return {
        initTrang: initTrang
    }
}(QuanLyMonHocService, QuanLyChungMonHocController);

var QuanLyLopMonHocController = function (quanLyLopMonHocService) {
    var bangDiemTable, lichHocTable, lopMonHocInfo, lopMonHocId, lichHocDto = {}, lichHocId, lopMonHocDto ={};
    var columns = [
        { data: 'mssv' },
        { data: 'hoVaTenLot' },
        { data: 'ten' }
    ];

    //Function cho danh sách điểm
    var addColumnsToTable = function (monHoc) { //Func dùng để thêm cột vào table trong DOM
        if (monHoc.loaiMon == 1) { //Loại môn thường
            if (monHoc.haiDiemDk == true) { //Có 2 cột điều kiện
                $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi</th><th>TB</th>');
            }
            else { //Chỉ có 1 cột điều kiện
                $("#cotDiem").append('<th>CC</th><th>ĐK</th><th>Thi</th><th>TB</th>');
            }
        }
        if (monHoc.loaiMon == 2) {  //Loại môn TA
            $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th>' +
                '<th>Thi Nói</th><th>Thi Nghe</th><th>Thi Viết</th><th>TB</th>');
        }
        if (monHoc.loaiMon == 3) { //Loại môn CPĐT
            $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi LT</th><th>Thi TH</th><th>TB</th>');
        }


    }
    var pushDiemCc = function () {
        //Push cột data điểm chuyên cần
        columns.push({
            data: 'diem',
            render: function (data, type, row) {
                return data.diemChuyenCan;
            },
            className: 'diemChuyenCan'
        });
    }
    var pushDiemDk = function (monHoc) {
        //Push cột data điểm điều kiện 1
        columns.push({
            data: 'diem',
            render: function (data, type, row) {
                return data.diemDieuKien1;
            },
            className: 'diemDieuKien1'
        });

        //Push cột data điểm điều kiện 2 nếu môn có 2 cột điều kiện
        if (monHoc.haiDiemDk == true) {
            columns.push({
                data: 'diem',
                render: function (data, type, row) {
                    return data.diemDieuKien2;
                },
                className: 'diemDieuKien2'
            });
        }
    }
    var pushDiemThi = function (monHoc) { //Push cột điểm thi
        //Nếu loại môn là môn thường
        if (monHoc.loaiMon == 1) {
            columns.push({
                data: 'diem',
                render: function (data, type, row) {
                    return data.diemThi;
                },
                className: 'diemThi'
            });
        }
        //Nếu loại môn là môn tiếng anh
        if (monHoc.loaiMon == 2) {
            //Cột thứ ba trong danhSachDiemBoSung là cột thi nói
            columns.push({
                data: 'diem',
                render: function (data) {
                    return data.danhSachDiemBoSung[2].diem;
                },
                className: 'diemThiNoi',
                createdCell: function(cell, cellData) {
                    $(cell).attr('id', cellData.danhSachDiemBoSung[2].id);
                }
            });
            //Cột thứ hai trong danhSachDiemBoSung là cột thi nghe
            columns.push({
                data: 'diem',
                render: function (data) {
                    return data.danhSachDiemBoSung[1].diem;
                },
                className: 'diemThiNghe',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData.danhSachDiemBoSung[1].id);
                }
            });
            //Cột đầu tiên trong danhSachDiemBoSung là cột thi viết
            columns.push({
                data: 'diem',
                render: function (data) {
                    return data.danhSachDiemBoSung[0].diem;
                },
                className: 'diemThiViet',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData.danhSachDiemBoSung[0].id);
                }
            });
        }

        //Nếu loại môn là môn CPĐT
        if (monHoc.loaiMon == 3) {
            //Cột thứ hai trong danhSachDiemBoSung là cột thi lý thuyết
            columns.push({
                data: 'diem',
                render: function (data) {
                    return data.danhSachDiemBoSung[1];
                },
                className: 'diemThiLT',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData.danhSachDiemBoSung[1].id);
                }
            });
            //Cột đầu tiên trong danhSachDiemBoSung là cột thi thực hành
            columns.push({
                data: 'diem',
                render: function (data, type, row) {
                    return data.danhSachDiemBoSung[0];
                },
                className: 'diemThiTH',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData.danhSachDiemBoSung[0].id);
                }
            });
        }
    }
    var pushDiemTb = function () {
        columns.push({
            data: 'diem',
            render: function (data, type, row) {
                return data.diemTb;
            },
            className: 'diemTb'
        });
    }
    var addColumnsData = function (monHoc) {  //Func dùng để định dạng columns trong DataTable
        pushDiemCc();
        pushDiemDk(monHoc);
        pushDiemThi(monHoc);
        pushDiemTb();
    }
    var initDiemSinhVienDataTable = function (sinhVienVaDiem) {
        bangDiemTable = $("#bangDiem").DataTable({
            data: sinhVienVaDiem,
            "order": [[0, 'asc']],
            columns: columns,
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 75
                },
                {
                    targets: 1,
                    width: 150
                },
                {
                    targets: 2,
                    width: 100
                }


            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var taoDiemSinhVien = function (data) {
        initDiemSinhVienDataTable(data);
    }

    //Chỉnh sửa điểm
    var initializeXEditable = function () {
        var diemChuyenCanEdit = function () {
            var value2Send;
            $(".diemChuyenCan").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1, //pk này ko có tác dụng gì (vì sd params ở dưới rồi), nhưng phải báo pk thì xeditable mới sent data tới server
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function(data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function() {
                    var data = {};
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.diemChuyenCan = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemDieuKien1Edit = function () {
            var value2Send;
            $(".diemDieuKien1").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1, //pk này ko có tác dụng gì (vì sd params ở dưới rồi), nhưng phải báo pk thì xeditable mới sent data tới server
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function(data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function() {
                    var data = {};
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.diemDieuKien1 = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemDieuKien2Edit = function () {
            var value2Send;
            $(".diemDieuKien2").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1, //pk này ko có tác dụng gì (vì sd params ở dưới rồi), nhưng phải báo pk thì xeditable mới sent data tới server
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function(data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function() {
                    var data = {};
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.diemDieuKien2 = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemDieuKienEdit = function (monHoc) {
            diemDieuKien1Edit();
            if (monHoc.haiDiemDk) diemDieuKien2Edit();
        }
        var diemThiThuongEdit = function () {
            var value2Send;
            $(".diemThi").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function (data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = {};
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.diemThi = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThiNoiEdit = function () {
            var value2Send;
            $(".diemThiNoi").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function (data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = {danhSachDiemBoSung : [{}]};
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.danhSachDiemBoSung[0].id = $(this).attr("id");
                    data.danhSachDiemBoSung[0].diem = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThiNgheEdit = function () {
            var value2Send;
            $(".diemThiNghe").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function (data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = { danhSachDiemBoSung: [{}] };
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.danhSachDiemBoSung[0].id = $(this).attr("id");
                    data.danhSachDiemBoSung[0].diem = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThiVietEdit = function () {
            var value2Send;
            $(".diemThiViet").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/ChinhSuaDiem',
                success: function (data) {
                    var currentCell = $(this);
                    var diemTbCell = $(this).closest("tr").find('.diemTb');
                    bangDiemTable.cell(currentCell).data(data).draw();
                    bangDiemTable.cell(diemTbCell).data(data).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = { danhSachDiemBoSung: [{}] };
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = lopMonHocId.monHocId;
                    data.danhSachDiemBoSung[0].id = $(this).attr("id");
                    data.danhSachDiemBoSung[0].diem = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThucHanhEdit = function() {
            var value2Send;
        }
        var diemLyThuyetEdit = function() {
            var value2Send;
        }
        var diemThiEdit = function (monHoc) {
            //Nếu loại môn là môn thường
            if (monHoc.loaiMon == 1) {
                diemThiThuongEdit();
            }
            //Nếu loại môn là môn tiếng anh
            if (monHoc.loaiMon == 2) {
                diemThiNoiEdit();
                diemThiNgheEdit();
                diemThiVietEdit();
            }
            //Nếu loại môn là môn CPĐT
            if (monHoc.loaiMon == 3) {
                diemThucHanhEdit();
                diemLyThuyetEdit();
            }
        }

        diemChuyenCanEdit();
        diemDieuKienEdit(lopMonHocInfo.monHoc);
        diemThiEdit(lopMonHocInfo.monHoc);
    }


    //Func hiển thị thông tin lớp môn học (tên lớp, môn học,danh sách lịch học)
    var initLichHocDataTable = function (lichHoc) {
        lichHocTable = $("#lichHoc").DataTable({
            data: lichHoc,
            "order": [[0, 'desc']],
            columns: [
                { data: 'ngayBatDau' },
                { data: 'thu246' },
                { data: 'buoiSang' },
                { data: 'baTietDau' },
                { data: 'giaoVienDay' },
                { data: 'phongHoc' },
                { data: 'id' }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 200,
                    render: function (data, type, row) {
                        return returnThoiGianHoc(row);
                    }
                },
                {
                    targets: 1,
                    width: 100,
                    render: function (data, type, row) {
                        return returnNgayHoc(data);
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnBuoiHoc(data);
                    }
                },
                {
                    targets: 3,
                    width: 75,
                    render: function (data, type, row) {
                        return returnTietHoc(data);
                    }
                },
                {
                    targets: 6,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary suaLichHoc-js">Sửa lịch học</button>'
                             + '<button class="btn btn-danger xoaLichHoc-js">Xóa</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var updateDom = function (lopMonHocInfo) {
        $('#tenLop').html(lopMonHocInfo.tenLop);
        $('#tenMonHoc').html(lopMonHocInfo.monHoc.tenMonHoc);
        $("#kyHieuMonHoc").html(lopMonHocInfo.monHoc.kyHieuMonHoc);
        $("#soHocPhan").html(lopMonHocInfo.monHoc.soHocPhan);
        $("#soTiet").html(lopMonHocInfo.monHoc.soTiet);
        $('#ngayThi').html(returnNgayGio(lopMonHocInfo.ngayThi));
        $('#diaDiemThi').html(lopMonHocInfo.diaDiemThi);
        setLoaiMon(lopMonHocInfo.monHoc.loaiMon);
        setSoCotDieuKien(lopMonHocInfo.monHoc.haiDiemDk);
        setHocKi(lopMonHocInfo.hocKi);
    }
    var updateInfoLopMonHoc = function (data) {
        lopMonHocInfo = data;
        addColumnsToTable(lopMonHocInfo.monHoc);
        addColumnsData(lopMonHocInfo.monHoc);
        initLichHocDataTable(lopMonHocInfo.lichHoc);
        updateDom(lopMonHocInfo);
    }
    var updateLopMonHocInput = function() {
        $('#tenLop-input').val(lopMonHocInfo.tenLop);
        $('#tenMonHoc-input').val(lopMonHocInfo.monHoc.tenMonHoc);
        $('#hocKi-input').val(lopMonHocInfo.hocKi);
        $('#ngay').val(returnDateForInput(lopMonHocInfo.ngayThi));
        $('#gio').val(returnHourForInput(lopMonHocInfo.ngayThi));
        $('#diaDiemThi-input').val(lopMonHocInfo.diaDiemThi);
    }
    var hienThiModalSuaLopMonHoc = function () {
        updateLopMonHocInput();
        $('#saveLopMonHocModal').modal('show');
    }
    var returnDateForSuaLopMonHoc = function () {
        return $("#ngay").val() + "T" + $("#gio").val();
    }
    var mapLopMonHocObject = function() {
        lopMonHocDto.lopId = lopMonHocId.lopId;
        lopMonHocDto.monHocId = lopMonHocId.monHocId;
        lopMonHocDto.ngayThi = returnDateForSuaLopMonHoc();
        lopMonHocDto.diaDiemThi = $('#diaDiemThi-input').val();
    }
    var updateAfterSuaLopMonHoc = function() {
        alert('Đã sửa đăng kí môn học của lớp');
        window.location.reload(false);
    }
    var saveLopMonHoc = function() {
        mapLopMonHocObject();
        quanLyLopMonHocService.suaLopMonHoc(lopMonHocDto, updateAfterSuaLopMonHoc);
    }

    //Func thêm, chỉnh sửa lịch học
    var resetInput = function () {
        $("#ngayBatDau-input").val(null);
        $("#ngayKetThuc-input").val(null);
        $("#thu246-input").val(1);
        $("#buoiSang-input").val(1);
        $("#baTietDau-input").val(1);
        $("#giaoVienDay-input").val("");
        $("#phongHoc-input").val("");
    }
    var updateLichHocInput = function (lichHoc) {
        $("#ngayBatDau-input").val(returnDateForInput(lichHoc.ngayBatDau));
        $("#ngayKetThuc-input").val(returnDateForInput(lichHoc.ngayKetThuc));
        $("#thu246-input").val(returnIntFromBoolean(lichHoc.thu246));
        $("#buoiSang-input").val(returnIntFromBoolean(lichHoc.buoiSang));
        $("#baTietDau-input").val(returnIntFromBoolean(lichHoc.baTietDau));
        $("#giaoVienDay-input").val(lichHoc.giaoVienDay);
        $("#phongHoc-input").val(lichHoc.phongHoc);
    }
    var setNgayKetThucMinDate = function () {
        var minDate = new Date($('#ngayBatDau-input').val());
        minDate.setDate(minDate.getDate() + 1);
        $('#ngayKetThuc-input').attr('min', returnDateForInput(minDate));
    }
    var hienThiModalThemLichHoc = function () {
        resetInput();
        lichHocId = 0;
        $('#saveLichHoc').html('Thêm lịch');
        $('#saveLichHocModal').modal('show');
    }
    var hienThiModalSuaLichHoc = function (e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        lichHocId = button.closest("tr").attr("id");
        quanLyLopMonHocService.getLichHocData(lichHocId, updateLichHocInput);
        $('#saveLichHoc').html('Thay đổi');
        $('#saveLichHocModal').modal('show');
    }
    var mapLichHocObject = function () {
        lichHocDto.id = lichHocId;
        lichHocDto.lopId = lopMonHocId.lopId;
        lichHocDto.monHocId = lopMonHocId.monHocId;
        lichHocDto.ngayBatDau = $("#ngayBatDau-input").val();
        lichHocDto.ngayKetThuc = $("#ngayKetThuc-input").val();
        lichHocDto.thu246 = parseInt($("#thu246-input").val());
        lichHocDto.buoiSang = parseInt($("#buoiSang-input").val());
        lichHocDto.baTietDau = parseInt($("#baTietDau-input").val());
        lichHocDto.giaoVienDay = $("#giaoVienDay-input").val();
        lichHocDto.phongHoc = $("#phongHoc-input").val();
    }
    var updateDanhSachLichHoc = function(data) {
        reloadTable(lichHocTable, data);
    }
    var updateAfterSaveLichHoc = function () {
        quanLyLopMonHocService.getDanhSachLichHoc(lopMonHocId, updateDanhSachLichHoc);
        if (lichHocId == 0) { alert("Đã thêm lịch học") }
        else { alert("Đã chỉnh sửa lịch học") }
        $('#saveLichHocModal').modal('hide');
    }
    var updateAfterXoaLichHoc = function () {
        quanLyLopMonHocService.getDanhSachLichHoc(lopMonHocId, updateDanhSachLichHoc);
        alert("Đã xóa lịch học.");
        $('#xoaLichHocModal').modal('hide');
    }
    var saveLichHoc = function () {
        mapLichHocObject();
        quanLyLopMonHocService.saveLichHoc(lichHocDto, updateAfterSaveLichHoc);
    }
    var hienThiModalXoaLichHoc = function(e) {
        var button = $(e.target);
        //Lấy id lịch học muốn xóa
        lichHocId = button.closest("tr").attr("id");
        $('#xoaLichHocModal').modal('show');
    }
    var xoaLichHoc = function() {
        quanLyLopMonHocService.xoaLichHoc(lichHocId, updateAfterXoaLichHoc);
    }

    //Init trang func
    var initTrang = function (dataSent) {
        lopMonHocId = dataSent;
        dataTableSetting();
        quanLyLopMonHocService.getLopMonHocData(lopMonHocId, updateInfoLopMonHoc);
        quanLyLopMonHocService.getDiemLopMonHoc(lopMonHocId, taoDiemSinhVien);
        $('#chinhSuaLopMonHoc').on('click', hienThiModalSuaLopMonHoc);
        $('body').on('click', '.suaLichHoc-js', hienThiModalSuaLichHoc);
        $('#themLichHoc').on('click', hienThiModalThemLichHoc);
        $('#ngayBatDau-input').on("focusout", setNgayKetThucMinDate);
        $('#saveLichHoc').on('click', saveLichHoc);
        $('body').on('click', '.xoaLichHoc-js', hienThiModalXoaLichHoc);
        $('#xoaLichHoc').on('click', xoaLichHoc);
        $('#saveLopMonHoc').on('click', saveLopMonHoc);
        $('#bangDiem').on('click', initializeXEditable);
    }

    //Các function public
    return {
        initTrang: initTrang
    }
}(QuanLyLopMonHocService);

var XemDiemMonHocSinhVienController = function (xemDiemMonHocSinhVienService) {
    var lopMonHoc, sinhVienVaDiem, monHocId, lichHocTable;

    var insertTenSinhVien = function (sinhVienVaDiem) {
        $('#tenSinhVien').html(sinhVienVaDiem.hoVaTenLot + ' ' + sinhVienVaDiem.ten);
    }
    var pushDiemCc = function (diem) {
        //Push cột data điểm chuyên cần
        $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.diemChuyenCan)+ '</td>');
    }
    var pushDiemDk = function (diem) {
        //Push cột data điểm điều kiện 1
        $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.diemDieuKien1) + '</td>');

        //Push cột data điểm điều kiện 2 nếu môn có 2 cột điều kiện
        if (diem.haiDiemDk == true) {
            $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.diemDieuKien2) + '</td>');
        }
    }
    var pushDiemThi = function (diem) { //Push cột điểm thi
        //Nếu loại môn là môn thường
        if (diem.loaiMon == 1) {
            $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.diemThi) + '</td>');
        }
        //Nếu loại môn là môn tiếng anh
        if (diem.loaiMon == 2) {
            //Cột thứ ba trong danhSachDiemBoSung là cột thi nói
            $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[2].diem) + '</td>');
            //Cột thứ hai trong danhSachDiemBoSung là cột thi nghe
            $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[1].diem) + '</td>');
            //Cột đàu tiên trong danhSachDiemBoSung là cột thi viết
            $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[0].diem) + '</td>');
        }

        //Nếu loại môn là môn CPĐT
        if (diem.loaiMon == 3) {
            //Cột thứ hai trong danhSachDiemBoSung là cột thi lý thuyết
            $('#duLieu').append('<td>' + diem.danhSachDiemBoSung[1].diem + '</td>');
            //Cột đầu tiên trong danhSachDiemBoSung là cột thi thực hành
            $('#duLieu').append('<td>' + diem.danhSachDiemBoSung[0].diem + '</td>');
        }
    }
    var pushDiemTb = function (diem) {
        $('#duLieu').append('<td>' + returnEmptyStringForNull(diem.diemTb) + '</td>');
    }
    var insertDiemSinhVien = function() {
        pushDiemCc(sinhVienVaDiem.diem);
        pushDiemDk(sinhVienVaDiem.diem);
        pushDiemThi(sinhVienVaDiem.diem);
        pushDiemTb(sinhVienVaDiem.diem);
    }
    var initLichHocDataTable = function (lichHoc) {
        lichHocTable = $("#lichHoc").DataTable({
            data: lichHoc,
            "order": [[0, 'desc']],
            columns: [
                { data: 'ngayBatDau' },
                { data: 'thu246' },
                { data: 'buoiSang' },
                { data: 'baTietDau' },
                { data: 'giaoVienDay' },
                { data: 'phongHoc' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    width: 200,
                    render: function (data, type, row) {
                        return returnThoiGianHoc(row);
                    }
                },
                {
                    targets: 1,
                    width: 100,
                    render: function (data, type, row) {
                        return returnNgayHoc(data);
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnBuoiHoc(data);
                    }
                },
                {
                    targets: 3,
                    width: 75,
                    render: function (data, type, row) {
                        return returnTietHoc(data);
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var insertLichThiLai = function (lichThiLai) {
        if (lichThiLai.thoiGianThi == null || lichThiLai.diaDiemThi == null) { return; }
        $('#thongTinThiLai').css('display', 'block');
        $('#ngayThiLai').html(returnNgayGio(lichThiLai.thoiGianThi));
        $('#diaDiemThiLai').html(lichThiLai.diaDiemThi);
    }
    var updateDom = function () {
        //Set thông tin môn học
        $('#tenMonHoc').html(sinhVienVaDiem.diem.tenMonHoc);
        $("#kyHieuMonHoc").html(sinhVienVaDiem.diem.kyHieuMonHoc);
        $("#soHocPhan").html(sinhVienVaDiem.diem.soHocPhan);
        $("#soTiet").html(sinhVienVaDiem.diem.soTiet);
        setLoaiMon(sinhVienVaDiem.diem.loaiMon);
        setSoCotDieuKien(sinhVienVaDiem.diem.haiDiemDk);
        insertLichThiLai(sinhVienVaDiem.lichThiLai);
        document.title = sinhVienVaDiem.diem.tenMonHoc + ' - NAPASTUDENT';
        //Set thông tin lớp môn học
        $('#tenLop').html(lopMonHoc.tenLop);
        $('#ngayThi').html(returnNgayGio(lopMonHoc.ngayThi));
        $('#diaDiemThi').html(lopMonHoc.diaDiemThi);
        setHocKi(lopMonHoc.hocKi);
    }
    var addColumnsToTable = function (diem) { //Func dùng để thêm cột vào table trong DOM
        if (diem.loaiMon == 1) { //Loại môn thường
            if (diem.haiDiemDk == true) { //Có 2 cột điều kiện
                $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi</th><th>TB</th>');
            }
            else { //Chỉ có 1 cột điều kiện
                $("#cotDiem").append('<th>CC</th><th>ĐK</th><th>Thi</th><th>TB</th>');
            }
        }
        if (diem.loaiMon == 2) {  //Loại môn TA
            $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th>' +
                '<th>Thi Nói</th><th>Thi Nghe</th><th>Thi Viết</th><th>TB</th>');
        }
        if (diem.loaiMon == 3) { //Loại môn CPĐT
            $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi LT</th><th>Thi TH</th><th>TB</th>');
        }
    }
    var bindData= function(data) {
        lopMonHoc = data.lopMonHoc;
        sinhVienVaDiem = data.sinhVien;
    }
    var updateThongTin = function (data) {
        bindData(data);
        initLichHocDataTable(lopMonHoc.lichHoc);
        addColumnsToTable(sinhVienVaDiem.diem);
        updateDom();
        insertDiemSinhVien();     
        insertTenSinhVien(sinhVienVaDiem);
    }

    var initTrang = function (idMonHoc) {
        monHocId = idMonHoc;
        dataTableSetting();
       xemDiemMonHocSinhVienService.getPageData(monHocId, updateThongTin);
  }
 
  return {
      initTrang: initTrang
  }  
}(XemDiemMonHocSinhVienService);

var XemDiemHocKiSinhVienController = function () {
    var sinhVienVaDiem, hocKi;
    var bindData = function (data) {
        sinhVienVaDiem = data;
    }
    var addHeaderToTable = function (diem) {
        if (diem.loaiMon === 1) {
            //Nếu là loại môn thường
            if (diem.haiDiemDk) {
                //Nếu có 2 cột điều kiện thì sẽ có tổng 5 cột điểm (CC,DK1,DK2,Thi,TB)
                $("#monHoc").append('<th colspan="5">' + diem.tenMonHoc + '</th>');
                $("#hocPhan").append('<th colspan="5">' + diem.soHocPhan + '</th>');
                $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi</th><th>TB</th>');
            } else {
                //Nếu có 1 cột điều kiện thì sẽ có tổng 4 cột điểm (CC,DK1,Thi,TB)
                $("#monHoc").append('<th colspan="4">' + diem.tenMonHoc + '</th>');
                $("#hocPhan").append('<th colspan="4">' + diem.soHocPhan + '</th>');
                $("#cotDiem").append('<th>CC</th><th>ĐK</th><th>Thi</th><th>TB</th>');
            }
        }
        if (diem.loaiMon === 2) {
            //Nếu là loại môn TA thì sẽ có tổng 7 cột điểm (CC,DK1,DK2,ThiNghe,ThiNoi,ThiViet,TB)
            $("#monHoc").append('<th colspan="7">' + diem.tenMonHoc + '</th>');
            $("#hocPhan").append('<th colspan="7">' + diem.soHocPhan + '</th>');
            $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi Nói</th><th>Thi Nghe</th>' +
                                 '<th>Thi Viết</th><th>TB</th>');
        }
        if (diem.loaiMon === 3) {
            //Nếu là loại môn CPĐT thì sẽ có tổng 6 cột điểm (CC,DK1,DK2,ThiTH,ThiLT,TB)
            $("#monHoc").append('<th colspan="6">' + diem.tenMonHoc + '</th>');
            $("#hocPhan").append('<th colspan="6">' + diem.soHocPhan + '</th>');
            $("#cotDiem").append('<th>CC</th><th>ĐK1</th><th>ĐK2</th><th>Thi LT</th><th>Thi TH</th><th>TB</th>');
        }
    }
    var addDiemToTable = function (diem) {
        $("#diem").append('<td>' + returnEmptyStringForNull(diem.diemChuyenCan) + '</td>');
        $("#diem").append('<td>' + returnEmptyStringForNull(diem.diemDieuKien1) + '</td>');
        if (diem.haiDiemDk) {
            //Nếu có 2 cột điều kiện
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.diemDieuKien2) + '</td>');
        }
        if (diem.loaiMon === 1) {
            //Nếu là loại môn thường
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.diemThi) + '</td>');
        }
        if (diem.loaiMon === 2) {
            //Nếu là loại môn TA
            //Cột thứ ba trong danhSachDiemBoSung là cột thi nói
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[2]) + '</td>');
            //Cột thứ hai trong danhSachDiemBoSung là cột thi nghe
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[1]) + '</td>');
            //Cột đàu tiên trong danhSachDiemBoSung là cột thi viết
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[0]) + '</td>');
        }
        if (diem.loaiMon === 3) {
            //Nếu là loại môn CPĐT
            //Cột thứ hai trong danhSachDiemBoSung là cột thi lý thuyết
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[1]) + '</td>');
            //Cột đầu tiên trong danhSachDiemBoSung là cột thi thực hành
            $("#diem").append('<td>' + returnEmptyStringForNull(diem.danhSachDiemBoSung[0]) + '</td>');
        }
        $("#diem").append('<td>' + returnEmptyStringForNull(diem.diemTb) + '</td>');
    }
    var updateThongTin = function() {
        $('#hocKi').html(hocKi);
        $('#tenSinhVien').html(sinhVienVaDiem.hoVaTenLot + ' ' + sinhVienVaDiem.ten);
        $('#mssv').html(sinhVienVaDiem.mssv);
    }
    var insertDiem = function() {
        sinhVienVaDiem.diem.forEach(function(diem) {
            addHeaderToTable(diem);
            addDiemToTable(diem);
        });
    }
    var updatePage = function (data) {
        bindData(data);
        insertDiem();
        updateThongTin();
    }
    var getData = function (hocKi, updatePage) {
        $.ajax({
            url: "/api/MonHoc/LayDiemHocKiSinhVien/" + hocKi,
            type: "GET",
            async: false,
            success: function (data) {
                updatePage(data);
            }
        });
    }

    var initTrang = function (hocKiModel) {
        hocKi = hocKiModel;
        getData(hocKi, updatePage);
    }
    return {
        initTrang : initTrang
    }
}();

var QuanLyThiLaiMonController = function (quanLyThiLaiService) {
    var monHocId,monHoc, lichThiLaiTable, danhSachSvPhaiThiLaiTable;

    var updateDom = function (monHocData) {
        monHoc = monHocData;
        $("#tenMonHoc").html(monHoc.tenMonHoc);
        $("#kyHieuMonHoc").html(monHoc.kyHieuMonHoc);
        $("#soHocPhan").html(monHoc.soHocPhan);
        $("#soTiet").html(monHoc.soTiet);
        setLoaiMon(monHoc.loaiMon);
        setSoCotDieuKien(monHoc.haiDiemDk);
    }
    var chuyenDenTrangQuanLyThiLai = function(e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        var thiLaiId = button.closest("tr").attr("id");
        window.location = '/MonHoc/QuanLyThiLai/' + thiLaiId;
    }
    var returnDaThiLai = function (diemSinhVien) {
        if (diemSinhVien.lichThiLaiId == null) return '<strong style = "color: red">Chưa có lịch thi lại</strong>';
        return '<button class="btn btn-success quanLyThiLai-js">Đã có lịch thi lại</button>';
    }
    var returnThiLaiDaKetThuc = function (daThiXong) {
        if (daThiXong) return '<strong style="color:blue">Đã kết thúc</strong>';
        return '<strong style="color:#22ff22">Đang thi</strong>';
    }
    var initDanhSachSinhVienPhaiThiLaiTable = function (monHocId) {
        danhSachSvPhaiThiLaiTable = $("#danhSachSvPhaiThiLai").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsSvPhaiThiLai/" + monHocId,
                dataSrc: ''
            },
            "order": [[0, 'desc']],
            columns: [
                { data: 'mssv' },
                { data: '' },
                { data: 'tenLop' },
                { data: 'diemTb' },
                { data: '' }
            ],
            rowId: "lichThiLaiId",
            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return row.hoVaTenLot + ' ' + row.ten;
                    }
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return returnDaThiLai(row);
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var initLichThiLaiTable = function (monHocId) {
        lichThiLaiTable = $("#danhSachLichThiLai").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsLichThiLai/" + monHocId,
                dataSrc: ''
            },
            "order": [[0, 'desc']],
            columns: [
                { data: 'thoiGianThi' },
                { data: 'diaDiemThi' },
                { data: 'daThiXong' },
                { data: 'id' }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return returnNgayGio(data);
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnThiLaiDaKetThuc(data);
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary quanLyThiLai-js">Quản lý</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }

    var initTrang = function (idMonHoc) {
        monHocId = idMonHoc;
        dataTableSetting();
        quanLyThiLaiService.layMonHoc(monHocId, updateDom);
        initDanhSachSinhVienPhaiThiLaiTable(monHocId);
        initLichThiLaiTable(monHocId);
        $('body').on('click', '.quanLyThiLai-js', chuyenDenTrangQuanLyThiLai);
    }

    return{
        initTrang: initTrang
    }
}(QuanLyThiLaiService);

var QuanLyThiLaiController = function (quanLyThiLaiService) {
    var bangDiemTable, danhSachSvPhaiThiLaiTable, lichThiLaiId, thongTinThiLai;
    var thongTinThiLaiDto = {}, dangKiThiLaiDto = {};
    
    //Chỉnh sửa điểm
    var columns = [
        { data: 'mssv' },
        { data: 'hoVaTenLot' },
        { data: 'ten' },
        { data: 'kyHieuTenLop' }
    ];
    var returnDiemThiLai = function (diem) {
        //Nếu đã thi lại rồi thì mới hiển thị điểm, vì chưa thi lại thì đó là điểm thi cũ
        if (diem !== null) {
            return diem;
        }
        return '';
    }
    var addColumnsToTable = function (monHoc) { //Func dùng để thêm cột vào table trong DOM
        if (monHoc.loaiMon == 1) {
            //Loại môn thường
            $("#cotDiem").append('<th>Thi</th>');
        }
        if (monHoc.loaiMon == 2) {
            //Loại môn TA
            $("#cotDiem").append('<th>Thi Nói</th><th>Thi Nghe</th><th>Thi Viết</th>');
        }
        if (monHoc.loaiMon == 3) {
            //Loại môn CPĐT
            $("#cotDiem").append('<th>Thi LT</th><th>Thi TH</th>');
        }
    }
    var pushDiemThi = function (monHoc) { //Push cột điểm thi
        //Nếu loại môn là môn thường
        if (monHoc.loaiMon == 1) {
            columns.push({
                data: 'diemThi',
                render: function (data) {
                    return returnDiemThiLai(data);
                },
                className: 'diemThi'
            });
        }
        //Nếu loại môn là môn tiếng anh
        if (monHoc.loaiMon == 2) {
            columns.push({
                data: 'danhSachDiemBoSung',  //data sẽ là array danhSachDiemBoSung
                render: function (data) {
                    return returnDiemThiLai(data[2].diem); //Cột thứ ba trong danhSachDiemBoSung(data) là cột thi nói
                },
                className: 'diemThiNoi',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData[2].id);
                }
            });
            columns.push({
                data: 'danhSachDiemBoSung',  //data sẽ là array danhSachDiemBoSung
                render: function (data) {
                    return returnDiemThiLai(data[1].diem);  //Cột thứ hai trong danhSachDiemBoSung(data) là cột thi nghe
                },
                className: 'diemThiNghe',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData[1].id);
                }
            });

            columns.push({
                data: 'danhSachDiemBoSung',  //data sẽ là array danhSachDiemBoSung
                render: function (data) {
                    return returnDiemThiLai(data[0].diem);  //Cột đầu tiên trong danhSachDiemBoSung(data) là cột thi viết
                },
                className: 'diemThiViet',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData[0].id);
                }
            });
        }

        //Nếu loại môn là môn CPĐT
        if (monHoc.loaiMon == 3) {
            columns.push({
                data: 'danhSachDiemBoSung',   //data sẽ là array danhSachDiemBoSung
                render: function (data) {
                    return returnDiemThiLai(data[1].diem);  //Cột thứ hai trong danhSachDiemBoSung(data) là cột thi lý thuyết
                },
                className: 'diemThiLT',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData[1].id);
                }
            });
            columns.push({
                data: 'danhSachDiemBoSung',  //data sẽ là array danhSachDiemBoSung
                render: function (data) {
                    return returnDiemThiLai(data[0].diem);   //Cột đầu tiên trong danhSachDiemBoSung(data) là cột thi thực hành
                },
                className: 'diemThiTH',
                createdCell: function (cell, cellData) {
                    $(cell).attr('id', cellData[0].id);
                }
            });
        }
    }
    var initializeXEditable = function () {
        var diemThiThuongEdit = function () {
            var value2Send;
            $(".diemThi").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/SetDiemThiLai',
                success: function (data) {
                    var currentCell = $(this);
                    bangDiemTable.cell(currentCell).data(data.diemThi).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = {};
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = thongTinThiLai.monHoc.id;
                    data.diemThi = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThiNoiEdit = function () {
            var value2Send;
            $(".diemThiNoi").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/SetDiemThiLai',
                success: function (data) {
                    var currentCell = $(this);
                    bangDiemTable.cell(currentCell).data(data.danhSachDiemBoSung).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = { danhSachDiemBoSung: [{}] };
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = thongTinThiLai.monHoc.id;
                    data.danhSachDiemBoSung[0].id = $(this).attr("id");
                    data.danhSachDiemBoSung[0].diem = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThiNgheEdit = function () {
            var value2Send;
            $(".diemThiNghe").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/SetDiemThiLai',
                success: function (data) {
                    var currentCell = $(this);
                    bangDiemTable.cell(currentCell).data(data.danhSachDiemBoSung).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = { danhSachDiemBoSung: [{}] };
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = thongTinThiLai.monHoc.id;
                    data.danhSachDiemBoSung[0].id = $(this).attr("id");
                    data.danhSachDiemBoSung[0].diem = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThiVietEdit = function () {
            var value2Send;
            $(".diemThiViet").editable({
                type: 'number',
                mode: "inline",
                showbuttons: false,
                pk: 1,
                url: '/api/MonHoc/SetDiemThiLai',
                success: function (data) {
                    var currentCell = $(this);
                    bangDiemTable.cell(currentCell).data(data.danhSachDiemBoSung).draw();
                },
                ajaxOptions: {
                    type: 'put',
                    dataType: 'json'
                },
                params: function () {
                    var data = { danhSachDiemBoSung: [{}] };
                    data.sinhVienId = $(this).closest("tr").attr("id");
                    data.monHocId = thongTinThiLai.monHoc.id;
                    data.danhSachDiemBoSung[0].id = $(this).attr("id");
                    data.danhSachDiemBoSung[0].diem = value2Send;
                    return data;
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var diemThucHanhEdit = function () {
            var value2Send;
        }
        var diemLyThuyetEdit = function () {
            var value2Send;
        }
        var diemThiEdit = function (monHoc) {
            //Nếu loại môn là môn thường
            if (monHoc.loaiMon == 1) {
                diemThiThuongEdit();
            }
            //Nếu loại môn là môn tiếng anh
            if (monHoc.loaiMon == 2) {
                diemThiNoiEdit();
                diemThiNgheEdit();
                diemThiVietEdit();
            }
            //Nếu loại môn là môn CPĐT
            if (monHoc.loaiMon == 3) {
                diemThucHanhEdit();
                diemLyThuyetEdit();
            }
        }

        diemThiEdit(thongTinThiLai.monHoc);
    }
    var initDiemSinhVienDataTable = function (lichThiLaiId) {
        bangDiemTable = $("#bangDiem").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsDiemThiLaiSv/" + lichThiLaiId,
                dataSrc: ''          
            },
            "order": [[0, 'asc']],
            columns: columns,
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 75
                },
                {
                    targets: 1,
                    width: 150
                },
                {
                    targets: 2,
                    width: 100
                }


            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var taoBangDiem = function () {
        dataTableSetting();
        addColumnsToTable(thongTinThiLai.monHoc);
        pushDiemThi(thongTinThiLai.monHoc);
        initDiemSinhVienDataTable(lichThiLaiId);
    }
    //Danh sách sinh viên phải thi lại và đăng kí lịch thi lại
    var initDanhSachSvPhaiThiLaiTable = function (monHocId) {
        danhSachSvPhaiThiLaiTable = $("#danhSachSvPhaiThiLai").DataTable({
            ajax: {
                url: "/api/MonHoc/LayDsSvChuaCoLichThiLai/" + monHocId,
                dataSrc: ''
            },
            "order": [[0, 'asc']],
            columns: [
                {
                    data: 'mssv'
                },
                {
                    data: 'hoVaTenLot',
                    render : function(data,type, row) {
                        return returnTenSinhVien(row);
                    }
                },
                {
                    data: 'kyHieuTenLop'
                },
                {
                    data: 'diemTb'
                },
                {
                    data: 'id',
                    render: function() {
                        return '<button class="btn btn-success dangKiThiLai">Đăng kí</button>';
                    }
                }
            ],
            rowId: "id",
            rowCallback: function (row, data) {

            }
        });
    }
    var taoBangDsSvPhaiThiLai = function () {
        if (thongTinThiLai.daThiXong) return; //Nếu thi lại đã kết thúc thì không hiển thị danh sách sv để đăng kí thi lại
        $('#danhSachSvPhaiThiLaiHolder').css('display', 'block');
        initDanhSachSvPhaiThiLaiTable(thongTinThiLai.monHoc.id);
    }
    var updatePageAfterDangKi = function() {
        bangDiemTable.ajax.reload();
        danhSachSvPhaiThiLaiTable.ajax.reload();

    }
    var dangKiLichThiLaiChoSv = function(e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        dangKiThiLaiDto.sinhVienId = button.closest("tr").attr("id");
        quanLyThiLaiService.dangKiLichThiLai(dangKiThiLaiDto, updatePageAfterDangKi);
    }
    //Update Dom và bind data
    var bindData = function(data) {
        thongTinThiLai = data;
        dangKiThiLaiDto.lichThiLaiId = lichThiLaiId;
        dangKiThiLaiDto.monHocId = thongTinThiLai.monHoc.id;
    }
    var returnTinhTrangThiLai = function(daThiXong) {
        if (daThiXong) {
            $('#tinhTrangThiLai').css('color', 'goldenrod');
            $('#tinhTrangThiLai').html('Đã kết thúc');
        } else {
            $('#tinhTrangThiLai').css('color', '#22ff22');
            $('#tinhTrangThiLai').html('Đang thi');
        }
    }
    var setLinkQuanLyThiLaiMonHoc = function (monHocId) {
        $('#linkQuanLyThiLaiMonHoc').attr('href', '/MonHoc/QuanLyThiLaiMon/' + monHocId);
    }
    var setThongTinThiLai = function() {
        $('#tenMonHoc').html(thongTinThiLai.monHoc.tenMonHoc);
        setLoaiMon(thongTinThiLai.monHoc.loaiMon);
        $('#ngayThi').html(returnNgayGio(thongTinThiLai.thoiGianThi));
        $('#diaDiemThi').html(thongTinThiLai.diaDiemThi);
        returnTinhTrangThiLai(thongTinThiLai.daThiXong);
        setLinkQuanLyThiLaiMonHoc(thongTinThiLai.monHoc.id);
    }
    var updatePage = function (data) {
        bindData(data);
        taoBangDiem(); //Tạo bảng điểm của sinh viên thi lại
        taoBangDsSvPhaiThiLai();
        setThongTinThiLai();
    }
    //Kết thúc thi lại
    var hienThiModalKetThucThiLai = function() {
        if (thongTinThiLai.daThiXong) {
            alert('Kì thi đã kết thúc từ trước.');
        } else {
            $('#ketThucThiLaiModal').modal('show');
        }
    }
    var updateAfterKetThucThiLai = function() {
        alert('Đã kết thúc kì thì lại.');
        window.location.reload(true);
    }
    var ketThucThiLai = function() {
        quanLyThiLaiService.ketThucThiLai(lichThiLaiId, updateAfterKetThucThiLai);
    }
    //Save lịch thi lại
    var hienThiModalSaveLichThiLai = function () {
        $('#ngay').val(returnDateForInput(thongTinThiLai.thoiGianThi));
        $('#gio').val(returnHourForInput(thongTinThiLai.thoiGianThi));
        $('#diaDiemThi-input').val(thongTinThiLai.diaDiemThi);
        $('#saveLichThiLaiModal').modal('show');
    }
    var updateAfterSaveLichThiLai = function() {
        alert('Đã chỉnh sửa thông tin');
        window.location.reload(true);
    }
    var saveLichThiLai = function() {
        thongTinThiLaiDto.id = lichThiLaiId;
        thongTinThiLaiDto.monHocId = thongTinThiLai.monHoc.id;
        thongTinThiLaiDto.diaDiemThi = $('#diaDiemThi-input').val();
        thongTinThiLaiDto.thoiGianThi = $("#ngay").val() + "T" + $("#gio").val();
        quanLyThiLaiService.saveLichThiLai(thongTinThiLaiDto, updateAfterSaveLichThiLai);
    }

    var initTrang = function (idLichThiLai) {
        lichThiLaiId = idLichThiLai;
        quanLyThiLaiService.layThongTinLichThiLai(lichThiLaiId, updatePage);
        $('#bangDiem').on('click', initializeXEditable);
        $('body').on('click', '.dangKiThiLai', dangKiLichThiLaiChoSv);
        $('#chinhSuaNgayThi').on('click', hienThiModalSaveLichThiLai);
        $('#saveLichThiLai').on('click', saveLichThiLai);
        $('#ketThucThiLaiBtn').on('click', hienThiModalKetThucThiLai);
        $('#ketThucThiLai').on('click', ketThucThiLai);
    }

    return {
        initTrang: initTrang
    }
}(QuanLyThiLaiService) ;