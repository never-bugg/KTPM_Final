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
    public class NhapSachController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public int ThemPhieuNhap(NhapSachModel pn)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO NhapSach (NgayNhap, MaNhanVien, TongTien) 
                             VALUES (@NgayNhap, @MaNhanVien, @TongTien);
                             SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NgayNhap", pn.NgayNhap);
                cmd.Parameters.AddWithValue("@MaNhanVien", pn.MaNhanVien);
                cmd.Parameters.AddWithValue("@TongTien", pn.TongTien);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public List<NhapSachModel> LayDanhSachPhieuNhap()
        {
            List<NhapSachModel> danhSach = new List<NhapSachModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaPhieuNhap, NgayNhap, MaNhanVien, TongTien FROM NhapSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NhapSachModel pn = new NhapSachModel
                        {
                            MaPhieuNhap = reader.GetInt32(0),
                            NgayNhap = reader.GetDateTime(1),
                            MaNhanVien = reader.GetInt32(2),
                            TongTien = reader.GetDecimal(3)
                        };
                        danhSach.Add(pn);
                    }
                }
            }
            return danhSach;
        }
    }
}
