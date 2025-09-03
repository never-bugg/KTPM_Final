using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Model
{
    public class NhanVienModel
    {
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string ChucVu { get; set; }
        public string LoaiNhanVien { get; set; }
        public int MaTaiKhoan { get; set; }

    }
    public static class Session
    {
        public static NhanVienModel NhanVienHienTai { get; set; }
        public static TaiKhoanModel TaiKhoanHienTai { get; set; }
    }
}
