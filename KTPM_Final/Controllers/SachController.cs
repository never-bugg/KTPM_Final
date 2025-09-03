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
    public class SachController
    {
        private readonly string connectionString = @"Data Source=DESKTOP-BIQ6LIN;Initial Catalog=NhaSachDB;Integrated Security=True";

        public DataTable LayDanhSachSach()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh, Anh, NamXuatBan, NhaXuatBan, MoTa FROM Sach";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable LayDanhSachSachDangKinhDoanh()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, Anh, NamXuatBan, NhaXuatBan, MoTa FROM Sach WHERE NgungKinhDoanh = 0";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool ThemSach(SachModel s)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Sach (TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh, Anh, NamXuatBan, NhaXuatBan, MoTa)
                             VALUES (@TenSach, @TacGia, @TheLoai, @SoLuong, @GiaNhap, @GiaBan, 0, @Anh, @NamXuatBan, @NhaXuatBan, @MoTa)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenSach", s.TenSach);
                cmd.Parameters.AddWithValue("@TacGia", s.TacGia);
                cmd.Parameters.AddWithValue("@TheLoai", s.TheLoai);
                cmd.Parameters.AddWithValue("@SoLuong", s.SoLuong);
                cmd.Parameters.AddWithValue("@GiaNhap", s.GiaNhap);
                cmd.Parameters.AddWithValue("@GiaBan", s.GiaBan);
                cmd.Parameters.AddWithValue("@Anh", (object)s.Anh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NamXuatBan", s.NamXuatBan);
                cmd.Parameters.AddWithValue("@NhaXuatBan", (object)s.NhaXuatBan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MoTa", (object)s.MoTa ?? DBNull.Value);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaSach(SachModel s)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Sach SET TenSach = @TenSach, TacGia = @TacGia, TheLoai = @TheLoai,
                             SoLuong = @SoLuong, GiaNhap = @GiaNhap, GiaBan = @GiaBan,
                             Anh = @Anh, NamXuatBan = @NamXuatBan, NhaXuatBan = @NhaXuatBan, MoTa = @MoTa
                             WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", s.MaSach);
                cmd.Parameters.AddWithValue("@TenSach", s.TenSach);
                cmd.Parameters.AddWithValue("@TacGia", s.TacGia);
                cmd.Parameters.AddWithValue("@TheLoai", s.TheLoai);
                cmd.Parameters.AddWithValue("@SoLuong", s.SoLuong);
                cmd.Parameters.AddWithValue("@GiaNhap", s.GiaNhap);
                cmd.Parameters.AddWithValue("@GiaBan", s.GiaBan);
                cmd.Parameters.AddWithValue("@Anh", (object)s.Anh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NamXuatBan", s.NamXuatBan);
                cmd.Parameters.AddWithValue("@NhaXuatBan", (object)s.NhaXuatBan ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MoTa", (object)s.MoTa ?? DBNull.Value);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaSach(string maSach)
        {
            if (!int.TryParse(maSach, out int maSachInt))
                return false;

            if (CoLienKetNhapSachOrHoaDon(maSach))
                return false; // Có liên kết -> không cho xóa

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Sach WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", maSachInt);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public List<SachModel> TimKiemSach(string tuKhoa)
        {
            List<SachModel> list = new List<SachModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT MaSach, TenSach, TacGia, TheLoai, SoLuong, GiaNhap, GiaBan, NgungKinhDoanh, Anh, NamXuatBan, NhaXuatBan, MoTa
                             FROM Sach WHERE TenSach LIKE @TuKhoa OR TacGia LIKE @TuKhoa OR TheLoai LIKE @TuKhoa";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new SachModel
                    {
                        MaSach = reader["MaSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        TacGia = reader["TacGia"].ToString(),
                        TheLoai = reader["TheLoai"].ToString(),
                        SoLuong = Convert.ToInt32(reader["SoLuong"]),
                        GiaNhap = reader["GiaNhap"] != DBNull.Value ? Convert.ToDecimal(reader["GiaNhap"]) : 0,
                        GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                        NgungKinhDoanh = Convert.ToBoolean(reader["NgungKinhDoanh"]),
                        Anh = reader["Anh"] != DBNull.Value ? reader["Anh"].ToString() : null,
                        NamXuatBan = reader["NamXuatBan"] != DBNull.Value ? Convert.ToInt32(reader["NamXuatBan"]) : 0,
                        NhaXuatBan = reader["NhaXuatBan"] != DBNull.Value ? reader["NhaXuatBan"].ToString() : null,
                        MoTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : null
                    });
                }
                reader.Close();
            }
            return list;
        }

        public SachModel LaySachTheoMa(string maSach)
        {
            if (!int.TryParse(maSach, out int maSachInt))
                return null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Sach WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", maSachInt);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new SachModel
                    {
                        MaSach = reader["MaSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        TacGia = reader["TacGia"].ToString(),
                        TheLoai = reader["TheLoai"].ToString(),
                        SoLuong = Convert.ToInt32(reader["SoLuong"]),
                        GiaNhap = reader["GiaNhap"] != DBNull.Value ? Convert.ToDecimal(reader["GiaNhap"]) : 0,
                        GiaBan = reader["GiaBan"] != DBNull.Value ? Convert.ToDecimal(reader["GiaBan"]) : 0,
                        NgungKinhDoanh = Convert.ToBoolean(reader["NgungKinhDoanh"]),
                        Anh = reader["Anh"] != DBNull.Value ? reader["Anh"].ToString() : null,
                        NamXuatBan = reader["NamXuatBan"] != DBNull.Value ? Convert.ToInt32(reader["NamXuatBan"]) : 0,
                        NhaXuatBan = reader["NhaXuatBan"] != DBNull.Value ? reader["NhaXuatBan"].ToString() : null,
                        MoTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : null
                    };
                }
            }
            return null;
        }

        public bool CapNhatSoLuong(string maSach, int soLuongMoi)
        {
            // Lấy thông tin sách trước khi cập nhật
            var sachCu = LaySachTheoMa(maSach);
            if (sachCu == null) return false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sach SET SoLuong = @SoLuongMoi WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoLuongMoi", soLuongMoi);
                cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                conn.Open();
                bool result = cmd.ExecuteNonQuery() > 0;

                // Nếu cập nhật thành công, kiểm tra và phát sự kiện
                if (result)
                {
                    CheckAndNotifyInventoryChange(maSach, sachCu.TenSach, sachCu.SoLuong, soLuongMoi);
                }

                return result;
            }
        }

        /// <summary>
        /// Kiểm tra thay đổi tồn kho và phát sự kiện tương ứng
        /// </summary>
        private void CheckAndNotifyInventoryChange(string maSach, string tenSach, int soLuongCu, int soLuongMoi)
        {
            var observerManager = ObserverManager.Instance;

            // Nếu từ hết hàng chuyển thành có hàng
            if (soLuongCu == 0 && soLuongMoi > 0)
            {
                observerManager.NotifySachCoHangTroLai(maSach, tenSach, soLuongMoi);
            }
            // Nếu từ có hàng chuyển thành hết hàng
            else if (soLuongCu > 0 && soLuongMoi == 0)
            {
                observerManager.NotifySachHetHang(maSach, tenSach);
            }
            // Nếu số lượng giảm và dưới ngưỡng cảnh báo
            else if (soLuongMoi < soLuongCu && soLuongMoi > 0 && soLuongMoi < 10)
            {
                observerManager.NotifySachSapHetHang(maSach, tenSach, soLuongMoi);
            }
        }

        public bool CapNhatTrangThaiNgungKinhDoanh(string maSach, bool ngung)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sach SET NgungKinhDoanh = @Ngung WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ngung", ngung);
                cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CoNhapSach(string maSach)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM NhapSach WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool CoBanSach(string maSach)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaSach = @MaSach";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool CoLienKetNhapSachOrHoaDon(string maSach)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            SELECT 
                (SELECT COUNT(*) FROM ChiTietNhapSach WHERE MaSach = @MaSach) +
                (SELECT COUNT(*) FROM ChiTietHoaDon WHERE MaSach = @MaSach)
        ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSach", Convert.ToInt32(maSach));
                    int total = (int)cmd.ExecuteScalar();
                    return total > 0;
                }
            }
        }

        public List<SachModel> LaySachTonKhoDuoi10()
        {
            var list = new List<SachModel>();
            using (var conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaSach, TenSach, SoLuong FROM Sach WHERE SoLuong < 10";
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new SachModel
                    {
                        MaSach = reader["MaSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        SoLuong = Convert.ToInt32(reader["SoLuong"])
                    });
                }
            }
            return list;
        }




        public List<SachModel> ThongKeSachBan(DateTime from, DateTime to)
        {
            var result = new List<SachModel>();
            using (var conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        s.MaSach, 
                        s.TenSach, 
                        SUM(ct.SoLuong) AS SoLuongBan,
                        SUM(ct.SoLuong * ct.GiaBan) AS TongTienBan
                    FROM 
                        HoaDon hd
                        JOIN ChiTietHoaDon ct ON hd.MaHoaDon = ct.MaHoaDon
                        JOIN Sach s ON ct.MaSach = s.MaSach
                    WHERE 
                        hd.ThoiGianTao >= @from AND hd.ThoiGianTao <= @to
                    GROUP BY 
                        s.MaSach, s.TenSach
                ";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SachModel
                    {
                        MaSach = reader["MaSach"].ToString(),
                        TenSach = reader["TenSach"].ToString(),
                        SoLuongBan = reader["SoLuongBan"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongBan"]) : 0,
                        TongTienBan = reader["TongTienBan"] != DBNull.Value ? Convert.ToDecimal(reader["TongTienBan"]) : 0
                    });
                }
            }
            return result;
        }
    }
}
