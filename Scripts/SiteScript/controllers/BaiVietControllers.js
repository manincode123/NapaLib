var updateNav = function (chuyenMucId) {
    var nav = $('#nav' + chuyenMucId);
    var navwidth = 0;
    var themButtonWidth = nav.find('.chuyenMucCon .themButton').outerWidth(true);
    var margin = 10;

    $('#nav' + chuyenMucId + ' .chuyenMucCon > a:not(.themButton)').each(function () {
        navwidth += $(this).outerWidth(true);
    });
    //var availablespace = $('nav').outerWidth(true) - morewidth;
    var availablespace = nav.width()
        - nav.find(' .chuyenMuc').width() - themButtonWidth - margin;

    if (navwidth > availablespace) {
        var lastItem = nav.find(' .chuyenMucCon > a:not(.themButton)').last();
        lastItem.attr('data-width', lastItem.outerWidth(true));
        lastItem.wrap('<li></li>').parent().appendTo(nav.find(' .chuyenMucCon ul'));
        updateNav(chuyenMucId);
    }

    if (nav.find(' .chuyenMucCon li').length > 0) {
        nav.find(' .themButton').css('display', 'inline');
        nav.find(' .themButton').addClass('tenChuyenMucCon');
    } else {
        $('#nav' + chuyenMucId + ' .themButton').css('display', 'none');
        $('#nav' + chuyenMucId + ' .themButton').removeClass('tenChuyenMucCon');
    }
}

var SaveBaiVietController = function (saveBaiVietService) {
    var danhSachLop = [];
    var danhSachDonVi = [];
    var danhSachHoatDong = [];
    var baiVietDto = {};

    var insertDonViToList = function (danhSachLopData, danhSachDonViData) {
        danhSachLop = [];
        danhSachDonVi = [];
        danhSachLopData.forEach(function (lop) {
            //Add object vào list
            danhSachLop.push(lop);
        });
        danhSachDonViData.forEach(function (donVi) {
            //Add object vào list
            danhSachDonVi.push(donVi);
        });
    }
    var initDonViSelectList = function (danhSachLopData, danhSachDonViData) {
        insertDonViToList(danhSachLopData, danhSachDonViData);
        $('#tagDonVi').select2({
            language: "vi",
            data: danhSachDonVi
        });
        $('#tagLop').select2({
            language: "vi",
            data: danhSachLop
        });
    }
    var initChuyenMucSelectList = function(danhSachChuyenMuc) {
        $('#chuyenMuc').select2({
            language: "vi",
            data: danhSachChuyenMuc,
            escapeMarkup: function (text) { return text; }
        });
    }
    var initSelectList = function () {
        saveBaiVietService.getDonViData(initDonViSelectList);
        saveBaiVietService.getChuyenMucData(initChuyenMucSelectList);
    }
    var limitCharacter = function () {
        //Hạn chế số kí tự tên bài viết
        var tenBaiVietMax = 100;
        $('#tenBaiVietLimit').html('Còn ' + tenBaiVietMax + ' kí tự.'); //Init số kí tự ban đầu
        $('#tenBaiViet').keyup(function () {
            var soKiTuHienTai = $('#tenBaiViet').val().length;
            var soKiTuCon = tenBaiVietMax - soKiTuHienTai;
            $('#tenBaiVietLimit').html('Còn '+ soKiTuCon + ' kí tự.');
        });

        //Hạn chế số kí tự phần sơ lược
        var soLuocMax = 150;
        $('#soLuocLimit').html('Còn ' + soLuocMax + ' kí tự.');  //Init số kí tự ban đầu
        $('#soLuoc').keyup(function() {
            var soKiTuHienTai = $('#soLuoc').val().length;
            var soKiTuCon = soLuocMax - soKiTuHienTai;
            $('#soLuocLimit').html('Còn ' + soKiTuCon + ' kí tự.');
        });
    }
    var initTextEditor = function () {
        CKEDITOR.replace("NoiDung");
        CKFinder.setupCKEditor(null, '/ckfinder');
    }
    var initChonAnh = function () {
        $("#selectAnhBiaButton").on("click", function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $("#anhBia").attr("src", fileUrl);
            }
            finder.popup();
        });
    }
    var tagHoatDong = function (e, hoatDong) {
        if (danhSachHoatDong.includes(hoatDong.id)) {
            alert("Hoạt động đã được tag.");
            $("#tagHoatDong").typeahead("val", "");
            return;
        }
        var compiled = _.template($("#hoatDongTemplate").html());
        var html = compiled({
            anhBia: hoatDong.anhBia ,
            tenHoatDong: taoLinkHoatDong(hoatDong.tenHoatDong, hoatDong.id),
            thoiGianBatDau: moment(hoatDong.ngayBatDau).format("DD/MM/YYYY HH:mm"),
            thoiGianKetThuc: moment(hoatDong.ngayKetThuc).format("DD/MM/YYYY HH:mm"),
            diaDiem: hoatDong.diaDiem,
            idHoatDong : hoatDong.id
        });
        $("#DanhSachHoatDong").append(html);
        danhSachHoatDong.push(hoatDong.id);
        $("#tagHoatDong").typeahead("val", "");
    }
    var boTagHoatDong = function (e) {
        var span = $(e.target).closest("span");
        var idHoatDong = parseInt(span.attr("id"));
        var index = danhSachHoatDong.indexOf(idHoatDong);
        danhSachHoatDong.splice(index, 1);
        span.parent().parent().remove();
    }
    var mapBaiViet = function () {
        baiVietDto.id = $("#baiVietId").val();
        baiVietDto.noiDungBaiViet =  CKEDITOR.instances['NoiDung'].getData();
        baiVietDto.tenBaiViet = $("#tenBaiViet").val();
        baiVietDto.soLuoc = $('#soLuoc').val();
        baiVietDto.anhBia = $("#anhBia").attr("src");
        baiVietDto.chuyenMucBaiVietId = $("#chuyenMuc").val();
        baiVietDto.danhSachHoatDongTag = danhSachHoatDong;
        baiVietDto.danhSachLopTag = returnIntArray($("#tagLop").val());
        baiVietDto.danhSachDonViTag = returnIntArray($("#tagDonVi").val());
    }
    var xemTruocBaiViet = function() {
        $("#xemTruocBaiVietModal-soLuoc").html($('#soLuoc').val());
        $("#xemTruocBaiVietModal-noiDung").html(CKEDITOR.instances['NoiDung'].getData());
        $("#xemTruocBaiVietModal-title").html($("#tenBaiViet").val());
        $("#xemTruocBaiVietModal").modal("show");
    }
    var updateAfterThem = function () {
        hideLoader();
        alert("Đã thêm bài viết");
        window.location.replace("/BaiViet/CaNhan");
    }
    var themBaiViet = function () {
        if ($("#baiVietForm").valid()) {
            showLoader();
            mapBaiViet();
            saveBaiVietService.themBaiViet(baiVietDto, updateAfterThem);
        } else {
            alert("Hãy nhập đúng những thông tin cần thiết.");
        }
    }
    var initTypeahead = function () {
        var danhSachHoatDong4TypeAhead = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('tenHoatDong', 'diaDiem'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/api/HoatDong/TimKiem?searchTerm=%QUERY',
                wildcard: '%QUERY'
            }
        });
        $('#tagHoatDong').typeahead(
                {
                    highlight: true,
                    classNames: {
                        suggestion: 'typeahead-suggestion-BaiViet',
                        hint: 'typeahead-hint-BaiViet',
                        selectable: 'typeahead-selectable-BaiViet',
                        menu: 'typeahead-menu-BaiViet'
                    }
                },
                {
                    name: 'hoatDong',
                    source: danhSachHoatDong4TypeAhead,
                    display: function (data) {
                        return data.tenHoatDong;
                    },
                    templates: {
                        suggestion: function (data) {
                            return '<div><img src="' + data.anhBia + '" class="anhBiaHoatDongTypeAhead"/><span><strong>' + data.tenHoatDong + '</strong><div>'
                                + moment(data.ngayBatDau).format("DD/MM/YYYY HH:mm") + ' - ' + moment(data.ngayKetThuc).format("DD/MM/YYYY HH:mm") + '</div><div>' + data.diaDiem + '</div></span></div>';
                        }
                    }

                })
            .on("typeahead:autocomplete", tagHoatDong)
            .on("typeahead:select", tagHoatDong)
            .on("typeahead:change",
                function () {
                    //vm.tacGiaId = temp;
                    //temp = 0;
                });
    }
    var validateForm = function() {
        $("#baiVietForm").validate({
            ignore:[],
            rules: {
                tenBaiViet: "required",
                soLuoc: "required",
                NoiDung: {
                    required: function (textarea) {
                        CKEDITOR.instances[textarea.id].updateElement(); // update textarea
                        var editorcontent = textarea.value.replace(/<[^>]*>/gi, ''); // strip tags
                        return editorcontent.length === 0;
                    }
                }
            },
            messages: {
                tenBaiViet: "Vui lòng nhập tên bài viết",
                soLuoc: "Vui lòng nhập sơ lược bài viết",
                NoiDung: "Vui lòng nhập nội dung bài viết"
            }
        });
    }
    var initTrang = function () {
        initTextEditor();
        initChonAnh();
        limitCharacter();
        initTypeahead();
        initSelectList();
        validateForm();
        $("body").on("click", ".buttonHolder-TrangThemBaiViet", boTagHoatDong);
        $("#xemTruocBaiVietButton").on("click",xemTruocBaiViet);
    }
    var initTrangThemBaiViet = function () {
        initTrang();
        $("#luuBaiVietButton").on("click", themBaiViet);
    }

    //Phần chỉnh sửa                                        
    var selectLop = function (lopTag) {
        var danhSachLopTag = [];
        lopTag.forEach(function (lop) {
            danhSachLopTag.push(lop.id);
        });
        $("#tagLop").val(danhSachLopTag);
        $("#tagLop").trigger('change');
    }
    var selectDonVi = function (donViTag) {
        var danhSachDonViTag = [];
        donViTag.forEach(function (donVi) {
            danhSachDonViTag.push(donVi.id);
        });
        $("#tagDonVi").val(danhSachDonViTag);
        $("#tagDonVi").trigger('change');
    }
    var showHoatDongDaTag = function (hoatDongTag) {
        hoatDongTag.forEach(function (hoatDong) {
            var compiled = _.template($("#hoatDongTemplate").html());
            var html = compiled({
                anhBia: hoatDong.anhBia,
                tenHoatDong: taoLinkHoatDong(hoatDong.tenHoatDong, hoatDong.id),
                thoiGianBatDau: moment(hoatDong.ngayBatDau).format("DD/MM/YYYY HH:mm"),
                thoiGianKetThuc: moment(hoatDong.ngayKetThuc).format("DD/MM/YYYY HH:mm"),
                diaDiem: hoatDong.diaDiem,
                idHoatDong: hoatDong.id
            });
            $("#DanhSachHoatDong").append(html);
            danhSachHoatDong.push(hoatDong.id);
        });
    }
    var setTenBaiViet = function (tenBaiViet) {
        var tenBaiVietMax = 100;
        $("#tenBaiViet").val(tenBaiViet);
        var soKiTuHienTai = $('#tenBaiViet').val().length;
        var soKiTuCon = tenBaiVietMax - soKiTuHienTai;
        $('#tenBaiVietLimit').html('Còn ' + soKiTuCon + ' kí tự.');
    }
    var setSoLuocBaiViet = function (soLuoc) {
        var soLuocMax = 150;
        $('#soLuoc').val(soLuoc);
        var soKiTuHienTai = $('#soLuoc').val().length;
        var soKiTuCon = soLuocMax - soKiTuHienTai;
        $('#soLuocLimit').html('Còn ' + soKiTuCon + ' kí tự.');
    }
    var setChuyenMuc = function (chuyenMucBaiVietId) {
        $("#chuyenMuc").val(chuyenMucBaiVietId);
        $("#chuyenMuc").trigger("change");
    }
    var updatePage = function (baiViet) {
        $("#baiVietId").val(baiViet.id);
        CKEDITOR.instances['NoiDung'].setData(baiViet.noiDungBaiViet);
        setTenBaiViet(baiViet.tenBaiViet);
        setSoLuocBaiViet(baiViet.soLuoc);
        $("#anhBia").attr("src",baiViet.anhBia);
        setChuyenMuc(baiViet.chuyenMucBaiVietId);
        selectLop(baiViet.lopTag);
        selectDonVi(baiViet.donViTag);
        showHoatDongDaTag(baiViet.hoatDongTag);
    }
    var updateAfterChinhSua = function(baiVietId) {
        hideLoader();
        alert("Đã chỉnh sửa bài viết");
        window.location.replace("/BaiViet/" + baiVietId);
    }
    var chinhSuaBaiViet = function () {
        if ($("#baiVietForm").valid()) {
            showLoader();
            mapBaiViet();
            saveBaiVietService.chinhSuaBaiViet(baiVietDto, updateAfterChinhSua);
        } else {
            alert("Hãy nhập đúng những thông tin cần thiết.");
        }
    }
    var initTrangSuaBaiViet = function(baiVietId) {
        initTrang();
        saveBaiVietService.getBaiViet(baiVietId, updatePage);
        $("#luuBaiVietButton").on("click", chinhSuaBaiViet);
    }

    return{
        initTrangThemBaiViet: initTrangThemBaiViet,
        initTrangSuaBaiViet: initTrangSuaBaiViet
    }
}(SaveBaiVietService);

var XemBaiVietController = function (xemBaiVietService) {

    var showHoatDongDaTag = function (hoatDongTag) {
        if (hoatDongTag.length === 0) {
            $("#DanhSachHoatDong").html("Không có hoạt động liên quan.<hr>");
            return;
        }
        hoatDongTag.forEach(function (hoatDong) {
            var compiled = _.template($("#hoatDongTemplate").html());
            var html = compiled({
                anhBia: hoatDong.anhBia,
                tenHoatDong: taoLinkHoatDong(hoatDong.tenHoatDong, hoatDong.id),
                thoiGianBatDau: moment(hoatDong.ngayBatDau).format("DD/MM/YYYY HH:mm"),
                thoiGianKetThuc: moment(hoatDong.ngayKetThuc).format("DD/MM/YYYY HH:mm"),
                diaDiem: hoatDong.diaDiem,
                idHoatDong: hoatDong.id
            });
            $("#DanhSachHoatDong").append(html);
        });
    }                                                                                
    var returnNguoiVaThoiGianDang = function (nguoiTao, ngayTao) {
        var tenNguoiTao = '<a href="/BaiViet/TacGia/' + nguoiTao.id + '" class="tenSvDs"><strong>'
                          + returnTenSinhVien(nguoiTao) + " </strong></a>";
        var html = tenNguoiTao+ " đăng lúc " + moment(ngayTao).format("HH:mm DD/MM/YYYY");
        return html;
    }
    var returnDonViDuocTagForDom= function (baiViet) {
        var object = "";
        baiViet.donViTag.forEach(function (donVi) {
            object += '<a class="donViTag" href="/BaiViet/DonVi/' + donVi.id + '">' + donVi.tenDonVi + '</a>';
        });
        baiViet.lopTag.forEach(function (lop) {
            object += '<a class="donViTag" href="/BaiViet/Lop/' + lop.id + '">' + lop.tenLop + '</a>';
        });
        return object;
    }
    var updateDom = function(baiVietDto) {
        showHoatDongDaTag(baiVietDto.hoatDongTag);
        document.title = baiVietDto.tenBaiViet + " | NAPASTUDENT";
        $("#chuyenMucBaiViet").html(baiVietDto.tenChuyenMuc);
        $("#chuyenMucBaiViet").attr("href","/BaiViet/ChuyenMuc/"+ baiVietDto.chuyenMucBaiVietId);
        $("#tenBaiViet-TrangBaiViet").html(baiVietDto.tenBaiViet);
        $("#soLuoc-TrangBaiViet").html(baiVietDto.soLuoc);
        $("#noiDungBaiViet").html(baiVietDto.noiDungBaiViet);
        $("#ngayTaoBaoViet").html(returnNguoiVaThoiGianDang(baiVietDto.nguoiTao,baiVietDto.ngayTao));
        $("#DanhSachDonVi").html(returnDonViDuocTagForDom(baiVietDto));
    }
    var getBaiViet = function (baiVietId) {
        xemBaiVietService.getBaiViet(baiVietId, updateDom);
    }
    var initTrang = function (baiVietId) {
        getBaiViet(baiVietId);
    }
   return {
       initTrang: initTrang,
       getBaiViet: getBaiViet
   }
    
}(XemBaiVietService);

var PheDuyetBaiVietController = function (pheDuyetBaiVietService) {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var baiVietPheDuyetTable;
    var baiVietId;
    var initbaiVietPheDuyetTable = function () {

        baiVietPheDuyetTable = $("#baiVietPheDuyet").DataTable({
            ajax: {
                url: "/api/BaiViet/DanhSachPheDuyet",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ''
            },
            "order": [[1, 'desc']],
            columns: [
                { data: 'anhBia' },
                { data: 'tenBaiViet' },
                { data: 'soLuoc' },
                { data: 'tenChuyenMuc' },
                { data: 'ngayTao' },
                { data: 'nguoiTao' },
                { data: 'id' }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 150,
                    render: function (data, type, row) {
                        return '<img src="' + data + '" class="img-fluid"/>';
                    }
                },
                {
                    targets: 1,
                    width: 300
                },
                {
                    targets: 2,
                    width: 300
                },
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                },
                {
                    targets: 5,
                    render: function (data, type, row) {
                        return data.tenNguoiTao;
                    }
                },
                {
                    targets: 6,
                    render: function (data, type, row) {
                        return '<button class="btn btn-primary xemBaiViet-js">Xem bài viết</button>' +
                               '<button class="btn btn-success pheDuyet-js">Phê duyệt</button>' +
                               '<button class="btn btn-danger xoa-js">Xóa</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var hienThiModalPheDuyetBaiViet = function (e) {
        var button = $(e.target);
        baiVietId = button.closest("tr").attr("id");
        $(".modal-title").html("Phê duyệt bài viết");
        $("#pheDuyetModal-body").html("Bạn có chắn chắn muốn phê duyệt bài viết này?");
        $("#pheDuyetBaiViet").css("display", "inline-block");
        $("#xoaBaiViet").css("display", "none");
        $("#chinhSuaBaiViet").css("display", "none");
        $(".modal-dialog").removeClass("modal-lg");
        $("#pheDuyetModal").modal("show");
    }
    var hienThiModalXoaBaiViet = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        baiVietId = button.closest("tr").attr("id");
        $(".modal-title").html("Phê duyệt bài viết");
        $("#pheDuyetModal-body").html("Bạn có chắn chắn muốn xóa bài viết này?");
        $("#pheDuyetBaiViet").css("display", "none");
        $("#chinhSuaBaiViet").css("display", "none");
        $("#xoaBaiViet").css("display", "inline-block");
        $(".modal-dialog").removeClass("modal-lg");
        $("#pheDuyetModal").modal("show");
    }
    var appendBaiVietToModal = function (data) {
        $(".modal-title").html("Xem trước bài viết");
        $(".modal-dialog").addClass("modal-lg");
        $("#pheDuyetModal-body").html(data);
        XemBaiVietController.getBaiViet(baiVietId);
        $("#pheDuyetBaiViet").css("display", "none");
        $("#xoaBaiViet").css("display", "none");
        $("#pheDuyetModal").modal("show");
    }
    var hienThiModalBaiViet = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        baiVietId = button.closest("tr").attr("id");
        pheDuyetBaiVietService.layBaiViet(appendBaiVietToModal);
    }
    var updatePage = function () {
        baiVietPheDuyetTable.ajax.reload();
        $("#pheDuyetModal").modal("hide");
    }
    var pheDuyetBaiViet = function () {
        pheDuyetBaiVietService.pheDuyetBaiViet(baiVietId, updatePage);
    }
    var xoaBaiViet = function () {
        pheDuyetBaiVietService.xoaBaiViet(baiVietId, updatePage);
    }
    var chuyenSangTrangChinhSua = function() {
        window.location.href = "/BaiViet/SuaBaiViet/" + baiVietId;
    }

    var initPage = function () {
        dataTableSetting();
        initbaiVietPheDuyetTable();
        $("body").on("click", ".xemBaiViet-js", hienThiModalBaiViet);
        $("#chinhSuaBaiViet").on("click", chuyenSangTrangChinhSua);
        $("body").on("click", ".pheDuyet-js", hienThiModalPheDuyetBaiViet);
        $("#pheDuyetBaiViet").on("click", pheDuyetBaiViet);
        $("body").on("click", ".xoa-js", hienThiModalXoaBaiViet);
        $("#xoaBaiViet").on("click", xoaBaiViet);
    }
    return{
        initPage : initPage
    }
}(PheDuyetBaiVietService);

var QuanLyChuyenMucController = function (quanLyChuyenMucService) {
    var quanLyChuyenMucTable;
    var chuyenMucId;
    var danhSachChuyenMuc =[];
    var tenChuyenMucMax, moTaMax;
    var chuyenMucDto={};
    var csrfToken = $("input[name='__RequestVerificationToken']").val();

    var initQuanLyChuyenMucTable = function () {
        quanLyChuyenMucTable = $("#quanLyChuyenMuc").DataTable({
            ajax: {
                url: "/api/ChuyenMuc/DanhSachQuanLyChuyenMuc",
                headers: { __RequestVerificationToken: csrfToken },
                dataSrc: ""
            },
            "order": [[3, 'asc']],
            columns: [
                { data: "anhBia" },
                { data: "tenChuyenMuc" },
                { data: "moTa" },
                { data: "chuyenMucCha" },
                { data: "soBaiViet" },
                { data: "id" }
            ],
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    width: 150,
                    render: function (data, type, row) {
                        return '<img src="' + data + '" class="img-fluid"/>';
                    }
                },
                {
                    targets: 1,
                    width: 200,
                    className: "tenChuyenMuc",
                    render: function(data, type, row) {
                        return '<a href="/BaiViet/ChuyenMuc/' + row.id + '" class="link">' + data + '</a>';
                    }
                },
                {
                    targets: 2,
                    width: 300
                },
                {
                    targets: 3,
                    render: function (data, type, row) {
                        return data.tenChuyenMuc;
                    },
                    width: 200
                },
                {
                    targets: 4,
                    width: 50
                },
                {
                    targets: 5,
                    width: 150,
                    render: function () {
                        return '<button class="btn btn-primary chinhSuaChuyenMuc-js">Chỉnh sửa</button>' +
                               '<button class="btn btn-danger xoaChuyenMuc-js">Xóa</button>';
                    }
                }
            ],
            rowCallback: function (row, data) {

            }
        });
    }
    var initChonAnh = function () {
        $("#selectAnhBiaButton").on("click", function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $("#anhBia").attr("src", fileUrl);
            }
            finder.popup();
        });
    }
    var limitCharacter = function () {
        //Hạn chế số kí tự tên chuyên mục
        tenChuyenMucMax = $("#tenChuyenMuc").attr("maxlength");
        $("#tenChuyenMucLimit").html("Còn " + tenChuyenMucMax + " kí tự."); //Init số kí tự ban đầu
        $("#tenChuyenMuc").keyup(function () {
            var soKiTuHienTai = $('#tenChuyenMuc').val().length;
            var soKiTuCon = tenChuyenMucMax - soKiTuHienTai;
            $('#tenChuyenMucLimit').html('Còn ' + soKiTuCon + ' kí tự.');
        });

        //Hạn chế số kí tự phần mô tả
        moTaMax = $("#moTa").attr("maxlength");;
        $("#moTaLimit").html("Còn " + moTaMax + " kí tự.");  //Init số kí tự ban đầu
        $("#moTa").keyup(function () {
            var soKiTuHienTai = $('#moTa').val().length;
            var soKiTuCon = moTaMax - soKiTuHienTai;
            $('#moTaLimit').html('Còn ' + soKiTuCon + ' kí tự.');
        });
    }
    var initSelectList = function (data) {
        danhSachChuyenMuc = data;
        $('#chuyenMucCha').select2({
            language: "vi",
            data: danhSachChuyenMuc,
            escapeMarkup: function (text) { return text; }
        });
    }
    var setTenChuyenMuc = function (tenChuyenMuc) {
        $("#tenChuyenMuc").val(tenChuyenMuc);
        var soKiTuHienTai = tenChuyenMuc.length;
        var soKiTuCon = tenChuyenMucMax - soKiTuHienTai;
        $('#tenChuyenMucLimit').html('Còn ' + soKiTuCon + ' kí tự.');

    }
    var setMoTa = function (moTa) {
        $("#moTa").val(moTa);
        var soKiTuHienTai = moTa.length;
        var soKiTuCon = moTaMax - soKiTuHienTai;
        $('#moTaLimit').html('Còn ' + soKiTuCon + ' kí tự.');
    }
    var setChuyenMucCha = function (chuyenMucChaId) {
        if (chuyenMucChaId == null) chuyenMucChaId = 0;
        $("#chuyenMucCha").val(chuyenMucChaId);
        $("#chuyenMucCha").trigger("change");
    }
    var setAnhBia = function (anhBia) {
        if (anhBia !== "") {
            $("#anhBia").attr("src", anhBia);
        }
    }
    var updateInput = function (chuyenMuc) {
        setTenChuyenMuc(chuyenMuc.tenChuyenMuc);
        setMoTa(chuyenMuc.moTa);
        setChuyenMucCha(chuyenMuc.chuyenMucChaId);
        setAnhBia(chuyenMuc.anhBia);
        $("#chuyenMucId").val(chuyenMucId);
    }
    var hienThiSuaChuyenMucModal = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        chuyenMucId = button.closest("tr").attr("id");
        $(".modal-title").html("Chỉnh sửa chuyên mục");
        $("#chinhSuaChuyenMuc").html("Chỉnh sửa");
        quanLyChuyenMucService.layChuyenMucSelectList(chuyenMucId,initSelectList);
        quanLyChuyenMucService.layChuyenMuc(chuyenMucId, updateInput);
        $("#xoaChuyenMuc").css("display", "none");
        $("#quanLyChuyenMucModal").modal("show");
    }
    var hienThiThemChuyenMucModal = function () {
        chuyenMucId = 0;
        $(".modal-title").html("Thêm chuyên mục mới");
        $("#chinhSuaChuyenMuc").html("Thêm chuyên mục");
        quanLyChuyenMucService.layChuyenMucSelectList(chuyenMucId, initSelectList);
        var chuyenMuc = {
            tenChuyenMuc: "",
            moTa: "",
            anhBia: ""
        }
        updateInput(chuyenMuc);
        $("#xoaChuyenMuc").css("display", "none");
        $("#quanLyChuyenMucModal").modal("show");
    }
    var mapChuyenMucObject = function() {
        chuyenMucDto.id = $("#chuyenMucId").val();
        chuyenMucDto.tenChuyenMuc = $("#tenChuyenMuc").val();
        chuyenMucDto.moTa = $("#moTa").val();
        chuyenMucDto.anhBia = $("#anhBia").attr("src");
        chuyenMucDto.chuyenMucChaId = $("#chuyenMucCha").val();
    }
    var updateDom = function () {
        hideLoader();
        quanLyChuyenMucTable.ajax.reload();
        if (chuyenMucId === 0)  alert("Đã thêm chuyên mục"); 
        else  alert("Đã chỉnh sửa chuyên mục"); 
        $("#quanLyChuyenMucModal").modal("hide");
    }
    var saveChuyenMuc = function () {
        if ($("#chuyenMucForm").valid()) {
            showLoader();
            mapChuyenMucObject();
            quanLyChuyenMucService.saveChuyeMuc(chuyenMucDto, updateDom);
        } else {
            alert("Hãy nhập đúng những thông tin cần thiết.");
        }
    }
    var initializeXEditable = function () {
        var tenChuyenMucEdit = function () {
            var idChuyenMuc;
            var value2Send;
            $(".tenChuyenMuc").editable({
                type: 'text',
                mode: "inline",
                showbuttons: false,
                pk: function () {
                    idChuyenMuc = $(this).closest("tr").attr("id");
                    return idChuyenMuc;
                },
                url: '/api/ChuyenMuc/SaveChuyenMuc',
                success: function () {
                    quanLyChuyenMucTable.ajax.reload();
                },
                params: {
                    id: function () {
                        return parseInt(idChuyenMuc);
                    },
                    tenChuyenMuc: function () {
                        return value2Send;
                    }
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var soLuocChuyenMucEdit = function () {
            var idChuyenMuc, value2Send;
            $(".kyHieuMonLoai").editable({
                type: 'text',
                mode: "inline",
                showbuttons: false,
                pk: function () {
                    idChuyenMuc = $(this).closest("tr").attr("id");
                    return idChuyenMuc;

                },
                url: '/api/MonLoai/SuaMonLoai',
                success: function () {
                    danh_sach_mon_loai.ajax.reload();
                },
                params: {
                    id: function () {
                        return parseInt(idChuyenMuc);
                    },
                    soLuoc: function () {
                        return value2Send;
                    }
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                }
            });
        }
        var chuyenMucChaEdit = function () {
            var idChuyenMuc;
            var value2Send;

            $(".chuyenMucCha").editable({
                type: 'select',
                mode: "inline",
                pk: function () {
                    idChuyenMuc = $(this).closest("tr").attr("id");
                    return idChuyenMuc;
                },
                source: layDanhSachChuyenMuc,
                url: '/api/ChuyenMuc/SuaChuyenMuc',
                params: {
                    id: function () {
                        return parseInt(idChuyenMuc);
                    },
                    chuyenMucChaId: function () {
                        return value2Send;
                    }
                },
                validate: function (value) {
                    if ($.trim(value) == '') {
                        return 'Ô này không được để trống';
                    }
                    value2Send = value;
                },
                success: function () {
                    danh_sach_mon_loai.ajax.reload();
                }
            });
        }

        chuyenMucChaEdit();

    }
    var validateForm = function () {
        $("#chuyenMucForm").validate({
            rules: {
                tenChuyenMuc: "required",
                moTa: "required"
            },
            messages: {
                tenChuyenMuc: "Vui lòng nhập tên chuyên mục",
                moTa: "Vui lòng nhập mô tả chuyên mục"
            }
        });
    }
    var initPage = function() {
        dataTableSetting();
        initQuanLyChuyenMucTable();
        limitCharacter();
        initChonAnh();
        validateForm();
        $("body").on("click", ".chinhSuaChuyenMuc-js", hienThiSuaChuyenMucModal);
        $("#themChuyenMuc").on("click", hienThiThemChuyenMucModal);
        $("#chinhSuaChuyenMuc").on("click", saveChuyenMuc);
    }

    return {
        initPage :initPage
    }
}(QuanLyChuyenMucService);

var QuanLyBaiVietController = function (quanLyBaiVietService) {
    var quanLyBaiVietTable, baiVietCaNhanTable;
    var baiVietId;
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    //Func chung
    var hienThiModalXoaBaiViet = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        baiVietId = button.closest("tr").attr("id");
        $(".modal-title").html("Phê duyệt bài viết");
        $("#quanLyBaiVietModal-body").html("Bạn có chắn chắn muốn xóa bài viết này?");
        $("#xoaBaiViet").css("display", "inline-block");
        $(".modal-dialog").removeClass("modal-lg");
        $("#quanLyBaiVietModal").modal("show");
    }
    var appendBaiVietToModal = function (data) {
        $(".modal-title").html("Xem trước bài viết");
        $(".modal-dialog").addClass("modal-lg");
        $("#quanLyBaiVietModal-body").html(data);
        XemBaiVietController.getBaiViet(baiVietId);
        $("#xoaBaiViet").css("display", "none");
        $("#quanLyBaiVietModal").modal("show");
        hideLoader();
    }
    var hienThiModalBaiViet = function (e) {
        showLoader();
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        baiVietId = button.closest("tr").attr("id");
        quanLyBaiVietService.layBaiViet(appendBaiVietToModal);
    }
    var chuyenSangTrangChinhSua = function (e) {
        var button = $(e.target);
        //Lấy id hoạt động muốn xóa
        baiVietId = button.closest("tr").attr("id");
        window.location.href = "/BaiViet/SuaBaiViet/" + baiVietId;
    }
    var quanLyBaiVietFunction = function () {
        //Có function này để controller QuanLyBaiVietCaNhanController có thể sử dụng lại
        $("body").on("click", ".xemBaiViet-js", hienThiModalBaiViet);
        $("body").on("click", ".chinhSuaBaiViet-js", chuyenSangTrangChinhSua);
        $("body").on("click", ".xoa-js", hienThiModalXoaBaiViet);

    }
    //Trang quản lý bài viết chung
    var initQuanLyBaiVietTable = function () {

        quanLyBaiVietTable = $("#quanLyBaiViet").DataTable({
            ajax: {
                url: "/api/BaiViet/DanhSachQuanLy",
                type: "POST",
                dataSrc: "data",
                headers: { __RequestVerificationToken: csrfToken },
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
            pageLength: 20,
            lengthMenu: [20, 50, 75, 100],
            "order": [[4, "desc"]],
            columns: [
                { data: "anhBia" },
                { data: "tenBaiViet" },
                { data: "soLuoc" },
                { data: "tenChuyenMuc" },
                { data: "ngayTao" },
                { data: "nguoiTao" },
                { data: "soLuotXem" },
                { data: "" }
            ],
            rowId: "id",
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
                    width: 300,
                    orderable: false
                },
                {
                    targets: 2,
                    width: 300,
                    orderable: false
                },
                {
                    targets: 3,
                    orderable: false
                },
                {
                    targets: 4,
                    name: "NgayTao",
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY HH:mm:ss", "vi")
                },
                {
                    targets: 5,
                    orderable: false,
                    render: function (data, type, row) {
                        return row.hoVaTenLot + " " + row.tenSinhVien;
                    }
                },
                {
                    targets: 6,
                    name: "SoLuotXem"
                },
                {
                    targets: 7,
                    orderable: false,
                    render: function () {
                        return '<button class="btn btn-primary xemBaiViet-js">Xem bài viết</button>' +
                               '<button class="btn btn-success chinhSuaBaiViet-js">Chỉnh sửa</button>' +
                               '<button class="btn btn-danger xoa-js">Xóa</button>';
                    }
                }
            ]
        });
    }
    var updatePage = function () {
        quanLyBaiVietTable.ajax.reload();
        alert("Đã xóa");
        $("#quanLyBaiVietModal").modal("hide");
    }
    var xoaBaiViet = function () {
        quanLyBaiVietService.xoaBaiViet(baiVietId, updatePage);
    }
    var initPage_TrangQuanLy = function () {
        dataTableSetting();
        initQuanLyBaiVietTable();
        $("#xoaBaiViet").on("click", xoaBaiViet);   //Không đặt chung quanLyBaiVietFunction vì bị conflict khi sử dụng ở QuanLyBaiVietCaNhanController
        quanLyBaiVietFunction();
    }
    //Trang quản lý bài viết cá nhân
    var traTinhTrangBaiViet = function (baiViet) {
        if (baiViet.daXoa) return "<b style='color:red'>Đã xóa</b>";
        if (baiViet.duocPheDuyet) return "<b style='color:#39d612'>Được phê duyệt</b>";
        return "<b style='color:#d612b5'>Chờ phê duyệt</b>";
    }
    var initBaiVietCaNhanTable = function () {
        baiVietCaNhanTable = $("#baiVietCaNhan").DataTable({
            ajax: {
                url: "/api/BaiViet/BaiVietCaNhan",
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
            pageLength: 20,
            lengthMenu: [20, 50, 75, 100],
            "order": [[3, "desc"]],
            columns: [
                { data: "anhBia" },
                { data: "tenBaiViet" },
                { data: "tenChuyenMuc" },
                { data: "ngayTao" },
                { data: "soLuotXem" },
                { data: "" },
                { data: "" }
            ],
            rowId: "id",
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
                    width: 300,
                    orderable: false
                },
                {
                    targets: 2,

                    orderable: false
                },
                {
                    targets: 3,
                    name: "NgayTao",
                    render: $.fn.dataTable.render.moment("YYYY-MM-DDTHH:mm:ss", "DD/MM/YYYY HH:mm:ss", "vi")
                },
                {
                    targets: 4,
                    name: "SoLuotXem"
                },
                {
                    targets: 5,
                    orderable: false,
                    render: function (data, type, row) {
                        return traTinhTrangBaiViet(row);
                    }
                },
                {
                    targets: 6,
                    orderable: false,
                    width: 100,
                    render: function () {
                        return '<button class="btn btn-primary xemBaiViet-js">Xem bài viết</button>' +
                            '<button class="btn btn-success chinhSuaBaiViet-js">Chỉnh sửa</button>' +
                            '<button class="btn btn-danger xoa-js">Xóa</button>';
                    }
                }
            ]
        });
    }
    var updatePage_TrangBaiVietCaNhan = function () {
        baiVietCaNhanTable.ajax.reload();
        alert("Đã xóa");
        $("#quanLyBaiVietModal").modal("hide");
    }
    var xoaBaiViet_TrangBaiVietCaNhan = function () {
        quanLyBaiVietService.xoaBaiViet(baiVietId, updatePage_TrangBaiVietCaNhan);
    }
    var initPage_TrangBaiVietCaNhan = function () {
        dataTableSetting();
        initBaiVietCaNhanTable();
        $("#xoaBaiViet").on("click", xoaBaiViet_TrangBaiVietCaNhan);   //Không đặt chung quanLyBaiVietFunction vì bị conflict khi sử dụng ở QuanLyBaiVietCaNhanController
        quanLyBaiVietFunction();
    }
    return {
        initPage_TrangQuanLy: initPage_TrangQuanLy,
        initPage_TrangBaiVietCaNhan: initPage_TrangBaiVietCaNhan
    }
}(QuanLyBaiVietService);

var DanhSachBaiVietController = function (danhSachBaiVietService) {
    var sinhVienId, lopId, donViId, chuyenMucId;
    var recordStart = 0, pageSize = 10;
    var daHetBaiViet;

    var setDaHetBaiViet = function() {
        $(".khongConBaiViet").show();
        daHetBaiViet = true;
    }
    var themBaiViet = function (data) {
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        $(".tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: data }));
        if (data.length < pageSize) setDaHetBaiViet();
        else recordStart += pageSize;
        $(".baiVietLoader").hide();
    }
    //Trang bài viết theo sinh viên
    var initPage_BaiVietSinhVien = function (idSinhVien) {
        sinhVienId = idSinhVien;
        //Lấy bài viết lần đầu
        $(".baiVietLoader").css("display", "block");
        danhSachBaiVietService.layBaiVietSinhVien(sinhVienId, recordStart, themBaiViet);
        //Thêm bài viết nếu kéo xuống cuối trang
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 50){
                if (!daHetBaiViet) {
                    $(".baiVietLoader").css("display", "block");
                    danhSachBaiVietService.layBaiVietSinhVien(sinhVienId, recordStart, themBaiViet);
                }
            }
        });
    }
    //Trang bài viết theo lớp
    var initPage_BaiVietLop = function (idLop) {
        lopId = idLop;
        //Lấy bài viết lần đầu
        $(".baiVietLoader").css("display", "block");
        danhSachBaiVietService.layBaiVietLop(lopId, recordStart, themBaiViet);
        //Thêm bài viết nếu kéo xuống cuối trang
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 50){
                if (!daHetBaiViet) {
                    $(".baiVietLoader").css("display", "block");
                    danhSachBaiVietService.layBaiVietLop(lopId, recordStart, themBaiViet);
                }
            }
        });
    }
    //Trang bài viết theo đơn vị
    var initPage_BaiVietDonVi = function (idDonVi) {
        donViId = idDonVi;
        //Lấy bài viết lần đầu
        $(".baiVietLoader").css("display", "block");
        danhSachBaiVietService.layBaiVietDonVi(donViId, recordStart, themBaiViet);
        //Thêm bài viết nếu kéo xuống cuối trang
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 50){
                if (!daHetBaiViet) {
                    $(".baiVietLoader").css("display", "block");
                    danhSachBaiVietService.layBaiVietDonVi(donViId, recordStart, themBaiViet);
                }
            }
        });
    }
    //Trang bài viết chuyên mục
    var updateDom_BaiVietChuyenMuc = function (data) {
        var chuyenMuc = _.template($("#chuyenMucCon_Template").html());
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        //Lưu ý data.chuyenMucDto[0] không phải là dành cho chuyên mục con mà dành cho tất cả bài viết thuộc chuyên mục (xem api)
            //Nếu không có chuyên mục con
        if (data.chuyenMucDto.length === 1) {
            $(".chuyenMucWrap").append("<div>Không có chuyên mục con.</div>");
            $(".tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: data.chuyenMucDto[0].baiViet }));
        }
            //Nếu có chuyên mục con
        else { 
            data.chuyenMucDto.forEach(function (chuyenMucCon, index) {
                if (index === 0) {
                    $(".tinMoiWrapper").append(baiVietMoi({ danhSachBaiViet: chuyenMucCon.baiViet }));
                } else {
                    $(".chuyenMucWrap").append(chuyenMuc({ chuyenMuc: chuyenMucCon }));
                    updateNav(chuyenMucCon.id);
                }

            });
        }
        //Nếu số bài viết >= 10, nghĩa là còn bài viết, lần tới sẽ lấy 10 bài viết tiếp theo
        if (data.chuyenMucDto[0].baiViet.length < 10) setDaHetBaiViet();
        else {
            recordStart += pageSize;
        }
        //Set tên chuyên mục
        $("#tenChuyenMuc").html(data.tenChuyenMucGoc);
        //Set tên chuyên mục cha (nếu có)
        if (data.chuyenMucChaId !== 0) {
            $("#chuyenMucCha").html(data.tenChuyenMucCha);
            $("#chuyenMucCha").attr("href", "/BaiViet/ChuyenMuc/" + data.chuyenMucChaId);
            $("#chuyenMucCha").parent().css("display", "block");
        }
        //Set title
        document.title = "Chuyên mục: " + data.tenChuyenMucGoc + " - NAPASTUDENT";
    }
    var initPage_BaiVietChuyenMuc = function (idChuyenMuc) {
        chuyenMucId = idChuyenMuc;
        //Tạo dữ liệu lần đầu cho chuyên mục
        danhSachBaiVietService.layChuyenMucData(chuyenMucId, updateDom_BaiVietChuyenMuc);
        //Thêm bài viết nếu kéo xuống cuối trang
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 50) {
                if (!daHetBaiViet) {
                    $(".baiVietLoader").css("display", "block");
                    danhSachBaiVietService.layBaiVietChuyenMuc(chuyenMucId, recordStart, themBaiViet);
                }
            }
        });
    }
    //Trang bài viết trang chủ
    var updateDom_TrangChu = function (data) {
        //Cái chuyenMuc có data[0] ko phải là chuyên mục thực sự mà là tổng hợp các bài viết nổi bật
        var featured = _.template($("#FeaturedArticle_Template").html());
        $(".featured").append(featured({ chuyenMuc: data[0] }));
        //Cái này là cột bài viết nổi bật bên phải
        var subFeatured = _.template($("#subFeaturedArticle_Template").html());
        $(".sub-featured").append(subFeatured({ chuyenMuc: data[0] }));
        //Lấy template cho chuyên mục 
        var chuyenMuc = _.template($("#chuyenMucCon_Template").html());
        //Lặp qua từng chuyên mục
        data.forEach(function (chuyenMucCon, index) {
            if (index === 0) return;
            $(".chuyenMucWrap").append(chuyenMuc({ chuyenMuc: chuyenMucCon }));
            updateNav(chuyenMucCon.id);
        });
    }
    var initPage_TrangChu = function () {
        danhSachBaiVietService.layThongTinTrangChu(updateDom_TrangChu);
    }

    return {
        initPage_BaiVietSinhVien: initPage_BaiVietSinhVien,
        initPage_BaiVietLop: initPage_BaiVietLop,
        initPage_BaiVietDonVi : initPage_BaiVietDonVi,
        initPage_BaiVietChuyenMuc: initPage_BaiVietChuyenMuc,
        initPage_TrangChu : initPage_TrangChu
    }
}(DanhSachBaiVietService);