var QuanLyChungMonHocService = function() {
    var layMonHoc = function (monHocId, updateInput) {
        $.ajax({
            url: "/api/MonHoc/ChiTietCoBan/" + monHocId,
            type: "GET",
            async: false,
            success: function (data) {
                updateInput(data);
            }
        });
    }
    var saveMon = function (monHocDto, updateAfterSave) {
        $.ajax({
            url: "/api/MonHoc/SaveMonHoc",
            type: "POST",
            data: monHocDto,
            async: false,
            success: function (data) {
                updateAfterSave();
            }
        });
    }
    var xoaMon = function (monHocId, updateAfterXoaMon) {
        $.ajax({
            url: "/api/MonHoc/XoaMonHoc/"+monHocId,
            type: "DELETE",
            async: false,
            success: function () {
                updateAfterXoaMon();
            }
        });
    }
    return{
        layMonHoc: layMonHoc,
        saveMon: saveMon,
        xoaMon: xoaMon
    }
}();

var QuanLyMonHocService = function() {
    var layMonHoc = function (monHocId, updateInputOrDom) {
        $.ajax({
            url: "/api/MonHoc/ChiTietCoBan/" + monHocId,
            type: "GET",
            async: false,
            success: function (data) {
                updateInputOrDom(data);
            }
        });
    }
    var taoDiemSinhVien = function (taoDiemMonHocDto) {
        $.ajax({
            url: "/api/MonHoc/TaoDiem",
            type: "POST",
            data: taoDiemMonHocDto,
            success: function (data) {
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });
    }
    var saveMon = function (monHocDto, updateAfterSaveMonHoc) {
        $.ajax({
            url: "/api/MonHoc/SaveMonHoc",
            type: "POST",
            data: monHocDto,
            async: false,
            success: function (data) {
                updateAfterSaveMonHoc();
            }
        });
    }
    var getLopSelectList = function (initLopSelectList) {
        $.ajax({
            url: "/api/Lop/SelectList",
            type: "GET",
            success: function (data) {
                initLopSelectList(data);
            }
        });
    }
    var saveLopMonHoc = function (dangKiLopMonHocDto,updateAfterSaveLopMonHoc) {
        $.ajax({
            url: "/api/MonHoc/DangKiMonHoc",
            type: "POST",
            data: dangKiLopMonHocDto,
            async: false,
            success: function (data) {
                taoDiemSinhVien(dangKiLopMonHocDto);
                updateAfterSaveLopMonHoc();
            },
            error: function (xhr, textStatus, errorThrown) {
                updateAfterSaveLopMonHoc(xhr);
            }
        });
    }
    var xoaDangKiMonHoc = function (xoaDangKiMonHocDto, updateAfterXoaDangKiMonHoc) {
        $.ajax({
            url: "/api/MonHoc/XoaDangKiMonHoc",
            type: "Delete",
            data: xoaDangKiMonHocDto,
            async: false,
            success: function (data) {
                updateAfterXoaDangKiMonHoc();
            },
            error: function (xhr, textStatus, errorThrown) {
            }
        });
    }
    
    return {
        layMonHoc: layMonHoc,
        saveMon: saveMon,
        getLopSelectList: getLopSelectList,
        saveLopMonHoc: saveLopMonHoc,
        xoaDangKiMonHoc: xoaDangKiMonHoc
    }
}();

var QuanLyLopMonHocService = function() {
    var getLopMonHocData = function (lopMonHocId, updateInfoLopMonHoc) {
        $.ajax({
            url: "/api/MonHoc/QuanLyLopMonHoc",
            type: "POST",
            data: lopMonHocId,
            async: false,
            success: function (data) {
                updateInfoLopMonHoc(data);
            }
        });
    }
    var getDiemLopMonHoc = function (lopMonHocId, taoDiemSinhVien) {
        $.ajax({
            url: "/api/MonHoc/LayDiemLopMonHoc",
            type: "POST",
            data: lopMonHocId,
            async: false,
            success: function (data) {
                taoDiemSinhVien(data);
            }
        });
    }
    var getLichHocData = function (lichHocId, updateLichHocInput) {
        $.ajax({
            url: "/api/MonHoc/LayLichHoc/" + lichHocId,
            type: "GET",
            async: false,
            success: function (data) {
                updateLichHocInput(data);
            }
        });
    }
    var saveLichHoc = function(lichHocDto,updateAfterSaveLichHoc) {
        $.ajax({
            url: "/api/MonHoc/DangKiLichHoc/",
            type: "POST",
            data: lichHocDto,
            async: false,
            success: function (data) {
                updateAfterSaveLichHoc(data);
            }
        });
    }
    var getDanhSachLichHoc = function (lopMonHocId,updateDanhSachLichHoc) {
        $.ajax({
            url: "/api/MonHoc/LayDanhSachLichHoc/",
            type: "POST",
            data : lopMonHocId,
            async: false,
            success: function (data) {
                updateDanhSachLichHoc(data);
            }
        });
    }
    var xoaLichHoc = function(lichHocId, updateAfterXoaLichHoc) {
        $.ajax({
            url: "/api/MonHoc/XoaLichHoc/"+lichHocId,
            type: "DELETE",
            async: false,
            success: function () {
                updateAfterXoaLichHoc();
            }
        });
    };
    var suaLopMonHoc = function(lopMonHocDto, updateAfterSuaLopMonHoc) {
        $.ajax({
            url: "/api/MonHoc/SuaLopMonHoc/",
            type: "PUT",
            data: lopMonHocDto,
            async: false,
            success: function (data) {
                updateAfterSuaLopMonHoc();
            }
        });
    }
    return {
        getLopMonHocData: getLopMonHocData,
        getDiemLopMonHoc: getDiemLopMonHoc,
        getLichHocData: getLichHocData,
        saveLichHoc: saveLichHoc,
        getDanhSachLichHoc: getDanhSachLichHoc,
        xoaLichHoc: xoaLichHoc,
        suaLopMonHoc: suaLopMonHoc
    }
}();

var XemDiemMonHocSinhVienService = function () {
    var getPageData = function (monHocId, updateThongTin) {
        $.ajax({
            url: "/api/MonHoc/LayDiemSinhVienMon/" + monHocId,
            type: "GET",
            async: false,
            success: function (data) {
                updateThongTin(data);
            }
        });
    }
    return {
        getPageData : getPageData
    }
}();

var QuanLyThiLaiService = function() {
    var layMonHoc = function (monHocId, updateDom) {
        $.ajax({
            url: "/api/MonHoc/ChiTietCoBan/" + monHocId,
            type: "GET",
            success: function (data) {
                updateDom(data);
            }
        });
    }
    var layThongTinLichThiLai = function(lichThiLaiId, updatePage) {
        $.ajax({
            url: "/api/MonHoc/LayThongTinThiLai/" + lichThiLaiId,
            type: "GET",
            success: function (data) {
                updatePage(data);
            }
        });
    }
    var dangKiLichThiLai = function(dangKiThiLaiDto, updatePageAfterDangKi) {
        $.ajax({
            url: '/api/MonHoc/DangKiLichThiLai',
            type: 'POST',
            data: dangKiThiLaiDto,
            success: function() {
                updatePageAfterDangKi();
            }
        });
    }
    var saveLichThiLai = function(thongTinThiLaiDto, updateAfterSaveLichThiLai) {
        $.ajax({
            url: '/api/MonHoc/SaveLichThiLai',
            type: 'POST',
            data: thongTinThiLaiDto,
            success: function () {
                updateAfterSaveLichThiLai();
            }
        });
    }
    var ketThucThiLai= function(lichThiLaiId, updateAfterKetThucThiLai) {
        $.ajax({
            url: '/api/MonHoc/KetThucThiLai/' + lichThiLaiId,
            type: 'POST',
            success: function () {
                updateAfterKetThucThiLai();
            }
        });
    }

   return {
       layMonHoc: layMonHoc,
       layThongTinLichThiLai: layThongTinLichThiLai,
       dangKiLichThiLai: dangKiLichThiLai,
       saveLichThiLai: saveLichThiLai,
       ketThucThiLai: ketThucThiLai
   }
}();
