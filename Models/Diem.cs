using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NAPASTUDENT.Models
{
    public class Diem
    {
        public int SinhVienId{ get;private set; }

        public SinhVien SinhVien { get; set; }

        public int MonHocId { get;private set; }

        public MonHoc MonHoc { get; set; }

        public int LopId { get;private set; }

        public LopMonHoc LopMonHoc { get; set; }

        [Range(0, 10)]

        public byte? DiemChuyenCan { get; private set; }

        [Range(0, 10)]
        public byte? DiemDieuKien1 { get; private set; }

        [Range(0, 10)]
        public byte? DiemDieuKien2 { get; private set; }

        [Range(0, 10)]
        public byte? DiemThi { get; private set; }

        [Range(0, 10)]
        public byte? DiemTb { get; private set; }

        public IList<DiemBoSung> DanhSachDiemBoSung { get; set; }

        public HocKi HocKi { get; set; }

        public LichThiLai LichThiLai { get; set; }

        public int? LichThiLaiId { get;private set; }

        public Diem()
        {
            DanhSachDiemBoSung = new List<DiemBoSung>();
        }

        public Diem(int monHocId, int lopId, int sinhVienId, HocKi hocKi)
        {
            SinhVienId = sinhVienId;
            HocKi = hocKi;
            MonHocId = monHocId;
            LopId = lopId;
        }

        public void TinhDiemTb()
        {
            int? DiemDieuKien;
            double? DiemThiDeTinh = null;
            //Tính điểm điều kiện
            if (MonHoc.HaiDiemDk)      //Nếu có 2 cột điều kiện thì lấy trung bình
            {
                if (DiemDieuKien1 == null || DiemDieuKien2 == null) return;
                DiemDieuKien = (DiemDieuKien1 + DiemDieuKien2) / 2;
            }
            else DiemDieuKien = DiemDieuKien1; //Nếu chỉ 1 cột thì lấy cột điều kiện 1
            
            if (MonHoc.LoaiMon != LoaiMon.MonThuong) //Nếu không phải là môn thường mà là TA hoặc CPĐT
            {
               //Nếu có bất cứ điểm bổ sung nào không có (null) thì bỏ qua
               if (DanhSachDiemBoSung.Any(dbs => dbs.Diem == null)) return;  
               
               if (MonHoc.LoaiMon == LoaiMon.MonTiengAnh) //Nếu là môn tiếng Anh
                {
                    var diemChuaLamTron =
                        DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.LoaiDiem == LoaiDiem.ThiNghe_Ta).Diem * 0.2
                      + DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.LoaiDiem == LoaiDiem.ThiNoi_Ta).Diem * 0.2
                      + DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.LoaiDiem == LoaiDiem.ThiViet_Ta).Diem * 0.6;
                    DiemThiDeTinh =  Math.Round( (double) diemChuaLamTron,MidpointRounding.AwayFromZero);
                }
                else   //Nếu là môn CPĐT
                {
                    var diemChuaLamTron =
                        DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.LoaiDiem == LoaiDiem.LyThuyet).Diem * 0.5
                      + DanhSachDiemBoSung.SingleOrDefault(dbs => dbs.LoaiDiem == LoaiDiem.ThucHanh).Diem * 0.5;
                    DiemThiDeTinh = Math.Round((double)diemChuaLamTron, MidpointRounding.AwayFromZero);
                }
            }
            else //Nếu là môn thường
            {
                if (DiemThi != null) DiemThiDeTinh = (double)DiemThi;
            }

            //Nếu có bất cứ điểm nào (Cc, Đk hoặc Thi) chưa có thì sẽ không tính điểm trung bình.
            if (DiemChuyenCan == null || DiemThiDeTinh == null || DiemDieuKien == null)  return;

            //Cách tính điểm trung bình: Lấy (Chuyên cần *0.1) + (Điểm Điều kiện *0.3) + (Điểm thi *0.6) rồi làm tròn
            DiemTb = (byte) Math.Round((double) (DiemChuyenCan * 0.1 + DiemDieuKien * 0.3 + DiemThiDeTinh * 0.6),
                                        MidpointRounding.AwayFromZero);
        }

        public void HocLai()
        {
            DiemChuyenCan = null;
            DiemDieuKien1 = null;
            DiemDieuKien2 = null;
            DiemThi = null;
            DiemTb = null;
            foreach (var diemBoSung in DanhSachDiemBoSung)
            {
                diemBoSung.ResetDiem();
            }
            LichThiLaiId = null;
        }

        public void SetDiemChuyenCan(byte? diemChuyenCan)
        {
            DiemChuyenCan = diemChuyenCan;
        }

        public void SetDiemDieuKien1(byte? diemDieuKien1)
        {
            DiemDieuKien1 = diemDieuKien1;  
        }

        public void SetDiemDieuKien2(byte? diemDieuKien2)
        {
            DiemDieuKien2 = diemDieuKien2;
        }

        public void SetDiemThi(byte? diemThi)
        {
            DiemThi = diemThi;
        }

        public void DangKiLichThiLai(int lichThiLaiId)
        {
            LichThiLaiId = lichThiLaiId;
            //Đăng kí lịch thi lại cho sinh viên sẽ reset Điểm thi cho sinh viên đó
            DiemThi = null;
            DiemTb = null;
            foreach (var diemBoSung in DanhSachDiemBoSung)
            {
                diemBoSung.ResetDiem();
            }
        }
    }
}