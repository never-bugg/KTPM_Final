using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Model
{
    public class SachModel
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public bool NgungKinhDoanh { get; set; }
        public int SoLuongBan { get; set; }
        public decimal TongTienBan { get; set; }
        public string Anh { get; set; }           // Đường dẫn hoặc tên file ảnh
        public int NamXuatBan { get; set; }       // Năm xuất bản
        public string NhaXuatBan { get; set; }    // Nhà xuất bản
        public string MoTa { get; set; }
    }
    public class ThongKeDoanhThuResult
    {
        public decimal DoanhThu { get; set; }
        public decimal LoiNhuan { get; set; }
        public int SoHoaDon { get; set; }
        public int SoSachBan { get; set; }
        public List<SachThongKeModel> TopBanChay { get; set; } = new List<SachThongKeModel>();
        public List<SachThongKeModel> TopBanIt { get; set; } = new List<SachThongKeModel>();
    }
    public class SachThongKeModel
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongBan { get; set; }
        public decimal TongTienBan { get; set; }
    }
}