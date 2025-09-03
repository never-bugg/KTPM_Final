using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Model
{
    public class HoaDonModel
    {
        public int MaHoaDon { get; set; }
        public decimal TongTien { get; set; }
        public DateTime ThoiGianTao { get; set; }
        public string TenNhanVien { get; set; }
        public int MaNhanVien { get; set; }
    }
}
