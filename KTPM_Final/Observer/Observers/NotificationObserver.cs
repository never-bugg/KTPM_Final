using System;
using System.Windows.Forms;
using KTPM_Final.Observer.Events;

namespace KTPM_Final.Observer.Observers
{
    /// <summary>
    /// Observer hiển thị thông báo cho người dùng
    /// </summary>
    public class NotificationObserver : IObserver
    {
        private readonly bool _showPopup;

        public NotificationObserver(bool showPopup = true)
        {
            _showPopup = showPopup;
        }

        public void Update(object eventData)
        {
            if (eventData is EventData data)
            {
                string message = GenerateMessage(data);
                
                if (_showPopup)
                {
                    ShowNotification(data.EventType, message);
                }
                
                // Log thông báo
                LogNotification(data.EventType, message);
            }
        }

        private string GenerateMessage(EventData data)
        {
            switch (data.EventType)
            {
                case EventType.SachDaBan:
                    if (data.Data is SachBanEventData sachBan)
                        return $"Đã bán {sachBan.SoLuongBan} cuốn '{sachBan.TenSach}'. Còn lại: {sachBan.SoLuongConLai}";
                    break;

                case EventType.SachDaNhap:
                    if (data.Data is SachNhapEventData sachNhap)
                        return $"Đã nhập {sachNhap.SoLuongNhap} cuốn '{sachNhap.TenSach}'. Tổng: {sachNhap.SoLuongSauNhap}";
                    break;

                case EventType.HoaDonDaTao:
                    if (data.Data is HoaDonEventData hoaDon)
                        return $"Đã tạo hóa đơn #{hoaDon.MaHoaDon} - Tổng tiền: {hoaDon.TongTien:N0} VND";
                    break;

                case EventType.SachSapHetHang:
                    if (data.Data is TonKhoEventData tonKho)
                        return $"CẢNH BÁO: Sách '{tonKho.TenSach}' sắp hết hàng! Còn {tonKho.SoLuongHienTai} cuốn";
                    break;

                case EventType.SachHetHang:
                    if (data.Data is TonKhoEventData tonKhoHet)
                        return $"CẢNH BÁO: Sách '{tonKhoHet.TenSach}' đã hết hàng!";
                    break;

                case EventType.SachCoHangTroyLai:
                    if (data.Data is TonKhoEventData tonKhoTroLai)
                        return $"Thông báo: Sách '{tonKhoTroLai.TenSach}' đã có hàng trở lại! Số lượng: {tonKhoTroLai.SoLuongHienTai}";
                    break;
            }

            return data.Message ?? "Có sự kiện mới trong hệ thống";
        }

        private void ShowNotification(EventType eventType, string message)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;
            string title = "Thông báo";

            switch (eventType)
            {
                case EventType.SachSapHetHang:
                case EventType.SachHetHang:
                    icon = MessageBoxIcon.Warning;
                    title = "Cảnh báo tồn kho";
                    break;
                case EventType.HoaDonDaTao:
                    icon = MessageBoxIcon.Information;
                    title = "Thành công";
                    break;
            }

            // Chỉ hiển thị popup cho các sự kiện quan trọng
            if (eventType == EventType.SachSapHetHang || 
                eventType == EventType.SachHetHang || 
                eventType == EventType.HoaDonDaTao)
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
            }
        }

        private void LogNotification(EventType eventType, string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{eventType}] {message}";
            System.Diagnostics.Debug.WriteLine(logMessage);
            
            // Có thể ghi vào file log nếu cần
            // File.AppendAllText("system_log.txt", logMessage + Environment.NewLine);
        }
    }
}
