var validateSinhVienForm = function () {
    $.validator.addMethod("mssv_validation", function (value, element, options) {
        var mssvReg = /^A(S|H)[0-9]{6}$/;;
        return mssvReg.test(value);
    }, $.validator.format("Vui lòng nhập đúng định dạng MSSV: AS(AH) + 6 chữ số từ 0-9"));

    $("#saveSinhVienForm").validate({
        ignore: "input[type=hidden]",
        rules: {
            "hoVaTenLotSinhVien-input": "required",
            "tenSinhVien-input": "required",
            "mssv-input": {
                required: true,
                minlength: 8,
                mssv_validation : true
            },
            "ngaySinh-input": "required",
            "gioiTinh-input": "required",
            "khoaHoc-input": "required",
            "danToc-input": "required",
            "tonGiao-input": "required"
        },
        messages: {
            "hoVaTenLotSinhVien-input": "Vui lòng nhập họ và tên lót",
            "tenSinhVien-input": "Vui lòng nhập họ tên",
            "mssv-input": {
                required: "Vui lòng nhập mã số sinh viên",
                minlength: "Mã số sinh viên vui lòng có ít nhất 8 kí tự"
            },
            "ngaySinh-input": "Vui lòng nhập ngày sinh",
            "gioiTinh-input": "Vui lòng chọn giới tính",
            "khoaHoc-input": "Vui lòng chọn khóa học",
            "danToc-input": "Vui lòng chọn dân tộc",
            "tonGiao-input": "Vui lòng chọn tôn giáo"
        }
    });

}
var limitCharacter = function() {
    limitCharacterForInput("hoVaTenLotSinhVien-input");
    limitCharacterForInput("tenSinhVien-input");
}
var QuanLySinhVienController = function (quanLySinhVienService) {
    //Biến chứa data ảnh bìa để save lên server
    var dataAnhDaiDien = new FormData();
    var sinhVienDto = {};
    var danhSachSinhVienTable;
    var danhSachKhoa, danhSachDanToc, danhSachGioiTinh, danhSachTonGiao;
    var rawImg;
    var csrfToken = $("input[name='__RequestVerificationToken']").val();


    var initDanhSachSinhVienTable = function () {
        danhSachSinhVienTable = $("#danhSachSinhVien").DataTable({
            ajax: {
                url: "/api/SinhVien/DanhSachTatCaSinhVien",
                type:"POST",
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
            pageLength: 50,
            lengthMenu: [25, 50, 75, 100],
            "order": [[0, "asc"]],
            columns: [
                { data: "mssv" },
                { data: "hoVaTenLot" },
                { data: "ten" },
                { data: "ngaySinh" },
                { data: "tenGioiTinh" },
                { data: "tenKhoa" },
                { data: "kyHieuTenLop" },
                { data: "" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    name: "MSSV"
                },
                {
                    targets: 1,
                    orderable: false
                },
                {
                    targets: 2,
                    name: "Ten"
                },
                
                {
                    targets: 3,
                    render: $.fn.dataTable.render.moment("DD/MM/YYYY", "DD/MM/YYYY", "vi"),
                    name: "NgaySinh"
                },
                {
                    targets: 4,
                    name: "GioiTinh"
                },
                {
                    targets: 5,
                    name: "Khoa"
                },
                {
                    targets: 6,
                    render: function (data) {
                        return returnKyHieuTenLop(data);
                    },
                    name: "KyHieuTenLop"
                },
                {
                    targets: 7,
                    render: function () {
                        return '<button class="btn btn-primary chinhSuaSinhVien-js">Chỉnh sửa</button>';
                    },
                    orderable: false
                }
            ]
        });
    }

    //Phần upload ảnh bìa sinh viên
    var exportCroppie = function ($uploadCrop, dataAnhBia, mssv) {
        var tenFile = "" + mssv;
        //Tạo File ảnh thật sự để save lên server (lưu vào formData dataAnhBia trước khi gửi ajax)
        $uploadCrop.croppie('result',
            {
                type: "blob",
                size: { width: 300, height: 400 }
            }).then(function (blob) {
                dataAnhBia.delete("image");
                dataAnhBia.append("image", blob, tenFile);
            });

        //Tạo File ảnh để hiển thị cho người dùng
        $uploadCrop.croppie('result', {
            type: 'base64',
            format: 'jpeg',
            size: { width: 450, height: 600 }
        }).then(function (linkAnh) {
            $('#anhBiaSv').attr('src', "");
            $('#anhBiaSv').attr('src', linkAnh);
            $('#cropImagePop').modal('hide');
        });
    }
    var initCroppie = function () {
        $("#anhBiaSv_brower").change(function () {
            if (this.files && this.files[0]) {
                var imageDir = new FileReader();
                imageDir.readAsDataURL(this.files[0]);
                imageDir.onload = function (e) {
                    $('#anhBiaSv-Wrapper').addClass('ready');
                    $('#cropImagePop').modal('show'); //Show modal
                    $('#editAnhBiaSvBtn').css("display", "block"); //Hiển thị nút chỉnh sửa
                    rawImg = e.target.result; //Bind link ảnh thô với file vừa up lên
                }
            }
        });
        //Tạo instance croppie
        //Lưu ý quan trọng, khi sử dụng croppie, node chứa instance croppie (ở đây là anhBiaSv-Wrapper) phải
        // được khai báo width và height cụ thể bằng px, không thể croppie sẽ không hiện ra.
        var $uploadCrop = $('#anhBiaSv-Wrapper').croppie({
            viewport: {
                width: 300,
                height: 400,
                type: 'square'
            },
            enableExif: true
        });
        //Bind rawImg (file hình ảnh thô) vào croppie khi modal thêm ảnh hiện
        $('#cropImagePop').on('shown.bs.modal', function () {
            $uploadCrop.croppie('bind', { url: rawImg });
        });
        //Tạo ảnh khi nhấn nút crop
        $('#cropImageBtn').on('click', function (ev) {
            //Lấy mssv trước chứ không thể lấy từ sinhVienDto.mssv vì nó chưa bind Data
            var mssv = $('#mssv-input').val();
            exportCroppie($uploadCrop, dataAnhDaiDien, mssv);
        });
    }
    var bindAnhDaiDien = function(data) {
        sinhVienDto.anhDaiDien = data;
    }
    var uploadAnhDaiDien = function () {
        if (dataAnhDaiDien.has('image')) {
            quanLySinhVienService.uploadAnhDaiDien(dataAnhDaiDien, bindAnhDaiDien);
        }
    }
    //Phần thêm, chỉnh sửa sinh viên
    var bindDuLieuForm = function (data) {
        danhSachKhoa = data.danhSachKhoa;
        danhSachDanToc = data.danhSachDanToc,
        danhSachGioiTinh = data.danhSachGioiTinh;
        danhSachTonGiao = data.danhSachTonGiao;
    }
    var initSelectList = function () {
        $("#gioiTinh-input").select2({
            dropdownParent: $("#saveSinhVienModal"),
            placeholder: "Chọn giới tính",
            language: "vi",
            data: danhSachGioiTinh
        });
        $("#khoaHoc-input").select2({
            dropdownParent: $("#saveSinhVienModal"),
            placeholder: "Chọn một khóa",
            language: "vi",
            data: danhSachKhoa
        });
        $("#danToc-input").select2({
            dropdownParent: $("#saveSinhVienModal"),
            placeholder: "Chọn dân tộc",
            language: "vi",
            data: danhSachDanToc
        });
        $("#tonGiao-input").select2({
            dropdownParent: $("#saveSinhVienModal"),
            placeholder: "Chọn tôn giáo",
            language: "vi",
            data: danhSachTonGiao
        });
    }
    var populateForm = function (data) {
        bindDuLieuForm(data);
        initSelectList();
    }
    var resetInput = function() {
        $("#sinhVienId").val(0);  //Cái này phải là 0 chứ ko được là null
        setDataForInputWithLimit("hoVaTenLotSinhVien-input", "");
        setDataForInputWithLimit("tenSinhVien-input", "");
        $("#mssv-input").val("AS");
        $("#ngaySinh-input").val(null);
        $("#gioiTinh-input").val(null).trigger("change");
        $("#khoaHoc-input").val(null).trigger("change");
        $("#danToc-input").val(null).trigger("change");
        $("#tonGiao-input").val(null).trigger("change");
        $("#anhBiaSv_brower").val(null);
        $("#anhBiaSv").attr("src", "/Content/AnhBia/AnhSV/avatar.png");
    }
    var hienThiModalThemSinhVien = function () {
        resetInput();
        $("#saveSinhVien").html("Thêm sinh viên");
        $('#saveSinhVienModal').modal('show');
    }

    /*Phần chỉnh sửa sinh viên*/
    var updateInput = function (sinhVien) {
        $("#sinhVienId").val(sinhVien.id);
        setDataForInputWithLimit("hoVaTenLotSinhVien-input", sinhVien.hoVaTenLot);
        setDataForInputWithLimit("tenSinhVien-input", sinhVien.ten);
        $("#mssv-input").val(sinhVien.mssv);
        $("#ngaySinh-input").val(returnDateForInput(sinhVien.ngaySinh));
        $("#gioiTinh-input").val(sinhVien.gioiTinhId).trigger("change");
        $("#khoaHoc-input").val(sinhVien.khoaHocId).trigger("change");
        $("#danToc-input").val(sinhVien.danTocId).trigger("change");
        $("#tonGiao-input").val(sinhVien.tonGiaoId).trigger("change");
        $("#anhBiaSv").attr("src", sinhVien.anhDaiDien);
        rawImg = sinhVien.anhDaiDien;
        $("#gioiThieu-input").val(sinhVien.gioiThieu);
    }
    var hienThiModalSuaThongTin = function (e) {
        var button = $(e.target);
        var sinhVienId = button.closest("tr").attr("id");
        quanLySinhVienService.getSinhVienData(sinhVienId, updateInput);
        $("#saveSinhVien").html("Sửa thông tin");
        $('#saveSinhVienModal').modal("show");
    }
    var bindSinhVienDto = function () {
        sinhVienDto.id = parseInt($('#sinhVienId').val());
        sinhVienDto.hoVaTenLot = $('#hoVaTenLotSinhVien-input').val();
        sinhVienDto.ten = $('#tenSinhVien-input').val();
        sinhVienDto.mssv = $('#mssv-input').val();
        sinhVienDto.ngaySinh = $('#ngaySinh-input').val();
        sinhVienDto.gioiTinhId = $('#gioiTinh-input').val();
        sinhVienDto.khoaHocId = $('#khoaHoc-input').val();
        sinhVienDto.danTocId = $('#danToc-input').val();
        sinhVienDto.tonGiaoId = $('#tonGiao-input').val();
        sinhVienDto.anhDaiDien = $("#anhBiaSv").attr("src");
        //Ảnh đại diện sẽ được bind ở phần uploadAnhDaiDien nhưng phải giữ cái phía trên phòng trường hợp không up
        sinhVienDto.gioiThieu = $('#gioiThieu-input').val();
    }
    var updateAfterSaveSinhVien = function (xhr) {
        hideLoader();
        if (xhr == null) { //Nếu thành công
            //reload bảng sinh viên
            danhSachSinhVienTable.ajax.reload();
            //Hiển thị thông báo phù hợp
            if (sinhVienDto.id == 0) alert("Đã thêm sinh viên mới.");
            else alert("Đã chỉnh sửa thông tin sinh viên.");
        } else { //Nếu bị lỗi
            alert(xhr.responseJSON.message);
        }
        //Ẩn modal
        $("#saveSinhVienModal").modal("hide");
    }
    var saveSinhVien = function () {
        if ($("#saveSinhVienForm").valid()) {
            showLoader();
            bindSinhVienDto();
            uploadAnhDaiDien();
            quanLySinhVienService.saveSinhVien(sinhVienDto, updateAfterSaveSinhVien);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    var initTrang = function () {
        dataTableSetting();
        initDanhSachSinhVienTable();
        quanLySinhVienService.getDuLieuChoFormSaveSinhVien(populateForm);
        initCroppie();
        validateSinhVienForm();
        limitCharacter();
        $('#themSinhVienBtn').on('click', hienThiModalThemSinhVien);
        $('#saveSinhVien').on('click', saveSinhVien);
        $("body").on("click", ".chinhSuaSinhVien-js", hienThiModalSuaThongTin);
    }

    return{
        initTrang: initTrang,
        bindAnhDaiDien: bindAnhDaiDien,
        uploadAnhDaiDien: uploadAnhDaiDien,
        initCroppie: initCroppie,
        populateForm: populateForm,
        bindSinhVienDto: bindSinhVienDto
    }
}(QuanLySinhVienService);
var ThemLoSinhVienController = function(quanLySinhVienService) {
    var formData = new FormData();
    var danhSachKhoa, danhSachDanToc, danhSachGioiTinh, danhSachTonGiao;
    var danhSachKhoaTable, danhSachDanTocTable, danhSachGioiTinhTable, danhSachTonGiaoTable;
    var danhSachSinhVienDaTao, danhSachSinhVienKhongTaoDuoc;
    var danhSachSinhVienDaTaoTable, danhSachSinhVienKhongTaoDuocTable;

    //Tạo bảng dữ liệu
    var bindDuLieuMaHoa = function (data) {
        danhSachKhoa = data.danhSachKhoa;
        danhSachDanToc = data.danhSachDanToc,
        danhSachGioiTinh = data.danhSachGioiTinh;
        danhSachTonGiao = data.danhSachTonGiao;
    }
    var initDuLieuMaHoaTable = function () {
        danhSachGioiTinhTable = $("#danhSachGioiTinhTable").DataTable({
            data: danhSachGioiTinh,
            deferRender: true,
            "order": [[1, 'asc']],
            columns: [
                { data: 'text' },
                { data: 'id' }
            ]
        });
        danhSachKhoaTable = $("#danhSachKhoaTable").DataTable({
            data: danhSachKhoa,
            deferRender: true,
            "order": [[1, 'asc']],
            columns: [
                { data: 'text' },
                { data: 'id' }
            ]
        });
        danhSachDanTocTable = $("#danhSachDanTocTable").DataTable({
            data: danhSachDanToc,
            deferRender: true,
            "order": [[1, 'asc']],
            columns: [
                { data: 'text' },
                { data: 'id' }
            ]
        });
        danhSachTonGiaoTable = $("#danhSachTonGiaoTable").DataTable({
            data: danhSachTonGiao,
            deferRender: true,
            "order": [[1, 'asc']],
            columns: [
                { data: 'text' },
                { data: 'id' }
            ]
        });
    }
    var taoBangDuLieuMaHoa = function (data) {
        bindDuLieuMaHoa(data);
        initDuLieuMaHoaTable();
    }
    //Xử lý file
    var initBangKetQuaTable = function () {
        danhSachSinhVienDaTaoTable = $("#danhSachSinhVienDaTaoTable").DataTable({
            data: danhSachSinhVienDaTao,
            deferRender: true,
            "order": [[0, 'asc']],
            columns: [
                { data: 'mssv' },
                { data: '' },
                { data: 'ngaySinh' }
            ],
            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return returnTenSinhVien(row);
                    }
                },
                {
                    targets: 2,
                    render: function (data) {
                        return returnNgay(data);
                    }
                }
            ]
        });
        danhSachSinhVienKhongTaoDuocTable = $("#danhSachSinhVienKhongTaoDuocTable").DataTable({
            data: danhSachSinhVienKhongTaoDuoc,
            deferRender: true,
            "order": [[0, 'asc']],
            columns: [
                { data: 'sinhVien' },
                { data: 'sinhVien' },
                { data: 'sinhVien' },
                { data: 'loi' },
                { data: 'soDongBiLoi' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {
                        return data.mssv;
                    }
                },
                {
                    targets: 1,
                    render: function (data) {
                        return returnTenSinhVien(data);
                    }
                },
                {
                    targets: 2,
                    render: function (data) {
                        return returnNgay(data.ngaySinh);
                    }
                }
            ]
        });
    }
    var bindDataKetQua = function (data) {
        danhSachSinhVienDaTao = data.danhSachSinhVienDaTao;
        danhSachSinhVienKhongTaoDuoc = data.danhSachSinhVienKhongTaoDuoc;
    }
    var reloadBangKetQuaTable = function () {
        reloadTable(danhSachSinhVienDaTaoTable, danhSachSinhVienDaTao);
        reloadTable(danhSachSinhVienKhongTaoDuocTable, danhSachSinhVienKhongTaoDuoc);
    }
    var traKetQuaTaoSinhVien = function (data) {
        var ketQua = "Đã tạo " + data.soSinhVienDaTao + " sinh viên. ";
        if (data.soSinhVienKhongTaoDuoc !== 0) {
            ketQua = ketQua + data.soSinhVienKhongTaoDuoc + " sinh viên không tạo được. Xem thông tin bên dưới.";
        }
        alert(ketQua);
        bindDataKetQua(data);
        reloadBangKetQuaTable();
        $(".loader").hide();
    }
    var uploadFileExcel = function () {
        $(".loader").show();
        quanLySinhVienService.taoBatchSinhVien(formData,traKetQuaTaoSinhVien);
    }
    var uploadFileEventListener = function () {
        $("#uploadFile").change(function () {
            $('#upload-file-info').html(this.files[0].name);
            formData.delete('uploadFile');
            formData.append('uploadFile', this.files[0]);
        });
    }


    var initTrang = function () {
        dataTableSetting();
        initBangKetQuaTable();
        quanLySinhVienService.getDuLieuMaHoa(taoBangDuLieuMaHoa);
        uploadFileEventListener();
        $("#taoSinhVienBtn").on("click", uploadFileExcel);
    }

    return{
        initTrang : initTrang
    }

}(QuanLySinhVienService);
var QuanLyHoiVienController = function(quanLyHoiVienService) {
    var danhSachHoiVienTable, danhSachSinhVienDaDangKiTable, danhSachLoiDangKiHoiVienTable;
    var danhSachSinhVienDaDangKi, danhSachLoiDangKiHoiVien;
    var dangKiHoiVienDto = {};
    var formData = new FormData();
    var mssvSinhVienMuonXoa;
    //Func
    var taoTable = function () {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        //Bảng danh sách hội viên
        danhSachHoiVienTable = $("#danhSachHoiVien").DataTable({
            ajax: {
                url: "/api/SinhVien/LayDanhSachHoiVien",
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
            pageLength: 50,
            lengthMenu: [25, 50, 75, 100],
            "order": [[6, "desc"]],
            columns: [
                { data: "mssv" },
                { data: "hoVaTenLot" },
                { data: "ten" },
                { data: "ngaySinh" },
                { data: "tenGioiTinh" },
                { data: "kyHieuTenLop" },
                { data: "ngayVaoHoi" },
                { data: "" }
            ],
            rowId: "mssv",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    name: "MSSV"
                },
                {
                    targets: 1,
                    orderable: false
                },
                {
                    targets: 2,
                    name: "Ten"
                },

                {
                    targets: 3,
                    render: $.fn.dataTable.render.moment("DD/MM/YYYY", "DD/MM/YYYY", "vi"),
                    name: "NgaySinh"
                },
                {
                    targets: 4,
                    name: "GioiTinh"
                },
                {
                    targets: 5,
                    render: function (data) {
                        return returnKyHieuTenLop(data);
                    },
                    name: "KyHieuTenLop"
                },
                {
                    targets: 6,
                    render: $.fn.dataTable.render.moment("DD/MM/YYYY", "DD/MM/YYYY", "vi"),
                    name: "NgayVaoHoi"
                },
                {
                    targets: 7,
                    render: function() {
                        return '<button class="btn btn-danger xoaHoiVien-js">Xóa</button>';
                    },
                    orderable: false
                }
            ]
        });
        //Bảng danh sách sinh viên đã đăng kí hội viên
        danhSachSinhVienDaDangKiTable = $("#danhSachSinhVienDaDangKiTable").DataTable({
            data: danhSachSinhVienDaDangKi,
            deferRender: true,
            "order": [[0, 'asc']],
            columns: [
                { data: 'mssv' },
                { data: '' },
                { data: 'ngayVao' }
            ],
            columnDefs: [
                {
                    targets: 1,
                    render: function(data, type, row) {
                        return returnTenSinhVien(row);
                    }
                },
                {
                    targets: 2,
                    render: function(data) {
                        return returnNgay(data);
                    }
                }
            ]
        });
        //Bảng danh sách sinh viên không đăng kí được hội viên
        danhSachLoiDangKiHoiVienTable = $("#danhSachLoiDangKiHoiVienTable").DataTable({
            data: danhSachLoiDangKiHoiVien,
            deferRender: true,
            "order": [[0, 'asc']],
            columns: [
                { data: 'mssv' },
                { data: 'ngayVao' },
                { data: 'loi' },
                { data: 'soDongBiLoi' }
            ],
            columnDefs: [
                {
                    targets: 1,
                    render: function(data) {
                        return returnNgay(data);
                    }
                },
                {
                    targets: 3,
                    render: function(data) {
                        return "Dòng " + data;
                    }
                }
            ]
        });
    }
    //Đăng kí hội viên
    var bindDangKiHoiVienDto = function(ngayVaoHoi, mssv) {
        dangKiHoiVienDto.ngayVao = ngayVaoHoi;
        dangKiHoiVienDto.mssv = mssv;
    }
    var updateAfterDangKi = function() {
        danhSachHoiVienTable.ajax.reload();
        alert("Đã đăng kí hội viên cho sinh viên");
    }
    var dangKiHoiVien = function(e, sinhVien) {
        var ngayVaoHoi = $('#ngayVaoHoi-input').val();
        if (ngayVaoHoi === "") {
            alert('Không được để trống ngày vào hội.');
        } else {
            bindDangKiHoiVienDto(ngayVaoHoi, sinhVien.mssv);
            quanLyHoiVienService.dangKiHoiVien(dangKiHoiVienDto, updateAfterDangKi);
        }
        $('#sinhVienVaoHoi-input').typeahead('val', 'AS');
    }
    var initTypeahead = function() {
        var danhSachsinhVien = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('hoVaTenLot', 'ten', 'mssv', 'kyHieuTenLop'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/api/SinhVien/TimKiem?searchTerm=%QUERY',
                wildcard: '%QUERY'
            }
        });
        $('#sinhVienVaoHoi-input').typeahead(
                {
                    highlight: true
                },
                {
                    name: 'sinhVien',
                    display: function(data) {
                        return data.hoVaTenLot + " " + data.ten;
                    },
                    source: danhSachsinhVien,
                    templates: {
                        suggestion: function(data) {
                            return '<p><img src="' +
                                data.anhDaiDien +
                                '" class="anhSvTypeahead"/><strong>' +
                                data.hoVaTenLot +
                                ' ' +
                                data.ten +
                                '</strong> - ' +
                                data.mssv +
                                ' - ' +
                                returnKyHieuTenLop(data.kyHieuTenLop) +
                                '</p>';
                        }
                    }

                })
            .on("typeahead:autocomplete", dangKiHoiVien)
            .on("typeahead:select", dangKiHoiVien)
            .on("typeahead:change",
                function() {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
        $('#sinhVienVaoHoi-input').typeahead('val', 'AS');
    }
    //Xử lý file
    var bindDataKetQua = function(data) {
        danhSachSinhVienDaDangKi = data.danhSachSinhVienDaDangKi;
        danhSachLoiDangKiHoiVien = data.danhSachLoiDangKiHoiVien;
    }
    var reloadBangKetQuaTable = function() {
        danhSachHoiVienTable.ajax.reload();
        reloadTable(danhSachSinhVienDaDangKiTable, danhSachSinhVienDaDangKi);
        reloadTable(danhSachLoiDangKiHoiVienTable, danhSachLoiDangKiHoiVien);
    }
    var traKetQuaDangKiHoiVien = function(data) {
        var ketQua = "Đã đăng kí hội viên cho " + data.soSinhVienDaDangKi + " sinh viên. ";
        if (data.soLoiDangKi !== 0) {
            ketQua = ketQua + data.soLoiDangKi + " sinh viên không đăng kí hội viên được. Xem bảng lỗi bên dưới.";
        }
        alert(ketQua);
        bindDataKetQua(data);
        reloadBangKetQuaTable();
    }
    var uploadFileExcel = function() {
        quanLyHoiVienService.dangKiHoiVienHangLoat(formData, traKetQuaDangKiHoiVien);
    }
    var uploadFileEventListener = function() {
        $("#uploadFile").change(function() {
            $('#upload-file-info').html(this.files[0].name);
            formData.delete('uploadFile');
            formData.append('uploadFile', this.files[0]);
        });
    }
    //Xóa đăng kí hội viên
    var hienThiModalXoaHoiVien = function(e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        mssvSinhVienMuonXoa = button.closest("tr").attr("id");
        $('#tenHoiVien').html(button.closest("tr").children().eq(1).html());
        $('#xoaDangKiHoiVienModal').modal('show');
    }
    var updateAfterXoaDangKi = function() {
        danhSachHoiVienTable.ajax.reload();
        alert('Đã xóa.');
        $('#xoaDangKiHoiVienModal').modal('hide');
    }
    var xoaDangKiHoiVien = function() {
        quanLyHoiVienService.xoaDangKiHoiVien(mssvSinhVienMuonXoa, updateAfterXoaDangKi);
    }

    var initTrang = function() {
        dataTableSetting();
        taoTable();
        initTypeahead();
        uploadFileEventListener();
        $('#dangKiHoiVien').on('click', uploadFileExcel);
        $('body').on('click', '.xoaHoiVien-js', hienThiModalXoaHoiVien);
        $('#xoaDangKiHoiVien').on('click', xoaDangKiHoiVien);
    }

    return {
        initTrang: initTrang
    }
}(QuanLyHoiVienService);
var QuanLyDoanVienController = function(quanLyDoanVienService) {
    var danhSachDoanVienTable, danhSachSinhVienDaDangKiTable, danhSachLoiDangKiDoanVienTable;
    var danhSachSinhVienDaDangKi, danhSachLoiDangKiDoanVien;
    var dangKiDoanVienDto = {};
    var formData = new FormData();
    var mssvSinhVienMuonXoa;

    var taoTable = function () {
        var csrfToken = $("input[name='__RequestVerificationToken']").val();
        //Bảng danh sách hội viên
        danhSachDoanVienTable = $("#danhSachDoanVien").DataTable({
            ajax: {
                url: "/api/SinhVien/LayDanhSachDoanVien",
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
            pageLength: 50,
            lengthMenu: [25, 50, 75, 100],
            "order": [[6, "desc"]],
            columns: [
                { data: "mssv" },
                { data: "hoVaTenLot" },
                { data: "ten" },
                { data: "ngaySinh" },
                { data: "tenGioiTinh" },
                { data: "kyHieuTenLop" },
                { data: "ngayVaoDoan" },
                { data: "noiVaoDoan" },
                { data: "" }
            ],
            rowId: "mssv",
            columnDefs: [
                {
                    targets: 0,
                    width: 100,
                    name: "MSSV"
                },
                {
                    targets: 1,
                    orderable: false
                },
                {
                    targets: 2,
                    name: "Ten"
                },

                {
                    targets: 3,
                    render: $.fn.dataTable.render.moment("DD/MM/YYYY", "DD/MM/YYYY", "vi"),
                    name: "NgaySinh"
                },
                {
                    targets: 4,
                    name: "GioiTinh"
                },
                {
                    targets: 5,
                    render: function (data) {
                        return returnKyHieuTenLop(data);
                    },
                    name: "KyHieuTenLop"
                },
                {
                    targets: 6,
                    render: $.fn.dataTable.render.moment("DD/MM/YYYY", "DD/MM/YYYY", "vi"),
                    name: "NgayVaoDoan"
                },
                {
                    targets: 7,
                    name: "NoiVaoDoan"
                },
                {
                    targets: 8,
                    render: function () {
                        return '<button class="btn btn-danger xoaHoiVien-js">Xóa</button>';
                    },
                    orderable: false
                }
            ]
        });
        //Bảng danh sách sinh viên đã đăng kí hội viên
        danhSachSinhVienDaDangKiTable = $("#danhSachSinhVienDaDangKiTable").DataTable({
            data: danhSachSinhVienDaDangKi,
            deferRender: true,
            "order": [[0, 'asc']],
            columns: [
                { data: 'mssv' },
                { data: '' },
                { data: 'ngayVao' },
                { data: 'noiVao' }
            ],
            columnDefs: [
                {
                    targets: 1,
                    render: function (data, type, row) {
                        return returnTenSinhVien(row);
                    }
                },
                {
                    targets: 2,
                    render: function (data) {
                        return returnNgay(data);
                    }
                }
            ]
        });
        //Bảng danh sách sinh viên không đăng kí được hội viên
        danhSachLoiDangKiDoanVienTable = $("#danhSachLoiDangKiDoanVienTable").DataTable({
            data: danhSachLoiDangKiDoanVien,
            deferRender: true,
            "order": [[0, 'asc']],
            columns: [
                { data: 'mssv' },
                { data: 'ngayVao' },
                { data: 'noiVao' },
                { data: 'loi' },
                { data: 'soDongBiLoi' }
            ],
            columnDefs: [
               {
                   targets: 1,
                   render: function (data) {
                       return returnNgay(data);
                   }
               },
                {
                    targets: 4,
                    render: function (data) {
                        return "Dòng " + data;
                    }
                }
            ]
        });
    }
    //Đăng kí đoàn viên 1 người
    var bindDangKiDoanVienDto = function (ngayVaoDoan, noiVaoDoan, mssv) {
        dangKiDoanVienDto.ngayVao = ngayVaoDoan;
        dangKiDoanVienDto.noiVao = noiVaoDoan;
        dangKiDoanVienDto.mssv = mssv;
    }
    var updateAfterDangKi = function () {
        danhSachDoanVienTable.ajax.reload();
        alert("Đã đăng kí đoàn viên cho sinh viên");
    }
    var dangKiDoanVien = function (e, sinhVien) {
        var ngayVaoDoan = $('#ngayVaoDoan-input').val();
        var noiVaoDoan = $('#noiVaoDoan-input').val();
        if (ngayVaoDoan === "" || noiVaoDoan === "") {
            alert('Không được để trống ngày vào đoàn hoặc nơi vào đoàn.');
        } else {
            bindDangKiDoanVienDto(ngayVaoDoan, noiVaoDoan, sinhVien.mssv);
            quanLyDoanVienService.dangKiDoanVien(dangKiDoanVienDto, updateAfterDangKi);
        }
        $('#sinhVienVaoDoan-input').typeahead('val', 'AS');
    }
    var initTypeahead = function () {
        var danhSachsinhVien = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('hoVaTenLot', 'ten', 'mssv', 'kyHieuTenLop'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/api/SinhVien/TimKiem?searchTerm=%QUERY',
                wildcard: '%QUERY'
            }
        });
        $('#sinhVienVaoDoan-input').typeahead(
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
            .on("typeahead:autocomplete", dangKiDoanVien)
            .on("typeahead:select", dangKiDoanVien)
            .on("typeahead:change",
                function () {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
        $('#sinhVienVaoDoan-input').typeahead('val', 'AS');
    }
    //Đăng kí đoàn viên hàng loạt
    //Xử lý file
    var bindDataKetQua = function (data) {
        danhSachSinhVienDaDangKi = data.danhSachSinhVienDaDangKi;
        danhSachLoiDangKiDoanVien = data.danhSachLoiDangKiDoanVien;
    }
    var reloadBangKetQuaTable = function () {
        danhSachDoanVienTable.ajax.reload();
        reloadTable(danhSachSinhVienDaDangKiTable, danhSachSinhVienDaDangKi);
        reloadTable(danhSachLoiDangKiDoanVienTable, danhSachLoiDangKiDoanVien);
    }
    var traKetQuaDangKiDoanVien = function (data) {
        var ketQua = "Đã đăng kí đoàn viên cho " + data.soSinhVienDaDangKi + " sinh viên. ";
        if (data.soLoiDangKi !== 0) {
            ketQua = ketQua + data.soLoiDangKi + " sinh viên không đăng kí đoàn viên được. Xem bảng lỗi bên dưới.";
        }
        alert(ketQua);
        bindDataKetQua(data);
        reloadBangKetQuaTable();
    }
    var uploadFileExcel = function () {
        quanLyDoanVienService.dangKiDoanVienHangLoat(formData, traKetQuaDangKiDoanVien);
    }
    var uploadFileEventListener = function () {
        $("#uploadFile").change(function () {
            $('#upload-file-info').html(this.files[0].name);
            formData.delete('uploadFile');
            formData.append('uploadFile', this.files[0]);
        });
    }
    //Xóa đăng kí đoàn viên
    var hienThiModalXoaDoanVien = function (e) {
        var button = $(e.target);
        //Lấy id môn học muốn xóa
        mssvSinhVienMuonXoa = button.closest("tr").attr("id");
        $('#tenDoanVien').html(button.closest("tr").children().eq(1).html());
        $('#xoaDangKiDoanVienModal').modal('show');
    }
    var updateAfterXoaDangKi = function () {
        danhSachDoanVienTable.ajax.reload();
        alert('Đã xóa.');
        $('#xoaDangKiDoanVienModal').modal('hide');
    }
    var xoaDangKiDoanVien = function () {
        quanLyDoanVienService.xoaDangKiDoanVien(mssvSinhVienMuonXoa, updateAfterXoaDangKi);
    }


    var initTrang = function () {
        dataTableSetting();
        taoTable();
        initTypeahead();
        uploadFileEventListener();
        $('#dangKiDoanVien').on('click', uploadFileExcel);
        $('body').on('click', '.xoaDoanVien-js', hienThiModalXoaDoanVien);
        $('#xoaDangKiDoanVien').on('click', xoaDangKiDoanVien);
    }
    return {
        initTrang: initTrang
}
}(QuanLyDoanVienService);
var QuanLyTrangCaNhanController = function (quanLyTrangCaNhanService, quanLySinhVienService, quanLySinhVienController) {
    //Biến chứa data ảnh bìa để save lên server
    var dataAnhDaiDien = new FormData();
    var sinhVienDto = {}, diaChiDto = {}, sdtDto = {};
    var sdtTable, diaChiTable;
    var sinhVien ={}, sinhVienId;
    var rawImg;
    var daTaoBaiVietSv = false, daTaoDonViSv = false;

    //Hiển thị dom
    var returnDiaChi = function(data, row) {
        return row['soNhaTenDuong'] + " " + row['capXa'] + ", " + row['capHuyen'] + ", " + row['capTinh'];
    }
    var returnLoaiDiaChi= function(data) {
        if (data == "NoiOHienTai") return "Nơi ở hiện tại";
        else if (data == "HoKhauThuongTru") return "Hộ khẩu thường trú";
        else return "Địa chỉ liên lạc";
    }
    var initDiaChiTable = function () {
        diaChiTable = $("#diaChi-TrangCaNhan").DataTable({
            data: sinhVien.diaChi,
            searching: false,
            lengthChange: false,
            columns: [
                { data: '' },
                { data: 'loaiDiaChi' },
                { data: '' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        return returnDiaChi(data, row);
                    }
                },
                {
                    targets: 1,
                    render: function (data) {
                        return returnLoaiDiaChi(data);
                    }
                },
                {
                    targets: 2,
                    render: function () {
                        return '<button class="btn btn-danger xoaDiaChi-js">Xóa</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {
                $(row).addClass('sdtRow');
            },
            rowId: function(row) {
                return "dc_" + row.id;
            }
        });
    }
    var formatSdt = function (data) {
        return data.replace(/(\d{4})(\d{3})(\d{3})/, "$1-$2-$3");
    }
    var initSdtTable= function() {
        sdtTable = $("#sdt-TrangCaNhan").DataTable({
            data: sinhVien.sdt,
            searching: false,
            lengthChange: false,
            columns: [
                { data: 'soDienThoai' },
                { data: 'moTa' },
                { data: '' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data) {
                        return formatSdt(data);
                    }
                },
                {
                    targets: 2,
                    render: function () {
                        return '<button class="btn btn-danger xoaSdt-js">Xóa</button>';
                    }
                }
            ],
            rowCallback: function (row) {
                $(row).addClass('sdtRow');
            },
            rowId: function (row) {
                return "sdt_" + row.id;
            }
        });
    }
    var reloadTables = function() {
        reloadTable(sdtTable, sinhVien.sdt);
        reloadTable(diaChiTable, sinhVien.diaChi);
    }
    var returnHoiVienStatus = function (hoiVien) {
        if (hoiVien != null) {
            $("#hoiVienStatus").toggleClass("thamGiaTrue");
            $("#hoiVienStatus").append('<i class="fas fa-check"></i>');
            $('#ngayVaoHoi').html(returnNgay(hoiVien.ngayVao));
            $("#ngayVaoHoi").parent().css("display", "block");
        }
        else {
            $("#hoiVienStatus").toggleClass("thamGiaFalse");
            $("#hoiVienStatus").append('<i class="fas fa-times"></i>');
        }
    }
    var returnDoanVienStatus= function(doanVien) {
        if (doanVien != null) {
            $("#doanVienStatus").toggleClass("thamGiaTrue");
            $("#doanVienStatus").html('<i class="fas fa-check"></i>');
            $('#ngayVaoDoan').html(returnNgay(doanVien.ngayVao));
            $('#noiVaoDoan').html(doanVien.noiVao);
            $("#ngayVaoDoan").parent().css("display", "block");
            $("#noiVaoDoan").parent().css("display", "block");
        }
        else {
            $("#doanVienStatus").toggleClass("thamGiaFalse");
            $("#doanVienStatus").append('<i class="fas fa-times"></i>');
        }
    }
    var returnDangVienStatus = function (dangVien) {
        if (dangVien != null) {
            $("#dangVienStatus").toggleClass("thamGiaTrue");
            $("#dangVienStatus").append('<i class="fas fa-check"></i>');
            $('#ngayVaoDang').html(returnNgay(dangVien.ngayVao));
            $('#noiVaoDang').html(dangVien.noiVao);
            $("#ngayVaoDang").parent().css("display", "block");
            $("#noiVaoDang").parent().css("display", "block");
        }
        else {
            $("#dangVienStatus").toggleClass("thamGiaFalse");
            $("#dangVienStatus").append('<i class="fas fa-times"></i>');
        }
    }
    var updateThongTin = function() {
        $("#ten-TrangCaNhan").html(returnTenSinhVien(sinhVien));
        $("#danToc-TrangCaNhan").html(sinhVien.danToc);
        $("#MSSV-TrangCaNhan").html(sinhVien.mssv);
        $("#tonGiao-TrangCaNhan").html(sinhVien.tonGiao);
        $("#gioiTinh-TrangCaNhan").html(sinhVien.gioiTinh);
        $("#lop-TrangCaNhan").html(sinhVien.tenLop);
        $("#ngaySinh-TrangCaNhan").html(returnNgay(sinhVien.ngaySinh));
        $("#khoa-TrangCaNhan").html(sinhVien.khoaHoc.tenKhoa);
        $("#anhTrangCaNhanSV").attr("src", sinhVien.anhDaiDien);
        $("#gioiThieu-TrangCaNhan").html(sinhVien.gioiThieu);
        document.title = returnTenSinhVien(sinhVien);
        returnHoiVienStatus(sinhVien.hoiVien);
        returnDoanVienStatus(sinhVien.doanVien);
        returnDangVienStatus(sinhVien.dangVien);
    }
    var bindData = function (data) {
        sinhVien = data;
    }
    var updatePage = function(data) {
        bindData(data);
        updateThongTin();
        reloadTables();
    }
    //Phần upload ảnh bìa sinh viên
    var exportCroppie = function ($uploadCrop, dataAnhBia, mssv) {
        var tenFile = "" + mssv;
        //Tạo File ảnh thật sự để save lên server (lưu vào formData dataAnhBia trước khi gửi ajax)
        $uploadCrop.croppie('result',
            {
                type: "blob",
                size: { width: 300, height: 400 }
            }).then(function (blob) {
                dataAnhBia.delete("image");
                dataAnhBia.append("image", blob, tenFile);
            });

        //Tạo File ảnh để hiển thị cho người dùng
        $uploadCrop.croppie('result', {
            type: 'base64',
            format: 'jpeg',
            size: { width: 450, height: 600 }
        }).then(function (linkAnh) {
            $('#anhBiaSv').attr('src', "");
            $('#anhBiaSv').attr('src', linkAnh);
            $('#cropImagePop').modal('hide');
        });
    }
    var initCroppie = function () {
        $("#anhBiaSv_brower").change(function () {
            if (this.files && this.files[0]) {
                var imageDir = new FileReader();
                imageDir.readAsDataURL(this.files[0]);
                imageDir.onload = function (e) {
                    $('#anhBiaSv-Wrapper').addClass('ready');
                    $('#cropImagePop').modal('show'); //Show modal
                    $('#editAnhBiaSvBtn').css("display", "block"); //Hiển thị nút chỉnh sửa
                    rawImg = e.target.result; //Bind link ảnh thô với file vừa up lên
                }
            }
        });
        //Tạo instance croppie
        //Lưu ý quan trọng, khi sử dụng croppie, node chứa instance croppie (ở đây là anhBiaSv-Wrapper) phải
        // được khai báo width và height cụ thể bằng px, không thể croppie sẽ không hiện ra.
        var $uploadCrop = $('#anhBiaSv-Wrapper').croppie({
            viewport: {
                width: 300,
                height: 400,
                type: 'square'
            },
            enableExif: true
        });
        //Bind rawImg (file hình ảnh thô) vào croppie khi modal thêm ảnh hiện
        $('#cropImagePop').on('shown.bs.modal', function () {
            $uploadCrop.croppie('bind', { url: rawImg });
        });
        //Tạo ảnh khi nhấn nút crop
        $('#cropImageBtn').on('click', function (ev) {
            //Lấy mssv trước chứ không thể lấy từ sinhVienDto.mssv vì nó chưa bind Data
            var mssv = $('#mssv-input').val();
            exportCroppie($uploadCrop, dataAnhDaiDien, mssv);
        });
    }
    var bindAnhDaiDien = function (data) {
        sinhVienDto.anhDaiDien = data;
    }
    var uploadAnhDaiDien = function () {
        if (dataAnhDaiDien.has('image')) {
            quanLySinhVienService.uploadAnhDaiDien(dataAnhDaiDien, bindAnhDaiDien);
        }
    }
    //Phần chỉnh sửa sinh viên
    var updateInput = function () {
        $('#sinhVienId').val(sinhVien.id);
        setDataForInputWithLimit("hoVaTenLotSinhVien-input", sinhVien.hoVaTenLot);
        setDataForInputWithLimit("tenSinhVien-input", sinhVien.ten);
        $('#mssv-input').val(sinhVien.mssv);
        $('#ngaySinh-input').val(returnDateForInput(sinhVien.ngaySinh));
        $('#gioiTinh-input').val(sinhVien.gioiTinhId).trigger('change');
        $('#khoaHoc-input').val(sinhVien.khoaHoc.id).trigger('change');
        $('#danToc-input').val(sinhVien.danTocId).trigger('change');
        $('#tonGiao-input').val(sinhVien.tonGiaoId).trigger('change');
        $('#anhBiaSv').attr("src", sinhVien.anhDaiDien);
        rawImg = sinhVien.anhDaiDien;
        $("#gioiThieu-input").val(sinhVien.gioiThieu);
    }
    var hienThiModalSuaThongTin = function () {
        updateInput();
        $('#saveSinhVienModal').modal('show');
    }
    var bindSinhVienDto = function () {
        sinhVienDto.id = sinhVien.id;
        sinhVienDto.hoVaTenLot = $('#hoVaTenLotSinhVien-input').val();
        sinhVienDto.ten = $('#tenSinhVien-input').val();
        sinhVienDto.mssv = $('#mssv-input').val();
        sinhVienDto.ngaySinh = $('#ngaySinh-input').val();
        sinhVienDto.gioiTinhId = $('#gioiTinh-input').val();
        sinhVienDto.khoaHocId = $('#khoaHoc-input').val();
        sinhVienDto.danTocId = $('#danToc-input').val();
        sinhVienDto.tonGiaoId = $('#tonGiao-input').val();
        sinhVienDto.anhDaiDien = $("#anhBiaSv").attr("src");
        //Ảnh đại diện sẽ được bind ở phần uploadAnhDaiDien nhưng phải giữ cái phía trên phòng trường hợp không up
        sinhVienDto.gioiThieu = $('#gioiThieu-input').val();
    }
    var updateAfterSaveSinhVien = function (xhr) {
        hideLoader();
        if (xhr == null) { //Nếu thành công
            alert("Đã chỉnh sửa thông tin sinh viên.");
            location.reload(true);
        } else { //Nếu bị lỗi
            alert(xhr.responseJSON.message);
        }
    }
    var saveSinhVien = function () {
        if ($("#saveSinhVienForm").valid()) {
            showLoader();
            bindSinhVienDto();                                                      
            uploadAnhDaiDien();
            quanLyTrangCaNhanService.saveSinhVien(sinhVienDto, updateAfterSaveSinhVien);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    //Phần thêm, xóa địa chỉ
    var validateDiaChi = function() {
        $("#diaChiForm").validate({
            ignore: "input[type=hidden]",
            rules: {
                "diaChiTinh-input": "required",
                "diaChiHuyen-input": "required",
                "diaChiXa-input": "required",
                "soNhaTenDuong-input": "required",
                "loaiDiaChi-input": "required"
            },
            messages: {
                "diaChiTinh-input": "Vui lòng chọn tỉnh",
                "diaChiHuyen-input": "Vui lòng chọn huyện",
                "diaChiXa-input": "Vui lòng chọn xã",
                "soNhaTenDuong-input": "Vui lòng nhập địa chỉ",
                "loaiDiaChi-input": "Vui lòng chọn loại địa chỉ"
            }
        });

    }
    var initDanhSachTinh = function (data) {
        $("#diaChiTinh-input").select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một tỉnh",
            language: "vi",
            data: data
        });
    }
    var reloadDanhSachHuyenSelectList = function(data) {
        $("#diaChiHuyen-input").select2('destroy').empty().select2({
            data: data,
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một huyện",
            language: "vi"
        }).trigger('change');
        $("#diaChiHuyen-input").val(null).trigger('change');
        //Phai xoá xã của huyện cũ luôn
        $("#diaChiXa-input").select2('destroy').empty().select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một xã",
            language: "vi"
        }).trigger('change');
        $("#diaChiXa-input").val(null).trigger('change');
    }
    var reloadDanhSachXaSelectList = function (data) {
        $("#diaChiXa-input").select2('destroy').empty().select2({
            data: data,
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một xã",
            language: "vi"
        }).trigger('change');
        $("#diaChiXa-input").val(null).trigger('change');
    }
    var updateDiaChiSelectLists = function () {
        $("#diaChiTinh-input").val(null).trigger("change");
        //Init danh sách huyện (cái này giống như place holder thôi, vì chưa có dữ liệu)
        $("#diaChiHuyen-input").select2("destroy").empty().select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một huyện",
            language: "vi"
        });
        //Init danh sách xã  (cái này giống như place holder thôi, vì chưa có dữ liệu)
        $("#diaChiXa-input").select2("destroy").empty().select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một xã",
            language: "vi"
        });
        
    }
    var initDiaChiSelectLists = function () {
        //Init danh sách tỉnh
        quanLyTrangCaNhanService.layDanhSachTinh(initDanhSachTinh);
        //Init danh sách huyện (cái này giống như place holder thôi, vì chưa có dữ liệu)
        $("#diaChiHuyen-input").select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một huyện",
            language: "vi"
        });
        //Init danh sách xã  (cái này giống như place holder thôi, vì chưa có dữ liệu)
        $("#diaChiXa-input").select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn một xã",
            language: "vi"
        });
        //Init loại địa chỉ
        $("#loaiDiaChi-input").select2({
            dropdownParent: $("#saveDiaChiModal"),
            placeholder: "Chọn loại địa chỉ",
            language: "vi"
        });
        //Update danh sách Huyện khi chọn tỉnh
        $('#diaChiTinh-input').on('select2:select', function () {
            var idTinh = $('#diaChiTinh-input').val();
            if (idTinh == null) return;
            quanLyTrangCaNhanService.layDanhSachHuyen(idTinh, reloadDanhSachHuyenSelectList);
        });
        //Update danh sách Xã khi chọn huyện
        $('#diaChiHuyen-input').on('select2:select', function () {
            var idHuyen = $('#diaChiHuyen-input').val();
            if (idHuyen == null) return;
            quanLyTrangCaNhanService.layDanhSachXa(idHuyen, reloadDanhSachXaSelectList);
        });
    }
    var hienThiModalThemDiaChi = function () {
        updateDiaChiSelectLists();
        $('#soNhaTenDuong-input').val("");
        $('#saveDiaChiModal').modal('show');
    }
    var bindDiaChiDto = function() {
        diaChiDto.soNhaTenDuong = $('#soNhaTenDuong-input').val();
        diaChiDto.loaiDiaChi = parseInt($('#loaiDiaChi-input').val());
        diaChiDto.capXaId = parseInt($('#diaChiXa-input').val());
        diaChiDto.capHuyenId =  parseInt($('#diaChiHuyen-input').val());
        diaChiDto.capTinhId =  parseInt($('#diaChiTinh-input').val());     
        diaChiDto.sinhVienId = sinhVienId;
    }
    var updateAfterSaveDiaChi = function (data) {
        hideLoader();
        $('#saveDiaChiModal').modal('hide');
        reloadTable(diaChiTable, data);
    }
    var saveDiaChi = function () {
        if ($("#diaChiForm").valid()) {
            showLoader();
            bindDiaChiDto();
            quanLyTrangCaNhanService.saveDiaChi(diaChiDto, updateAfterSaveDiaChi);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    var updateAfterXoaDiaChi = function (idDiaChi) {
        diaChiTable.row('#dc_' + idDiaChi).remove().draw();
    }
    var xoaDiaChi = function(e) {
        var button = $(e.target);
        var idDiaChiMuonXoa = button.closest("tr").attr("id").slice(3);
        quanLyTrangCaNhanService.xoaDiaChi(idDiaChiMuonXoa, updateAfterXoaDiaChi);  //Lấy từ kí tự thứ 4 vì id địa chỉ có cấu trúc "dc_5"
    }
    //Thêm,xóa số điện thoại
    var validateSdt = function() {
        $.validator.addMethod("sdt_validation", function (value, element, options) {
            var isVnPhoneMobile = /^(0|\+84)(\s|\.|\-)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.|\-)?(\d{3})(\s|\.|\-)?(\d{3})$/;
            return isVnPhoneMobile.test(value);
        }, $.validator.format("Vui lòng nhập đúng định dạng số điện thoại Việt Nam: 0901-234-567"));
        $("#sdtForm").validate({
            ignore: "input[type=hidden]",
            rules: {
                "sdt-input": {
                    required: true,
                    sdt_validation: true
                },
                "moTaSdt-input": "required"
            },
            messages: {
                "sdt-input": {
                    required: "Vui lòng nhập số điện thoại"
                },
                "moTaSdt-input": "Vui lòng nhập mô tả số điện thoại"
            }
        });
    }
    var hienThiModalThemSdt = function () {
        $('#sdt-input').val(null);
        $('#moTaSdt-input').val(null);
        $('#saveSdtModal').modal('show');
    }
    var bindSdtDto = function() {
        var sdt = $("#sdt-input").val();
        sdtDto.id = 0;
        sdtDto.sinhVienId = sinhVienId;
        sdtDto.soDienThoai = sdt.replace(/\D+/g, "");
        sdtDto.moTa = $("#moTaSdt-input").val();
    }
    var updateAfterSaveSdt = function (data) {
        hideLoader();
        reloadTable(sdtTable, data);
        $('#saveSdtModal').modal('hide');
    }
    var saveSdt = function () {
        if ($("#sdtForm").valid()) {
            showLoader();
            bindSdtDto();
            quanLyTrangCaNhanService.saveSdt(sdtDto, updateAfterSaveSdt);
        } else alert("Hãy nhập chính xác các thông tin cần thiết.");
    }
    var updateAfterXoaSdt = function (idSdt) {
        sdtTable.row('#sdt_' + idSdt).remove().draw();
    }
    var xoaSdt = function (e) {
        var button = $(e.target);
        var idSdtMuonXoa = button.closest("tr").attr("id").slice(4); //Lấy từ kí tự thứ 5 vì id sđt có cấu trúc "sdt_5"
        quanLyTrangCaNhanService.xoaSdt(idSdtMuonXoa, updateAfterXoaSdt);
    }
    //Init Trang
    var initTrang = function (idSinhVien) {
        sinhVienId = idSinhVien;  
        dataTableSetting();
        initSdtTable();
        initDiaChiTable();
        initDiaChiSelectLists();
        quanLyTrangCaNhanService.getSinhVienData(sinhVienId, updatePage);
        initCroppie();
        quanLySinhVienService.getDuLieuChoFormSaveSinhVien(quanLySinhVienController.populateForm);
        validateSinhVienForm();
        limitCharacter();
        validateDiaChi();
        validateSdt();
        $("#chinhSuaSinhVien").on("click", hienThiModalSuaThongTin);
        $("#saveSinhVien").on("click", saveSinhVien);
        $("#themDiaChi").on("click", hienThiModalThemDiaChi);
        $("#saveDiaChi").on("click", saveDiaChi);
        $("body").on("click", ".xoaDiaChi-js", xoaDiaChi);
        $("#themSdt").on("click", hienThiModalThemSdt);
        $("#saveSdt").on("click", saveSdt);
        $("body").on("click", ".xoaSdt-js", xoaSdt);

    }


    /*Trang Chi tiết sinh viên (Trang cho người khác vào xem)*/
    var updateThongTin_TrangChiTietSinhVien = function (data) {
        bindData(data);
        updateThongTin();
    }
        //Init Bài viết sinh viên
    var themBaiViet = function (data) {
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        $("#nav-baiVietSv .tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: data }));
    }
    var initDanhSachBaiVietSinhVien = function () {
        if (daTaoBaiVietSv) return;
        quanLyTrangCaNhanService.layBaiVietSinhVien(sinhVienId, themBaiViet);
        daTaoBaiVietSv = true;
    }
        //Init Đơn vị sinh viên
    var themDonViVaoDom = function (danhSachDonVi) {
        //Lấy template cho hoạt động
        var cardDonVi_Template = _.template($("#cardDonVi_Template").html());
        //Thêm hoạt động đang tham gia
        danhSachDonVi.forEach(function (donVi) {
            $("#danhSachDonViSinhVien").append(cardDonVi_Template({ donVi: donVi }));
        });
    }
    var initTrangDonViCuaToi = function () {
        if (daTaoDonViSv) return;
        quanLyTrangCaNhanService.layDanhSachDonViSinhVien(sinhVienId,themDonViVaoDom);
        daTaoDonViSv = true;
    }

    var initTrangChiTietSinhVien = function(idSinhVien) {
        sinhVienId = idSinhVien;
        quanLyTrangCaNhanService.layThongTinCoBanSinhVien(sinhVienId, updateThongTin_TrangChiTietSinhVien);
        $("#nav-baiVietSv-tab").on("click", initDanhSachBaiVietSinhVien);
        $("#nav-donViSv-tab").on("click", initTrangDonViCuaToi);
    }
    return {
        initTrang: initTrang ,
        initTrangChiTietSinhVien: initTrangChiTietSinhVien
    }
    
}(QuanLyTrangCaNhanService, QuanLySinhVienService, QuanLySinhVienController);
var CacTrangCaNhanController = function (cacTrangCaNhanService) {
    //Trang hiển thị lớp của sinh viên
    var themLopCuaToiVaoDom = function (danhSachLop) {
        //Lấy template cho hoạt động
        var cardLop_Template = _.template($("#cardLop_Template").html());
        //Thêm hoạt động đang tham gia
        danhSachLop.forEach(function (lop) {
            $("#danhSachLopCuaToi").append(cardLop_Template({ lop: lop }));
        });
    }
    var initTrangLopCuaToi = function() {
        cacTrangCaNhanService.layDanhSachLopCuaToi(themLopCuaToiVaoDom);
    }
    //Trang hiển thị đơn vị của sinh viên
    var themDonViCuaToiVaoDom = function (danhSachLop) {
        //Lấy template cho hoạt động
        var cardDonVi_Template = _.template($("#cardDonVi_Template").html());
        //Thêm hoạt động đang tham gia
        danhSachLop.forEach(function (donVi) {
            $("#danhSachDonViCuaToi").append(cardDonVi_Template({ donVi: donVi }));
        });
    }
    var initTrangDonViCuaToi = function () {
        cacTrangCaNhanService.layDanhSachDonViCuaToi(themDonViCuaToiVaoDom);
    }
return {
    initTrangLopCuaToi: initTrangLopCuaToi,
    initTrangDonViCuaToi: initTrangDonViCuaToi
}
}(CacTrangCaNhanService);

