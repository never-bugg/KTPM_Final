using System;
using System.Windows.Forms;
using KTPM_Final.Observer.Events;

namespace KTPM_Final.Observer.Observers
{
    /// <summary>
    /// Observer cập nhật giao diện người dùng
    /// </summary>
    public class UIUpdateObserver : IObserver
    {
        private readonly Action<string> _updateStatusBar;
        private readonly Action<string, string> _updateInventoryWarning;

        public UIUpdateObserver(Action<string> updateStatusBar = null, 
                               Action<string, string> updateInventoryWarning = null)
        {
            _updateStatusBar = updateStatusBar;
            _updateInventoryWarning = updateInventoryWarning;
        }

        public void Update(object eventData)
        {
            if (eventData is EventData data)
            {
                ProcessUIUpdate(data);
            }
        }

        private void ProcessUIUpdate(EventData data)
        {
            try
            {
                switch (data.EventType)
                {
                    case EventType.SachDaBan:
                        HandleSachDaBan(data);
                        break;

                    case EventType.SachDaNhap:
                        HandleSachDaNhap(data);
                        break;

                    case EventType.HoaDonDaTao:
                        HandleHoaDonDaTao(data);
                        break;

                    case EventType.SachSapHetHang:
                    case EventType.SachHetHang:
                        HandleCanhBaoTonKho(data);
                        break;

                    case EventType.SachCoHangTroyLai:
                        HandleSachCoHangTroLai(data);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật UI: {ex.Message}");
            }
        }

        private void HandleSachDaBan(EventData data)
        {
            if (data.Data is SachBanEventData sachBan)
            {
                string message = $"Đã bán {sachBan.SoLuongBan} cuốn '{sachBan.TenSach}' - Còn lại: {sachBan.SoLuongConLai}";
                _updateStatusBar?.Invoke(message);

                // Cảnh báo nếu số lượng còn lại thấp
                if (sachBan.SoLuongConLai <= 5)
                {
                    _updateInventoryWarning?.Invoke(sachBan.TenSach, $"Còn {sachBan.SoLuongConLai} cuốn");
                }
            }
        }

        private void HandleSachDaNhap(EventData data)
        {
            if (data.Data is SachNhapEventData sachNhap)
            {
                string message = $"Đã nhập {sachNhap.SoLuongNhap} cuốn '{sachNhap.TenSach}' - Tổng: {sachNhap.SoLuongSauNhap}";
                _updateStatusBar?.Invoke(message);
            }
        }

        private void HandleHoaDonDaTao(EventData data)
        {
            if (data.Data is HoaDonEventData hoaDon)
            {
                string message = $"Hóa đơn #{hoaDon.MaHoaDon} - {hoaDon.TongTien:N0} VND - {hoaDon.TenNhanVien}";
                _updateStatusBar?.Invoke(message);
            }
        }

        private void HandleCanhBaoTonKho(EventData data)
        {
            if (data.Data is TonKhoEventData tonKho)
            {
                string warning = data.EventType == EventType.SachHetHang 
                    ? "HẾT HÀNG" 
                    : $"SẮP HẾT ({tonKho.SoLuongHienTai} cuốn)";
                
                _updateInventoryWarning?.Invoke(tonKho.TenSach, warning);
            }
        }

        private void HandleSachCoHangTroLai(EventData data)
        {
            if (data.Data is TonKhoEventData tonKho)
            {
                string message = $"'{tonKho.TenSach}' đã có hàng trở lại - Số lượng: {tonKho.SoLuongHienTai}";
                _updateStatusBar?.Invoke(message);
            }
        }
    }
}
