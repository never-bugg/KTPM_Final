using KTPM_Final.Model;
using KTPM_Final.Observer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Controllers
{
    public class ChiTietNhapSachController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public bool ThemChiTiet(ChiTietNhapSachModel ct)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Lấy thông tin sách trước khi nhập
                var sachController = new SachController();
                var sachTruocNhap = sachController.LaySachTheoMa(ct.MaSach.ToString());
                
                string query = @"INSERT INTO ChiTietNhapSach (MaPhieuNhap, MaSach, SoLuong, GiaNhap)
                             VALUES (@MaPhieuNhap, @MaSach, @SoLuong, @GiaNhap)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", ct.MaPhieuNhap);
                cmd.Parameters.AddWithValue("@MaSach", ct.MaSach);
                cmd.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
                cmd.Parameters.AddWithValue("@GiaNhap", ct.GiaNhap);
                conn.Open();
                bool result = cmd.ExecuteNonQuery() > 0;

                // Phát sự kiện khi nhập sách thành công
                if (result && sachTruocNhap != null)
                {
                    int soLuongSauNhap = sachTruocNhap.SoLuong + ct.SoLuong;
                    
                    ObserverManager.Instance.NotifySachDaNhap(
                        ct.MaSach.ToString(),
                        sachTruocNhap.TenSach,
                        ct.SoLuong,
                        soLuongSauNhap,
                        ct.GiaNhap,
                        ct.MaPhieuNhap
                    );
                }

                return result;
            }
        }
        public DataTable LayChiTietTheoMaPhieu(int maPhieuNhap)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ChiTietNhapSach WHERE MaPhieuNhap = @MaPhieuNhap";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
