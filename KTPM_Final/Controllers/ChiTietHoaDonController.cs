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
    public class ChiTietHoaDonController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public bool ThemChiTietHoaDon(ChiTietHoaDonModel chiTiet)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ChiTietHoaDon (MaHoaDon, MaSach, SoLuong, GiaBan)
                         VALUES (@MaHoaDon, @MaSach, @SoLuong, @GiaBan)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", chiTiet.MaHoaDon);
                cmd.Parameters.AddWithValue("@MaSach", chiTiet.MaSach);
                cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@GiaBan", chiTiet.GiaBan);

                conn.Open();
                bool result = cmd.ExecuteNonQuery() > 0;

                // Phát sự kiện khi thêm chi tiết hóa đơn thành công (bán sách)
                if (result)
                {
                    // Lấy thông tin sách để phát sự kiện
                    var sachController = new SachController();
                    var sach = sachController.LaySachTheoMa(chiTiet.MaSach);
                    
                    if (sach != null)
                    {
                        ObserverManager.Instance.NotifySachDaBan(
                            chiTiet.MaSach,
                            sach.TenSach,
                            chiTiet.SoLuong,
                            sach.SoLuong, // Số lượng còn lại sẽ được cập nhật sau
                            chiTiet.GiaBan,
                            chiTiet.MaHoaDon
                        );
                    }
                }

                return result;
            }
        }

        public List<ChiTietHoaDonModel> LayChiTietHoaDonTheoMaHoaDon(int maHoaDon)
        {
            List<ChiTietHoaDonModel> list = new List<ChiTietHoaDonModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT cthd.MaChiTiet, cthd.MaHoaDon, cthd.MaSach, s.TenSach, cthd.SoLuong, cthd.GiaBan
                             FROM ChiTietHoaDon cthd
                             JOIN Sach s ON cthd.MaSach = s.MaSach
                             WHERE cthd.MaHoaDon = @MaHoaDon";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ct = new ChiTietHoaDonModel
                    {
                        MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                        MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                        MaSach = reader["MaSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        SoLuong = Convert.ToInt32(reader["SoLuong"]),
                        GiaBan = Convert.ToDecimal(reader["GiaBan"])
                    };
                    list.Add(ct);
                }
                reader.Close();
            }
            return list;
        }

        // Lấy tất cả chi tiết hóa đơn
        public List<ChiTietHoaDonModel> LayTatCaChiTietHoaDon()
        {
            List<ChiTietHoaDonModel> list = new List<ChiTietHoaDonModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT cthd.MaChiTiet, cthd.MaHoaDon, cthd.MaSach, s.TenSach, cthd.SoLuong, cthd.GiaBan
                             FROM ChiTietHoaDon cthd
                             JOIN Sach s ON cthd.MaSach = s.MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ct = new ChiTietHoaDonModel
                    {
                        MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]),
                        MaHoaDon = Convert.ToInt32(reader["MaHoaDon"]),
                        MaSach = reader["MaSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        SoLuong = Convert.ToInt32(reader["SoLuong"]),
                        GiaBan = Convert.ToDecimal(reader["GiaBan"])
                    };
                    list.Add(ct);
                }
                reader.Close();
            }
            return list;
        }

        // Xóa chi tiết hóa đơn theo mã chi tiết
        public bool XoaChiTietHoaDon(int maChiTiet)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Sửa chi tiết hóa đơn
        public bool SuaChiTietHoaDon(ChiTietHoaDonModel chiTiet)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE ChiTietHoaDon 
                             SET MaHoaDon = @MaHoaDon, MaSach = @MaSach, SoLuong = @SoLuong, GiaBan = @GiaBan
                             WHERE MaChiTiet = @MaChiTiet";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTiet);
                cmd.Parameters.AddWithValue("@MaHoaDon", chiTiet.MaHoaDon);
                cmd.Parameters.AddWithValue("@MaSach", chiTiet.MaSach);
                cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@GiaBan", chiTiet.GiaBan);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
