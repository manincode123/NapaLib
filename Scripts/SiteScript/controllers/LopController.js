var returnLoaiLop = function (lopChuyenNganh) {
    return lopChuyenNganh ? "Lớp chuyên ngành" : "";
}
var validateLopForm = function () {
    $("#lopForm").validate({
        ignore: "input[type=hidden]",
        rules: {
            tenLop: "required",
            kyHieuTenLop: "required",
            khoaHocId: "required"
        },
        messages: {
            tenLop: "Vui lòng nhập tên lớp",
            kyHieuTenLop: "Vui lòng nhập ký hiệu tên lớp",
            khoaHocId: "Vui lòng nhập chọn khóa học"
        }
    });

}
var limitCharacter = function () {
    limitCharacterForInput("tenLop");
    limitCharacterForInput("kyHieuTenLop");
}

var QuanLyKhoaHocController = function (quanLyKhoaHocService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var danhSachKhoaHocTable, danhSachLopTable, khoaHocDto = {};

    var validateKhoaHocForm = function() {
        $("#khoaHocForm").validate({
            ignore: [],
            rules: {
                tenKhoaHoc: "required",
                namBatDau: "required",
                namKetThuc: "required"
            },
            messages: {
                tenKhoaHoc: "Vui lòng nhập tên khóa học",
                namBatDau: "Vui lòng nhập ngày bắt đầu khóa học",
                namKetThuc: "Vui lòng nhập ngày kết thúc khóa học"
            }
        });
    }
    var initDanhSachKhoaHocTable = function () {
        danhSachKhoaHocTable = $("#danhSachKhoaHoc").DataTable({
            ajax: {
                url: "/api/KhoaHoc/LayDanhSachKhoa",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            autoWidth: false,
            order: [[0, "desc"]],
            columns: [
                { data: "tenKhoa" },
                { data: "namBatDau" },
                { data: "namKetThuc" },
                { data: "" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 1,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY", "vi")
                },
                {
                    targets: 2,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY", "vi")
                },
                {
                    targets: 3,
                    width: 300,
                    render: function () {
                        return '<button class="btn btn-primary suaKhoaHoc-js">Sửa thông tin</button>' +
                                '<button class="btn btn-danger xemDsLopKhoa-js">Xem danh sách lớp</button>';
                    }
                }
            ]
        });
    }
    var taoLinkLop = function (tenLop, lopId) {
        return '<a href="/Lop/ChiTiet/' + lopId + '"class="link">' + tenLop + '</a>';
    }
    var initDanhSachLopTable = function () {
        danhSachLopTable = $("#danhSachLop").DataTable({
            data: [],
            "order": [[0, 'desc']],
            columns: [
                { data: 'tenLop' },
                { data: 'lopChuyenNganh' }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkLop(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: function (data) {
                        return returnLoaiLop(data);
                    }
                }
            ]
        });
    }
    var reloadDanhSachLop = function (danhSachLop) {
        reloadTable(danhSachLopTable, danhSachLop);
    }
    var xemDanhSachLop = function (e) {
        var button = $(e.target);
        var khoaHocId = button.closest("tr").attr("id");
        quanLyKhoaHocService.layDanhSachLopService(khoaHocId, reloadDanhSachLop);
        $('html,body').stop().animate({
            scrollTop: $("#danhSachLop").offset().top
        }, 1000);
        event.preventDefault();
                                                            
    }
    var resetInput = function () {
        $("#khoaHocId").val(0);
        setDataForInputWithLimit("tenKhoaHoc", "");
        $("#namBatDau").val(null);
        $("#namKetThuc").val(null);
    }
    var updateInput = function (khoaHoc) {
        $("#khoaHocId").val(khoaHoc.id);
        setDataForInputWithLimit("tenKhoaHoc", khoaHoc.tenKhoa);
        $("#namBatDau").val(returnDateForInput(khoaHoc.namBatDau));
        $("#namKetThuc").val(returnDateForInput(khoaHoc.namKetThuc));
    }
    var hienThiModalThemKhoaHoc = function () {
        resetInput();
        $("#SaveKhoaHocModal-title").html("Thêm Khóa học");
        $("#saveKhoaHoc").html("Thêm khóa mới");
        $("#SaveKhoaHocModal").modal("show");
    }
    var hienThiModalSuaKhoaHoc = function(e) {
        var button = $(e.target);
        var khoaHocId = button.closest("tr").attr("id");
        quanLyKhoaHocService.layKhoaHocData(khoaHocId, updateInput);
        $("#SaveKhoaHocModal-title").html("Sửa thông tin Khóa học");
        $("#saveKhoaHoc").html("Lưu thay đổi");
        $("#SaveKhoaHocModal").modal("show");
    }
    var bindKhoaHocDto = function() {
        khoaHocDto.id = $("#khoaHocId").val();
        khoaHocDto.tenKhoa = $("#tenKhoaHoc").val();
        khoaHocDto.namBatDau = $("#namBatDau").val();
        khoaHocDto.namKetThuc = $("#namKetThuc").val();
    }
    var updateAfterSaveKhoaHoc = function (xhr) {
        hideLoader();
        if (xhr == null) {
            if (khoaHocDto.id == 0) {
                alert("Đã thêm khóa học mới.");
            } else {
                alert("Đã thay đổi thông tin khóa học.");
            }
            danhSachKhoaHocTable.ajax.reload();
        } else {
            alert(xhr.responseJSON.message);
        }
        //Ẩn modal
        $("#SaveKhoaHocModal").modal("hide");
    }
    var saveKhoaHoc = function() {
        if ($("#khoaHocForm").valid()) {
            showLoader();
            bindKhoaHocDto();
            quanLyKhoaHocService.saveKhoaHoc(khoaHocDto, updateAfterSaveKhoaHoc);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    var initTrang = function () {
        dataTableSetting();
        initDanhSachKhoaHocTable();
        initDanhSachLopTable();
        validateKhoaHocForm();
        limitCharacterForInput("tenKhoaHoc");
        $("body").on("click", ".xemDsLopKhoa-js", xemDanhSachLop);
        $("body").on("click", ".suaKhoaHoc-js", hienThiModalSuaKhoaHoc);
        $("#themKhoaHoc").on("click", hienThiModalThemKhoaHoc);
        $("#saveKhoaHoc").on("click", saveKhoaHoc);
    }

return{
    initTrang: initTrang
}

}(QuanLyKhoaHocService);

var QuanLyChungLopController = function (quanLyChungLopService) {
    //Biến chung
    var lopDto = {};
    var danhSachKhoa;
    var dataAnhBia = new FormData();
    var rawImg;
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    //Biến trang quản lý chung
    var danhSachLopTable;
    //Biến trang quản lý chi tiết lớp
    var danhSachSvTable;
    var lop, lopId;
    var daTaoBaiVietLop = false, daTaoHoatDongLop = false, daTaoBanCanSuLop = false;

    //Phần upload ảnh bìa đơn vị
    var exportCroppie = function ($uploadCrop, dataAnhBia, lopId) {
        var tenFile = "AnhBiaLop" + lopId;
        //Tạo File ảnh thật sự để save lên server (lưu vào formData dataAnhBia trước khi gửi ajax)
        $uploadCrop.croppie('result',
            {
                type: "blob",
                size: { width: 1200, height: 400 }
            }).then(function (blob) {
                //dataAnhBia.delete("image"); 
                //Cái này sẽ được đặt ở method reset,update input của
                //hiển thị modal thêm và modal sửa đơn vị để tránh trường hợp dataAnhBia trước đó còn lại
                dataAnhBia.append("image", blob, tenFile);
            });

        //Tạo File ảnh để hiển thị cho người dùng
        $uploadCrop.croppie("result", {
            type: "base64",
            format: "jpeg",
            size: { width: 1200, height: 400 }
        }).then(function (linkAnh) {
            $("#anhBiaLop").attr("src", "");
            $("#anhBiaLop").attr("src", linkAnh);
            $("#cropImagePop").modal("hide");
        });
    }
    var initCroppie = function () {
        //Và làm ởn nhớ là khi sử dụng croppie thì nhớ Include file croppie.css
        $("#anhBiaLop_input").change(function () {
            if (this.files && this.files[0]) {
                var imageDir = new FileReader();
                imageDir.readAsDataURL(this.files[0]);
                imageDir.onload = function (e) {
                    $("#anhBiaLop-Wrapper").addClass("ready");
                    $("#cropImagePop").modal("show"); //Show modal
                    $("#editAnhBiaBtn").css("display", "block"); //Hiển thị nút chỉnh sửa
                    rawImg = e.target.result; //Bind link ảnh thô với file vừa up lên
                }
            }
        });
        //Tạo instance croppie
        //Lưu ý quan trọng, khi sử dụng croppie, node chứa instance croppie (ở đây là anhBiaDonVi-Wrapper) phải
        // được khai báo width và height cụ thể bằng px, không thể croppie sẽ không hiện ra.
        var $uploadCrop = $("#anhBiaLop-Wrapper").croppie({
            viewport: {
                width: 900,
                height: 300,
                type: "square"
            },
            enableExif: true
        });
        //Bind rawImg (file hình ảnh thô) vào croppie khi modal thêm ảnh hiện
        $("#cropImagePop").on("shown.bs.modal", function () {
            $uploadCrop.croppie("bind", { url: rawImg });
        });
        //Tạo ảnh khi nhấn nút crop
        $("#cropImageBtn").on("click", function () {
            var lopId = $("#lopId").val(); //Phải lấy bằng cách này vì lúc này chưa bindDonViDto()
            exportCroppie($uploadCrop, dataAnhBia, lopId);
        });
    }
    var bindAnhBiaLop = function (data) {
        lopDto.anhBia = data;
    }
    var uploadAnhBiaLop = function () {
        if (dataAnhBia.has("image")) {
            quanLyChungLopService.uploadAnhBiaLop(dataAnhBia, bindAnhBiaLop, lopDto.id);
        }
    }

    //Thêm, chỉnh sửa lớp (Func dùng chung cho 2 trang)
    var initKhoaHocSelectList = function () {
        danhSachKhoa = quanLyChungLopService.layDanhSachKhoa();
        $("#khoaHocId").select2({
            dropdownParent: $("#SaveLopModal"),
            placeholder: "Chọn một khóa",
            language: "vi",
            data: danhSachKhoa
        });
    }
    var setLopChuyenNganh = function (lopChuyenNganh) {
        if (lopChuyenNganh) $("#lopChuyenNganh-checkbox").prop("checked", true);
        else $("#lopChuyenNganh-checkbox").prop("checked", false);
    }
    var updateInput = function (dataLop) {
        $("#lopId").val(dataLop.id);
        setDataForInputWithLimit("tenLop", dataLop.tenLop);
        setDataForInputWithLimit("kyHieuTenLop", dataLop.kyHieuTenLop);
        $("#khoaHocId").val(dataLop.khoaHocId);
        $("#khoaHocId").trigger("change");
        setLopChuyenNganh(dataLop.lopChuyenNganh);
        $("#anhBiaLop-input").attr("src", dataLop.anhBia);
    }
    var resetInput = function () {
        $("#lopId").val(0);
        setDataForInputWithLimit("tenLop", "");
        setDataForInputWithLimit("kyHieuTenLop", "");
        $("#khoaHocId").val(null);
        $("#khoaHocId").trigger("change");
        $("#lopChuyenNganh-checkbox").prop("checked", false);
    }
    var mapObject = function () {
        lopDto.id = $("#lopId").val();
        lopDto.tenlop = $("#tenLop").val();
        lopDto.kyHieuTenLop = $("#kyHieuTenLop").val();
        lopDto.khoaHocId = $("#khoaHocId").val();
        lopDto.lopChuyenNganh = $("#lopChuyenNganh-checkbox").prop("checked");
        lopDto.anhBia = $("#anhBiaLop").attr("src");
    }
    var hienThiThemLopModal = function () {
        resetInput();
        $("#SaveLopModal").modal("show");
    }
    var hienThiModalSuaLop_TrangQuanLyChung = function (e) {
        //Lấy data lớp
        var button = $(e.target);
        var lopId = button.closest("tr").attr("id");
        quanLyChungLopService.getLopData(lopId, updateInput);
        //Hiển thị modal
        $("#anhBiaLop-helpText").hide();
        $("#anhBiaLop_input").show();
        $("#anhBiaLop_input").prop("disabled", false);
        $("#SaveLopModal").modal("show");
    }
    var updateTrangSauKhiSave_TrangQuanLyChung = function (xhr) {
        hideLoader();
        if (xhr == null) {
            if (lopDto.id == 0) alert("Đã tạo lớp mới.");
            else alert("Đã thay đổi thông tin lớp.");
            danhSachLopTable.ajax.reload();
        }
        else alert(xhr.responseJSON.message);
        $("#SaveLopModal").modal("hide");
    }
    var saveLop_TrangQuanLyChung = function () {
        if ($("#lopForm").valid()) {
            showLoader();
            mapObject();
            uploadAnhBiaLop();
            quanLyChungLopService.saveLop(lopDto, updateTrangSauKhiSave_TrangQuanLyChung);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    var hienThiModalSuaLop_TrangQuanLyLop = function () {
        quanLyChungLopService.getLopData(lopId, updateInput);
        //Hiển thị modal
        $("#anhBiaLop-helpText").hide();
        $("#anhBiaLop_input").show();
        $("#anhBiaLop_input").prop("disabled", false);
        $("#SaveLopModal").modal("show");
    }
    var updateTrangSauKhiSave_TrangQuanLyLop = function (xhr) {
        hideLoader();
        if (xhr == null) {
            alert("Đã thay đổi thông tin lớp.");
            location.reload(true);
        }
        else alert(xhr.responseJSON.message);
        $("#SaveLopModal").modal("hide");
    }
    var saveLop_TrangQuanLyLop = function () {
        if ($("#lopForm").valid()) {
            showLoader();
            mapObject();
            uploadAnhBiaLop();
            quanLyChungLopService.saveLop(lopDto, updateTrangSauKhiSave_TrangQuanLyLop);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }

    /*Trang quản lý chung lớp*/
    //Init Dom
    var taoLinkLop = function (tenLop, lopId) {
        return '<a href="/Lop/ChiTiet/' + lopId + '"class="link">' + tenLop + '</a>';
    }
    var taoLinkKhoa = function (tenKhoa, khoaId) {
        return '<a href="/Khoa/ChiTiet/' + khoaId + '" class="link">' + tenKhoa + '</a>';

    }
    var initDanhSachLop = function () {
        danhSachLopTable = $("#danhSachLop-TrangQuanLyChungLop").DataTable({
            ajax: {
                url: "/api/Lop/QuanLyChung",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            order: [[1, "desc"]],
            columns: [
                { data: "tenLop" },
                { data: "kyHieuTenLop" },
                { data: "tenKhoa" },
                { data: "soLuongSinhVien" },
                { data: "soLuongHoiVien" },
                { data: "soLuongDoanVien" },
                { data: "lopChuyenNganh" },
                { data: "" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    render: function(data, type, row) {
                        return taoLinkLop(data, row.id);
                    }
                },
                {
                    targets: 1,
                    render: function(data) {
                        return data;
                    }
                },
                {
                    targets: 2,
                    render: function(data, type, row) {
                        return taoLinkKhoa(data, row.khoaId);
                    }
                },
                {
                    targets: 6,
                    render: function(data) {
                        return returnLoaiLop(data);
                    }
                },
                {
                    targets: 7,
                    width: 150,
                    render: function () {
                        return '<button class="btn btn-primary suaLop-js">Sửa thông tin</button>' +
                               '<button class="btn btn-success quanLySinhVienLop">Quản lý Sinh viên</button>' +
                               '<button class="btn btn-info quanLyChucVuLop">Quản lý Chức vụ</button>';
                    }
                }
            ]
        });
    }
    //Func Chuyển trang
    var chuyenDenTrangQuanLySinhVienLop = function(e) {
        var button = $(e.target);
        var lopId = button.closest("tr").attr("id");
        window.location = "/Lop/QuanLySinhVienLop/" + lopId;
    }
    var chuyenDenTrangQuanLyChucVuLop  = function(e) {
        var button = $(e.target);
        var lopId = button.closest('tr').attr('id');
        window.location = "/Lop/QuanLyChucVuLop/" + lopId;

    }
    /*Trang quản lý chung lớp*/

    /*Trang quản lý lớp cụ thể*/
    //UpdateDom
    var themBaiViet = function (data) {
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        $("#nav-baiVietLop .tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: data }));
    }
    var themHoatDong = function (data) {
        var danhSachHoatDong = _.template($("#hoatDongMoi_Template").html());
        $("#nav-hoatDongLop .tinMoiWrapper").append(danhSachHoatDong({ danhSachHoatDong: data }));
    }
    var initDanhSachBaiVietLop = function () {
        if (daTaoBaiVietLop) return;
        quanLyChungLopService.layBaiVietLop(lopId, themBaiViet);
        daTaoBaiVietLop = true;
    }
    var initDanhSachHoatDongLop = function () {
        if (daTaoHoatDongLop) return;
        var layHoatDongDto = {};
        layHoatDongDto.orderType = "DESC";
        layHoatDongDto.lopId = lopId;
        quanLyChungLopService.layHoatDongLop(layHoatDongDto, themHoatDong);
        daTaoHoatDongLop = true;
    }
    var initDanhSachBanCanSuLop = function () {
        if (daTaoBanCanSuLop) return;
        quanLyChungLopService.layDanhSachChucVu(lopId, appendChucVuLop);  //Lay danh sach SV
        daTaoBanCanSuLop = true;
    }
    function appendChucVuLop(chucVuLop) {
        var soUyVienDoan = 0;
        var soUyVienHoi = 0;
        var tenChucVu;
        chucVuLop.forEach(function (chucVuSV) {
            switch (chucVuSV.chucVuId) {
                case 1:
                    tenChucVu = "LopTruong";
                    break;
                case 2:
                    tenChucVu = "LopPho";
                    break;
                case 3:
                    tenChucVu = "ThuQuy";
                    break;
                case 4:
                    tenChucVu = "BiThu";
                    break;
                case 5:
                    tenChucVu = "PhoBiThu";
                    break;
                case 6:
                    soUyVienDoan++;
                    tenChucVu = "UyVienDoan" + soUyVienDoan;
                    break;
                case 7:
                    tenChucVu = "ChiHoiTruong";
                    break;
                case 8:
                    tenChucVu = "ChiHoiPho";
                    break;
                case 9:
                    soUyVienHoi++;
                    tenChucVu = "UyVienHoi" + soUyVienHoi;
                    break;
            }

            $('#' + tenChucVu + 'Anh-TrangChiTietLop').attr("src", chucVuSV.sinhVien.anhDaiDien);
            $('#' + tenChucVu + 'Anh-TrangChiTietLop').parent().attr("href", returnSvLink(chucVuSV.sinhVien.id));
            $('#ten' + tenChucVu + '-TrangChiTietLop').attr("href", returnSvLink(chucVuSV.sinhVien.id));
            $('#ten' + tenChucVu + '-TrangChiTietLop').append(returnTenSinhVien(chucVuSV.sinhVien));
        });
    }
    function initDanhSachSvTable() {
        danhSachSvTable = $("#danhSachSv-TrangChiTietLop").DataTable({
            data: [],
            autoWidth: false,
            deferRender: true,
            dom: 'Bflrtip',
            buttons: [
                "copy", "excel", "pdf"
            ],
            columns: [
                { data: "anhDaiDien" },
                { data: "ten" },
                { data: "mssv" },
                { data: "gioiTinh" },
                { data: "ngaySinh" }
            ],
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    render: function (data) {
                        return returnAnhSv(data);
                    }
                },
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                },
                {
                    targets: 4,
                    render: function (data) {
                        return returnNgay(data);
                    }
                }
            ]
        });
    }
    var updateDom = function (result) {
        lop = result;
        $("#siSo-TrangChiTietLop").html(lop.soLuongSV);
        $("#soNu-TrangChiTietLop").html(lop.soNu);
        $("#soNam-TrangChiTietLop").html(lop.soNam);
        $("#khac-TrangChiTietLop").html(lop.khac);

        reloadTable(danhSachSvTable, lop.danhSachSinhVien);
    }


    //Execute
    var initTrangQuanLyChungLop = function () {
        dataTableSetting();
        initDanhSachLop();
        initKhoaHocSelectList();
        validateLopForm();
        limitCharacter();
        initCroppie();
        $("#saveLopBtn").on("click", hienThiThemLopModal);
        $("#saveLop").on("click", saveLop_TrangQuanLyChung);
        $("body").on("click", ".suaLop-js", hienThiModalSuaLop_TrangQuanLyChung);
        $("body").on("click", ".quanLySinhVienLop", chuyenDenTrangQuanLySinhVienLop);
        $("body").on("click", ".quanLyChucVuLop", chuyenDenTrangQuanLyChucVuLop);
    }
    var initTrangChiTietLop = function (idLop) {
        lopId = idLop;
        dataTableSetting();
        initDanhSachSvTable();
        quanLyChungLopService.layThongTinLop(lopId, updateDom);
        $("#nav-baiVietLop-tab").on("click", initDanhSachBaiVietLop);
        $("#nav-hoatDongLop-tab").on("click", initDanhSachHoatDongLop);
        $("#nav-banCanSu-tab").on("click", initDanhSachBanCanSuLop);
    }
    var initTrangQuanLyLop = function (idLop) {
        initTrangChiTietLop(idLop);
        initKhoaHocSelectList();
        validateLopForm();
        limitCharacter();
        initCroppie();
        $("#suaThongTinLopBtn").on("click", hienThiModalSuaLop_TrangQuanLyLop);
        $("#saveLop").on("click", saveLop_TrangQuanLyLop);
    }


    return {
        initTrangQuanLyChungLop: initTrangQuanLyChungLop,
        initTrangQuanLyLop: initTrangQuanLyLop,
        initTrangChiTietLop: initTrangChiTietLop
    }
}(QuanLyChungLopService);

var QuanLyLopController = function (quanLyChungLopService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var danhSachSvTable,danhSachKhoa;
    var lop, lopId, lopDto = {};
    var daTaoBaiVietLop = false,daTaoHoatDongLop = false,daTaoBanCanSuLop = false; 
    //UpdateDom
    var themBaiViet = function (data) {
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        $("#nav-baiVietLop .tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: data }));
    }
    var themHoatDong = function (data) {
        var danhSachHoatDong = _.template($("#hoatDongMoi_Template").html());
        $("#nav-hoatDongLop .tinMoiWrapper").append(danhSachHoatDong({ danhSachHoatDong: data }));
    }
    var initDanhSachBaiVietLop = function () {
        if (daTaoBaiVietLop) return;
        quanLyChungLopService.layBaiVietLop(lopId, themBaiViet);
        daTaoBaiVietLop = true;
    }
    var initDanhSachHoatDongLop = function () {
        if (daTaoHoatDongLop) return;
        var layHoatDongDto = {};
        layHoatDongDto.orderType = "DESC";
        layHoatDongDto.lopId = lopId;
        quanLyChungLopService.layHoatDongLop(layHoatDongDto, themHoatDong);
        daTaoHoatDongLop = true;
    }
    var initDanhSachBanCanSuLop = function () {
        if (daTaoBanCanSuLop) return;
        quanLyChungLopService.layDanhSachChucVu(lopId, appendChucVuLop);  //Lay danh sach SV
        daTaoBanCanSuLop = true;
    }

 
    function appendChucVuLop(chucVuLop) {
        var soUyVienDoan = 0;
        var soUyVienHoi = 0;
        var tenChucVu;
        chucVuLop.forEach(function (chucVuSV) {
            switch (chucVuSV.chucVuId) {
                case 1:
                    tenChucVu = "LopTruong";
                    break;
                case 2:
                    tenChucVu = "LopPho";
                    break;
                case 3:
                    tenChucVu = "ThuQuy";
                    break;
                case 4:
                    tenChucVu = "BiThu";
                    break;
                case 5:
                    tenChucVu = "PhoBiThu";
                    break;
                case 6:
                    soUyVienDoan++;
                    tenChucVu = "UyVienDoan" + soUyVienDoan;
                    break;
                case 7:
                    tenChucVu = "ChiHoiTruong";
                    break;
                case 8:
                    tenChucVu = "ChiHoiPho";
                    break;
                case 9:
                    soUyVienHoi++;
                    tenChucVu = "UyVienHoi" + soUyVienHoi;
                    break;
            }

            $('#' + tenChucVu + 'Anh-TrangChiTietLop').attr("src", chucVuSV.sinhVien.anhDaiDien);
            $('#' + tenChucVu + 'Anh-TrangChiTietLop').parent().attr("href", returnSvLink(chucVuSV.sinhVien.id));
            $('#ten' + tenChucVu + '-TrangChiTietLop').attr("href", returnSvLink(chucVuSV.sinhVien.id));
            $('#ten' + tenChucVu + '-TrangChiTietLop').append(returnTenSinhVien(chucVuSV.sinhVien));
        });
    }  
    function initDanhSachSvTable() {
        danhSachSvTable = $("#danhSachSv-TrangChiTietLop").DataTable({
            data: [],
            autoWidth: false,
            deferRender: true,
            dom: 'Bflrtip',
            buttons: [
                "copy", "excel", "pdf"
            ],
            columns: [
                { data: "anhDaiDien" },
                { data: "ten" },
                { data: "mssv" },
                { data: "gioiTinh" },
                { data: "ngaySinh" }
            ],
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    render: function (data) {
                        return returnAnhSv(data);
                    }
                },
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                },
                {
                    targets: 4,
                    render: function (data) {
                        return returnNgay(data);
                    }
                }
            ]
        });
    }    
    var updateDom = function (result) {
        lop = result;
        $("#siSo-TrangChiTietLop").html(lop.soLuongSV);
        $("#soNu-TrangChiTietLop").html(lop.soNu);
        $("#soNam-TrangChiTietLop").html(lop.soNam);
        $("#khac-TrangChiTietLop").html(lop.khac);

        reloadTable(danhSachSvTable, lop.danhSachSinhVien);
    }
    //Thêm, chỉnh sửa lớp
    var initKhoaHocSelectList = function () {
        danhSachKhoa = quanLyChungLopService.layDanhSachKhoa();
        $("#khoaHocId").select2({
            dropdownParent: $("#SaveLopModal"),
            placeholder: "Chọn một khóa",
            language: "vi",
            data: danhSachKhoa
        });
    }
    var setLopChuyenNganh = function (lopChuyenNganh) {
        if (lopChuyenNganh) $("#lopChuyenNganh-checkbox").prop("checked", true);
        else $("#lopChuyenNganh-checkbox").prop("checked", false);
    }
    var updateInput = function (dataLop) {
        $("#lopId").val(dataLop.id);
        setDataForInputWithLimit("tenLop", dataLop.tenLop);
        setDataForInputWithLimit("kyHieuTenLop", dataLop.kyHieuTenLop);
        $("#khoaHocId").val(dataLop.khoaHocId);
        $("#khoaHocId").trigger("change");
        setLopChuyenNganh(dataLop.lopChuyenNganh);
        $("#anhBiaLop-input").attr("src",dataLop.anhBia);
    }
    var hienThiModalSuaLop = function () {
        quanLyChungLopService.getLopData(lopId, updateInput);
        //Hiển thị modal
        $("#SaveLopModal").modal("show");
    }
    var updateTrangSauKhiSave = function (xhr) {
        hideLoader();
        if (xhr == null) {
            alert("Đã thay đổi thông tin lớp.");
            location.reload(true);
        }
        else alert(xhr.responseJSON.message);
        $("#SaveLopModal").modal("hide");
    }
    var mapObject = function () {
        lopDto.id = $("#lopId").val();
        lopDto.tenlop = $("#tenLop").val();
        lopDto.kyHieuTenLop = $("#kyHieuTenLop").val();
        lopDto.khoaHocId = $("#khoaHocId").val();
        lopDto.lopChuyenNganh = $("#lopChuyenNganh-checkbox").prop("checked");
        lopDto.anhBia = $("#anhBiaLop-input").attr("src");
    }
    var saveLop = function () {
        if ($("#lopForm").valid()) {
            showLoader();
            mapObject();
            quanLyChungLopService.saveLop(lopDto, updateTrangSauKhiSave);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    //Init Trang
    var initChonAnhBia = function () {
        $("body").on("click", "#selectAnhBiaButton", function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $("#anhBiaLop-input").attr("src", fileUrl);
            }
            finder.popup();
        });
    }
    var initPage = function (idLop) {
        lopId = idLop;
        dataTableSetting();
        initDanhSachSvTable();
        quanLyChungLopService.layThongTinLop(lopId, updateDom);
        $("#nav-baiVietLop-tab").on("click", initDanhSachBaiVietLop);
        $("#nav-hoatDongLop-tab").on("click", initDanhSachHoatDongLop);
        $("#nav-banCanSu-tab").on("click", initDanhSachBanCanSuLop);
    }
    var initPage_QuanLy = function (idLop) {
        initPage(idLop);
        initKhoaHocSelectList();
        initChonAnhBia();
        validateLopForm();
        limitCharacter();
        $("#suaThongTinLopBtn").on("click", hienThiModalSuaLop);
        $("#saveLop").on("click", saveLop);
    }

return {
    initPage: initPage,
    initPage_QuanLy: initPage_QuanLy
}

}(QuanLyChungLopService);

var QuanLyHoatDongChoPheDuyetLopController = function (quanLyHoatDongLopService) {
    var hdChoPheDuyetTable;
    var lopId, hoatDongId;
    var initHdChoPheDuyetTable = function () {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        hdChoPheDuyetTable = $("#hdChoPheDuyetTable-TrangPheDuyet").DataTable({
            ajax: {
                url: "/api/Lop/HoatDongChoPheDuyet/"+lopId,
                dataSrc: "",
                headers: { __RequestVerificationToken: csrfToken }
            },
            "order": [[1, 'desc']],
            columns: [
                { data: "tenHoatDong" },
                { data: "ngayBatDau" },
                { data: "ngayKetThuc" },
                { data: "" },
                { data: "" },
                { data: "" }
            ],
            rowId: "id",
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
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return returnTenSinhVien(row);
                    }
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return taoDonViToChuc(row);
                    }
                },
                {
                    targets: 5,
                    width: 100,
                    render: function (data, type, row) {
                        if (row.danhSachDonViToChuc.length + row.danhSachLopToChuc.length > 1)
                            return '<button class="btn btn-danger huy-js">Hủy hoạt động</button>' +
                                   '<button class="btn btn-warning huyToChuc-js">Hủy tham gia tổ chức</button>';
                        return '<button class="btn btn-danger huy-js">Hủy hoạt động</button>';

                    }
                }
            ]
        });
    }
    //Hủy tham gia tổ chức hoạt động
    var hienThiModalHuyThamGiaHoatDong = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        hoatDongId = button.closest("tr").attr("id");
        $("#thongBaoModal-body").html("Bạn có chắn chắn muốn hủy tham gia tổ chức hoạt động này? Lớp bạn sẽ bị xóa tên khỏi danh sách các đơn vị tổ chức.");
        $("#huyHoatDong").css("display", "none");
        $("#huyToChucHoatDong").css("display", "inline-block");
        $("#thongBaoModal").modal("show");
    }
    var reloadPage = function (xhr) {
        hideLoader();
        if (xhr == null) {
            alert("Đã bỏ tham gia tổ chức hoạt động hoạt động");
            hdChoPheDuyetTable.ajax.reload();
            
        } else alert(xhr.responseJSON.message);
        $("#thongBaoModal").modal("hide");
    }
    var huyToChucHoatDong = function () {
        showLoader();
        var huyToChucDto = {
            lopId : lopId,
            hoatDongId: parseInt(hoatDongId)
        }
        quanLyHoatDongLopService.huyToChucHoatDong(huyToChucDto,reloadPage);
    }
    //Hủy hoạt động
    var hienThiModalHuyHoatDong = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        hoatDongId = button.closest("tr").attr("id");
        $("#thongBaoModal-body").html("Bạn có chắn chắn muốn hủy hoạt động này?");
        $("#pheDuyetHoatDong").css("display", "none");
        $("#huyHoatDong").css("display", "inline-block");
        $("#thongBaoModal").modal("show");
    }
    var updatePageAfterHuyHoatDong = function (coHuyHoatDong, xhr) {
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
    var huyHoatDong = function () {
        showLoader();
        quanLyHoatDongLopService.huyHoatDong(hoatDongId, updatePageAfterHuyHoatDong);
    }

    var initTrang = function (idLop) {
        lopId = idLop;
        dataTableSetting();
        initHdChoPheDuyetTable();
        $("body").on("click", ".huy-js", hienThiModalHuyHoatDong);
        $("body").on("click", ".huyToChuc-js", hienThiModalHuyThamGiaHoatDong);
        $("#huyHoatDong").on("click", huyHoatDong);
        $("#huyToChucHoatDong").on("click", huyToChucHoatDong);
    }

    return {
        initTrang : initTrang
    }
}(QuanLyHoatDongLopService);

var QuanLySinhVienLopController = function (quanLySinhVienLopService) {
    var danhSachSv = [];
    var danhSachSvTable;
    var lopChuyenNganh;
    var idSinhVienMuonXoa, lopId;
    var sinhVienLopDto = {};

    var updateDom = function (data) {
        danhSachSv = data;
        reloadTable(danhSachSvTable, data);
    }
    var initDanhSachSvTable = function () {

        danhSachSvTable = $("#danhSachSv-TrangQuanLySinhVienLop").DataTable({
            data: [],
            autoWidth: false,
            dom: 'Bflrtip',
            buttons: [
                'copy', 'excel', 'pdf'
            ],
            rowId: 'id',
            columns: [
                { data: 'anhDaiDien' },
                { data: 'mssv' },
                { data: 'ten' },
                { data: 'ngaySinh' },
                { data: 'gioiTinh' },
                { data: 'id' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    render: function (data, type, row) {
                        return returnAnhSv(data);
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return '<button class="btn btn-danger js-delete-sinhVienLop">Xóa</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    //Thêm, xóa sinh viên
    var bindSinhVienLopDto = function (idSinhVien) {
        sinhVienLopDto.lopId = lopId;
        sinhVienLopDto.sinhVienId = idSinhVien;
        sinhVienLopDto.lopChuyenNganh = lopChuyenNganh;
    }
    //Thêm sinh viên
    var updatePageAfterThemSinhVien = function () {
        hideLoader();
        quanLySinhVienLopService.layDanhSachSinhVienLop(lopId, updateDom);
        alert("Đã thêm sinh viên.");
    }
    var themSinhVien = function (e, sinhVien) {
        //Kiểm tra xem sinh viên này đã có trong danh sách lớp chưa
        var sinhVienCoThamGia = $.grep(danhSachSv, function (obj) { return obj.mssv == sinhVien.mssv; })[0];
        if (sinhVienCoThamGia != null) alert("Sinh viên này đã có trong danh sách lớp");
        else {
            showLoader();
            bindSinhVienLopDto(sinhVien.id);
            quanLySinhVienLopService.themSinhVien(sinhVienLopDto, updatePageAfterThemSinhVien);
        }
        $("#input-TrangQuanLySinhVienLop").typeahead("val", "AS");
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
        $("#input-TrangQuanLySinhVienLop").typeahead(
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
                                + data.mssv + ' - ' + returnKyHieuTenLop(data.kyHieuTenLop) + '</p>';
                        }
                    }

                })
            .on("typeahead:autocomplete", themSinhVien)
            .on("typeahead:select", themSinhVien)
            .on("typeahead:change",
                function () {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
        $("#input-TrangQuanLySinhVienLop").typeahead("val", "AS");
    }
        //Xóa sinh viên
    var hienThiModalXoaSinhVien = function (e) {
        var button = $(e.target);
        //Lấy id chương trình muốn xóa
        idSinhVienMuonXoa = button.closest("tr").attr("id");
        var tenSinhVienMuonXoa = button.closest("tr").find(".tenSvDs").text();
        $("#tenSinhVienMuonXoa-TrangQuanLySinhVienLop").text(tenSinhVienMuonXoa);
        $("#XoaSinhVienModal").modal("show");
    }
    var updatePageAfterXoaSinhVien = function () {
        hideLoader();
        quanLySinhVienLopService.layDanhSachSinhVienLop(lopId, updateDom);
        alert("Đã xóa sinh viên.");
        $("#XoaSinhVienModal").modal("hide");
    }
    var xoaSinhVien = function () {
        showLoader();
        bindSinhVienLopDto(idSinhVienMuonXoa);
        quanLySinhVienLopService.xoaSinhVien(sinhVienLopDto, updatePageAfterXoaSinhVien);
        $("#XoaSinhVienModal").modal("hide");
    }


    var initTrang = function (idLop, lopChuyenNganhModel) {
        lopId = idLop;
        lopChuyenNganh = lopChuyenNganhModel;
        initTypeahead();
        dataTableSetting();
        initDanhSachSvTable();
        quanLySinhVienLopService.layDanhSachSinhVienLop(lopId, updateDom);
        $("body").on("click", ".js-delete-sinhVienLop", hienThiModalXoaSinhVien);
        $("#XoaSinhVien").on("click", xoaSinhVien);

    }

    return {
        initTrang: initTrang
    }
}(QuanLySinhVienLopService);

var QuanLyChucVuLopController = function (quanLyChucVuLopService) {
    var danhSachSinhVien, chucVuLop = [];
    var thongTinLop, chucVuMuonChinhSua, tenChucVu;
    var idSinhVienGiuChucVuMoi, idSinhVienGiuChucVuCu;
    var lopId;

    //Update Dom
    var appendChucVuLop = function (chucVuLop) {
        var sinhVienLink;
        var soUyVienDoan = 0, soUyVienHoi = 0;
        chucVuLop.forEach(function (chucVuSV) {
            switch (chucVuSV.chucVuId) {
            case 1:
                tenChucVu = "LopTruong";
                break;
            case 2:
                tenChucVu = "LopPho";
                break;
            case 3:
                tenChucVu = "ThuQuy";
                break;
            case 4:
                tenChucVu = "BiThu";
                break;
            case 5:
                tenChucVu = "PhoBiThu";
                break;
            case 6:
                soUyVienDoan++;
                tenChucVu = "UyVienDoan" + soUyVienDoan;
                break;
            case 7:
                tenChucVu = "ChiHoiTruong";
                break;
            case 8:
                tenChucVu = "ChiHoiPho";
                break;
            case 9:
                soUyVienHoi++;
                tenChucVu = "UyVienHoi"+soUyVienHoi;
                break;
            }
            sinhVienLink = returnSvLink(chucVuSV.sinhVien.id);
            $('#'+tenChucVu+'Anh-TrangChiTietLop').attr("src", chucVuSV.sinhVien.anhDaiDien);
            $('#' + tenChucVu + 'Anh-TrangChiTietLop').parent().attr("href", sinhVienLink);
            $('#ten'+tenChucVu+'-TrangChiTietLop').attr("href", sinhVienLink);
            $('#ten'+tenChucVu+'-TrangChiTietLop').html(returnTenSinhVien(chucVuSV.sinhVien));
            $('#ten' + tenChucVu + '-TrangChiTietLop').siblings(".ThayDoiChucVuButton")
                                                      .attr("sinhVienId", chucVuSV.sinhVien.id);
            $('#ten' + tenChucVu + '-TrangChiTietLop').siblings(".xoaChucVuButton")
                                                      .attr("sinhVienId", chucVuSV.sinhVien.id);
        });
    }
    var bindDanhSachSinhVien = function (data) {
        danhSachSinhVien = data;
    }
    //Thêm, thay đổi chức vụ
    var returnTenChucVu = function() {
        switch (chucVuMuonChinhSua) {
        case "1":   
            return "Lớp trưởng";
        case "2":
            return "Lớp phó";
        case "3":
            return "Thủ quỹ";
        case "4":
            return "Bí thư";
        case "5":
            return "Phó Bí thư";
        case "6":
            return "Ủy viên ban chấp hành Chi Đoàn";
        case "7":
            return "Chi Hội trưởng";
        case "8":
            return "Chi Hội phó";
        case "9":
            return "Ủy viên Ban chấp hành Chi Hội";
        }

    }
    var appendChucVuMuonDoi = function () {
        $("#tenChucVu-Modal").html(returnTenChucVu(chucVuMuonChinhSua));
        var sinhVienGiuChucVuCu = $.grep(danhSachSinhVien, function (obj) { return obj.id == idSinhVienGiuChucVuCu; })[0];
        if (sinhVienGiuChucVuCu == null) return;
        $('#anhSinhVienGiuChucVuCu').attr("src", sinhVienGiuChucVuCu.anhDaiDien);
        $('#anhSinhVienGiuChucVuCu').parent().attr("href", returnSvLink(sinhVienGiuChucVuCu.id));
        $('#tenSinhVienGiuChucVuCu').attr("href", returnSvLink(sinhVienGiuChucVuCu.id));
        $('#tenSinhVienGiuChucVuCu').html(returnTenSinhVien(sinhVienGiuChucVuCu));
    }
    var resetChinhSuaChucVu = function () {
        //Reset sinh viên giữ chức vụ cũ
        $('#anhSinhVienGiuChucVuCu').attr("src", "/Content/AnhBia/AnhSV/default-avatar.png");
        $('#anhSinhVienGiuChucVuCu').parent().attr("href", "");
        $('#tenSinhVienGiuChucVuCu').attr("href", "");
        $('#tenSinhVienGiuChucVuCu').empty();
        //Reset sinh viên giữ chức vụ mới
        $('#anhSinhVienGiuChucVuMoi').attr("src", "/Content/AnhBia/AnhSV/default-avatar.png");
        $('#anhSinhVienGiuChucVuMoi').parent().attr("href","");
        $('#tenSinhVienGiuChucVuMoi').attr("href","");
        $('#tenSinhVienGiuChucVuMoi').empty();
        $('#input-TrangQuanLyChucVuLop').typeahead("val", "");
        idSinhVienGiuChucVuMoi = 0;     
        chucVuMuonChinhSua = 0;
    }
    var hienThiModalThayDoiChucVu = function (e) {
        var button = $(e.target);
        resetChinhSuaChucVu();
        chucVuMuonChinhSua = button.attr("chucVu");
        idSinhVienGiuChucVuCu = button.attr("sinhVienId");
        appendChucVuMuonDoi();
        $("#ThayDoiChucVuModal").modal("show");
    }
    var themSinhVienThayDoi = function (e, sinhVienGiuChucVuMoi) {
        var sinhVienLink = returnSvLink(sinhVienGiuChucVuMoi.id);
        idSinhVienGiuChucVuMoi = sinhVienGiuChucVuMoi.id;
        $('#anhSinhVienGiuChucVuMoi').attr("src", sinhVienGiuChucVuMoi.anhDaiDien);
        $('#anhSinhVienGiuChucVuMoi').parent().attr("href", sinhVienLink);
        $('#tenSinhVienGiuChucVuMoi').attr("href", sinhVienLink);
        $('#tenSinhVienGiuChucVuMoi').html(returnTenSinhVien(sinhVienGiuChucVuMoi));
    }
    var initTypeahead = function () {
        var danhSachsinhVien = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('hoVaTenLot', 'ten', 'mssv', 'kyHieuTenLop'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: danhSachSinhVien
        });

        $('#input-TrangQuanLyChucVuLop').typeahead(
                {
                    highlight: true,
                    classNames: {
                        suggestion: 'typeahead-suggestion-QuanLyChucVuLop',
                        hint: 'typeahead-hint-QuanLyChucVuLop',
                        selectable: 'typeahead-selectable-QuanLyChucVuLop',
                        menu: 'typeahead-menu-QuanLyChucVuLop'
                    }
                },
                {
                    name: 'sinhVien',
                    display: function (data) {
                        return data.hoVaTenLot + " " + data.ten;
                    },
                    source: danhSachsinhVien,
                    templates: {
                        suggestion: function (data) {
                            return '<p><img src="' +
                                data.anhDaiDien +
                                '" class="anhSvTypeahead"/><strong>' +
                                data.hoVaTenLot +
                                ' ' +
                                data.ten +
                                '</strong> - ' +
                                data.mssv +
                                '</p>';
                        }
                    }
                })
            .on("typeahead:autocomplete", themSinhVienThayDoi)
            .on("typeahead:select", themSinhVienThayDoi)
            .on("typeahead:change",
                function () {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
    }
    var updatePageSauKhiChinhSua = function (data) {
        hideLoader();
        if (chucVuMuonChinhSua == 1 || chucVuMuonChinhSua == 4 || chucVuMuonChinhSua == 7)
            location.reload(true);
        appendChucVuLop(data);
        $("#ThayDoiChucVuModal").modal("hide");
    }
    var chinhSuaChucVu = function () {
        if (idSinhVienGiuChucVuMoi == 0) alert("Vui lòng chọn sinh viên để thay đổi");
        else {
            showLoader();
            var chinhSuaChucVuDto = {
                lopId: lopId,
                idSinhVienGiuChucVuMoi: idSinhVienGiuChucVuMoi,
                idSinhVienGiuChucVuCu: idSinhVienGiuChucVuCu,
                chucVuId: parseInt(chucVuMuonChinhSua)
            }
            quanLyChucVuLopService.chinhSuaChucVu(chinhSuaChucVuDto, updatePageSauKhiChinhSua);
        }
    }
    //Xóa chức vụ
    var appendChucVuMuonXoa = function () {
        $("#tenChucVu-ModalXoaChucVu").html(returnTenChucVu(chucVuMuonChinhSua));
        var sinhVien = $.grep(danhSachSinhVien, function (obj) { return obj.id == idSinhVienGiuChucVuCu; })[0];
        if (sinhVien == null) return;
        var sinhVienLink = returnSvLink(sinhVien.id);
        $('#anhSinhVienGiuChucVuCu-ModalXoaChucVu').attr("src", sinhVien.anhDaiDien);
        $('#anhSinhVienGiuChucVuCu-ModalXoaChucVu').parent().attr("href", sinhVienLink);
        $('#tenSinhVienGiuChucVuCu-ModalXoaChucVu').attr("href", sinhVienLink);
        $('#tenSinhVienGiuChucVuCu-ModalXoaChucVu').html(returnTenSinhVien(sinhVien));

    }
    var hienThiModalXoaChucVu = function(e) {
        var button = $(e.target);
        chucVuMuonChinhSua = button.attr("chucVu");
        idSinhVienGiuChucVuCu = button.attr("sinhVienId");
        if (idSinhVienGiuChucVuCu != null) {
            appendChucVuMuonXoa();
            $("#XoaChucVuModal").modal("show");
        } else {
            alert("Chưa có ai đảm nhận chức vụ này.");
        } 
    }
    var updatePageAfterXoaChucVu = function () {
        hideLoader();
        alert("Đã xóa chức vụ.");
        $("#XoaChucVuModal").modal("hide");
        location.reload(true);
    }

    var xoaChucVu = function () {
        showLoader();
            var xoaChucVuDto = {
                lopId: lopId,
                idSinhVienGiuChucVuCu: idSinhVienGiuChucVuCu,
                idSinhVienGiuChucVuMoi: 0,
                chucVuId: parseInt(chucVuMuonChinhSua)
            }
            quanLyChucVuLopService.xoaChucVu(xoaChucVuDto, updatePageAfterXoaChucVu);
    }                                                                                

    var initTrang = function (idLop) {
        lopId = idLop;
        quanLyChucVuLopService.layDanhSachSinhVien(lopId, bindDanhSachSinhVien);  //Lay danh sach SV
        quanLyChucVuLopService.layDanhSachChucVu(lopId, appendChucVuLop);  //Lay danh sach SV
        initTypeahead();
        $(".ThayDoiChucVuButton").on("click", hienThiModalThayDoiChucVu);
        $(".xoaChucVuButton").on("click", hienThiModalXoaChucVu);
        $("#ThayDoiChucVuLop").on("click", chinhSuaChucVu);
        $("#XoaChucVuLop").on("click", xoaChucVu);
    }

    return {
        initTrang: initTrang
}
}(QuanLyChucVuLopService);

var QuanLyHoatDongLopController = function (quanLyHoatDongLopService, quanLyHoatDongController) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var hdLopToChucTable, hdVaSoLuotTgTable;
    var bieuDoSoLuotThamGiaHoatDong, bieuDoSoHoatDongToChuc;
    var lopId, lopToChuc;
    var daUpdateHoatDongThamGia = false, daTaoBangHoatDong = false;
    var popoverContent = "";

    //Function để tạo bảng hoạt động lớp tổ chức và hoạt động sinh viên lớp tham gia
    var initHdLopToChucTable = function () {
        if (daTaoBangHoatDong) return;
        hdLopToChucTable = $("#hdLopToChuc-TrangChiTietLop").DataTable({
            ajax: {
                url: "/api/Lop/HoatDongToChuc/" + lopId,
                type: "POST",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: "data",
                data: function (d) {
                    d.pageSize = d.length;
                    d.draw = d.draw;
                    d.searchTerm = d.search.value;
                    d.orderColumn = d.columns[d.order[0].column].name;
                    d.orderType = d.order[0].dir;
                    d.startRecord = d.start;
                }
            },
            serverSide: true,
            processing: true,
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "tenHoatDong" },
                { data: "ngayBatDau" },
                { data: "ngayKetThuc" },
                { data: "soLuotThamGia" },
                { data: "capHoatDong" },
                { data: "" }
            ],
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.id);
                    }
                },
                {
                    targets: 1,
                    name: "NgayBatDau",
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY HH:mm:ss", "vi")
                },
                {
                    targets: 2,
                    name: "NgayKetThuc",
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY HH:mm:ss", "vi")
                },
                {
                    targets: 3,
                    name: "SoLuotThamGia"
                },
                {
                    targets: 4,
                    name: "CapHoatDong",
                    render: function (data) {
                        return returnCapHoatDong(data);
                    }
                },
                {
                    targets: 5,
                    orderable: false,
                    render: function (data, type, row) {
                        return taoTinhTrang(row);
                    }
                }
            ]
        });
        daTaoBangHoatDong = true;
    }
    var initHdVaSoLuotTgTable = function () {
        hdVaSoLuotTgTable = $("#hdVaSoLuotTg-TrangChiTietLop").DataTable({
            data: [],
            searching: false,
            lengthChange: false,
            columns: [
                { data: "tenHoatDong" },
                { data: "soLuotThamGiaLop" },
                { data: "soLuotThamGiaToanHoatDong" },
                { data: "ngayBatDau" },
                { data: "ngayKetThuc" }
            ],
            columnDefs: [
                {
                    targets: 3,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY HH:mm:ss", "vi")
                },
                {
                    targets: 4,
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY HH:mm:ss", "vi")
                },
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return taoLinkHoatDong(data, row.hoatDongId);
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var updateHoatDongVaLuotThamGia = function (result) {
        reloadTable(hdVaSoLuotTgTable, result.danhSachHdVaSoLuotThamGia);
        $("#thamGiaNamNay-TrangChiTietLop").html(result.tongLuotThamGiaTrongNam);
    }
    var updateHoatDongVaSoLuotThamGia = function (e) {
        var link = $(e.target);
        var namHocLay = link.attr("data-namHoc");
        quanLyHoatDongLopService.layLuotThamGiaHoatDongCuaLop(lopId,namHocLay, updateHoatDongVaLuotThamGia);    
    }
    //Func tạo hoạt đông đang diễn ra
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
    var themHoatDongVaoDom = function (danhSachHoatDong) {
        //Thêm số hoạt động
        $("#soHdDangDienRa-TrangChiTietLop").html(danhSachHoatDong.length);
        //Lấy template cho hoạt động
        var cardHoatDong_Template = _.template($("#cardHoatDong_Template").html());
        //Thêm hoạt động do lớp tổ chức và đang diễn ra
        danhSachHoatDong.forEach(function (hoatDong) {
            $("#danhSachHoatDong").append(cardHoatDong_Template({ hoatDong: hoatDong }));
            popoverContent = "";
            updateDvToChucHolder(hoatDong.id);
        });
    }
    var hienThisoHoatDongChoPheDuyet = function (soHdChoPheDuyet) {
        if (soHdChoPheDuyet > 0) {
            $("#soHdChoPheDuyetBdg").css("display", "inline-block");
            return soHdChoPheDuyet;
        }
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
                        label: 'Lượt tham gia của Lớp',
                        data: [],
                        datalabels: {
                            anchor: 'center',
                            align: 'center'
                        },
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 1
                    },
                    {
                        label: 'Lượt tham gia toàn Phân viện',
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
        chart.data.datasets[0].data = soLuotThamGiaHdTungThang.soLuotThamGiaLop;
        chart.data.datasets[1].data = soLuotThamGiaHdTungThang.soLuotThamGiaHocVien;
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
                        label: 'Của lớp tổ chức',
                        data: [],
                        datalabels: {
                            anchor: 'center',
                            align: 'center'
                        },
                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 1
                    },
                    {
                        label: 'Của cả phân viện',
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
        chart.data.datasets[0].data = soHoatDongToChucTungThang.soHoatDongLopToChuc;
        chart.data.datasets[1].data = soHoatDongToChucTungThang.soHoatDongHocVienToChuc;
        chart.update();
    }
    var taoNamHocThongKe = function (khoaHoc) {
        var namBatDau = (new Date(khoaHoc.namBatDau)).getFullYear();
        var namKetThuc = (new Date(khoaHoc.namKetThuc)).getFullYear();
        var linkNamHocTemplate = _.template($("#linkNamHoc_Template").html());
        for (var namHoc = namBatDau; namHoc < namKetThuc; namHoc++) {
            $("#linkNamHoc-ThongKe").append(linkNamHocTemplate({ namHoc: namHoc }));
            $("#linkNamHoc-ThamGia").append(linkNamHocTemplate({ namHoc: namHoc }));
            //Chưa làm cái này, vì lười
            //$("#linkNamHoc-HoatDongToChuc").append(linkNamHocTemplate({ namHoc: namHoc }));
        }
    }
    var themDuLieuVaoBieuDo = function (jsonResult) {
        addDataBieuDoSoLuotThamGiaHoatDong(bieuDoSoLuotThamGiaHoatDong, jsonResult.soLuotThamGiaHdTungThang);
        addDataBieuDoSoHoatDongToChuc(bieuDoSoHoatDongToChuc, jsonResult.soHoatDongToChucTungThang);
    }
    var updateBieuDo = function (e) {
        var link = $(e.target);
        var namHocLay = link.attr("data-namHoc");
        quanLyHoatDongLopService.layThongKeHoatDong(lopId, namHocLay, themDuLieuVaoBieuDo);
    }
    //Save Hoạt động
    var initChonAnhBia = function () {
        $("body").on("click", "#selectAnhBiaButton", function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $("#anhBiaHoatDong-input").attr("src", fileUrl);
            }
            finder.popup();
        });
    }
    var hienThiModalSaveHoatDong = function () {
        showLoader();
        lopToChuc = [lopId];
        quanLyHoatDongController.hienThiModalSaveHoatDong();
        $('#selectLopToChuc').val(lopToChuc);
        $('#selectLopToChuc').trigger('change');
        hideLoader();
    }
    var saveHoatDong = function (e) {
        lopToChuc = returnIntArray($('#selectLopToChuc').val());
        if (!lopToChuc.includes(lopId)) alert("Hoạt động này phải do lớp tổ chức.");
        else {
            quanLyHoatDongController.saveHoatDong(e);
        }
    }
    //Init trang
    var initTrang = function (idLop, khoaHoc) {
        lopId = idLop;
        taoNamHocThongKe(khoaHoc);
        dataTableSetting();
        initHdVaSoLuotTgTable();
        initBieuDoSoLuotThamGiaHoatDong();
        initBieuDoSoHoatDongToChuc();
        quanLyHoatDongLopService.layThongTinHoatDongLop(lopId, themHoatDongVaoDom);
        $("#hoatDongToChucBtn").on("click", initHdLopToChucTable);
        $("body").on("click", "#linkNamHoc-ThongKe .namHoc-link", updateBieuDo);
        $("body").on("click", "#linkNamHoc-ThamGia .namHoc-link", updateHoatDongVaSoLuotThamGia);
    }
    var initTrang_QuanLy = function (idLop, khoaHoc) {
        initTrang(idLop, khoaHoc);
        initChonAnhBia();
        $("#themHoatDong").on("click", hienThiModalSaveHoatDong);
        $("#themHoatDongBtn").on("click", saveHoatDong);
    }

    return {
        initTrang: initTrang,
        initTrang_QuanLy: initTrang_QuanLy
    }

}(QuanLyHoatDongLopService, QuanLyHoatDongController);


