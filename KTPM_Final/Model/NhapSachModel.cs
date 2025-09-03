using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Model
{
    public class NhapSachModel
    {
        public int MaPhieuNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public int MaNhanVien { get; set; }
        public decimal TongTien { get; set; }
    }
}
