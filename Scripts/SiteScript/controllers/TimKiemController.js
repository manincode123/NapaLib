var TimKiemService = function () {
    var csrfToken = $("input[name='__RequestVerificationToken']").val();
    var searchHoatDong = function (pagingSearchingSorting, initKetQuaHoatDong) {
        $.ajax({
            url: "/api/TimKiem/HoatDong",
            data: pagingSearchingSorting,
            type:"POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                initKetQuaHoatDong(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    };
    var searchBaiViet = function (pagingSearchingSorting, initKetQuaBaiViet) {
        $.ajax({
            url: "/api/TimKiem/BaiViet",
            data: pagingSearchingSorting,
            type: "POST",
            headers: { __RequestVerificationToken: csrfToken },
            success: function (data) {
                initKetQuaBaiViet(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    };
    var searchSinhVien = function (pagingSearchingSorting, initKetQuaSinhVien) {
        $.ajax({
            url: "/api/TimKiem/SinhVien",
            headers: { __RequestVerificationToken: csrfToken },
            type: "POST",
            data: pagingSearchingSorting,
            success: function (data) {
                initKetQuaSinhVien(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    };
    var searchDonVi = function (pagingSearchingSorting, initKetQuaDonVi) {
        $.ajax({
            url: "/api/TimKiem/DonVi",
            headers: { __RequestVerificationToken: csrfToken },
            type: "POST",
            data: pagingSearchingSorting,
            success: function (data) {
                initKetQuaDonVi(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    };
    var searchLop = function (pagingSearchingSorting, initKetQuaLop) {
        $.ajax({
            url: "/api/TimKiem/Lop",
            headers: { __RequestVerificationToken: csrfToken },
            type: "POST",
            data: pagingSearchingSorting,
            success: function (data) {
                initKetQuaLop(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    };
return{
    searchHoatDong: searchHoatDong,
    searchBaiViet: searchBaiViet,
    searchSinhVien: searchSinhVien,
    searchDonVi: searchDonVi,
    searchLop: searchLop
}
}();
var TimKiemController = function (timKiemService) {
    var searchTerm;
    var pagingSearchingSorting = {};
    var totalRecordFound = 0;
    var donViPagination = false, lopPagination = false, hoatDongPagination = false, baiVietPagination = false, sinhVienPagination = false;
    //Tìm kiếm hoạt động
    var themHoatDong = function (data) {
        var danhSachHoatDong = _.template($("#hoatDongMoi_Template").html());
        $("#ketQuaTimKiem-HoatDong").empty();
        $("#ketQuaTimKiem-HoatDong").append(danhSachHoatDong({ danhSachHoatDong: data }));
    }
    var updateKetQuaHoatDong = function (result) {
        themHoatDong(result.danhSachHoatDong);
    }
    var initHoatDongPagination = function (pageNumbers) {
        if (pageNumbers <= 1 || hoatDongPagination) return;
       $("#HoatDongPagination").twbsPagination({
            totalPages: pageNumbers,
            visiblePages: 5,
            first: "Đầu",
            last: "Cuối",
            prev: "Trang trước",
            next: "Trang sau",
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                pagingSearchingSorting.pageIndex = page;
                timKiemService.searchHoatDong(pagingSearchingSorting, updateKetQuaHoatDong);
            }
        });
        hoatDongPagination = true;
    }
    var initKetQuaHoatDong = function (result) {
        $("#HoatDong-section").css("display", "block");
        themHoatDong(result.danhSachHoatDong);
        initHoatDongPagination(result.pageNumbers);
        $("#HoatDongRecordNumber").html(result.totalRecords);
        totalRecordFound += result.totalRecords;
        document.title = totalRecordFound + ' kết quả tìm kiếm cho "' + searchTerm + '"';
    }
    //Tìm kiếm bài viết
    var themBaiViet = function (data) {
        var baiVietMoi = _.template($("#baiVietMoi_Template").html());
        $("#ketQuaTimKiem-BaiViet").empty();
        $("#ketQuaTimKiem-BaiViet").append(baiVietMoi({ danhSachBaiViet: data }));
    }
    var updateKetQuaBaiViet = function (result) {
        themBaiViet(result.danhSachBaiViet);
    }
    var initBaiVietPagination = function (pageNumbers) {
        if (pageNumbers <= 1 || baiVietPagination) return;
       $("#BaiVietPagination").twbsPagination({
            totalPages: pageNumbers,
            visiblePages: 5,
            first: "Đầu",
            last: "Cuối",
            prev: "Trang trước",
            next: "Trang sau",
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                pagingSearchingSorting.pageIndex = page;
                timKiemService.searchBaiViet(pagingSearchingSorting, updateKetQuaBaiViet);
            }
        });
        baiVietPagination = true;
    }
    var initKetQuaBaiViet = function (result) {
        $("#BaiViet-section").css("display", "block");
        themBaiViet(result.danhSachBaiViet);
        initBaiVietPagination(result.pageNumbers);
        $("#BaiVietRecordNumber").html(result.totalRecords);
        totalRecordFound += result.totalRecords;
        document.title = totalRecordFound + ' kết quả tìm kiếm cho "' + searchTerm + '"';
    }
    //Tìm kiếm sinh viên
    var themSinhVien = function (danhSachSinhVien) {
        //Lấy template cho sinh viên
        var cardSinhVien_Template = _.template($("#cardSinhVien_Template").html());
        //Xóa các kết quả trước đó
        $("#ketQuaTimKiem-SinhVien").empty();
        //Thêm đơn vị tìm được
        danhSachSinhVien.forEach(function (sinhVien) {
            $("#ketQuaTimKiem-SinhVien").append(cardSinhVien_Template({ sinhVien: sinhVien }));
        });

    }
    var updateKetQuaSinhVien = function (result) {
        themSinhVien(result.danhSachSinhVien);
    }
    var initSinhVienPagination = function (pageNumbers) {
        if (pageNumbers <= 1 || sinhVienPagination) return;
       $("#SinhVienPagination").twbsPagination({
            totalPages: pageNumbers,
            visiblePages: 5,
            first: "Đầu",
            last: "Cuối",
            prev: "Trang trước",
            next: "Trang sau",
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                pagingSearchingSorting.pageIndex = page;
                timKiemService.searchSinhVien(pagingSearchingSorting, updateKetQuaSinhVien);
            }
        });
        sinhVienPagination = true;
    }
    var initKetQuaSinhVien = function (result) {
        $("#SinhVien-section").css("display", "block");
        themSinhVien(result.danhSachSinhVien);
        initSinhVienPagination(result.pageNumbers);
        $("#SinhVienRecordNumber").html(result.totalRecords);
        totalRecordFound += result.totalRecords;
        document.title = totalRecordFound + ' kết quả tìm kiếm cho "' + searchTerm + '"';
    }
    //Tìm kiếm lớp
    var themLopVaoDom = function (danhSachLop) {
        //Lấy template cho hoạt động
        var cardLop_Template = _.template($("#cardLop_Template").html());
        //Xóa các kết quả trước
        $("#ketQuaTimKiem-Lop").empty();
        //Thêm hoạt động đang tham gia
        danhSachLop.forEach(function (lop) {
            $("#ketQuaTimKiem-Lop").append(cardLop_Template({ lop: lop }));
        });
    }
    var updateKetQuaLop = function (result) {
        themLopVaoDom(result.danhSachLop);
    }
    var initLopPagination = function (pageNumbers) {
        if (pageNumbers <= 1 || lopPagination) return;
        $("#LopPagination").twbsPagination({
            totalPages: pageNumbers,
            visiblePages: 5,
            first: "Đầu",
            last: "Cuối",
            prev: "Trang trước",
            next: "Trang sau",
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                pagingSearchingSorting.pageIndex = page;
                timKiemService.searchLop(pagingSearchingSorting, updateKetQuaLop);
            }
        });
        lopPagination = true;
    }
    var initKetQuaLop = function (result) {
        $("#Lop-section").css("display", "block");
        themLopVaoDom(result.danhSachLop);
        initLopPagination(result.pageNumbers);
        $("#LopRecordNumber").html(result.totalRecords);
        totalRecordFound += result.totalRecords;
        document.title = totalRecordFound + ' kết quả tìm kiếm cho "' + searchTerm + '"';
    }

    //Tìm kiếm Đơn vị
    var themDonViVaoDom = function (danhSachDonVi) {
        //Lấy template cho đơn vị
        var cardDonVi_Template = _.template($("#cardDonVi_Template").html());
        //Xóa các kết quả trước đó
        $("#ketQuaTimKiem-DonVi").empty();
        //Thêm đơn vị tìm được
        danhSachDonVi.forEach(function (donVi) {
            $("#ketQuaTimKiem-DonVi").append(cardDonVi_Template({ donVi: donVi }));
        });
    }
    var updateKetQuaDonVi = function (result) {
        themDonViVaoDom(result.danhSachDonVi);
    }
    var initDonViPagination = function (pageNumbers) {
        if (pageNumbers <= 1 || donViPagination) return;
        $("#DonViPagination").twbsPagination({
            totalPages: pageNumbers,
            visiblePages: 5,
            first: "Đầu",
            last: "Cuối",
            prev: "Trang trước",
            next: "Trang sau",
            startPage: 1,
            initiateStartPageClick: false,
            onPageClick: function (event, page) {
                pagingSearchingSorting.pageIndex = page;
                timKiemService.searchDonVi(pagingSearchingSorting, updateKetQuaDonVi);
            }
        });
        donViPagination = true;
    }
    var initKetQuaDonVi = function (result) {
        $("#DonVi-section").css("display", "block");
        themDonViVaoDom(result.danhSachDonVi);
        initDonViPagination(result.pageNumbers);
        $("#DonViRecordNumber").html(result.totalRecords);
        totalRecordFound += result.totalRecords;
        document.title = totalRecordFound + ' kết quả tìm kiếm cho "' +searchTerm +'"';
    }



    var intTrangTimKiem = function (searchTermModel,searchType) {
        searchTerm = searchTermModel;
        pagingSearchingSorting.searchTerm = searchTermModel;
        pagingSearchingSorting.pageIndex = 1;
        if (searchType === 1 || searchType === 2) {
            pagingSearchingSorting.pageSize = 10;

        } else {
            pagingSearchingSorting.pageSize = 9;
        }
        $("#searchTermValue").val(searchTermModel);
        $("#searchType").val(searchType);
        if (searchType === 1) timKiemService.searchHoatDong(pagingSearchingSorting, initKetQuaHoatDong);
        if (searchType === 2) timKiemService.searchBaiViet(pagingSearchingSorting, initKetQuaBaiViet);
        if (searchType === 3) timKiemService.searchSinhVien(pagingSearchingSorting, initKetQuaSinhVien);
        if (searchType === 4) timKiemService.searchLop(pagingSearchingSorting, initKetQuaLop);
        if (searchType === 5) timKiemService.searchDonVi(pagingSearchingSorting, initKetQuaDonVi);
    }
 return {
     intTrangTimKiem: intTrangTimKiem
 }
}(TimKiemService);