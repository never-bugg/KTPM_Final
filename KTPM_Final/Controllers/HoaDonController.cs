using KTPM_Final.Model;
using KTPM_Final.Observer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Controllers
{
    public class HoaDonController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public bool ThemHoaDon(HoaDonModel hoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO HoaDon (TongTien, ThoiGianTao, TenNhanVien, MaNhanVien)
                         VALUES (@TongTien, @ThoiGianTao, @TenNhanVien, @MaNhanVien);
                         SELECT SCOPE_IDENTITY();"; // Lấy ID tự động tạo
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TongTien", hoaDon.TongTien);
                cmd.Parameters.AddWithValue("@ThoiGianTao", hoaDon.ThoiGianTao);
                cmd.Parameters.AddWithValue("@TenNhanVien", hoaDon.TenNhanVien);
                cmd.Parameters.AddWithValue("@MaNhanVien", hoaDon.MaNhanVien);

                conn.Open();
                hoaDon.MaHoaDon = Convert.ToInt32(cmd.ExecuteScalar());
                bool result = hoaDon.MaHoaDon > 0;

                // Phát sự kiện khi tạo hóa đơn thành công
                if (result)
                {
                    // Đếm số lượng sản phẩm (có thể truyền từ ngoài vào)
                    int soLuongSanPham = GetSoLuongSanPhamTrongHoaDon(hoaDon.MaHoaDon);
                    
                    ObserverManager.Instance.NotifyHoaDonDaTao(
                        hoaDon.MaHoaDon,
                        hoaDon.TongTien,
                        hoaDon.ThoiGianTao,
                        hoaDon.TenNhanVien,
                        soLuongSanPham
                    );
                }

                return result;
            }
        }

        /// <summary>
        /// Đếm số lượng sản phẩm trong hóa đơn
        /// </summary>
        private int GetSoLuongSanPhamTrongHoaDon(int maHoaDon)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaHoaDon = @MaHoaDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
        public List<HoaDonModel> LayTatCaHoaDon()
        {
            List<HoaDonModel> list = new List<HoaDonModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaHoaDon, TongTien, ThoiGianTao, TenNhanVien, MaNhanVien FROM HoaDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HoaDonModel hd = new HoaDonModel
                    {
                        MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        ThoiGianTao = Convert.ToDateTime(reader["ThoiGianTao"]),
                        TenNhanVien = reader["TenNhanVien"].ToString(),
                        MaNhanVien = Convert.ToInt32(reader["MaNhanVien"])
                    };
                    list.Add(hd);
                }
                reader.Close();
            }
            return list;
        }
    }
}