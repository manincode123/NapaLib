var validateHoatDongForm = function() {
    $.validator.addMethod("require_from_group_multiselect2", function (value, element, options) {
        //Nếu không có đơn vị nào tổ chức thì phải là hoạt động ngoài học viện
        //Hoạt động trong học viện phải có ít nhất 1 đơn vị tổ chức
        if ($("#selectLopToChuc").val().length + $("#selectDonViToChuc").val().length < 1) {
            return $("#hdNgoaiHocVien-checkbox").prop("checked");
        }
        return true;
    }, $.validator.format("Please fill at least {0} of these fields."));
    $.validator.addMethod("validateThoiGianDienRaHoatDong", function (value, element, options) {
        //Thời gian kết thúc hoạt động phải trễ hơn thời gian bắt đầu hoạt động
        var ngayBatDau = new Date(returnDateFromInput($("#gioBatDau").val(), $("#ngayBatDau").val()));
        var ngayKetThuc = new Date(returnDateFromInput($("#gioKetThuc").val(), $("#ngayKetThuc").val()));
        return ngayKetThuc > ngayBatDau;
    }, $.validator.format("Thời gian kết thúc hoạt động không thể sớm hơn thời gian bắt đầu hoạt động."));
    $("#hoatDongForm").validate({
        ignore: "input[type=hidden]",
        rules: {
            tenHoatDong: "required",
            soLuoc: "required",
            diaDiem: "required",
            noiDung: {
                required: function (textarea) {
                    CKEDITOR.instances[textarea.id].updateElement(); // update textarea
                    var editorcontent = textarea.value.replace(/<[^>]*>/gi, ''); // strip tags
                    return editorcontent.length === 0;
                }
            },
            ngayBatDau: {
                required: true,
                validateThoiGianDienRaHoatDong : true
            },
            gioBatDau: {
                required: true,
                validateThoiGianDienRaHoatDong : true
            },
            ngayKetThuc: {
                required: true,
                validateThoiGianDienRaHoatDong: true
            },
            gioKetThuc: {
                required: true,
                validateThoiGianDienRaHoatDong: true
            },
            selectLopToChuc: "require_from_group_multiselect2",
            selectDonViToChuc: "require_from_group_multiselect2"
        },
        messages: {
            tenHoatDong: "Vui lòng nhập tên hoạt động",
            soLuoc: "Vui lòng nhập sơ lược hoạt động",
            noiDung: "Vui lòng nhập nội dung hoạt động",
            diaDiem: "Vui lòng nhập địa điểm diễn ra hoạt động",
            ngayBatDau: {
                required: "Vui lòng nhập ngày bắt đầu của hoạt động"
            },
            gioBatDau: {
                required: "Vui lòng nhập giờ bắt đầu của hoạt động"
            },
            ngayKetThuc: {
                required: "Vui lòng nhập ngày kết thúc của hoạt động"
            },
            gioKetThuc: {
                required: "Vui lòng nhập giờ kết thúc của hoạt động"
            },
            selectLopToChuc: {
                require_from_group_multiselect2: "Vui lòng chọn ít nhất 1 đơn vị/lớp tổ chức"
            },
            selectDonViToChuc: {
                require_from_group_multiselect2: "Vui lòng chọn ít nhất 1 đơn vị/lớp tổ chức"
            }
        },
        highlight: function (element, errorClass, validClass) {
            if ($(element).hasClass("select2-hidden-accessible")) {
                $(element).siblings(".select2").addClass("has-error").removeClass("has-success");
                $(element).parents(".form-group").addClass("has-error").removeClass("has-success");
            } else {
                $(element).addClass("is-invalid").removeClass("is-valid");
                $(element).parents(".form-group").addClass("has-error").removeClass("has-success");
            }
        },
        //When removing make the same adjustments as when adding
        unhighlight: function (element, errorClass, validClass) {
            if ($(element).hasClass("select2-hidden-accessible")) {
                $(element).siblings(".select2").addClass("has-success").removeClass("has-error");
                $(element).parents(".form-group").removeClass("has-error").addClass("has-success");
            } else {
                $(element).addClass("is-valid").removeClass("is-invalid");
                $(element).parents(".form-group").addClass("has-success").removeClass("has-error");
            }
        }
    });

}
var QuanLyHoatDongController = function (quanLyHoatDongService) {
    var hdDangDienRaTable;
    var danhSachHdDangDienRa;
    var hdNamNayTable;
    var danhSachHdNamNay;
    var hdThangNayTable;
    var danhSachHdThangNay;
    var luotThamGiaThangNay, luotThamGiaNamNay, soHoatDongTcNamNay, soHoatDongTcThangNay, soHoatDongDangDienRa,soHdChoPheDuyet;

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
    var initHdDangDienRaTable = function () {
        hdDangDienRaTable = $("#hdDangDienRaTable-TrangQuanLyHd").DataTable({
            data: [],
            order: [[1, "desc"]],
            searching: false,
            lengthChange: false,
            columns: [
                { data: 'tenHoatDong' },
                { data: 'ngayBatDau' },
                { data: 'ngayKetThuc' },
                { data: 'soLuotThamGia' },
                { data: 'danhSachDonViToChuc' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 2,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return taoDonViToChuc(row);
                    }
                }
            ]
        });
    }
    var initHdNamNayTable = function () {
        hdNamNayTable = $("#hdNamNayTable-TrangQuanLyHd").DataTable({
            data: [],
            "order": [[5, 'asc']],
            columns: [
                { data: 'tenHoatDong' },
                { data: 'ngayBatDau' },
                { data: 'ngayKetThuc' },
                { data: 'soLuotThamGia' },
                { data: 'danhSachDonViToChuc' },
                { data: 'daKetThuc' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 2,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return taoDonViToChuc(row);
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return taoTinhTrang(row);
                    }
                }
            ]
        });
    }
    var initHdThangNayTable = function () {
        hdThangNayTable = $("#hdThangNayTable-TrangQuanLyHd").DataTable({
            data: [],
            order: [[5, 'asc'],[1,"desc"]],
            columns: [
                { data: 'tenHoatDong' },
                { data: 'ngayBatDau' },
                { data: 'ngayKetThuc' },
                { data: 'soLuotThamGia' },
                { data: 'danhSachDonViToChuc' },
                { data: 'daKetThuc' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 2,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return taoDonViToChuc(row);
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return taoTinhTrang(row);
                    }
                }
            ]
        });
    }
    var hienThisoHoatDongChoPheDuyet = function() {
        if (soHdChoPheDuyet > 0) {
            $("#soHdChoPheDuyetBdg").css("display", "inline-block");
            return soHdChoPheDuyet;
        }
    }
    var trangQuanLyHdMap = function (data) {
        danhSachHdDangDienRa = data.hoatDongDangDienRa;
        danhSachHdNamNay = data.hoatDong;
        danhSachHdThangNay = data.hoatDongThangNay;
        luotThamGiaThangNay = data.luotThamGiaThangNay;
        luotThamGiaNamNay = data.luotThamGiaNamNay;
        soHoatDongTcNamNay = data.soHoatDongTcNamNay;
        soHoatDongTcThangNay = data.soHoatDongTcThangNay;
        soHoatDongDangDienRa = data.soHoatDongDangDienRa;
        soHdChoPheDuyet = data.soHoatDongChoPheDuyet;
    }
    var updateTrang = function () {
        reloadTable(hdDangDienRaTable, danhSachHdDangDienRa);
        reloadTable(hdNamNayTable, danhSachHdNamNay);
        reloadTable(hdThangNayTable, danhSachHdThangNay);
        $("#hdNamNay-TrangQuanLyHd").append(soHoatDongTcNamNay);
        $("#hdThangNay-TrangQuanLyHd").append(soHoatDongTcThangNay);
        $("#soHdDangDienRa-TrangQuanLyHd").append(soHoatDongDangDienRa);
        $("#thamGiaThangNay-TrangQuanLyHd").append(luotThamGiaThangNay);
        $("#thamGiaNamNay-TrangQuanLyHd").append(luotThamGiaNamNay);
        $("#soHdChoPheDuyetBdg").append(hienThisoHoatDongChoPheDuyet());
    }

    //Phần code cho modal Tạo,chỉnh sửa hoạt động
    var hoatDong = {};;
    var danhSachLopToChuc = [];
    var danhSachDonViToChuc = [];
    var insertDataToList = function (danhSachLop, danhSachDonVi) {
        danhSachLopToChuc = [];
        danhSachDonViToChuc = [];
        danhSachLop.forEach(function (lop) {
            //Add object vào list
            danhSachLopToChuc.push(lop);
        });
        danhSachDonVi.forEach(function (donVi) {
            //Add object vào list
            danhSachDonViToChuc.push(donVi);
        });
    }
    var initSelectList = function (danhSachLop, danhSachDonVi) {
        insertDataToList(danhSachLop, danhSachDonVi);
        $('#selectDonViToChuc').select2({
            language: "vi",
            data: danhSachDonViToChuc
        });
        $('#selectLopToChuc').select2({
            language: "vi",
            data: danhSachLopToChuc
        });
    }
    var insertHtmlToSaveHoatDongModal = function (data) {
        $("#themHoatDongModal-body").html(data);
    }
    var initTextEditor = function () {
        CKEDITOR.replace("noiDung");
        CKFinder.setupCKEditor(null, '/ckfinder');
    } //Text editor cho nội dung hoạt động
    var hienThiModalSaveHoatDong = function () {
        $("#themHoatDongModal-title").html("Thêm hoạt động");
        $("#themHoatDongBtn").html("Thêm hoạt động");
        quanLyHoatDongService.loadSaveHoatDongHtml(insertHtmlToSaveHoatDongModal);
        quanLyHoatDongService.getSelectListData(initSelectList);
        limitCharacterForInput("tenHoatDong");
        limitCharacterForInput("soLuoc");
        limitCharacterForInput("diaDiem");
        initTextEditor();
        validateHoatDongForm();
        $("#themHoatDongModal").modal("show");
    }
    var initChonAnhBia = function () {
        $("body").on("click", "#selectAnhBiaButton", function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $("#anhBiaHoatDong-input").attr("src", fileUrl);
            }
            finder.popup();
        });
    }
    var mapHoatDongObject = function () {
        hoatDong = {};
        hoatDong.tenHoatDong = $("#tenHoatDong").val();
        hoatDong.soLuoc = $("#soLuoc").val();
        hoatDong.noiDung = CKEDITOR.instances['noiDung'].getData();
        hoatDong.ngayBatDau = returnDateFromInput($("#gioBatDau").val(), $("#ngayBatDau").val());
        hoatDong.ngayKetThuc = returnDateFromInput($("#gioKetThuc").val(), $("#ngayKetThuc").val());
        hoatDong.danhSachDonViToChuc = returnIntArray($("#selectDonViToChuc").val());
        hoatDong.danhSachLopToChuc = returnIntArray($("#selectLopToChuc").val());
        hoatDong.hoatDongNgoaiHocVien = $("#hdNgoaiHocVien-checkbox").prop("checked");
        hoatDong.capHoatDong = $("#capHoatDong-input").val();
        hoatDong.anhBia = $("#anhBiaHoatDong-input").attr("src");
        hoatDong.diaDiem = $("#diaDiem").val();
        hoatDong.id = $("#hoatDongId").val();
    }
    var saveHoatDong = function (e) {
        if ($("#hoatDongForm").valid()) {
            e.preventDefault();
            showLoader();
            mapHoatDongObject();
            quanLyHoatDongService.saveHoatDong(hoatDong, reloadPage);
        } else {
            alert("Hãy nhập đúng các thông tin cần thiết.");
        }
    }
    var ketThucHoatDong = function() {
        var hoatDongId = $("#hoatDongId").val();
        quanLyHoatDongService.ketThucHoatDong(hoatDongId, reloadPage);
    }
    var huyHoatDong = function () {
        var hoatDongId = $("#hoatDongId").val();
        quanLyHoatDongService.huyHoatDong(hoatDongId, reloadPage);
    }
    var moLaiHoatDong = function () {
        var hoatDongId = $("#hoatDongId").val();
        quanLyHoatDongService.moLaiHoatDong(hoatDongId, reloadPage);
    }
    var khoiPhucHoatDong = function () {
        var hoatDongId = $("#hoatDongId").val();
        quanLyHoatDongService.khoiPhucHoatDong(hoatDongId, reloadPage);
    }

    var initTrangQuanLyHoatDong = function () {
        dataTableSetting();
        initHdDangDienRaTable();
        initChonAnhBia();
        initHdNamNayTable();
        initHdThangNayTable();
        quanLyHoatDongService.quanLyHoatDong(trangQuanLyHdMap, updateTrang);
        $("#themHoatDong").on("click", hienThiModalSaveHoatDong);
        $("#themHoatDongBtn").on("click", saveHoatDong);

    }

    return {
        initTrangQuanLyHoatDong: initTrangQuanLyHoatDong,
        initSelectList: initSelectList,
        saveHoatDong: saveHoatDong,
        ketThucHoatDong: ketThucHoatDong,
        huyHoatDong: huyHoatDong,
        moLaiHoatDong: moLaiHoatDong,
        khoiPhucHoatDong: khoiPhucHoatDong,
        hienThiModalSaveHoatDong: hienThiModalSaveHoatDong,
        mapHoatDongObject: mapHoatDongObject,
        hienThisoHoatDongChoPheDuyet: hienThisoHoatDongChoPheDuyet
    }

}(QuanLyHoatDongService);

var HoatDongController = function (hoatDongService, quanLyHoatDongService, quanLyHoatDongController) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var hoatDong;
    var daTaoDanhSachTg = false;
    var danhSachSvThamGia;
    var danhSachSvTgTable;
    var DanhSachChuongTrinhHoatDong;
    var ChuongTrinhHoatDongTable;
    var chuongTrinhHoatDong ={};
    var idCtrMuonXoa;
    /*Validation*/
    var limitCharacter = function () {
        //Hạn chế số kí tự tiêu đề chương trình
        limitCharacterForInput("tieuDe");
        //Hạn chế số kí tự phần nội dung chương trình
        limitCharacterForInput("noiDungChuongTrinh");
        //Các hạn chế số lượng kí tự cho tên hoạt động, sơ lược, và địa điểm được init khi hiển thị modal
        //Vì nếu không jquery sẽ ko bắt được element mới tạo sau
    }
    var validateForm = function () {
        $("#chuongTrinhHoatDongForm").validate({
            rules: {
                tieuDe: "required",
                noiDungChuongTrinh: "required",
                ngayChuongTrinh: "required",
                gioChuongTrinh : "required"
            },
            messages: {
                tieuDe: "Vui lòng nhập tiêu đề chương trình",
                noiDungChuongTrinh: "Vui lòng nhập nội dung chương trình",
                ngayChuongTrinh: "Vui lòng nhập ngày của chương trình",
                gioChuongTrinh: "Vui lòng nhập giờ của chương trình khi đã chọn hiển thị giờ"
            }
        });
    }

    function returnNgay(time) {
        return moment(time).format("DD");
    }
    function returnThang(time) {
        return moment(time).format("MM");
    }
    function returnNam(time) {
        return moment(time).format("YYYY");
    }
    function returnGio(time) {
        return moment(time).format("HH:mm");
    }
    
    //Phần để update và quản lý dom
    function returnTinhTrangKetThucForDom() {
        if (!hoatDong.biHuy) {
            if (hoatDong.daKetThuc) {
                return "Đã kết thúc vào";
            }
            return "Dự kiến kết thúc vào";
        }
    }
    var returnThoiGianKetThuc = function (time) {
        if (!hoatDong.biHuy) {
            return moment(time).format("HH:mm DD/MM/YYYY");
        }
    }
    function returnDonViToChucForDom() {
        var object = '';
        hoatDong.donViToChuc.forEach(function (donVi) {
            object += '<a class="linkDonVi-TrangChiTietHd" href="/DonVi/HoatDong/' + donVi.donViId + '">' + donVi.tenDonVi + '</a>';
        });
        hoatDong.lopToChuc.forEach(function (lop) {
            object += '<a class="linkDonVi-TrangChiTietHd" href="/Lop/HoatDong/' + lop.lopId + '">' + lop.tenLop + '</a>';
        });
        return object;
    }
    function initDanhSachSvThamGiaTable_QuanLy() {
        if (!daTaoDanhSachTg) {
            danhSachSvTgTable = $("#danhSachSvTg").DataTable({
                ajax: {
                    url: "/api/HoatDong/DanhSachThamGia/" + hoatDong.id,
                    headers: { __RequestVerificationToken: csrfToken },
                    dataSrc: ""
                },
                autoWidth: false,
                dom: "Bflrtip",
                buttons: [
                    "copy", "excel", "pdf"
                ],
                columns: [
                    { data: "ten" },
                    { data: "mssv" },
                    { data: "sdt" },
                    { data: "kyHieuTenLop" },
                    { data: "ngayThamGia" },
                    { data: "duocPheDuyet"}
                ],
                columnDefs: [
                    {
                        targets: 0,
                        render: function(data, type, row) {
                            return returnTenSinhVienWithLink(row);
                        }
                    },
                    {
                        targets: 4,
                        render: function(data) {
                            return moment(data).format("DD/MM/YYYY HH:mm");
                        }
                    },
                    {
                        targets: 5,
                        render: function(data) {
                            if (data) return "Được phê duyệt";
                            else return "Đã đăng kí";
                        }
                    }
                ]
            });
            daTaoDanhSachTg = true;
        }
        $('#danhSachSvTg-Wrapper').collapse("toggle");
    }
    function initDanhSachSvThamGiaTable_Khach() {
        if (!daTaoDanhSachTg) {
            //Tạo bảng
            danhSachSvTgTable = $("#danhSachSvTg").DataTable({
                ajax: {
                    url: "/api/HoatDong/DanhSachThamGia/" + hoatDong.id,
                    headers: { __RequestVerificationToken: csrfToken },
                    dataSrc: ""
                },
                autoWidth: false,
                dom: "Bflrtip",
                buttons: [
                    "copy", "excel", "pdf"
                ],
                columns: [
                    { data: "" },
                    { data: "mssv" },
                    { data: "kyHieuTenLop" },
                    { data: "duocPheDuyet" }
                ],
                columnDefs: [
                    {
                        targets: 0,
                        render: function (data, type, row) {
                            return returnTenSinhVienWithLink(row);
                        }
                    },
                    {
                        targets: 3,
                        render: function (data) {
                            if (data) return "Được phê duyệt";
                            else return "Đã đăng kí";
                        }
                    }
                ]
            });
            daTaoDanhSachTg = true;
        }
        $('#danhSachSvTg-Wrapper').collapse("toggle");
    }
    var mapObject = function (data) {
        //Map dữ liệu trả về với các biến
        hoatDong = data;
        danhSachSvThamGia = data.danhSachSvThamGia;
        if (data.danhSachChuongTrinhHoatDong != null)
            DanhSachChuongTrinhHoatDong = data.danhSachChuongTrinhHoatDong;
    }
    var themIconCapHoatDong = function(capHoatDong) {
        var iconCapHoatDong = _.template($("#iconCapHoatDong_Template").html());
        $("#tenHoatDong-TrangChiTietHd").before(iconCapHoatDong({ capHoatDong: capHoatDong }));
    }
    var updateThamGiaTheoDoiButton = function () {
        if (hoatDong.coThamGia) { //Nếu có tham gia
            if (new Date(hoatDong.ngayKetThuc) > new Date()) { //Nếu có tham gia và hoạt động chưa kết thúc
                $('#DangKiHoatDongBtnText').html('Đang tham gia');
                $('#DangKiHoatDongBtn').removeClass('daDangKi');
            } else { //Nếu có tham gia và hoạt động đã kết thúc
                $('#DangKiHoatDongBtnText').html('Đã tham gia');
                $('#DangKiHoatDongBtn').removeClass('daDangKi');
            }
        } else { //Nếu chưa tham gia
            if (hoatDong.coDangKi) {  //Nếu đã đăng kí tham gia
                $('#DangKiHoatDongBtnText').html('Đã đăng kí');
                $('#DangKiHoatDongBtn').addClass('daDangKi');
            } else { //Nếu chưa đăng kí
                $('#DangKiHoatDongBtnText').html('Đăng kí');
                $('#DangKiHoatDongBtn').removeClass('daDangKi');
            }
        }

        if (hoatDong.coTheoDoi) {
            $('#TheoDoiHoatDongBtnText').html('Đang theo dõi');
            $('#TheoDoiHoatDongBtn').addClass('daTheoDoi');
        } else {
            $('#TheoDoiHoatDongBtnText').html('Theo dõi');
            $('#TheoDoiHoatDongBtn').removeClass('daTheoDoi');
        }
    }
    var themThongTinHdVaoDom = function () {
        $("#anhBiaHoatDong").attr("src", hoatDong.anhBia);
        $("#tenHoatDong-TrangChiTietHd").html(hoatDong.tenHoatDong);
        $("#ngayDienRa-TrangChiTietHd").html(returnNgay(hoatDong.ngayBatDau));
        $("#thangDienRa-TrangChiTietHd").html(returnThang(hoatDong.ngayBatDau));
        $("#namDienRa-TrangChiTietHd").html(returnNam(hoatDong.ngayBatDau));
        $("#gioDienRa-TrangChiTietHd").html(returnGio(hoatDong.ngayBatDau));
        $("#soLuotThamGia-TrangChiTietHd").html(hoatDong.soLuotThamGia);
        $("#tinhTrang-TrangChiTietHd").html(taoTinhTrang(hoatDong));
        $("#tinhTrangKetThuc-TrangChiTietHd").html(returnTinhTrangKetThucForDom());
        $("#tgKetThuc-TrangChiTietHd").html(returnThoiGianKetThuc(hoatDong.ngayKetThuc));
        $("#danhSachDonViToChuc-TrangChiTietHd").html(returnDonViToChucForDom());
        $("#diaDiem-TrangChiTietHd").html(hoatDong.diaDiem);
        $("#soLuocHoatDong").html(hoatDong.soLuoc);
        $("#noiDungHoatDong").html(hoatDong.noiDung);
        $("#hoatDongId").val(hoatDong.id);
        themIconCapHoatDong(hoatDong.capHoatDong);
        updateThamGiaTheoDoiButton();
        document.title = hoatDong.tenHoatDong;
        if (DanhSachChuongTrinhHoatDong.length != 0) {
            initChuongTrinhHoatDong();
        } else {
            $("#chuongTrinhHoatDong").html("<div>Chưa có chương trình nào.</div>");
        }

    }
    //Theo dõi, đăng kí hoạt động
    var hienThiModalHuyDangKi = function() {
        $('#thongBaoModal-body').html("Bạn có chắc muốn hủy đăng kí hoạt động này?");
        $("#huyDangKi").css("display", "inline-block");
        $("#huyTheoDoi").css("display", "none");
        $("#thongBaoModal").modal("show");
    }
    var hienThiModalHuyTheoDoi = function () {
        $('#thongBaoModal-body').html("Bạn có chắc muốn hủy theo dõi hoạt động này?");
        $("#huyTheoDoi").css("display", "inline-block");
        $("#huyDangKi").css("display", "none");
        $("#thongBaoModal").modal("show");
    }
    var updateAfterDangKi_HuyDangKi = function (dangKi,error) {
        if (error != null) {
            alert(error);
        } else {
            if (dangKi) {
                hoatDong.coDangKi = true;
                hoatDong.coTheoDoi = true;
                alert("Đã đăng kí");
            } else {
                hoatDong.coDangKi = false;
                hoatDong.coTheoDoi = false;
                alert("Đã hủy đăng kí");
            }
            updateThamGiaTheoDoiButton();
            $("#thongBaoModal").modal("hide");
        }
    }
    var dangKiHoatDong = function () {
        hoatDongService.dangKiHoatDong(hoatDong.id, updateAfterDangKi_HuyDangKi);
    }
    var huyDangKi = function () {
        hoatDongService.huyDangKi(hoatDong.id, updateAfterDangKi_HuyDangKi);
    }
    var dangKiHoatDongHandler = function() {
        if (hoatDong.coDangKi) {
            hienThiModalHuyDangKi();
        } else {
            dangKiHoatDong();
        }
    }
    var updateAfterTheoDoi_BoTheoDoi = function (theoDoi) {
        if (theoDoi) { //Nếu theo dõi
            hoatDong.coTheoDoi = true;
            alert("Đã đăng kí theo dõi");
        } else { //Nếu bỏ theo dõi
            hoatDong.coTheoDoi = false;
            alert("Đã bỏ theo dõi");
        }         
        updateThamGiaTheoDoiButton();
        $('#thongBaoModal').modal('hide');
    }
    var theoDoiHoatDong = function () {
        hoatDongService.theoDoiHoatDong(hoatDong.id, updateAfterTheoDoi_BoTheoDoi);
    }
    var huyTheoDoi = function () {
        hoatDongService.huyTheoDoi(hoatDong.id, updateAfterTheoDoi_BoTheoDoi);
    }
    var theoDoiHoatDongHandler = function() {
        if (hoatDong.coTheoDoi) {
            hienThiModalHuyTheoDoi();
        } else {
            theoDoiHoatDong();
        }
    }

    //Phần thêm, chỉnh sửa chương trình
    var resetChuongTrinhInput = function() {
        setDataForInputWithLimit("tieuDe", "");
        setDataForInputWithLimit("noiDungChuongTrinh", "");
        $("#gio-checkbox").prop("checked", false);
        $("#gioChuongTrinh").prop("disabled", true);
        $("#ngayChuongTrinh").val(null);
        $("#gioChuongTrinh").val(null);
        $("#idChuongTrinh").val(0);
    }
    var hienThiGio = function () {
        //đừng xóa nhé thằng kia, cái này còn dùng để callback khi click gio-checkbox
        var checkBox = $("#gio-checkbox").prop("checked");
        if (checkBox) $("#gioChuongTrinh").prop("disabled", false);
        else $("#gioChuongTrinh").prop("disabled", true);
    }
    function initChuongTrinhHoatDongTable() {
        ChuongTrinhHoatDongTable = $("#chuongTrinhHoatDongTable").DataTable({
            data: [],
            autoWidth: false,
            columns: [
                 { data: "tgDienRa" },
                 { data: "tieuDe" },
                 { data: "noiDungChuongTrinh" },
                 { data: "loaiHienThi" },
                 { data: "id" }

            ],
            columnDefs: [

                {
                    targets: 0,
                    render: function (data, type, row) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return (data == 1) ? "Hiển thị ngày" : "Hiển thị giờ";
                    }
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary js-chinhSua-ctr">Chỉnh sửa</button><button class="btn btn-danger js-delete-ctr">Xóa</button>';
                    }
                }
            ],
            rowId: function(row) {
                return "ct-" + row.id;
            },
            rowCallback: function (row, data) {

            }
        });
    }
    function initChuongTrinhHoatDong() {
        $("#chuongTrinhHoatDong").empty();
        var jtLine = $("#chuongTrinhHoatDong").jTLine({
            callType: "jsonObject",
            structureObj: DanhSachChuongTrinhHoatDong,
            map: {
                "dataRoot": "/",
                "title": "tieuDe",
                "subTitle": "tgDienRa",
                "dateValue": "tgDienRa",
                "pointCnt": "tgDienRa",
                "bodyCnt": "noiDungChuongTrinh"
            },
            distanceMode: "fixDistance", // 'fixDistance' 'auto' 'predefinedDistance'
            eventsMinDistance: 60,       // Consider It as Distance Unit "by Pixel"
            fixDistanceValue: 2,         // if eventsMinDistance = 60 & fixDistanceValue= 2 then the value is: (60*2) = 120 px
            firstPointMargin: 1,
            formatTitle: function (title) {

                return '<h2><span style="color:green">' + title + "</span></h2>";

            },
            formatSubTitle: function (subTitle) {
                return '<em><span style="color:blue">' + moment(subTitle).format("HH:mm DD/MM/YYYY") + "</span></em>";
            },
            formatPointContent: function (pointCnt, data) {
                if (data.loaiHienThi == 1) {
                    return moment(pointCnt).format("DD/MM");

                } else {
                    return moment(pointCnt).format("HH:mm");
                }
            }
        });
    }
    var setGioInputForSaveChuongTrinh = function (loaiHienThi) {
        if (loaiHienThi == 1) {
            $("#gio-checkbox").prop('checked', false);
        } else {
            $("#gio-checkbox").prop('checked', true);
        }
        hienThiGio();
    }
    var returnLoaiHienThi = function () {
        //Trả về loại hiển thị cho checkbox của Save chương trình Modal
        var checkBox = $("#gio-checkbox").prop("checked");
        if (checkBox == true) return 2;
        return 1;
    }
    var returnDateForSaveChuongTrinh = function () {
        var gio = $("#gioChuongTrinh").val();
        if (gio == null || gio == "") return $("#ngayChuongTrinh").val() + "T00:00";
        return $("#ngayChuongTrinh").val() + "T" + gio;
    }
    var hienThiModalQlyChuongTrinh = function () {
        initChuongTrinhHoatDongTable();
        reloadTable(ChuongTrinhHoatDongTable, DanhSachChuongTrinhHoatDong);
        $("#QlChuongTrinhHd").modal("show");
    };
    var reloadChuongTrinhHoatDongTimeLine = function () {
        DanhSachChuongTrinhHoatDong.sort(sort_by("tgDienRa"));
        reloadTable(ChuongTrinhHoatDongTable, DanhSachChuongTrinhHoatDong);
        initChuongTrinhHoatDong();
    }
    var hienThiModalThemChuongTrinh = function () {
        if (hoatDong.biHuy) {
            $("#thongBaoModal-body").html("Hoạt động này đã bị hủy trước đó. Bạn không thể thêm chương trình.");
            $("#khoiPhucHoatDong").css("display", "none");
            $("#moLaiHoatDong").css("display", "none");
            $("#thongBaoModal").modal("show");
        } else {
            resetChuongTrinhInput();
            $("#SaveCtrModalTitle").html("Thêm chương trình");
            $("#SaveChuongTrinhHoatDongBtn").html("Thêm");
            $("#SaveChuongTrinhHoatDong").modal("show");
        }
       
    };
    var hienThiModalSuaChuongTrinh = function (e) {
        var button = $(e.target);
        //Lấy chương trình muốn chỉnh sửa
        var id = button.closest("tr").attr("id").slice(3);
        var chuongTrinh = $.grep(DanhSachChuongTrinhHoatDong, function(obj) {
             return obj.id == id;
        })[0];
        var tgDienRa = new Date(chuongTrinh.tgDienRa);
        setDataForInputWithLimit("tieuDe",chuongTrinh.tieuDe);
        setDataForInputWithLimit("noiDungChuongTrinh", chuongTrinh.noiDungChuongTrinh);
        setGioInputForSaveChuongTrinh(chuongTrinh.loaiHienThi);
        $("#ngayChuongTrinh").val(returnDateForInput(tgDienRa));
        $("#gioChuongTrinh").val(tgDienRa.toLocaleTimeString());
        $("#idChuongTrinh").val(chuongTrinh.id);
        $("#SaveCtrModalTitle").html("Sửa chương trình");
        $("#SaveChuongTrinhHoatDongBtn").html("Sửa");
        $("#SaveChuongTrinhHoatDong").modal("show");
    };     
    var mapChuongTrinhObject = function () {
        chuongTrinhHoatDong = {};
        chuongTrinhHoatDong.tieuDe = $("#tieuDe").val();
        chuongTrinhHoatDong.noiDungChuongTrinh = $("#noiDungChuongTrinh").val();
        chuongTrinhHoatDong.loaiHienThi = returnLoaiHienThi();
        chuongTrinhHoatDong.tgDienRa = returnDateForSaveChuongTrinh();
        chuongTrinhHoatDong.hoatDongId = hoatDong.id;
        chuongTrinhHoatDong.id = $("#idChuongTrinh").val();
    }
    var updateChuongTrinhHoatDongSauSave = function (data) {
        //Tìm index chương trình
        var chuongTrinhIndex =
            findObjectIndexInArrayByKey(DanhSachChuongTrinhHoatDong, "id", data.id);
        
        if (chuongTrinhIndex == null) //Nếu không có = chương trình mới thêm
            DanhSachChuongTrinhHoatDong.push(data);
        else //Nếu có index = chương trình được sửa, ghi đè
            DanhSachChuongTrinhHoatDong[chuongTrinhIndex] = data;

        reloadChuongTrinhHoatDongTimeLine();
        $("#SaveChuongTrinhHoatDong").modal("hide");
    }
    var saveChuongTrinhHoatDong = function () {
        if ($("#chuongTrinhHoatDongForm").valid()) {
            mapChuongTrinhObject();
            hoatDongService.saveChuongTrinh(chuongTrinhHoatDong, updateChuongTrinhHoatDongSauSave);
        } else alert("Hãy điền chính xác các thông tin.");
    }

    //Phần xóa chương trình
    var hienThiModalXoaChuongTrinh = function (e) {
        var button = $(e.target);
        //Lấy id chương trình muốn xóa
        idCtrMuonXoa = button.closest("tr").attr("id").slice(3);
        $("#DeleteChuongTrinhHoatDong").modal("show");
    }
    var updateChuongTrinhHoatDongSauDelete = function () {
        var chuongTrinhIndex =
            findObjectIndexInArrayByKey(DanhSachChuongTrinhHoatDong, "id", idCtrMuonXoa);
        if (chuongTrinhIndex != null) {
            DanhSachChuongTrinhHoatDong.splice(chuongTrinhIndex, 1);
            
        }
        reloadChuongTrinhHoatDongTimeLine();

    }
    var deleteChuongTrinhHoatDong = function() {

        hoatDongService.deleteChuongTrinh(idCtrMuonXoa, updateChuongTrinhHoatDongSauDelete);

        $("#DeleteChuongTrinhHoatDong").modal("hide");
    }

    //Phần chỉnh sửa hoạt động
    var initChonAnhBia = function () {
        $("body").on("click", "#selectAnhBiaButton", function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $("#anhBiaHoatDong-input").attr("src", fileUrl);
            }
            finder.popup();
        });
    }
    var insertHtmlToChinhSuaHoatDongModal = function (data) {
        $("#ChinhSuaHoatDongModal-body").html(data);
    }
    var selectingOption = function () {
        var lopToChuc = [];
        var donViToChuc = [];

        hoatDong.lopToChuc.forEach(function (lop) {
            lopToChuc.push(lop.lopId);
        });
        hoatDong.donViToChuc.forEach(function (donVi) {
            donViToChuc.push(donVi.donViId);
        });
        $('#selectLopToChuc').val(lopToChuc);
        $('#selectLopToChuc').trigger('change');
        $('#selectDonViToChuc').val(donViToChuc);
        $('#selectDonViToChuc').trigger('change');
    }
    var setHoatDongNgoaiHocVien = function (hoatDongNgoaiHocVien) {
        if (hoatDongNgoaiHocVien) $("#hdNgoaiHocVien-checkbox").prop('checked', true);
        else $("#hdNgoaiHocVien-checkbox").prop('checked', false);
    }
    var setDuocPheDuyet = function (duocPheDuyet) {
        if (duocPheDuyet) $("#duocPheDuyet").prop('checked', true);
        else $("#duocPheDuyet").prop('checked', false);
    }
    var mapHoatDongToInput = function () {
        setDataForInputWithLimit("tenHoatDong",hoatDong.tenHoatDong);
        setDataForInputWithLimit("soLuoc",hoatDong.soLuoc);
        setDataForInputWithLimit("diaDiem", hoatDong.diaDiem);
        CKEDITOR.instances['noiDung'].setData(hoatDong.noiDung); //set data cho text editor
        $("#ngayBatDau").val(returnDateForInput(hoatDong.ngayBatDau));
        $("#gioBatDau").val(returnGio(hoatDong.ngayBatDau));
        $("#ngayKetThuc").val(returnDateForInput(hoatDong.ngayKetThuc));
        $("#gioKetThuc").val(returnGio(hoatDong.ngayKetThuc));
        $("#hoatDongId").val(hoatDong.id);
        $("#capHoatDong-input").val(hoatDong.capHoatDong);
        $("#anhBiaHoatDong-input").attr("src", hoatDong.anhBia);
        selectingOption();
        setHoatDongNgoaiHocVien(hoatDong.hoatDongNgoaiHocVien);
        setDuocPheDuyet(hoatDong.duocPheDuyet);
    }
    var initTextEditor = function () {
        CKEDITOR.replace("noiDung");
        CKFinder.setupCKEditor(null, '/ckfinder');
    } //Text editor cho nội dung hoạt động
    var hienThiModalChinhSuaHoatDong = function () {
        quanLyHoatDongService.loadSaveHoatDongHtml(insertHtmlToChinhSuaHoatDongModal);
        quanLyHoatDongService.getSelectListData(quanLyHoatDongController.initSelectList);
        limitCharacterForInput("tenHoatDong");
        limitCharacterForInput("soLuoc");
        limitCharacterForInput("diaDiem");
        initTextEditor();
        mapHoatDongToInput();
        validateHoatDongForm();
        $("#ChinhSuaHoatDongModal").modal("show");
    }
    var hienThiModalKetThucHoatDong = function () {
        if (hoatDong.biHuy) {
            $("#thongBaoModal-body").html("Hoạt động này đã bị hủy trước đó.");
            $("#khoiPhucHoatDong").css("display", "none");
            $("#moLaiHoatDong").css("display", "none");
            $("#thongBaoModal").modal("show");
        }
        else {
            if (hoatDong.daKetThuc) {
                $("#thongBaoModal-body").html("Hoạt động này đã kết thúc. Bạn có muốn mở lại hoạt động?");
                $("#khoiPhucHoatDong").css("display", "none");
                $("#moLaiHoatDong").css("display", "inline-block");
                $("#thongBaoModal").modal("show");
            } else {
                $("#KetThucHoatDongModal").modal("show");
            }
        }
          
    }
    var hienThiModalHuyHoatDong = function () {
          if (hoatDong.biHuy) {
              $("#thongBaoModal-body").html("Hoạt động này đã bị hủy trước đó. Bạn có muốn khôi phục lại hoạt động?");
              $("#khoiPhucHoatDong").css("display", "inline-block");
              $("#moLaiHoatDong").css("display", "none");
              $("#thongBaoModal").modal("show");
          }
          else {
              $("#HuyHoatDongModal").modal("show");
          }
       
    }

    //Phần code để khởi tạo trang
    var initTrang_QuanLy = function (hoatDongId) {
        dataTableSetting();
        validateForm();
        limitCharacter();
        hoatDongService.layChiTietHoatDong(hoatDongId, mapObject, themThongTinHdVaoDom);
        $("#xemDanhSachTg").on("click", initDanhSachSvThamGiaTable_QuanLy);

        $("#nutQlyChuongTrinhHoatDong").on("click", hienThiModalQlyChuongTrinh);

        $("body").on("click", "#nutThemChuongTrinhHoatDong", hienThiModalThemChuongTrinh);
        $("body").on("click", ".js-chinhSua-ctr", hienThiModalSuaChuongTrinh);
        $("#SaveChuongTrinhHoatDongBtn").on("click",saveChuongTrinhHoatDong);

        $("#gio-checkbox").on("click", hienThiGio);


        $("body").on("click", ".js-delete-ctr", hienThiModalXoaChuongTrinh);
        $("#DeleteChuongTrinhHoatDongBtn").on("click", deleteChuongTrinhHoatDong);

        initChonAnhBia();
        $("#ChinhSuaHoatDongBtn").on("click", hienThiModalChinhSuaHoatDong);
        $("#ChinhSuaHoatDong").on("click", quanLyHoatDongController.saveHoatDong);
        $("#KetThucHoatDongBtn").on("click", hienThiModalKetThucHoatDong);
        $("#KetThucHoatDong").on("click", quanLyHoatDongController.ketThucHoatDong);
        $("#HuyHoatDongBtn").on("click", hienThiModalHuyHoatDong);
        $("#HuyHoatDong").on("click", quanLyHoatDongController.huyHoatDong);
        $("#khoiPhucHoatDong").on("click", quanLyHoatDongController.khoiPhucHoatDong);
        $("#moLaiHoatDong").on("click", quanLyHoatDongController.moLaiHoatDong);
                                 
    }
    var initTrangKhach = function(hoatDongId) {
        dataTableSetting();
        hoatDongService.layChiTietHoatDong(hoatDongId, mapObject, themThongTinHdVaoDom);
        $('#xemDanhSachTg').on("click", initDanhSachSvThamGiaTable_Khach);
        $('#DangKiHoatDongBtn').on('click', dangKiHoatDongHandler);
        $('#TheoDoiHoatDongBtn').on('click', theoDoiHoatDongHandler);
        $('#huyDangKi').on('click', huyDangKi);
        $('#huyTheoDoi').on('click', huyTheoDoi);
    }

    return {
        initTrang_QuanLy: initTrang_QuanLy,
        initTrangKhach: initTrangKhach,
        taoLinkHoatDong: taoLinkHoatDong
    }
}(HoatDongService, QuanLyHoatDongService, QuanLyHoatDongController);

var DiemDanhController = function (hoatDongController, hoatDongService) {
    var danhSachSvThamGia;
    var danhSachSvTgTable, danhSachDangKiTable;
    var hoatDong, thamGiaDto ={};
    var idSinhVienMuonXoa;
    var csrfToken = $("input[name='__RequestVerificationToken']").val();

   //Func điểm danh, xóa điểm danh, phê duyệt đăng kí
    var reloadDanhSachSvThamGia = function() {
        danhSachSvTgTable.ajax.reload();
    }
    var updateAfterDiemDanhXoaDiemDanh = function (error) {
        if (error != null) {
            alert(error);
        } else {
            reloadDanhSachSvThamGia();
        }
    }
    var bindThamGiaDto = function (hoatDongId, sinhVienId) {
        thamGiaDto.hoatDongId = hoatDongId;
        thamGiaDto.sinhVienId = sinhVienId;
    }
    var diemDanhSinhVien = function (e, sinhVien) {
        //sinhVien là object do typeahead truyền vào
        if (hoatDong.biHuy) {
            alert("Hoạt động này đã bị hủy nên không thể điểm danh sinh viên.");
            $("#input-TrangDiemDanh").typeahead("val", 'AS');
            return;
        }
        if (!hoatDong.duocPheDuyet) {
            alert("Hoạt động này chưa được phê duyệt nên không thể điểm danh sinh viên.");
            $("#input-TrangDiemDanh").typeahead("val", 'AS');
            return;
            }
        //Kiểm tra xem sinh viên này đã được điểm danh chưa
        var luotThamGia = $.grep(danhSachSvThamGia, function (obj) { return obj.id == sinhVien.id; })[0];
        if (luotThamGia != null) {
            alert("Sinh viên này đã được điểm danh");
            $("#input-TrangDiemDanh").typeahead("val", 'AS');
            return;
        }
        bindThamGiaDto(hoatDong.id, sinhVien.id);
        hoatDongService.diemDanhHoatDong(thamGiaDto, updateAfterDiemDanhXoaDiemDanh);
        $('#input-TrangDiemDanh').typeahead("val", "AS");
    }
    var hienThiModalXoaLuotThamGia = function (e) {
        var button = $(e.target);
        //Lấy id chương trình muốn xóa
        idSinhVienMuonXoa = button.closest("tr").attr("id");
        $("#XoaLuotThamGiaModal").modal("show");
    }
    var xoaLuotThamGia = function () {
        bindThamGiaDto(hoatDong.id, idSinhVienMuonXoa);
        hoatDongService.xoaLuotThamGia(thamGiaDto,updateAfterDiemDanhXoaDiemDanh);
        $("#XoaLuotThamGiaModal").modal("hide");
    }
    var initTypeahead = function () {
        var danhSachsinhVien = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace("hoVaTenLot", "ten", "mssv", "kyHieuTenLop"),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/api/SinhVien/TimKiem?searchTerm=%QUERY",
                wildcard: "%QUERY"
            }
        });
        $('#input-TrangDiemDanh').typeahead(
                {
                    highlight: true
                },
                {
                    name: 'sinhVien',
                    display: function (data) {
                        return data.hoVaTenLot + " " + data.ten;
                    },
                    source: danhSachsinhVien,
                    templates: {
                        suggestion: function (data) {
                            return '<p><img src="' + data.anhDaiDien + '" class="anhSvTypeahead"/><strong>' + data.hoVaTenLot + ' ' + data.ten + '</strong> - '
                                + data.mssv + ' - ' + data.kyHieuTenLop + '</p>';
                        }
                    }

                })
            .on("typeahead:autocomplete", diemDanhSinhVien)
            .on("typeahead:select", diemDanhSinhVien)
            .on("typeahead:change",
                function () {
                });
    }
    var updateAfterPheDuyetDangKi = function (sinhVienId, error) {
        if (error != null) {
            alert(error);
        } else {
            danhSachDangKiTable.row("#" + sinhVienId).remove().draw();
            reloadDanhSachSvThamGia();
        }
    }
    var updateAfterXoaDangKi = function (sinhVienId) {
        danhSachDangKiTable.row('#' + sinhVienId).remove().draw();
    }
    var pheDuyetDangKi= function(e) {
        var button = $(e.target);
        var idSinhVien = button.closest("tr").attr("id");
        bindThamGiaDto(hoatDong.id, idSinhVien);
        hoatDongService.pheDuyetDangKi(thamGiaDto, updateAfterPheDuyetDangKi);
    }
    var xoaDangKi = function(e) {
        var button = $(e.target);
        var idSinhVien = button.closest("tr").attr("id");
        bindThamGiaDto(hoatDong.id, idSinhVien);
        hoatDongService.xoaDangKi(thamGiaDto, updateAfterXoaDangKi);
    }

    //Func tạo trang
    var initDanhSachSvThamGiaTable = function () {
        danhSachSvTgTable = $("#danhSachSvTg-TrangDiemDanh").DataTable({
            ajax: {
                url: "/api/HoatDong/LayDanhSachDaDiemDanh/" + hoatDong.id,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: function (json) {
                    danhSachSvThamGia = json; //Bind như vậy để update biến danhSachSvThamGia mỗi khi bảng update
                    return json;
                }
            },
            order: [[3,'desc']],
            columns: [
                { data: "" },
                { data: "mssv" },
                { data: "kyHieuTenLop" },
                { data: "ngayThamGia" },
                { data: "" }
            ],
            autoWidth: false,
            deferRender: true,
            dom: "Bflrtip",
            buttons: [
                "copy", "excel", "pdf"
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                },
                {
                    targets: 3,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                },
                {
                    targets: 4,
                    width: 50,
                    render: function () {
                        return '<button class="btn btn-danger js-delete-thamGia">Xóa</button>';
                    }
                }
            ],
            rowId: "id"
        });
    }
    var initDanhSachDangKiTable = function () {
        danhSachDangKiTable = $("#danhSachDangKi-TrangDiemDanh").DataTable({
            data: hoatDong.danhSachSinhVienDangKi,
            order: [[1,'desc']],                        
            columns: [
                { data: "" },
                { data: "mssv" },
                { data: "kyHieuTenLop" },
                { data: "" }
            ],
            deferRender: true,
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                },
                {
                    targets: 3,
                    width: 150,
                    render: function () {
                        return '<button class="btn btn-success js-pheDuyet-dangKi">Phê duyệt</button>'
                              +'<button class="btn btn-danger js-delete-dangKi">Xóa</button>';
                    }
                }
            ],
            rowId: "id"
        });
    }
    var mapObject = function (data) {
        hoatDong = data;
    }
    var themThongTinHdVaoDom = function (data) {
        mapObject(data);
        $("#tenHoatDong-TrangDiemDanh").append(hoatDong.tenHoatDong);
        $("#ngayDienRa-TrangDiemDanh").append(moment(hoatDong.ngayBatDau).format("DD-MM-YYYY"));
        $("#tinhTrang-TrangDiemDanh").append(taoTinhTrang(hoatDong));

        initDanhSachSvThamGiaTable();
        initDanhSachDangKiTable();
    };                                                         

    var initTrangDiemDanh = function (hoatDongId) {

        dataTableSetting();
        initTypeahead();

        hoatDongService.layDiemDanhHoatDong(hoatDongId, themThongTinHdVaoDom);

        $("body").on("click", ".js-delete-thamGia", hienThiModalXoaLuotThamGia);
        $("body").on("click", ".js-pheDuyet-dangKi", pheDuyetDangKi);
        $("body").on("click", ".js-delete-dangKi", xoaDangKi);
        $("#XoaLuotThamGia").on("click", xoaLuotThamGia);
    };

    //Func cho trang điều hướng điểm danh
    var chuyenSangTrangDiemDanh = function (e, hoatDong) {
        window.location.href = "/HoatDong/DiemDanh/"+hoatDong.id;
    }
    var initTypeahead_DieuHuongDiemDanh = function () {
        var danhSachHoatDong4TypeAhead = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace("tenHoatDong", "diaDiem"),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/api/HoatDong/TimKiem?searchTerm=%QUERY",
                headers: { __RequestVerificationToken: csrfToken },
                wildcard: "%QUERY"
            }
        });
        $("#input-TrangDiemDanh").typeahead(
                {
                    highlight: true
                },
                {
                    name: "hoatDong",
                    source: danhSachHoatDong4TypeAhead,
                    display: function (data) {
                        return data.tenHoatDong;
                    },
                    templates: {
                        suggestion: function (data) {
                            return '<div><img src="' + data.anhBia + '" class="anhBiaHoatDongTypeAhead"/><span><strong>' + data.tenHoatDong + '</strong><div>'
                                + moment(data.ngayBatDau).format("DD/MM/YYYY HH:mm") + " - " + moment(data.ngayKetThuc).format("DD/MM/YYYY HH:mm") + '</div><div>' + data.diaDiem + '</div></span></div>';
                        }
                    }

                })
            .on("typeahead:autocomplete", chuyenSangTrangDiemDanh)
            .on("typeahead:select", chuyenSangTrangDiemDanh);
    }
    var initTrangDieuHuongDiemDanh = function () {
        initTypeahead_DieuHuongDiemDanh();
    };

    return {
        initTrangDiemDanh: initTrangDiemDanh,
        initTrangDieuHuongDiemDanh: initTrangDieuHuongDiemDanh
    }

}(HoatDongController, HoatDongService);

var PheDuyetController = function (quanLyHoatDongService) {
    var hdChoPheDuyetTable;
    var hoatDongId;
    var csrfToken = $("input[name='__RequestVerificationToken']").val();

    var taoDonViToChuc = function (row) {
        var html = '';
        if (row.danhSachDonViToChuc != null) {
            row.danhSachDonViToChuc.forEach(function (donVi) {
                html += "<div>" + donVi + "</div>";
            });
        }
        if (row.danhSachLopToChuc != null) {
            row.danhSachLopToChuc.forEach(function (lop) {
                html += "<div>" + lop + "</div>";
            });

        }
        return '<div>' + html + '</div>';
    }
    var initHdChoPheDuyetTable = function () {
        hdChoPheDuyetTable = $("#hdChoPheDuyetTable-TrangPheDuyet").DataTable({
            ajax : {
                url: "/api/HoatDong/TrangPheDuyetHoatDong",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ''
            },
            "order": [[1, 'desc']],
            columns: [
                { data: 'tenHoatDong' },
                { data: 'ngayBatDau' },
                { data: 'ngayKetThuc' },
                { data: 'danhSachDonViToChuc' },
                { data: 'capHoatDong' },
                { data: 'id' },
                { data: 'id' }
            ],
            rowId :"id",
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                       return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 2,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return taoDonViToChuc(row);
                    }
                },
                {
                    targets: 4,
                    render: function (data) {
                        return returnCapHoatDong(data);
                    }
                },
                {
                    targets: 5,
                    render: function () {
                        return '<button class="btn btn-success pheDuyet-js">Phê duyệt</button>';
                    }
                },
                {
                    targets: 6,
                    render: function () {
                        return '<button class="btn btn-danger huy-js">Hủy</button>';
                    }
                }
            ]
        });
    }
    var hienThiModalPheDuyetHoatDong = function (e) {
        var button = $(e.target);
        hoatDongId = button.closest("tr").attr("id");
        $("#thongBaoModal-body").html("Bạn có chắn chắn muốn phê duyệt hoạt động này?");
        $("#pheDuyetHoatDong").css("display", "inline-block");
        $("#huyHoatDong").css("display", "none");
        $("#thongBaoModal").modal("show");
    }
    var hienThiModalHuyHoatDong = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        hoatDongId = button.closest("tr").attr("id");
        $("#thongBaoModal-body").html("Bạn có chắn chắn muốn hủy hoạt động này?");
        $("#pheDuyetHoatDong").css("display", "none");
        $("#huyHoatDong").css("display", "inline-block");
        $("#thongBaoModal").modal("show");
    }
    var updatePage = function (coHuyHoatDong, xhr) {
        hideLoader();
        if (xhr == null) {
            hdChoPheDuyetTable.ajax.reload();
            if (coHuyHoatDong) alert("Đã hủy hoạt động");
            else alert("Đã phê duyệt");
        } else {
            alert(xhr.responseJSON.message);
        }
        $("#thongBaoModal").modal("hide");
    }
    var pheDuyetHoatDong = function () {
        showLoader();
        quanLyHoatDongService.pheDuyetHoatDong(hoatDongId, updatePage);
    }
    var huyHoatDong = function () {
        showLoader();
        quanLyHoatDongService.huyHoatDongTrangPheDuyet(hoatDongId, updatePage);
    }
    var initTrangPheDuyet = function () {
        dataTableSetting();
        initHdChoPheDuyetTable();
        $("body").on("click", ".pheDuyet-js", hienThiModalPheDuyetHoatDong);
        $("#pheDuyetHoatDong").on("click", pheDuyetHoatDong);
        $("body").on("click", ".huy-js", hienThiModalHuyHoatDong);
        $("#huyHoatDong").on("click", huyHoatDong);
    }

    return {
        initTrangPheDuyet: initTrangPheDuyet,
        hienThiModalHuyHoatDong: hienThiModalHuyHoatDong,
        taoDonViToChuc: taoDonViToChuc,
        huyHoatDong: huyHoatDong
    }
}(QuanLyHoatDongService);

var TrangChuHoatDongController = function (trangChuHoatDongService) {
    var popoverContent = "";
    var recordStart = 0, pageSize = 10;
    var daDanhDau = false, daHetThongBao = false;
    var updateDvToChucHolder = function (hoatDongId) {
        var holder = $('#dvtc_' + hoatDongId);
        var holderWidth = 0;
        var themDonViWidth = holder.find('.themDonVi-CardHoatDong').outerWidth(true);
        $('#dvtc_' + hoatDongId + '> a').each(function () {
            holderWidth += $(this).outerWidth(true);
        });
        var availablespace = holder.width() - themDonViWidth;

        if (holderWidth > availablespace) {
            var donVi = holder.find('a:not(.themDonVi-CardHoatDong)').last();
            donVi.addClass("donViBoSung");
            popoverContent = donVi[0].outerHTML + popoverContent; //lấy html của tag <a>
            donVi.remove(); //Xóa khỏi
            updateDvToChucHolder(hoatDongId);
        }
        if (popoverContent != '') {
            holder.find(".themDonVi-CardHoatDong").popover({
                html: true,
                title: "Các đơn vị khác",
                content: popoverContent
            });
            holder.find(' .themDonVi-CardHoatDong').css('display', 'inline');
        } else {
            holder.find(' .themDonVi-CardHoatDong').css('display', 'none');
        }
    }
    var updateDvTc_HdSvTao_Holder = function (hoatDongId) {

        var holder = $('#dvtcSVtao_' + hoatDongId);
        var holderWidth = 0;
        var themDonViWidth = holder.find('.themDonVi-CardHoatDong').outerWidth(true);
        $('#dvtcSVtao_' + hoatDongId + '> a').each(function () {
            holderWidth += $(this).outerWidth(true);
        });
        var availablespace = holder.width() - themDonViWidth;

        if (holderWidth > availablespace) {
            var donVi = holder.find('a:not(.themDonVi-CardHoatDong)').last();
            donVi.addClass("donViBoSung");
            popoverContent = donVi[0].outerHTML + popoverContent; //lấy html của tag <a>
            donVi.remove(); //Xóa khỏi
            updateDvToChucHolder(hoatDongId);
        }
        if (popoverContent != '') {
            holder.find(".themDonVi-CardHoatDong").popover({
                html: true,
                title: "Các đơn vị khác",
                content: popoverContent
            });
            holder.find(' .themDonVi-CardHoatDong').css('display', 'inline');
        } else {
            holder.find(' .themDonVi-CardHoatDong').css('display', 'none');
        }
    }
    var themClassCollapse = function () {
        /*Thêm class này sau khi update nav vì nếu collapse có trước div sẽ ko hiển thị,
        ko tính được độ dài các nav*/
        $("#hoatDongThamGia").addClass("collapse");
        $("#hoatDongDangKi").addClass("collapse");
        $("#hoatDongTheoDoi").addClass("collapse");
        $("#hoatDongToChuc").addClass("collapse");
    }
    var themHoatDongVaoDom_TrangCaNhan = function (data) {
        //Lấy template cho hoạt động
        var cardHoatDong_Template = _.template($("#cardHoatDong_Template").html());
        //Thêm hoạt động đang tham gia
        data.danhSachHoatDongThamGia.forEach(function (hoatDong) {
            $("#hoatDongThamGia").append(cardHoatDong_Template({ hoatDong: hoatDong }));
            popoverContent = '';
            updateDvToChucHolder(hoatDong.id);
        });
        $("#thamGiaCount").html(data.danhSachHoatDongThamGia.length);
        //Thêm hoạt động đã đăng kí
        data.danhSachHoatDongDangKi.forEach(function (hoatDong) {
            $("#hoatDongDangKi").append(cardHoatDong_Template({ hoatDong: hoatDong }));
            popoverContent = '';
            updateDvToChucHolder(hoatDong.id);
        });
        $("#dangKiCount").html(data.danhSachHoatDongDangKi.length);
        //Thêm hoạt động cấp liên chi hội
        data.danhSachHoatDongTheoDoi.forEach(function (hoatDong) {
            $("#hoatDongTheoDoi").append(cardHoatDong_Template({ hoatDong: hoatDong }));
            popoverContent = '';
            updateDvToChucHolder(hoatDong.id);
        });
        $("#theoDoiCount").html(data.danhSachHoatDongTheoDoi.length);
        //Thêm hoạt động do sinh viên tạo
        var cardHoatDongSinhVienTao_Template = _.template($("#cardHoatDongSinhVienTao_Template").html());
        data.danhSachHoatDongToChuc.forEach(function (hoatDong) {
            $("#hoatDongToChuc").append(cardHoatDongSinhVienTao_Template({ hoatDong: hoatDong }));
            popoverContent = '';
            updateDvTc_HdSvTao_Holder(hoatDong.id);
        });
        $("#toChucCount").html(data.danhSachHoatDongToChuc.length);
        //Thêm class Collapse
        themClassCollapse();
    }
    var themHoatDongVaoDom_TrangHoatDongChung = function (data) {
        //Lấy template cho hoạt động
        var cardHoatDong_Template = _.template($("#cardHoatDong_Template").html());
        //Thêm hoạt động đang tham gia
        //Thêm hoạt động cấp phân viện
        if (data.dsHdCapPhanVien.length > 0) {
            data.dsHdCapPhanVien.forEach(function(hoatDong) {
                $("#hoatDongCapPhanVien").append(cardHoatDong_Template({ hoatDong: hoatDong }));
                popoverContent = '';
                updateDvToChucHolder(hoatDong.id);
            });
        } else $("#hoatDongCapPhanVien").append("<h3 class='khongCoHoatDong'>Tạm thời không có hoạt động cấp Phân viện nào đang diễn ra</h3>");
        //Thêm hoạt động cấp khoa
        if (data.dsHdCapKhoa.length > 0) {
            data.dsHdCapKhoa.forEach(function (hoatDong) {
                $("#hoatDongCapKhoa").append(cardHoatDong_Template({ hoatDong: hoatDong }));
                popoverContent = '';
                updateDvToChucHolder(hoatDong.id);
            });
        } else $("#hoatDongCapKhoa").append("<h3 class='khongCoHoatDong'>Tạm thời không có hoạt động cấp Khóa nào đang diễn ra</h3>");

        //Thêm hoạt động cấp liên chi hội
        if (data.dsHdCapLienChiHoi.length > 0) {
            data.dsHdCapLienChiHoi.forEach(function (hoatDong) {
                $("#hoatDongCapLienChiHoi").append(cardHoatDong_Template({ hoatDong: hoatDong }));
                popoverContent = '';
                updateDvToChucHolder(hoatDong.id);
            });
        } else $("#hoatDongCapLienChiHoi").append("<h3 class='khongCoHoatDong'>Tạm thời không có hoạt động cấp Liên Chi nào đang diễn ra</h3>");
        //Thêm hoạt động cấp chi hội
        if (data.dsHdCapChiHoi.length > 0) {
            data.dsHdCapChiHoi.forEach(function (hoatDong) {
                $("#hoatDongCapChiHoi").append(cardHoatDong_Template({ hoatDong: hoatDong }));
                popoverContent = '';
                updateDvToChucHolder(hoatDong.id);
            });
        } else $("#hoatDongCapChiHoi").append("<h3 class='khongCoHoatDong'>Tạm thời không có hoạt động cấp Liên Chi nào đang diễn ra</h3>");

    }
    var returnThongBaoCount = function (thongBaoCount) {
        if (thongBaoCount <= 0) return;
        $("#thongBaoCount").html(thongBaoCount);
    }
    var setDaHetThongBao = function () {
        $(".khongConBaiViet").show();
        daHetThongBao = true;
    }
    var themThongBao = function (data) {
        var thongBao_Template = _.template($("#danhSachThongBao_Template").html());
        $(thongBao_Template({ danhSachThongBao: data })).insertBefore(".baiVietLoader");
        if (data.length < pageSize) setDaHetThongBao();
        else recordStart += pageSize;
        $(".baiVietLoader").hide();
    }
    //Func này để tạo thông báo lần đầu tiên
    var taoThongBao = function (danhSachThongBao) {
        //Lấy template
        var thongBao_Template = _.template($("#danhSachThongBao_Template").html());
        //Khởi tạo thông báo chưa đọc
        if (danhSachThongBao.thongBaoChuaDoc.length > 0) {
            $("#thongBaoChuaDoc").css("display", "block");
            $(thongBao_Template({ danhSachThongBao: danhSachThongBao.thongBaoChuaDoc })).insertAfter("#thongBaoChuaDoc");
            returnThongBaoCount(danhSachThongBao.thongBaoChuaDoc.length);
        }
        //Khởi tạo thông báo trước đó
        if (danhSachThongBao.thongBaoTruocDo.length > 0) {
            $("#thongBaoTruocDo").css("display", "block");
            $(thongBao_Template({ danhSachThongBao: danhSachThongBao.thongBaoTruocDo })).insertAfter("#thongBaoTruocDo");
            //Set recordStart thêm 10
            recordStart += pageSize;
        }
    }
    var hienThiDanhSachThongBao = function () {
        $("#danhSachThongBao").collapse('toggle');
        $("#thongBaoCount").html("");
        if (daDanhDau) return;     
        trangChuHoatDongService.danhDauThongBaoDaDoc();
        daDanhDau = true;
    }  
    var initTrangHoatDongChung = function() {
        trangChuHoatDongService.layHoatDongTrangHoatDongChung(themHoatDongVaoDom_TrangHoatDongChung);
    }
    var initTrangCaNhan = function () {
        trangChuHoatDongService.layHoatDongTrangCaNhan(themHoatDongVaoDom_TrangCaNhan);
        trangChuHoatDongService.layThongBaoHoatDong(taoThongBao);
        $("#thongBaoToggle").on("click", hienThiDanhSachThongBao);
        //Load thêm thông báo trước đó nếu user scroll xuống cuối
        $(".danhSachThongBaoHoatDong").on("scroll", function () {
            if ($(this)[0].scrollHeight - $(this).scrollTop() - 1 <= $(this).innerHeight()) {
                if (!daHetThongBao) {
                    $(".baiVietLoader").css("display", "block");
                    trangChuHoatDongService.layThemThongBao(recordStart, themThongBao);
                }
            }
        });
    }

    /*Trang hoạt động theo đơn vị*/
    var themDonViVaoDom = function (danhSachDonVi) {
        //Lấy template cho hoạt động
        var cardDonVi_Template = _.template($("#cardDonVi_Template").html());
        //Thêm hoạt động đang tham gia
        danhSachDonVi.forEach(function (donVi) {
            if (donVi.loaiDonVi == 1)  //Đơn vị trực thuộc đoàn học viên
                $("#danhSachDonViDoan").append(cardDonVi_Template({ donVi: donVi }));
            if (donVi.loaiDonVi == 2)  //Đơn vị trực thuộc hội sinh viên
                $("#danhSachDonViHoi").append(cardDonVi_Template({ donVi: donVi }));
        });
    }
    var initTrangHoatDongTheoDonVi = function () {
        trangChuHoatDongService.layDanhSachDonVi(themDonViVaoDom);
    }
    return {
        initTrangHoatDongChung: initTrangHoatDongChung,
        initTrangCaNhan: initTrangCaNhan,
        initTrangHoatDongTheoDonVi: initTrangHoatDongTheoDonVi
    }
}(TrangChuHoatDongService);

var ThongKeHoatDongChungController = function (trangChuHoatDongService) {
    var danhSachHoatDongToChucTable, bieuDoSoLuotThamGiaHoatDong, bieuDoSoHoatDongToChuc, bieuDoHoatDongTheoCap;
    //Table
    var taoDonViToChuc = function (row) {
        var html = "";
        row.danhSachDonViToChuc.forEach(function (donVi) {
            html += "<li>" + donVi + "</li>";
        });
        row.danhSachLopToChuc.forEach(function (lop) {
            html += "<li>" + lop + "</li>";
        });
        return '<ul style="list-style: none;">' + html + "</ul>";
    }
    var initDanhSachHoatDongToChucTable = function () {
        danhSachHoatDongToChucTable = $("#hoatDongToChuc_ThongKeChung").DataTable({
            data: [],
            order: [[1, "desc"]],
            columns: [
                { data: 'tenHoatDong' },
                { data: 'ngayBatDau' },
                { data: 'ngayKetThuc' },
                { data: 'soLuotThamGia' },
                { data: 'capHoatDong' },
                { data: '' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")

                },
                {
                    targets: 2,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "HH [giờ] mm [ngày] DD/MM/YYYY", "vi")

                },
                {
                    targets: 4,
                    render: function (data) {
                        return returnCapHoatDong(data);
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return taoDonViToChuc(row);
                    }
                }
            ]
        });
    }
    //Function cho biểu đồ
    function initBieuDoSoLuotThamGiaHoatDong() {
        var ctx = $("#bieuDoSoLuotThamGiaHoatDong");
        bieuDoSoLuotThamGiaHoatDong = new Chart(ctx, {
            type: 'bar',
            responsive: true,
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Toàn Phân viện',
                        data: [],
                        datalabels: {
                            anchor: 'center',
                            align: 'center'
                        },
                        backgroundColor: 'rgba(54, 162, 235, 0.2)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                plugins: {
                    datalabels: {
                        color: 'black',
                        display: true,
                        font: {
                            weight: 'bold',
                            size: 20
                        },
                        formatter: Math.round
                    }
                },
                title: {
                    display: true,
                    fontSize: 25,
                    text: 'Số lượt tham gia hoạt động'
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                min: 0
                            }
                        }
                    ]
                }
            }
        });
    }
    function addDataBieuDoSoLuotThamGiaHoatDong(chart, soLuotThamGiaHdTungThang) {
        chart.data.labels = soLuotThamGiaHdTungThang.danhSachThang;
        chart.data.datasets[0].data = soLuotThamGiaHdTungThang.soLuotThamGiaHocVien;
        chart.update();
    }
    function initBieuDoSoHoatDongToChuc() {
        var ctx = $("#bieuDoSoHoatDongToChuc");
        bieuDoSoHoatDongToChuc = new Chart(ctx, {
            type: 'bar',
            responsive: true,
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Toàn phân viện',
                        data: [],
                        datalabels: {
                            anchor: 'center',
                            align: 'center'
                        },
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                plugins: {
                    datalabels: {
                        color: 'black',
                        display: true,
                        font: {
                            weight: 'bold',
                            size: 20
                        },
                        formatter: Math.round
                    }
                },
                title: {
                    display: true,
                    fontSize: 25,
                    text: 'Số hoạt động được tổ chức'
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true
                            },
                            offset: true
                        }
                    ]
                }
            }
        });
    }
    function addDataBieuDoSoHoatDongToChuc(chart, soHoatDongToChucTungThang) {
        chart.data.labels = soHoatDongToChucTungThang.danhSachThang;
        chart.data.datasets[0].data = soHoatDongToChucTungThang.soHoatDongHocVienToChuc;
        chart.update();
    }
    function initBieuDoHoatDongTheoCap() {
        var ctx = $("#bieuDoHoatDongTheoCap");
        bieuDoHoatDongTheoCap = new Chart(ctx, {
            type: 'bar',
            responsive: true,
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Hoạt động theo từng cấp',
                        data: [],
                        datalabels: {
                            anchor: 'center',
                            align: 'center'
                        },
                        backgroundColor: 'rgba(255, 172, 51, 0.3)',
                        borderColor: 'rgba(255, 172, 51, 1)',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                plugins: {
                    datalabels: {
                        color: 'black',
                        display: true,
                        font: {
                            weight: 'bold',
                            size: 20
                        },
                        formatter: Math.round
                    }
                },
                title: {
                    display: true,
                    fontSize: 25,
                    text: 'Số hoạt động theo cấp hoạt động'
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true
                            },
                            offset: true
                        }
                    ]
                }
            }
        });
    }
    function addDataBieuDoHoatDongTheoCap(chart, soHoatDongTungCapHoatDong) {
        chart.data.labels = soHoatDongTungCapHoatDong.danhSachCapHoatDong;
        chart.data.datasets[0].data = soHoatDongTungCapHoatDong.soHoatDongTungCap;
        chart.update();
    }
    var initBieuDo = function() {
        initBieuDoSoLuotThamGiaHoatDong();
        initBieuDoSoHoatDongToChuc();
        initBieuDoHoatDongTheoCap();
    }
    //Update Dom
    var taoNamHocThongKe = function () {
        var namBatDau = 2000;//Lấy năm học mặc định là 2000-2001 
        var namKetThuc;
        var hienTai = new Date();
        if (hienTai.getMonth()+1 < 8) //Nghĩa là đang là tháng 1-7 của năm học này, sẽ phải lấy từ tháng 8 năm trước
        {
            //Phải cộng 1 vì getMonth() trả từ 0-11
            namKetThuc = hienTai.getFullYear() - 1;
        }
        else      //Nếu là đang trong tháng 8 -12 thì lấy luôn năm nay
        {
            namKetThuc = hienTai.getFullYear();
        }
        var optionNamHocTemplate = _.template($("#optionNamHoc_Template").html());
        for (var namHoc = namBatDau; namHoc <= namKetThuc; namHoc++) {
            $("#namHocPicker").append(optionNamHocTemplate({ namHoc: namHoc }));
        }
    }
    var updatePage = function (data) {
        $("#soHoatDongToChuc_ThongKeChung").html(data.soHoatDongToChucTrongNam);
        $("#soLuotThamGia_ThongKeChung").html(data.soLuotThamGiaTrongNam);
        reloadTable(danhSachHoatDongToChucTable, data.danhSachHoatDong);
        addDataBieuDoSoLuotThamGiaHoatDong(bieuDoSoLuotThamGiaHoatDong, data.soLuotThamGiaTungThang);
        addDataBieuDoSoHoatDongToChuc(bieuDoSoHoatDongToChuc, data.soHoatDongToChucTungThang);
        addDataBieuDoHoatDongTheoCap(bieuDoHoatDongTheoCap, data.soHoatDongTungCapHoatDong);
    }
    var layThongKeHoatDongChung = function () {
        var namHocLay = $("#namHocPicker").val();
        trangChuHoatDongService.layThongKeHoatDongChung(namHocLay, updatePage);
    }
    var initTrang = function () {
        dataTableSetting();
        initDanhSachHoatDongToChucTable();
        taoNamHocThongKe();
        initBieuDo();
        $("#namHocPicker").change(layThongKeHoatDongChung);
    }

    return {
        initTrang : initTrang
    }
}(TrangChuHoatDongService);

var ThongKeHoatDongSinhVienController = function (trangChuHoatDongService) {
    var sinhVienDto = {};
    var hoatDongSvThamGiaTable;
    var bieuDoSoLuotThamGiaHoatDong;
    //Tạo biểu đồ
    var initBieuDoSoLuotThamGiaHoatDong = function() {
        var ctx = $("#bieuDoSoLuotThamGiaHoatDong");
        bieuDoSoLuotThamGiaHoatDong = new Chart(ctx, {
            type: 'bar',
            responsive: true,
            data: {
                labels: [],
                datasets: [
                    {
                        label: 'Số lượt tham gia',
                        data: [],
                        datalabels: {
                            anchor: 'center',
                            align: 'center'
                        },
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                plugins: {
                    datalabels: {
                        color: 'black',
                        display: true,
                        font: {
                            weight: 'bold',
                            size: 20
                        },
                        formatter: Math.round
                    }
                },
                title: {
                    display: true,
                    fontSize: 25,
                    text: 'Số lượt tham gia hoạt động'
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                min: 0
                            }
                        }
                    ]
                }
            }
        });
    }
    function addDataBieuDoSoLuotThamGiaHoatDong(chart, data) {
        //Vì dùng chung Dto SoHoatDongToChucTrongThang (xem chi tiết ở api)
        chart.data.labels = data.danhSachThang;
        chart.data.datasets[0].data = data.soHoatDongHocVienToChuc;
        chart.update();
    }
    var taoNamHocThongKe = function (khoaHoc) {
        var namBatDau = (new Date(khoaHoc.namBatDau)).getFullYear();
        var namKetThuc = (new Date(khoaHoc.namKetThuc)).getFullYear();
        var linkNamHocTemplate = _.template($("#linkNamHoc_Template").html());
        for (var namHoc = namBatDau; namHoc < namKetThuc; namHoc++) {
            $("#linkNamHocThongKe").append(linkNamHocTemplate({ namHoc: namHoc }));
        }
    }
    var themDuLieuVaoBieuDo = function (jsonResult) {
        addDataBieuDoSoLuotThamGiaHoatDong(bieuDoSoLuotThamGiaHoatDong, jsonResult);
    }
    var updateBieuDo = function (e) {
        var link = $(e.target);
        var namHocLay = link.attr("id");
        trangChuHoatDongService.layThongKeHoatDongSinhVien(sinhVienDto.id, namHocLay, themDuLieuVaoBieuDo);
    }
    //Tạo bảng
    var initHoatDongSvThamGiaTable = function () {
        hoatDongSvThamGiaTable = $("#hoatDongThamGia_ThongKeHdSv").DataTable({
            data: [],
            "order": [[1, "asc"]],
            columns: [
                { data: "tenHoatDong" },
                { data: "ngayBatDau" },
                { data: "ngayKetThuc" }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                },
                {
                    targets: 2,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                }
            ]
        });
    }
    //UpdateDom
    var updateDom = function () {
        $("#anhBiaSv_ThongKeHdSv").attr("src", sinhVienDto.anhDaiDien);
        $(".thongTinSvContainer_ThongKeHdSv").attr("href", returnSvLink(sinhVienDto.id));
        $("#tenSinhVien_ThongKeHdSv").html(returnTenSinhVien(sinhVienDto));
        $("#MSSV_ThongKeHdSv").html(sinhVienDto.mssv);
        $("#tenLop_ThongKeHdSv").html(sinhVienDto.kyHieuTenLop);
        reloadTable(hoatDongSvThamGiaTable, sinhVienDto.danhSachHoatDongThamGia);
        taoNamHocThongKe(sinhVienDto.khoaHoc);
    }
    var reloadPage = function (data) {
        sinhVienDto = data;
        updateDom();
    }
    //Init Trang
    var initTrang = function (sinhVienId) {
        dataTableSetting();
        initHoatDongSvThamGiaTable();
        initBieuDoSoLuotThamGiaHoatDong();
        trangChuHoatDongService.layHoatDongSinhVienThamGia(sinhVienId, reloadPage);
        $("body").on("click", ".namHoc-link", updateBieuDo);
    }
    return{
        initTrang: initTrang
    }

}(TrangChuHoatDongService)