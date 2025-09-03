using System;
using System.Data.SqlClient;
using KTPM_Final.Observer.Events;

namespace KTPM_Final.Observer.Observers
{
    /// <summary>
    /// Observer cập nhật thống kê và báo cáo trong database
    /// </summary>
    public class StatisticsObserver : IObserver
    {
        private readonly string _connectionString;

        public StatisticsObserver(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Update(object eventData)
        {
            if (eventData is EventData data)
            {
                UpdateStatistics(data);
            }
        }

        private void UpdateStatistics(EventData data)
        {
            try
            {
                switch (data.EventType)
                {
                    case EventType.SachDaBan:
                        if (data.Data is SachBanEventData sachBan)
                            UpdateSalesStatistics(sachBan);
                        break;

                    case EventType.HoaDonDaTao:
                        if (data.Data is HoaDonEventData hoaDon)
                            UpdateRevenueStatistics(hoaDon);
                        break;

                    case EventType.SachSapHetHang:
                    case EventType.SachHetHang:
                        if (data.Data is TonKhoEventData tonKho)
                            UpdateInventoryAlert(tonKho, data.EventType);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật thống kê: {ex.Message}");
            }
        }

        private void UpdateSalesStatistics(SachBanEventData sachBan)
        {
            // Cập nhật thống kê bán hàng theo ngày
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO ThongKeBanHang (NgayBan, MaSach, SoLuongBan, DoanhThu)
                    VALUES (@NgayBan, @MaSach, @SoLuongBan, @DoanhThu)
                    ON DUPLICATE KEY UPDATE 
                    SoLuongBan = SoLuongBan + @SoLuongBan,
                    DoanhThu = DoanhThu + @DoanhThu";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NgayBan", DateTime.Today);
                cmd.Parameters.AddWithValue("@MaSach", sachBan.MaSach);
                cmd.Parameters.AddWithValue("@SoLuongBan", sachBan.SoLuongBan);
                cmd.Parameters.AddWithValue("@DoanhThu", sachBan.SoLuongBan * sachBan.GiaBan);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateRevenueStatistics(HoaDonEventData hoaDon)
        {
            // Cập nhật doanh thu theo ngày
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO ThongKeDoanhThu (NgayBan, SoHoaDon, TongDoanhThu)
                    VALUES (@NgayBan, 1, @TongDoanhThu)
                    ON DUPLICATE KEY UPDATE 
                    SoHoaDon = SoHoaDon + 1,
                    TongDoanhThu = TongDoanhThu + @TongDoanhThu";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NgayBan", DateTime.Today);
                cmd.Parameters.AddWithValue("@TongDoanhThu", hoaDon.TongTien);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateInventoryAlert(TonKhoEventData tonKho, EventType eventType)
        {
            // Ghi lại cảnh báo tồn kho
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO CanhBaoTonKho (MaSach, TenSach, SoLuong, LoaiCanhBao, ThoiGianCanhBao)
                    VALUES (@MaSach, @TenSach, @SoLuong, @LoaiCanhBao, @ThoiGianCanhBao)";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSach", tonKho.MaSach);
                cmd.Parameters.AddWithValue("@TenSach", tonKho.TenSach);
                cmd.Parameters.AddWithValue("@SoLuong", tonKho.SoLuongHienTai);
                cmd.Parameters.AddWithValue("@LoaiCanhBao", eventType.ToString());
                cmd.Parameters.AddWithValue("@ThoiGianCanhBao", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
