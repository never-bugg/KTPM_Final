using KTPM_Final.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTPM_Final.Controllers
{
    public class NhanVienController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public DataTable LayDanhSachNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM NhanVien";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool ThemNhanVien(NhanVienModel nv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO NhanVien VALUES (@Ten, @SDT, @Email, @ChucVu, @LoaiNV, @MaTK)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", nv.TenNhanVien);
                cmd.Parameters.AddWithValue("@SDT", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@LoaiNV", nv.LoaiNhanVien);
                cmd.Parameters.AddWithValue("@MaTK", nv.MaTaiKhoan);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CapNhatNhanVien(NhanVienModel nv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE NhanVien SET TenNhanVien=@Ten, SoDienThoai=@SDT, Email=@Email,
                             ChucVu=@ChucVu, LoaiNhanVien=@LoaiNV, MaTaiKhoan=@MaTK WHERE MaNhanVien=@MaNV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", nv.MaNhanVien);
                cmd.Parameters.AddWithValue("@Ten", nv.TenNhanVien);
                cmd.Parameters.AddWithValue("@SDT", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@LoaiNV", nv.LoaiNhanVien);
                cmd.Parameters.AddWithValue("@MaTK", nv.MaTaiKhoan);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaNhanVien(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM NhanVien WHERE MaNhanVien=@MaNV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        private DBHelper dbHelper = new DBHelper();


        public NhanVienModel LayTheoMaTaiKhoan(int maTK)
        {
            string query = "SELECT * FROM NhanVien WHERE MaTaiKhoan = @MaTK";
            DataTable dt = new DataTable();

            try
            {
                dbHelper.OpenConnection();
                SqlCommand cmd = new SqlCommand(query, dbHelper.GetConnection());
                cmd.Parameters.AddWithValue("@MaTK", maTK);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            finally
            {
                dbHelper.CloseConnection();
            }

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new NhanVienModel
                {
                    MaNhanVien = Convert.ToInt32(row["MaNhanVien"]),
                    TenNhanVien = row["TenNhanVien"].ToString(),
                    SoDienThoai = row["SoDienThoai"].ToString(),
                    Email = row["Email"].ToString(),
                    ChucVu = row["ChucVu"].ToString(),
                    LoaiNhanVien = row["LoaiNhanVien"].ToString(),
                    MaTaiKhoan = Convert.ToInt32(row["MaTaiKhoan"])
                };
            }
            return null;
        }
        public string LayTenNhanVien(int maNhanVien)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TenNhanVien FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
        }
    }
}
