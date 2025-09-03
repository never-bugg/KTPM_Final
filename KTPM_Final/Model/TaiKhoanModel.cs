using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Model
{
    public class TaiKhoanModel
    {
        public string MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }
        public bool KichHoat { get; set; }

        public TaiKhoanModel() { }

        public TaiKhoanModel(string ma, string ten, string matKhau, string vaiTro, bool kichHoat)
        {
            MaTaiKhoan = ma;
            TenDangNhap = ten;
            MatKhau = matKhau;
            VaiTro = vaiTro;
            KichHoat = kichHoat;
        }
    }
}
