var QuanLyChungDonViController = function (quanLyChungDonViService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var danhSachDonViTable;
    var dataAnhBia = new FormData();
    var rawImg, donViDto = {};
    var donViMuonXoaId;

    var taoLinkDonVi = function (tenDonVi, donViId) {
        return '<a href="/DonVi/ThongTin/' + donViId + '"class="link">' + tenDonVi + "</a>";
    }
    var returnDonViTrucThuoc = function (loaiDonVi) {
        if (loaiDonVi == 1) return "Đoàn Phân viện";
        if (loaiDonVi == 2) return "Hội Sinh viên Phân viện";
    }
    var initDanhSachDonVi = function () {
        danhSachDonViTable = $("#danhSachDonVi").DataTable({
            ajax: {
                url: "/api/DonVi/DanhSachDonVi",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhBia" },
                { data: "tenDonVi" },
                { data: "ngayThanhLap" },
                { data: "soThanhVien" },
                { data: "loaiDonVi" },
                { data: "" }
            ],
            rowId: "donViId",
            columnDefs: [
                {
                    targets: 0,
                    width: 150,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
                    }
                },
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return taoLinkDonVi(data, row.donViId);
                    }
                },
                {
                    targets: 2,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    targets: 4,
                    render: function (data) {

                        return returnDonViTrucThuoc(data);
                    }
                },
                {
                    targets: 5,
                    width: 150,
                    render: function () {
                        return '<button class="btn btn-primary thayDoiDonVi-js">Thay đổi thông tin</button>' +
                               '<button class="btn btn-success quanLyThanhVien-js">Quản lý Thành viên</button>' +
                               '<button class="btn btn-info quanLyChucVuDonVi-js">Quản lý Chức vụ</button>' +
                               '<button class="btn btn-danger xoaDonVi-js">Xóa</button>';
                    }
                }
            ]
        });
    }
    var chuyenSangTrangQuanLyThanhVien = function(e) {
        var button = $(e.target);
        var donViId = button.closest("tr").attr("id");
        window.location.href="/DonVi/QuanLyThanhVien/" + donViId;
    }
    var chuyenSangTrangQuanLyChucVu = function (e) {
        var button = $(e.target);
        var donViId = button.closest("tr").attr("id");
        window.location.href = "/DonVi/QuanLyChucVu/"+donViId;
        
    }
    //Up, chỉnh sửa ảnh bìa
    //Phần upload ảnh bìa đơn vị
    var exportCroppie = function ($uploadCrop, dataAnhBia,donViId) {
        var tenFile = "AnhBiaDonVi"+donViId;
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
            $("#anhBiaDonVi").attr("src", "");
            $("#anhBiaDonVi").attr("src", linkAnh);
            $("#cropImagePop").modal("hide");
        });
    }
    var initCroppie = function () {
        //Và làm ởn nhớ là khi sử dụng croppie thì nhớ Include file croppie.css
        $("#anhBiaDonVi_input").change(function () {
            if (this.files && this.files[0]) {
                var imageDir = new FileReader();
                imageDir.readAsDataURL(this.files[0]);
                imageDir.onload = function (e) {
                    $("#anhBiaDonVi-Wrapper").addClass("ready");
                    $("#cropImagePop").modal("show"); //Show modal
                    $("#editAnhBiaBtn").css("display", "block"); //Hiển thị nút chỉnh sửa
                    rawImg = e.target.result; //Bind link ảnh thô với file vừa up lên
                }
            }
        });
        //Tạo instance croppie
        //Lưu ý quan trọng, khi sử dụng croppie, node chứa instance croppie (ở đây là anhBiaDonVi-Wrapper) phải
        // được khai báo width và height cụ thể bằng px, không thể croppie sẽ không hiện ra.
        var $uploadCrop = $("#anhBiaDonVi-Wrapper").croppie({
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
            var donViId = $("#donViId").val(); //Phải lấy bằng cách này vì lúc này chưa bindDonViDto()
            exportCroppie($uploadCrop, dataAnhBia, donViId);
        });
    }
    var bindAnhBia = function (data) {
        donViDto.anhBia = data;
    }
    var uploadAnhBia = function () {
        if (dataAnhBia.has("image")) {
            quanLyChungDonViService.uploadAnhBiaDonVi(dataAnhBia, bindAnhBia, donViDto.id);
        }
    }

    //Chỉnh sửa, thêm đơn vị
    var validateDonViForm = function() {
        $("#donViForm").validate({
            ignore:[],
            rules: {
                tenDonVi: "required",
                ngayThanhLap: "required",
                loaiDonVi: "required",
                gioiThieu: {
                    required: function (textarea) {
                        CKEDITOR.instances[textarea.id].updateElement(); // update textarea
                        var editorcontent = textarea.value.replace(/<[^>]*>/gi, ''); // strip tags
                        return editorcontent.length === 0;
                    }
                }
            },
            messages: {
                tenDonVi: "Vui lòng nhập tên đơn vị",
                ngayThanhLap: "Vui lòng nhập ngày thành lập đơn vị",
                loaiDonVi: "Vui lòng chọn đơn vị trực thuộc",
                gioiThieu: "Vui lòng nhập giới thiệu đơn vị"
            }
        });
    }
    var initTextEditor = function () {
        CKEDITOR.replace("gioiThieu");
        CKFinder.setupCKEditor(null, "/ckfinder");
    } //Text editor cho giới thiệu đơn vị
    var updateInput = function (dataDonVi) {
        dataAnhBia.delete("image"); //Tránh trường hợp dataAnhBia còn lại trước đó
        $("#donViId").val(dataDonVi.id);
        setDataForInputWithLimit("tenDonVi", dataDonVi.tenDonVi);
        $("#loaiDonVi").val(dataDonVi.loaiDonVi);
        $("#ngayThanhLap").val(returnDateForInput(dataDonVi.ngayThanhLap));
        $("#anhBiaDonVi").attr("src", dataDonVi.anhBia);
        CKEDITOR.instances["gioiThieu"].setData(dataDonVi.gioiThieu); //set data cho text editor
    }
    var resetInput = function () {
        dataAnhBia.delete("image");
        $("#donViId").val(0);
        $("#tenDonVi").val("");
        $("#loaiDonVi").val(null);
        $("#ngayThanhLap").val(null);
        $("#anhBiaDonVi").attr("src", "/Content/AnhBiaDonVi/AnhBiaHocVien.png");
        CKEDITOR.instances["gioiThieu"].setData(""); //set data cho text editor
    }
    var hienThiThemDonViModal = function () {
        resetInput();
        $("#SaveDonViModal-title").html("Thêm đơn vị");
        $("#anhBiaDonVi-helpText").show();
        $("#anhBiaDonVi_input").hide();
        $("#anhBiaDonVi_input").prop("disabled", true);
        $("#SaveDonViModal").modal("show");
    }
    var hienThiSuaDonViModal = function (e) {
        var button = $(e.target);
        var donViId = button.closest("tr").attr("id");
        quanLyChungDonViService.getDonViData(donViId, updateInput);
        $("#anhBiaDonVi-helpText").hide();
        $("#anhBiaDonVi_input").show();
        $("#anhBiaDonVi_input").prop("disabled", false);
        $("#SaveDonViModal-title").html("Chỉnh sửa thông tin đơn vị");
        $("#SaveDonViModal").modal("show");
    }
    var bindDonViDto = function() {
        donViDto.id = $("#donViId").val();
        donViDto.tenDonVi= $("#tenDonVi").val();
        donViDto.anhBia= $("#anhBiaDonVi").attr("src");
        donViDto.loaiDonVi = $("#loaiDonVi").val();
        donViDto.ngayThanhLap = $("#ngayThanhLap").val();
        donViDto.gioiThieu = CKEDITOR.instances["gioiThieu"].getData();
        donViDto.anhBia = $("#anhBiaDonVi").attr("src");
    }
    var updateAfterSaveDonVi = function (suaThongTin, xhr) {
        hideLoader();
        //Nếu thành công
        if (xhr == null) {
            danhSachDonViTable.ajax.reload();
            if (suaThongTin) alert("Đã thêm đơn vị mới.");
            else alert("Đã chỉnh sửa thông tin đơn vị.");
        }
        //Nếu bị lỗi
        else alert(xhr.responseJSON.message); 
        $("#SaveDonViModal").modal("hide");
    }
    var saveDonVi = function () {
        if ($("#donViForm").valid()) {
            showLoader();
            bindDonViDto();
            uploadAnhBia();
            quanLyChungDonViService.saveDonVi(donViDto, updateAfterSaveDonVi);
        } else alert("Hãy nhập chính xác các thông tin cần thiết");
    }
    var hienThiXoaDonViModal = function(e) {
        var button = $(e.target);
        donViMuonXoaId = button.closest("tr").attr("id");
        var tenDonVi = button.closest("tr").find("a").html();
        $("#tenDonViMuonXoa").html(tenDonVi);
        $("#XoaDonViModal").modal("show");
    }
    var updateAfterXoaDonVi = function (xhr) {
        $(".loader").hide();
        //Nếu thành công
        if (xhr == null) {
            danhSachDonViTable.ajax.reload();
            alert("Đã xóa đơn vị.");
        }
        //Nếu bị lỗi
        else alert(xhr.responseJSON.message); 
        $("#XoaDonViModal").modal("hide");
    }
    var xoaDonVi = function() {
        $(".loader").show();
        quanLyChungDonViService.xoaDonVi(donViMuonXoaId, updateAfterXoaDonVi);
    }

    var initTrangQuanLyChungDonVi = function () {
        dataTableSetting();
        initDanhSachDonVi();
        initTextEditor();
        initCroppie();
        validateDonViForm();
        limitCharacterForInput("tenDonVi");
        $("#thenDonViBtn").on("click", hienThiThemDonViModal);
        $("body").on("click", ".thayDoiDonVi-js", hienThiSuaDonViModal);
        $("body").on("click", ".xoaDonVi-js", hienThiXoaDonViModal);
        $("body").on("click", ".quanLyThanhVien-js", chuyenSangTrangQuanLyThanhVien);
        $("body").on("click", ".quanLyChucVuDonVi-js", chuyenSangTrangQuanLyChucVu);
        $("#saveDonVi").on("click", saveDonVi);
        $("#xoaDonVi").on("click", xoaDonVi);
    }


    /*Phần trang thông tin đơn vị (để chung để khỏi tạo controller riêng*/
    //Danh sách biến
    var danhSachThanhVienTable, danhSachCuuThanhVienTable, danhSachChucVuTable;
    var daTaoDanhSachThanhVien = false, daTaoDanhSachCuuThanhVien = false,
        daTaoDanhSachChucVu = false, daTaoBaiVietDonVi = false,daTaoHoatDongDonVi = false;
    var donViId, daDangKi;

    var themBaiViet = function (data) {
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        $("#nav-baiVietDonVi .tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: data }));
    }
    var themHoatDong = function (data) {
        var danhSachHoatDong = _.template($("#hoatDongMoi_Template").html());
        $("#nav-hoatDongDonVi .tinMoiWrapper").append(danhSachHoatDong({ danhSachHoatDong: data }));
    }                             
    //Init các bảng
    var initDanhSachThanhVienTable = function () {
        if (daTaoDanhSachThanhVien) return;
        //Tạo bảng
        danhSachThanhVienTable = $("#danhSachThanhVienDonVi").DataTable({
            ajax: {
                url: "/api/DonVi/ThanhVien/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "ngayGiaNhap" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
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
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                }
            ]
        });
        //Đánh dấu đã tạo
        daTaoDanhSachThanhVien = true;
    }
    var initDanhSachChucVuTable = function () {
        if (daTaoDanhSachChucVu) return;
        //Tạo bảng
        danhSachChucVuTable = $("#danhSachChucVuDonVi").DataTable({
            ajax: {
                url: "/api/DonVi/ChucVu/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "tenChucVu" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                }
            ]
        });
        //Đánh dấu đã tạo
        daTaoDanhSachChucVu = true;
    }
    var initDanhSachCuuThanhVienTable = function () {
        if (daTaoDanhSachCuuThanhVien) return;
        //Tạo bảng
        danhSachCuuThanhVienTable = $("#danhSachCuuThanhVien").DataTable({
            ajax: {
                url: "/api/DonVi/CuuThanhVien/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "ngayGiaNhap" },
                { data: "ngayRoi" },
                { data: "ghiChu" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
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
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    targets: 4,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                }
            ]
        });
        //Đánh dấu đã tạo
        daTaoDanhSachCuuThanhVien = true;
    }
    var initDanhSachBaiVietDonVi = function () {
        if (daTaoBaiVietDonVi) return;
        quanLyChungDonViService.layBaiVietDonVi(donViId, themBaiViet);
        daTaoBaiVietDonVi = true;
    }
    var initDanhSachHoatDongDonVi = function () {
        if (daTaoHoatDongDonVi) return;
        var layHoatDongDto = {};
        layHoatDongDto.orderType = "DESC";
        layHoatDongDto.donViId = donViId;
        quanLyChungDonViService.layHoatDongDonVi(layHoatDongDto, themHoatDong);
        daTaoHoatDongDonVi = true;
    }
    //Đăng kí thành viên hoạt động
    var validateDangKiThanhVienForm = function() {
        $("#dangKiThanhVienForm").validate({
            ignore: [],
            rules: {
                donDangKiThanhVien: "required"
            },
            messages: {
                donDangKiThanhVien: "Vui lòng nhập giới thiệu, lý do bạn muốn tham gia đơn vị"
            }
        });
       
    }
    var updateDangKiThanhVienButton = function () {
        if (daDangKi) {  //Nếu đã đăng kí tham gia
            $("#DangKiThanhVienBtnText").html("Đã đăng kí");
            $("#DangKiThanhVienBtn").addClass("daDangKi");
        } else { //Nếu chưa đăng kí
            $("#DangKiThanhVienBtnText").html("Đăng kí thành viên");
            $("#DangKiThanhVienBtn").removeClass("daDangKi");
        }
        $("#DangKiThanhVienBtn").show();
    }
    var updateAfterDangKi_HuyDangKi = function (dangKi, xhr) {
        hideLoader();
        if (xhr != null) {
            alert(xhr.responseJSON.message);
        } else {
            if (dangKi) {
                daDangKi = true;
                alert("Đã đăng kí");
            } else {
                daDangKi = false;
                alert("Đã hủy đăng kí");
            }
            updateDangKiThanhVienButton();
            $("#thongBaoModal").modal("hide");
        }
    }
    var dangKiThanhVien = function () {
        if ($("#dangKiThanhVienForm").valid()) {
            showLoader();
            var dangKiThanhVienDto = {
                donViId: donViId,
                gioiThieu: $("#donDangKiThanhVien").val()
            }
            quanLyChungDonViService.dangKiThanhVien(dangKiThanhVienDto, updateAfterDangKi_HuyDangKi);
        }
    }
    var huyDangKiThanhVien = function () {
        showLoader();
        quanLyChungDonViService.huyDangKiThanhVien(donViId, updateAfterDangKi_HuyDangKi);
    }
    var dangKiThanhVienHandler = function () {
        if (daDangKi) {
            $("#huyDangKiThanhVienThongBao").show();
            $("#dangKiThanhVienThongBao").hide();
            $("#dangKiThanhVien").hide();
            $("#huyDangKiThanhVien").show();
        }
        else {
            $("#huyDangKiThanhVienThongBao").hide();
            $("#dangKiThanhVienThongBao").show();
            $("#dangKiThanhVien").show();
            $("#huyDangKiThanhVien").hide();
        }
        $("#thongBaoModal").modal("show");

    }
    //Thay đổi thông tin đơn vị
    var hienThiModalSuaDonVi_TrangThongTinDonVi = function () {
        quanLyChungDonViService.getDonViData(donViId, updateInput);
        $("#SaveDonViModal").modal("show");
    }
    var updateAfterSave_TrangThongTinDonVi = function (suaThongTin, xhr) {
        hideLoader();
        //Nếu thành công
        if (xhr == null) {
            alert("Đã chỉnh sửa thông tin đơn vị.");
            location.reload(true);
        }
        //Nếu bị lỗi
        else alert(xhr.responseJSON.message);
        $("#SaveDonViModal").modal("hide");
    }
    var saveDonVi_TrangThongTinDonVi = function() {
        if ($("#donViForm").valid()) {
        showLoader();
        bindDonViDto();
        uploadAnhBia();
        quanLyChungDonViService.saveDonVi(donViDto, updateAfterSave_TrangThongTinDonVi);
        } else alert("Hãy nhập chính xác các thông tin cần thiết");
    }
    //Func init Trang
    var initTrangThongTinDonVi = function (idDonVi,daDangKibool) {
        donViId = idDonVi;
        daDangKi = daDangKibool;
        dataTableSetting();
        initDanhSachThanhVienTable();
        updateDangKiThanhVienButton();
        validateDangKiThanhVienForm();
        $("#nav-chucVuDonVi-tab").on("click", initDanhSachChucVuTable);
        $("#nav-cuuThanhVien-tab").on("click", initDanhSachCuuThanhVienTable);
        $("#nav-baiVietDonVi-tab").on("click", initDanhSachBaiVietDonVi);
        $("#nav-hoatDongDonVi-tab").on("click", initDanhSachHoatDongDonVi);
        $("#DangKiThanhVienBtn").on("click", dangKiThanhVienHandler);
        $("#dangKiThanhVien").on("click", dangKiThanhVien);
        $("#huyDangKiThanhVien").on("click", huyDangKiThanhVien);
    }
    var initTrangThongTinDonVi_QuanLy = function (idDonVi,quanLyThongTin) {
        donViId = idDonVi;
        dataTableSetting();
        initDanhSachThanhVienTable();
        $("#nav-chucVuDonVi-tab").on("click", initDanhSachChucVuTable);
        $("#nav-cuuThanhVien-tab").on("click", initDanhSachCuuThanhVienTable);
        $("#nav-baiVietDonVi-tab").on("click", initDanhSachBaiVietDonVi);
        $("#nav-hoatDongDonVi-tab").on("click", initDanhSachHoatDongDonVi);
        //Nếu có quyền thay đổi thông tin thì mới init
        if (quanLyThongTin) {
            initTextEditor();
            initCroppie();
            validateDonViForm();
            limitCharacterForInput("tenDonVi");
            $("#thayDoiThongTin").on("click", hienThiModalSuaDonVi_TrangThongTinDonVi);
            $("#saveDonVi").on("click", saveDonVi_TrangThongTinDonVi);
        }
    }

    /*Trang danh sách đơn vị*/
    //Trang hiển thị đơn vị của sinh viên
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
    var initTrangDanhSachDonVi = function () {
        quanLyChungDonViService.layDanhSachDonVi(themDonViVaoDom);
    }

    return {
        initTrangQuanLyChungDonVi: initTrangQuanLyChungDonVi,
        initTrangThongTinDonVi: initTrangThongTinDonVi,
        initTrangThongTinDonVi_QuanLy: initTrangThongTinDonVi_QuanLy,
        initTrangDanhSachDonVi: initTrangDanhSachDonVi
    }
}(QuanLyChungDonViService);

var QuanLyThanhVienDonViController = function(quanLyThanhVienDonViService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var danhSachDangKiThanhVienTable, danhSachThanhVienTable, danhSachCuuThanhVienTable;
    var daTaoDanhSachThanhVien = false, daTaoDanhSachCuuThanhVien = false, daTaoDanhSachDangKiThanhVien = false;
    var donViId, dataThanhVien = {}, currentRow;

    var initDanhSachDangKiThanhVienTable = function () {
        if (daTaoDanhSachDangKiThanhVien) return;
        //Tạo bảng
        danhSachDangKiThanhVienTable = $("#danhSachDangKiThanhVien").DataTable({
            ajax: {
                url: "/api/DonVi/DanhSachDangKi/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "ngayGiaNhap" },
                { data: "ghiChu" },
                { data: "" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
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
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    targets: 5,
                    width: 100,
                    render: function () {
                        return "<button class='btn btn-success pheDuyetDangKi-js'>Phê duyệt</button>" +
                               "<button class='btn btn-danger xoaDangKi-js'>Xóa</button>";
                    }
                }
            ]
        });
        //Đánh dấu đã tạo
        daTaoDanhSachDangKiThanhVien = true;
    }
    var initDanhSachThanhVienTable = function () {
        if (daTaoDanhSachThanhVien) return;
        //Tạo bảng
        danhSachThanhVienTable = $("#danhSachThanhVienDonVi").DataTable({
            ajax: {
                url: "/api/DonVi/ThanhVien/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "ngayGiaNhap" },
                { data: "" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
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
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    targets: 4,
                    width: 100,
                    render: function () {
                        return "<button class='btn btn-primary thayDoiThanhVien-js'>Thay đổi</button>" +
                            "<button class='btn btn-success nghiHuuThanhVien-js'>Tốt nghiệp</button>" +
                            "<button class='btn btn-danger xoaThanhVien-js'>Xóa</button>";
                    }
                }
            ]
        });
        //Đánh dấu đã tạo
        daTaoDanhSachThanhVien = true;
    }
    var initDanhSachCuuThanhVienTable = function () {
        if (daTaoDanhSachCuuThanhVien) return;
        //Tạo bảng
        danhSachCuuThanhVienTable = $("#danhSachCuuThanhVien").DataTable({
            ajax: {
                url: "/api/DonVi/CuuThanhVien/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[1, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "ngayGiaNhap" },
                { data: "ngayRoi" },
                { data: "ghiChu" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
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
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                },
                {
                    targets: 4,
                    render: function (data) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                }
            ]
        });
        //Đánh dấu đã tạo
        daTaoDanhSachCuuThanhVien = true;
    }

    //Func quản lý
    var validateThemThanhVienForm = function() {
        $("#thanhVienDonViForm").validate({
            ignore: [],
            rules: {
                ngayGiaNhap: "required",
                ngayRoi: "required",
                quaTrinhCongTac: "required"
            },
            messages: {
                ngayGiaNhap: "Vui lòng nhập ngày gia nhập đơn vị của sinh viên",
                ngayRoi: "Vui lòng nhập ngày ngừng tham gia đơn vị của sinh viên",
                quaTrinhCongTac: "Vui lòng nhập quá trình công tác tại đơn vị"
            }
        });
    }
    //Thêm thành viên
    //Thêm sinh viên
    var updateAfterThemThanhVien = function (xhr) {
        hideLoader();
        if (xhr != null) {
            alert(xhr.responseJSON.message);
        } else {
            danhSachThanhVienTable.row.add(dataThanhVien).draw();
            alert("Đã thêm thành viên.");
        }
        $("#themThanhVienDonViInput").typeahead("val", "AS");
    }
    var themThanhVien = function (e, sinhVien) {
        showLoader();
        dataThanhVien = {};
        //Data để thêm row vào datatable
        dataThanhVien.anhDaiDien = sinhVien.anhDaiDien;
        dataThanhVien.id = sinhVien.id;
        dataThanhVien.hoVaTenLot = sinhVien.hoVaTenLot;
        dataThanhVien.ten = sinhVien.ten;
        dataThanhVien.anhDaiDien = sinhVien.anhDaiDien;
        dataThanhVien.mssv = sinhVien.mssv;
        //data để gửi tới service
        dataThanhVien.sinhVienId = sinhVien.id;
        dataThanhVien.donViId = donViId;
        dataThanhVien.ngayGiaNhap = new Date();
        quanLyThanhVienDonViService.themThanhVien(dataThanhVien, updateAfterThemThanhVien);
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
        $("#themThanhVienDonViInput").typeahead(
                {
                    highlight: true
                },
                {
                    name: "sinhVien",
                    display: function (data) {
                        return data.hoVaTenLot + " " + data.ten;
                    },
                    source: danhSachsinhVien,
                    templates: {
                        suggestion: function (data) {
                            return '<p><img src="' + data.anhDaiDien + '" class="anhSvTypeahead"/><strong>' + data.hoVaTenLot + ' ' + data.ten + '</strong> - '
                                + data.mssv + " - " + returnKyHieuTenLop(data.kyHieuTenLop) + "</p>";
                        }
                    }

                })
            .on("typeahead:autocomplete", themThanhVien)
            .on("typeahead:select", themThanhVien)
            .on("typeahead:change",
                function () {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
        $("#themThanhVienDonViInput").typeahead("val", "AS");
    }
    //Tốt nghiệp thành viên
    var hienThiModalTotNghiep = function(e) {
        currentRow = $(e.target).closest("tr");
        //Ẩn/hiện button
        $("#totNghiepThanhVien").show();
        $("#pheDuyetThanhVien").hide();
        $("#xoaThanhVien").hide();
        $("#thayDoiThanhVien").hide();
        //Ẩn hiện input
        $("#ngayGiaNhapContainer").show();
        $("#ngayRoiContainer").show();
        $("#quaTrinhContainer").show();
        $("#ngayGiaNhap").prop("disabled", false);
        $("#ngayRoi").prop("disabled", false);
        $("#quaTrinhCongTac").prop("disabled", false);
        $("#xoaThanhVienContainer").hide();
        $("#SaveThanhVienModal-title").html("Tốt nghiệp thành viên");
        $("#SaveThanhVienModal").modal("show");
        //Set input
        dataThanhVien = {};  //Reset
        dataThanhVien = danhSachThanhVienTable.row(currentRow).data();
        $("#tenThanhVien").html(returnTenSinhVienWithLink(dataThanhVien));
        $("#sinhVienId").val(dataThanhVien.id);
        $("#anhThanhVien").attr("src", dataThanhVien.anhDaiDien);
        $("#ngayGiaNhap").val(returnDateForInput(dataThanhVien.ngayGiaNhap));
    }
    var updateAfterTotNghiep = function (xhr) {
        hideLoader();
        if (xhr != null) {
            alert(xhr.responseJSON.message);
        } else {
            danhSachThanhVienTable.row(currentRow).remove().draw();
            if (daTaoDanhSachCuuThanhVien) danhSachCuuThanhVienTable.row.add(dataThanhVien).draw();
            alert("Đã tốt nghiệp thành viên");
        }
        $("#SaveThanhVienModal").modal("hide");
    }
    var totNghiepThanhVien = function () {
        if ($("#thanhVienDonViForm").valid()) {
            showLoader();
            dataThanhVien.ghiChu = $("#quaTrinhCongTac").val();
            dataThanhVien.ngayGiaNhap = $("#ngayGiaNhap").val();
            dataThanhVien.ngayRoi = $("#ngayRoi").val();
            dataThanhVien.donViId = donViId;
            dataThanhVien.sinhVienId = dataThanhVien.id;
            quanLyThanhVienDonViService.totNghiepThanhVien(dataThanhVien, updateAfterTotNghiep);
        } else alert("Hãy nhập chính xác các thông tin cần thiết");
    }
    //Xóa thành viên
    var hienThiModalXoaThanhVien = function(e) {
        currentRow = $(e.target).closest("tr");
        //Ẩn/hiện button
        $("#totNghiepThanhVien").hide();
        $("#pheDuyetThanhVien").hide();
        $("#xoaThanhVien").show();
        $("#thayDoiThanhVien").hide();
        //Ẩn hiện input
        $("#ngayGiaNhapContainer").hide();
        $("#ngayRoiContainer").hide();
        $("#quaTrinhContainer").hide();
        $("#ngayGiaNhap").prop("disabled",true);
        $("#ngayRoi").prop("disabled", true);
        $("#quaTrinhCongTac").prop("disabled", true);
        $("#xoaThanhVienContainer").show();
        $("#SaveThanhVienModal-title").html("Xóa thành viên");
        $("#SaveThanhVienModal").modal("show");
        //Update field
        dataThanhVien = {};  //Reset
        dataThanhVien = danhSachThanhVienTable.row(currentRow).data();
        $("#tenThanhVien").html(returnTenSinhVienWithLink(dataThanhVien));
        $("#sinhVienId").val(dataThanhVien.id);
        $("#anhThanhVien").attr("src", dataThanhVien.anhDaiDien);
    }
    var updateAfterXoa = function (xhr) {
        hideLoader();
        if (xhr != null) {
            alert(xhr.responseJSON.message);
        } else {
            danhSachThanhVienTable.row(currentRow).remove().draw();
            alert("Đã xóa thành viên.");
        }
        $("#SaveThanhVienModal").modal("hide");
    }
    var xoaThanhVien = function () {
        showLoader();
        dataThanhVien.donViId = donViId;
        dataThanhVien.sinhVienId = dataThanhVien.id;
        quanLyThanhVienDonViService.xoaThanhVien(dataThanhVien, updateAfterXoa);
    } 
    //Thay đổi thành viên
    var hienThiModalSuaThanhVien = function(e) {
        currentRow = $(e.target).closest("tr");
        //Hiển thị input
        $("#ngayGiaNhapContainer").show();
        $("#ngayRoiContainer").hide();
        $("#quaTrinhContainer").hide();
        $("#ngayGiaNhap").prop("disabled", false);
        $("#ngayRoi").prop("disabled", true);
        $("#quaTrinhCongTac").prop("disabled", true);
        $("#xoaThanhVienContainer").hide();
        //Ẩn/hiện button
        $("#totNghiepThanhVien").hide();
        $("#xoaThanhVien").hide();
        $("#thayDoiThanhVien").show();
        $("#pheDuyetThanhVien").hide();
        //Update field
        dataThanhVien = {};  //Reset
        dataThanhVien = danhSachThanhVienTable.row(currentRow).data();
        $("#tenThanhVien").html(returnTenSinhVienWithLink(dataThanhVien));
        $("#sinhVienId").val(dataThanhVien.id);
        $("#anhThanhVien").attr("src", dataThanhVien.anhDaiDien);
        $("#ngayGiaNhap").val(returnDateForInput(dataThanhVien.ngayGiaNhap));
        $("#SaveThanhVienModal-title").html("Sửa thông tin thành viên");
        $("#SaveThanhVienModal").modal("show");
    }
    var updateAfterThayDoi = function (xhr) {
        hideLoader();
        if (xhr != null) alert(xhr.responseJSON.message);
        else {
            danhSachThanhVienTable.row(currentRow).data(dataThanhVien).draw();
            alert("Đã thay đổi thông tin thành viên.");
        } 
        $("#SaveThanhVienModal").modal("hide");
    }
    var thayDoiThanhVien = function () {
        if ($("#thanhVienDonViForm").valid()) {
            showLoader();
            dataThanhVien.ngayGiaNhap = $("#ngayGiaNhap").val();
            dataThanhVien.donViId = donViId;
            dataThanhVien.sinhVienId = dataThanhVien.id;
            quanLyThanhVienDonViService.thayDoiThanhVien(dataThanhVien, updateAfterThayDoi);
        } else alert("Hãy nhập chính xác các thông tin cần thiết");
    }
    //Phê duyệt thành viên
    var updateAfterPheDuyetDangKi = function(xhr) {
        hideLoader();
        if (xhr != null) alert(xhr.responseJSON.message);
        else {
            danhSachDangKiThanhVienTable.row(currentRow).remove().draw();
            danhSachThanhVienTable.row.add(dataThanhVien).draw();
            alert("Đã phê duyệt đăng kí.");
        }
    }
    var pheDuyetDangKi = function (e) {
        showLoader();
        currentRow = $(e.target).closest("tr");
        dataThanhVien = {};  //Reset
        dataThanhVien = danhSachDangKiThanhVienTable.row(currentRow).data();
        dataThanhVien.donViId = donViId;
        dataThanhVien.sinhVienId = dataThanhVien.id;
        dataThanhVien.ngayGiaNhap = new Date(); //Cái này để tí nữa draw lại bảng danhSachThanhVienTable
        quanLyThanhVienDonViService.pheDuyetDangKi(dataThanhVien, updateAfterPheDuyetDangKi);
    }
    var updateAfterXoaDangKi = function(xhr) {
        hideLoader();
        if (xhr != null) alert(xhr.responseJSON.message);
        else {
            danhSachDangKiThanhVienTable.row(currentRow).remove().draw();
            alert("Đã xóa đăng kí.");
        }
    }
    var xoaDangKi = function (e) {
        showLoader();
        currentRow = $(e.target).closest("tr");
        dataThanhVien = {};  //Reset
        dataThanhVien = danhSachDangKiThanhVienTable.row(currentRow).data();
        dataThanhVien.donViId = donViId;
        dataThanhVien.sinhVienId = dataThanhVien.id;
        quanLyThanhVienDonViService.xoaThanhVien(dataThanhVien, updateAfterXoaDangKi);
    }

    //Init trang
    var initTrangQuanLyThanhVien = function (idDonVi) {
        donViId = idDonVi;
        dataTableSetting();
        initDanhSachDangKiThanhVienTable();
        initDanhSachThanhVienTable();
        initTypeahead();
        validateThemThanhVienForm();
        $("#danhSachCuuThanhVienBtn").on("click", initDanhSachCuuThanhVienTable);
        $("#totNghiepThanhVien").on("click", totNghiepThanhVien);
        $("#thayDoiThanhVien").on("click", thayDoiThanhVien);
        $("#xoaThanhVien").on("click", xoaThanhVien);
        $("body").on("click", ".pheDuyetDangKi-js", pheDuyetDangKi);
        $("body").on("click", ".xoaDangKi-js", xoaDangKi);
        $("body").on("click", ".nghiHuuThanhVien-js", hienThiModalTotNghiep);
        $("body").on("click", ".xoaThanhVien-js", hienThiModalXoaThanhVien);
        $("body").on("click", ".thayDoiThanhVien-js", hienThiModalSuaThanhVien);

    }

return {
    initTrangQuanLyThanhVien: initTrangQuanLyThanhVien
}
}(QuanLyThanhVienDonViService);

var QuanLyChucVuDonViController = function (quanLyChucVuDonViService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var donViId, chucVuDonViDto = {}, danhSachThanhVien;
    var danhSachChucVuTable, currentRow;
    
    var chonChucVuTruongDonVi = function () {
        if ($("#chucVuId").val() != 10) {
            $("#quanLyThongTin").prop("disabled", false);
            $("#quanLyThanhVien").prop("disabled", false);
            $("#quanLyChucVu").prop("disabled", false);
            $("#quanLyHoatDong").prop("disabled", false);
            return;
        }
        $("#quanLyThongTin").prop("checked", true);
        $("#quanLyThanhVien").prop("checked", true);
        $("#quanLyChucVu").prop("checked", true);
        $("#quanLyHoatDong").prop("checked", true);
        $("#quanLyThongTin").prop("disabled", true);
        $("#quanLyThanhVien").prop("disabled", true);
        $("#quanLyChucVu").prop("disabled", true);
        $("#quanLyHoatDong").prop("disabled", true);
    }

    var updateThanhVien = function(thanhVien) {
        $("#tenThanhVien").html(returnTenSinhVienWithLink(thanhVien));
        $("#sinhVienId").val(thanhVien.id);
        $("#anhThanhVien").attr("src", thanhVien.anhDaiDien);
    }
    var updateChucVuInput = function (chucVuDonViDto) {
        setDataForInputWithLimit("tenChucVu", chucVuDonViDto.tenChucVu);
        $("#chucVuId").val(chucVuDonViDto.chucVuId);
        //Checkbox quyền hạn
        if (chucVuDonViDto.chucVuId == 10) {
            chonChucVuTruongDonVi();

        } else {
            $("#quanLyThongTin").prop("checked", chucVuDonViDto.quanLyThongTin);
            $("#quanLyThanhVien").prop("checked", chucVuDonViDto.quanLyThanhVien);
            $("#quanLyChucVu").prop("checked", chucVuDonViDto.quanLyChucVu);
            $("#quanLyHoatDong").prop("checked", chucVuDonViDto.quanLyHoatDong);
        }

    }
    var enableInput = function() {
        $("input").prop("disabled", false);
        $("#chucVuId").prop("disabled", false);
    }
    var disableInput = function() {
        $("input").prop("disabled", true);
        $("#chucVuId").prop("disabled", true);

    }
    var resetInput = function () {
        chucVuDonViDto = {};  //Reset
        $("#tenThanhVien").html(null);
        $("#themChucVuDonViInput").typeahead("val", "AS");
        $("#sinhVienId").val(null);
        $("#anhThanhVien").attr("src", "/Content/AnhBia/AnhSV/avatar.png");
        $("#chucVuId").val(null);
        $("#tenChucVu").val(null);
        $("#quanLyThongTin").prop("checked",false);
        $("#quanLyThanhVien").prop("checked", false);
        $("#quanLyChucVu").prop("checked", false);
        $("#quanLyHoatDong").prop("checked", false);
        enableInput();
        $("#SaveChucVuModal-title").html("Thêm chức vụ");
    }
    var validateChucVuDonViForm = function () {
        $("#chucVuDonViForm").validate({
            ignore: [],
            rules: {
                sinhVienId: "required",
                chucVuId: "required",
                tenChucVu: "required"
            },
            messages: {
                sinhVienId: "Vui lòng chọn sinh viên giữ chức vụ",
                chucVuId: "Vui lòng chọn loại chức vụ",
                tenChucVu: "Vui lòng nhập tên chức vụ"
            }
        });

    }
    //Thêm chức vụ
    var hienThiModalThemChucVu = function() {
        resetInput();
        $("#themChucVuDonViInputContainer").show();
        $("#xoaChucVuText").hide();
        $("#xoaChucVu").hide();
        $("#saveChucVu").show();
        $("#SaveChucVuModal-title").html("Thêm chức vụ");
        $("#SaveChucVuModal").modal("show");
        //Data này để check xem là thêm hay sửa chức vụ (tí nữa gửi tới service)
        chucVuDonViDto.themChucVu = true;
    }
    var themThanhVien = function (e, thanhVien) {
        updateThanhVien(thanhVien);
        //5 data này dùng để tí nữa update danhSachChucVuTable
        chucVuDonViDto.id = thanhVien.id;
        chucVuDonViDto.anhDaiDien = thanhVien.anhDaiDien;
        chucVuDonViDto.hoVaTenLot = thanhVien.hoVaTenLot;
        chucVuDonViDto.ten = thanhVien.ten;
        chucVuDonViDto.mssv = thanhVien.mssv;
    }
    var initTypeahead = function () {
        var danhSachsinhVien = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace("hoVaTenLot", "ten", "mssv"),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: danhSachThanhVien
        });
        $("#themChucVuDonViInput").typeahead(
                {
                    highlight: true
                },
                {
                    name: "sinhVien",
                    display: function (data) {
                        return data.hoVaTenLot + " " + data.ten;
                    },
                    source: danhSachsinhVien,
                    templates: {
                        suggestion: function (data) {
                            return '<p><img src="' + data.anhDaiDien + '" class="anhSvTypeahead"/><strong>'
                                + returnTenSinhVien(data) + "</strong> - "
                                + data.mssv + "</p>";
                        }
                    }

                })
            .on("typeahead:autocomplete", themThanhVien)
            .on("typeahead:select", themThanhVien)
            .on("typeahead:change",
                function () {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
        $("#themChucVuDonViInput").typeahead("val", "AS");
    }
    var bindDanhSachThanhVienVaInitTypeahead = function (data) {
        danhSachThanhVien = data;
        initTypeahead();
    }
    //Sửa chức vụ
    var hienThiModalSuaChucVu = function(e) {
        currentRow = $(e.target).closest("tr");
        //Update field
        chucVuDonViDto = {};  //Reset
        chucVuDonViDto = danhSachChucVuTable.row(currentRow).data();
        enableInput();//Phải dặt trước update input
        updateThanhVien(chucVuDonViDto);
        updateChucVuInput(chucVuDonViDto);        
        $("#themChucVuDonViInputContainer").hide();
        $("#xoaChucVuText").hide();
        $("#xoaChucVu").hide();
        $("#saveChucVu").show();
        $("#SaveChucVuModal-title").html("Chỉnh sửa chức vụ");
        $("#SaveChucVuModal").modal("show");
        //Data này để check xem là thêm hay sửa chức vụ
        chucVuDonViDto.themChucVu = false;
    }
    //Func chung cho save chức vụ
    var bindChucVuDto = function () {
        //Chỉ những data này mới được gửi đến server
        chucVuDonViDto.sinhVienId = $("#sinhVienId").val();
        chucVuDonViDto.donViId = donViId;
        chucVuDonViDto.quanLyThongTin = $("#quanLyThongTin").prop("checked");
        chucVuDonViDto.quanLyThanhVien = $("#quanLyThanhVien").prop("checked");
        chucVuDonViDto.quanLyChucVu = $("#quanLyChucVu").prop("checked");
        chucVuDonViDto.quanLyHoatDong = $("#quanLyHoatDong").prop("checked");
        chucVuDonViDto.tenChucVu = $("#tenChucVu").val();
        chucVuDonViDto.chucVuId = $("#chucVuId").val();
    }
    var updateAfterSaveChucVu = function(themChucVu,xhr) {
        hideLoader();
        if (xhr != null) alert(xhr.responseJSON.message);
        else {
            if (themChucVu) {
                danhSachChucVuTable.row.add(chucVuDonViDto).draw();
                alert("Đã thêm chức vụ.");
            } else {
                danhSachChucVuTable.row(currentRow).data(chucVuDonViDto).draw();
                alert("Đã chỉnh sửa chức vụ.");
            }

        }
        $("#SaveChucVuModal").modal("hide");
    }
    var saveChucVu = function () {
        if ($("#chucVuDonViForm").valid()) {
            showLoader();
            bindChucVuDto();
            quanLyChucVuDonViService.saveChucVu(chucVuDonViDto, updateAfterSaveChucVu);
        } else alert("Hãy nhập chính xác các thông tin cần thiết");
    }
    //Xóa chức vụ
    var hienThiModalXoaChucVu = function (e) {
        currentRow = $(e.target).closest("tr");
        //Update field
        chucVuDonViDto = {};  //Reset
        chucVuDonViDto = danhSachChucVuTable.row(currentRow).data();
        disableInput();
        updateThanhVien(chucVuDonViDto);
        updateChucVuInput(chucVuDonViDto);
        $("#themChucVuDonViInputContainer").hide();
        $("#xoaChucVuText").show();
        $("#xoaChucVu").show();
        $("#saveChucVu").hide();
        $("#SaveChucVuModal-title").html("Xóa chức vụ");
        $("#SaveChucVuModal").modal("show");
    }
    var updateAfterXoaChucVu = function(xhr) {
        hideLoader();
        if (xhr != null) alert(xhr.responseJSON.message);
        else {
            alert("Đã xóa chức vụ");
            location.reload(true); //Reload lại để đảm bảo trường hợp xóa chức danh của chính họ
        } 
        $("#SaveChucVuModal").modal("hide");
    }
    var xoaChucVu = function() {
        showLoader();
        bindChucVuDto();
        quanLyChucVuDonViService.xoaChucVu(chucVuDonViDto, updateAfterXoaChucVu);
    }
    //Hiển thị DOM
    var returnLoaiChucVu = function (chucVuId) {
        if (chucVuId == 10) return "Trưởng đơn vị";
        if (chucVuId == 11) return "Phó đơn vị";
        if (chucVuId == 12) return "Cán bộ đơn vị";
    }
    var returnCacQuyen = function(chucVu) {
        var quyenHan = "";
        if (chucVu.quanLyThongTin) quyenHan += "<li>Quản lý thông tin</li>";
        if (chucVu.quanLyThanhVien) quyenHan += "<li>Quản lý thành viên</li>";
        if (chucVu.quanLyChucVu) quyenHan += "<li>Quản lý chức vụ</li>";
        if (chucVu.quanLyHoatDong) quyenHan += "<li>Quản lý hoạt động</li>";
        return "<ul>" + quyenHan + "</ul>";
    }
    var initDanhSachChucVuTable = function() {
        //Tạo bảng
        danhSachChucVuTable = $("#danhSachChucVuDonVi").DataTable({
            ajax: {
                url: "/api/DonVi/ChucVuQuanLy/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            deferRender: true,
            autoWidth: false,
            order: [[4, "desc"]],
            columns: [
                { data: "anhDaiDien" },
                { data: "mssv" },
                { data: "" },
                { data: "tenChucVu" },
                { data: "chucVuId" },
                { data: "" },
                { data: "" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    orderable: false,
                    render: function (data) {
                        return '<img src="' + data + '" class="img-fluid"/>';
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row) {
                        return returnTenSinhVienWithLink(row);
                    }
                },
                {
                    targets: 4,
                    render: function (data) {
                        return returnLoaiChucVu(data);
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return returnCacQuyen(row);
                    }
                },
                {
                    targets: 6,
                    width: 100,
                    render: function () {
                        return "<button class='btn btn-primary js-chinhSuaChucVu'>Chỉnh sửa</button>" +
                               "<button class='btn btn-danger js-xoaChucVu'>Xóa</button>";
                    }
                }
            ]
        });
    }
    //Init trang
    var initTrangQuanLyChucVu = function(idDonVi) {
        donViId = idDonVi;
        dataTableSetting();
        initDanhSachChucVuTable();
        quanLyChucVuDonViService.layDanhSachThanhVien(donViId, bindDanhSachThanhVienVaInitTypeahead);
        validateChucVuDonViForm();
        limitCharacterForInput("tenChucVu");
        $("#js-themChucVu").on("click", hienThiModalThemChucVu);
        $("#saveChucVu").on("click", saveChucVu);
        $("#xoaChucVu").on("click", xoaChucVu);
        $("#chucVuId").on("change", chonChucVuTruongDonVi);
        $("body").on("click", ".js-chinhSuaChucVu", hienThiModalSuaChucVu);
        $("body").on("click", ".js-xoaChucVu", hienThiModalXoaChucVu);
    }
return{
    initTrangQuanLyChucVu: initTrangQuanLyChucVu
}
}(QuanLyChucVuDonViService);

var QuanLyHoatDongChoPheDuyetDonViController = function (quanLyHoatDongDonViService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var hdChoPheDuyetTable;
    var donViId, hoatDongId;
    var initHdChoPheDuyetTable = function () {
        hdChoPheDuyetTable = $("#hdChoPheDuyetTable-TrangPheDuyet").DataTable({
            ajax: {
                url: "/api/DonVi/HoatDongChoPheDuyet/" + donViId,
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
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
                        console.log(row);
                        if (row.danhSachDonViToChuc.length + row.danhSachLopToChuc.length > 1)
                            return '<button class="btn btn-danger huy-js">Hủy hoạt động</button>' +
                                   '<button class="btn btn-warning huyToChuc-js">Hủy tham gia tổ chức</button>';
                        return '<button class="btn btn-danger huy-js">Hủy hoạt động</button>';

                    }
                }
            ]
        });
    }
    var hienThiModalHuyThamGiaHoatDong = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        hoatDongId = button.closest("tr").attr("id");
        $("#thongBaoModal-body").html("Bạn có chắn chắn muốn hủy tham gia tổ chức hoạt động này? Đơn vị bạn sẽ bị xóa tên khỏi danh sách các đơn vị tổ chức.");
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
            donViId: donViId,
            hoatDongId: parseInt(hoatDongId)
        }
        quanLyHoatDongDonViService.huyToChucHoatDong(huyToChucDto, reloadPage);
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
        quanLyHoatDongDonViService.huyHoatDong(hoatDongId, updatePageAfterHuyHoatDong);
    }

    var initTrang = function (idDonVi) {
        donViId = idDonVi;
        dataTableSetting();
        initHdChoPheDuyetTable();
        $("body").on("click", ".huy-js", hienThiModalHuyHoatDong);
        $("body").on("click", ".huyToChuc-js", hienThiModalHuyThamGiaHoatDong);
        $("#huyHoatDong").on("click", huyHoatDong);
        $("#huyToChucHoatDong").on("click", huyToChucHoatDong);
    }

    return {
        initTrang: initTrang
    }
}(QuanLyHoatDongDonViService);

var QuanLyHoatDongDonViController = function (quanLyHoatDongDonViService, quanLyHoatDongController) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var donViId, popoverContent;
    var bieuDoSoHoatDongToChuc, hoatDongDonViToChucTable;
    var daTaoBangHoatDong = false;
    var donViToChuc = [];//biến danh sách đơn vị tổ chức cho thêm hoạt động

    //Function để tạo bảng hoạt động đơn vị tổ chức
    var initHdDonViToChucTable = function () {
        if (daTaoBangHoatDong) return;
        hoatDongDonViToChucTable = $("#hoatDongDonViToChuc").DataTable({
            ajax: {
                url: "/api/DonVi/HoatDongToChuc/" + donViId,
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
        $("#soHdDangDienRa-DonVi").html(danhSachHoatDong.length);
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
    function initBieuDoSoHoatDongToChuc() {
        var ctx = $("#bieuDoSoHoatDongToChuc");
        bieuDoSoHoatDongToChuc = new Chart(ctx, {
            type: "bar",
            responsive: true,
            data: {
                labels: [],
                datasets: [
                    {
                        label: "Của đơn vị tổ chức",
                        data: [],
                        datalabels: {
                            anchor: "center",
                            align: "center"
                        },
                        backgroundColor: "rgba(255, 99, 132, 0.2)",
                        borderColor: 'rgba(255,99,132,1)',
                        borderWidth: 1
                    },
                    {
                        label: "Của cả phân viện",
                        data: [],
                        datalabels: {
                            anchor: "center",
                            align: "center"
                        },
                        backgroundColor: "rgba(54, 162, 235, 0.2)",
                        borderColor: "rgba(54, 162, 235, 1)",
                        borderWidth: 1
                    }
                ]
            },
            options: {
                plugins: {
                    datalabels: {
                        color: "black",
                        display: true,
                        font: {
                            weight: "bold",
                            size: 20
                        },
                        formatter: Math.round
                    }
                },
                title: {
                    display: true,
                    fontSize: 25,
                    text: "Số hoạt động được tổ chức"
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
    var taoNamHocThongKe = function (thoiGianThanhLap) {
        var thanhLap = new Date(thoiGianThanhLap);
        var namBatDau = thanhLap.getFullYear();
        var namKetThuc;
        var hienTai = new Date();
        if (hienTai.getMonth() + 1 < 8) //Nghĩa là đang là tháng 1-7 của năm học này, sẽ phải lấy từ tháng 8 năm trước
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
    var themDuLieuVaoBieuDo = function (soHoatDongToChucTungThang) {
        addDataBieuDoSoHoatDongToChuc(bieuDoSoHoatDongToChuc, soHoatDongToChucTungThang);
    }
    var updateBieuDo = function () {
       var namHocLay = $("#namHocPicker").val();
       quanLyHoatDongDonViService.layThongKeHoatDong(donViId, namHocLay, themDuLieuVaoBieuDo);
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
        donViToChuc = [donViId];
        quanLyHoatDongController.hienThiModalSaveHoatDong();
        $("#selectDonViToChuc").val(donViToChuc);
        $("#selectDonViToChuc").trigger("change");
        hideLoader();
    }
    var saveHoatDong = function (e) {
        //Check xem có danh sách đơn vị tổ chức có bao gồm đơn vị này không không
        donViToChuc = returnIntArray($('#selectDonViToChuc').val());
        if (!donViToChuc.includes(donViId)) alert("Hoạt động này phải do đơn vị tổ chức.");
        else {
            quanLyHoatDongController.saveHoatDong(e);
        }
    }

    //Init trang
    var initTrang = function (idDonVi,thoiGianThanhLap) {
        donViId = idDonVi;
        dataTableSetting();  
        initBieuDoSoHoatDongToChuc();
        taoNamHocThongKe(thoiGianThanhLap);
        quanLyHoatDongDonViService.layThongTinHoatDong(donViId, themHoatDongVaoDom);
        $("#hoatDongToChucBtn").on("click", initHdDonViToChucTable);
        $("#namHocPicker").change(updateBieuDo);
    }
    var initTrang_QuanLy = function (idDonVi, thoiGianThanhLap) {
        initTrang(idDonVi, thoiGianThanhLap);
        initChonAnhBia();
        $("#themHoatDong").on("click", hienThiModalSaveHoatDong);
        $("#themHoatDongBtn").on("click", saveHoatDong);
    }
return {
    initTrang: initTrang,
    initTrang_QuanLy : initTrang_QuanLy
}
}(QuanLyHoatDongDonViService, QuanLyHoatDongController);

