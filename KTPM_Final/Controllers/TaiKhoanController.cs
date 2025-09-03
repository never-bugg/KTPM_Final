using KTPM_Final.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTPM_Final.Controllers
{
    public class TaiKhoanController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public List<TaiKhoanModel> LayDanhSachTaiKhoan()
        {
            List<TaiKhoanModel> danhSach = new List<TaiKhoanModel>();
            string query = "SELECT * FROM TaiKhoanNguoiDung";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaiKhoanModel tk = new TaiKhoanModel
                    (
                        reader["MaTaiKhoan"].ToString(),
                        reader["TenDangNhap"].ToString(),
                        reader["MatKhau"].ToString(),
                        reader["VaiTro"].ToString(),
                        Convert.ToBoolean(reader["KichHoat"])
                    );
                    danhSach.Add(tk);
                }
            }
            return danhSach;
        }

        public bool ThemTaiKhoan(TaiKhoanModel tk)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM TaiKhoanNguoiDung WHERE TenDangNhap = @Ten";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Ten", tk.TenDangNhap);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    return false; // Username already exists
                }

                string insertQuery = "INSERT INTO TaiKhoanNguoiDung (TenDangNhap, MatKhau, VaiTro, KichHoat) VALUES (@Ten, @MatKhau, @VaiTro, @KichHoat)";
                tk.MatKhau = HashPassword(tk.MatKhau);
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Ten", tk.TenDangNhap);
                insertCmd.Parameters.AddWithValue("@MatKhau", tk.MatKhau);
                insertCmd.Parameters.AddWithValue("@VaiTro", tk.VaiTro);
                insertCmd.Parameters.AddWithValue("@KichHoat", tk.KichHoat);

                return insertCmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CapNhatTaiKhoan(TaiKhoanModel tk)
        {
            string query = "UPDATE TaiKhoanNguoiDung SET TenDangNhap=@Ten, MatKhau=@MatKhau, VaiTro=@VaiTro, KichHoat=@KichHoat WHERE MaTaiKhoan=@Ma";
            tk.MatKhau = HashPassword(tk.MatKhau);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", tk.MaTaiKhoan);
                cmd.Parameters.AddWithValue("@Ten", tk.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", tk.MatKhau);
                cmd.Parameters.AddWithValue("@VaiTro", tk.VaiTro);
                cmd.Parameters.AddWithValue("@KichHoat", tk.KichHoat);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public TaiKhoanModel DangNhap(string tenDangNhap, string matKhau)
        {
            string query = "SELECT * FROM TaiKhoanNguoiDung WHERE TenDangNhap = @Ten AND MatKhau = @Mat";
            matKhau = HashPassword(matKhau);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", tenDangNhap);
                cmd.Parameters.AddWithValue("@Mat", matKhau);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TaiKhoanModel tk = new TaiKhoanModel
                    {
                        MaTaiKhoan = reader["MaTaiKhoan"].ToString(),
                        TenDangNhap = reader["TenDangNhap"].ToString(),
                        MatKhau = reader["MatKhau"].ToString(),
                        VaiTro = reader["VaiTro"].ToString(),
                        KichHoat = Convert.ToBoolean(reader["KichHoat"])
                    };
                    return tk;
                }
                return null;
            }
        }
        public bool XoaTaiKhoan(string maTaiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TaiKhoanNguoiDung WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public List<TaiKhoanModel> TimKiemTaiKhoan(string tenDangNhap, string vaiTro)
        {
            List<TaiKhoanModel> danhSach = new List<TaiKhoanModel>();
            string query = "SELECT * FROM TaiKhoanNguoiDung WHERE LOWER(TenDangNhap) LIKE @Ten AND (@VaiTro IS NULL OR VaiTro = @VaiTro)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", $"%{tenDangNhap}%");
                cmd.Parameters.AddWithValue("@VaiTro", string.IsNullOrEmpty(vaiTro) ? (object)DBNull.Value : vaiTro);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaiKhoanModel tk = new TaiKhoanModel
                    (
                        reader["MaTaiKhoan"].ToString(),
                        reader["TenDangNhap"].ToString(),
                        reader["MatKhau"].ToString(),
                        reader["VaiTro"].ToString(),
                        Convert.ToBoolean(reader["KichHoat"])
                    );
                    danhSach.Add(tk);
                }
            }
            return danhSach;
        }
        public List<TaiKhoanModel> LayTaiKhoanChuaGanNhanVien()
        {
            List<TaiKhoanModel> list = new List<TaiKhoanModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TaiKhoanNguoiDung WHERE MaTaiKhoan NOT IN (SELECT MaTaiKhoan FROM NhanVien WHERE MaTaiKhoan IS NOT NULL)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TaiKhoanModel
                        {
                            MaTaiKhoan = reader["MaTaiKhoan"].ToString(),
                            TenDangNhap = reader["TenDangNhap"].ToString()
                        });
                    }
                }
            }
            return list;
        }
        private static string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}