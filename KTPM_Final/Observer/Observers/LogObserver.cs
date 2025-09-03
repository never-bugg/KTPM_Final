using System;
using System.IO;
using KTPM_Final.Observer.Events;

namespace KTPM_Final.Observer.Observers
{
    /// <summary>
    /// Observer ghi log các sự kiện vào file
    /// </summary>
    public class LogObserver : IObserver
    {
        private readonly string _logFilePath;

        public LogObserver(string logFilePath = "system_events.log")
        {
            _logFilePath = logFilePath;
            
            // Tạo thư mục logs nếu chưa tồn tại
            string directory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void Update(object eventData)
        {
            if (eventData is EventData data)
            {
                WriteLog(data);
            }
        }

        private void WriteLog(EventData data)
        {
            try
            {
                string logEntry = FormatLogEntry(data);
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi ghi log: {ex.Message}");
            }
        }

        private string FormatLogEntry(EventData data)
        {
            string details = GetEventDetails(data);
            return $"[{data.Timestamp:yyyy-MM-dd HH:mm:ss}] [{data.EventType}] {details}";
        }

        private string GetEventDetails(EventData data)
        {
            switch (data.EventType)
            {
                case EventType.SachDaBan:
                    if (data.Data is SachBanEventData sachBan)
                        return $"MaSach: {sachBan.MaSach}, TenSach: {sachBan.TenSach}, " +
                               $"SoLuongBan: {sachBan.SoLuongBan}, ConLai: {sachBan.SoLuongConLai}, " +
                               $"GiaBan: {sachBan.GiaBan}, MaHoaDon: {sachBan.MaHoaDon}";
                    break;

                case EventType.SachDaNhap:
                    if (data.Data is SachNhapEventData sachNhap)
                        return $"MaSach: {sachNhap.MaSach}, TenSach: {sachNhap.TenSach}, " +
                               $"SoLuongNhap: {sachNhap.SoLuongNhap}, TongSau: {sachNhap.SoLuongSauNhap}, " +
                               $"GiaNhap: {sachNhap.GiaNhap}, MaPhieuNhap: {sachNhap.MaPhieuNhap}";
                    break;

                case EventType.HoaDonDaTao:
                    if (data.Data is HoaDonEventData hoaDon)
                        return $"MaHoaDon: {hoaDon.MaHoaDon}, TongTien: {hoaDon.TongTien}, " +
                               $"NhanVien: {hoaDon.TenNhanVien}, SoSP: {hoaDon.SoLuongSanPham}";
                    break;

                case EventType.SachSapHetHang:
                case EventType.SachHetHang:
                case EventType.SachCoHangTroyLai:
                    if (data.Data is TonKhoEventData tonKho)
                        return $"MaSach: {tonKho.MaSach}, TenSach: {tonKho.TenSach}, " +
                               $"SoLuong: {tonKho.SoLuongHienTai}";
                    break;
            }

            return data.Message ?? "Không có chi tiết";
        }
    }
}
